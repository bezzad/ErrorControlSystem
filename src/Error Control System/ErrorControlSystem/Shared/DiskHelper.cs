using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ErrorControlSystem.Properties;

namespace ErrorControlSystem.Shared
{
    public static class DiskHelper
    {
        /// <summary>
        /// Get Size of directory by all sub directory and files.
        /// </summary>
        /// <param name="dir">The Directory</param>
        /// <returns>Size (bytes) of Directory</returns>
        public static long GetDirectorySize(this DirectoryInfo dir)
        {
            long sum = 0;

            // get IEnumerable from all files in the current directory and all sub directories
            try
            {
                var subDirectories = dir.GetDirectories()
                    .Where(d => (d.Attributes & FileAttributes.ReparsePoint) == 0).AsParallel();

                Parallel.ForEach(subDirectories, d =>
                {
                    var value = d.GetDirectorySize();
                    Interlocked.Add(ref sum, value);
                });

                // get the size of all files
                sum += (from file in dir.GetFiles() select file.Length).Sum();
            }
            catch { }


            return sum;
        }


        public static async Task<bool> WriteSettingAsync(string key, string value, bool attach = false)
        {
            return await Task.Run(() =>
            {
                try
                {
                    Settings.Default[key] = (attach ? ReadSetting(key) : "") + value;

                    Settings.Default.Save();

                    return true;
                }
                catch
                {
                    return false;
                }
            });
        }

        public static string ReadSetting(string key)
        {
            try { return (string)Settings.Default[key]; }
            catch { return ""; }
        }

    }
}
