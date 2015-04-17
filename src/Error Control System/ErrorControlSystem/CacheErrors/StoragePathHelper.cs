
namespace ErrorControlSystem.CacheErrors
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using System;
    using System.IO;

    using ErrorControlSystem.DbConnectionManager;


    public static class StoragePathHelper
    {
        /// <summary>
        /// Gets the <see cref="System.String"/> path of the given <see cref="ErrorControlSystem.CacheErrors.StoragePath"/> path.
        /// </summary>
        /// <param name="path">The <see cref="ErrorControlSystem.CacheErrors.StoragePath"/> path.</param>
        /// <returns>Converted path + app caching folders</returns>
        public static String GetPath(StoragePath path)
        {
            string dataDir;

            switch (path)
            {
                case StoragePath.LocalApplicationData: // LocalApplicationData: "C:\Users\[UserName]\AppData\Local"
                    dataDir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                    break;

                case StoragePath.InternetCache:
                    dataDir = Environment.GetFolderPath(Environment.SpecialFolder.InternetCache);
                    break;

                case StoragePath.CurrentDirectory: // CurrentDirectory: "App Executable File Path\"
                    dataDir = Environment.CurrentDirectory;
                    break;

                case StoragePath.WindowsTemp: // WindowsTemp: "App Executable File Path\"
                    dataDir = Path.GetTempPath();
                    break;

                case StoragePath.Custom:
                    dataDir = ErrorHandlingOption.CustomStoragePath;
                    break;

                default: // default value is LocalApplicationData
                    dataDir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                    break;
            }


            // Application Name and Major Version 
            var appNameVer = String.Format(Connection.GetRunningAppNameVersion());

            // Storage Path LocalApplicationData\[AppName] v[AppMajorVersion]\
            return Path.Combine(dataDir, appNameVer);
        }

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
    }
}
