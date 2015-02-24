using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ErrorHandlerEngine.ExceptionManager;
using ErrorHandlerEngine.ModelObjecting;
using ErrorHandlerEngine.Properties;

namespace ErrorHandlerEngine.CacheHandledErrors
{
    /// <summary>
    /// Routing Where the data must be saved
    /// </summary>
    public static class StorageRouter
    {
        #region Properties

        public static string ErrorLogFilePath;
        public static string SnapshotImagesPath;

        #endregion

        #region Constructor

        static StorageRouter()
        {
            // Load Saved Path
            LoadLogPath();

            // Save Paths
            RegisterErrorPathsAsync();
        }

        #endregion

        #region Methods

        private static void LoadLogPath()
        {
            ErrorLogFilePath = ReadSetting("ErrorLogPath");
            SnapshotImagesPath = ReadSetting("SnapshotsPath");

            CheckLogPath();
            CheckSnapshotsPath();
        }

        private static void CheckLogPath()
        {
            if (!string.IsNullOrEmpty(ErrorLogFilePath) && File.Exists(ErrorLogFilePath)) return; // That path's is correct.

            // LocalApplicationData: "C:\Users\[UserName]\AppData\Local"
            var appDataDir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

            // Application Name and Major Version 
            var appNameVer = String.Format("{0} v{1}",
                AppDomain.CurrentDomain.FriendlyName.Substring(0, AppDomain.CurrentDomain.FriendlyName.IndexOf('.')),
                Version.Parse(Application.ProductVersion).Major);

            // Storage Path LocalApplicationData\[AppName] v[AppMajorVersion]\
            var storageDirPath = Path.Combine(appDataDir, appNameVer);

            var dir = Directory.CreateDirectory(storageDirPath);
            dir.Attributes = FileAttributes.Directory;

            ErrorLogFilePath = Path.Combine(storageDirPath, "Errors.log");

            var file = File.Create(ErrorLogFilePath);
            file.Close();
            file.Dispose();
        }

        private static void CheckSnapshotsPath()
        {
            if (!string.IsNullOrEmpty(SnapshotImagesPath) && Directory.Exists(SnapshotImagesPath)) return;

            SnapshotImagesPath = Path.Combine(
                    ErrorLogFilePath.Substring(0, ErrorLogFilePath.LastIndexOf(@"\", StringComparison.OrdinalIgnoreCase)),
                    "Snapshots");

            var dir = Directory.CreateDirectory(SnapshotImagesPath);
            dir.Attributes = FileAttributes.Directory;
        }

        private static async void RegisterErrorPathsAsync()
        {
            // Add Error data path to [ErrorLogPath] of setting file:
            await WriteSettingAsync("ErrorLogPath", ErrorLogFilePath);
            //
            // Add Error Snapshots path to [SnapshotsPath] of setting file:
            await WriteSettingAsync("SnapshotsPath", SnapshotImagesPath);
        }

        public static async Task<bool> WriteSettingAsync(string key, string value, bool attach = false)
        {
            return await Task.Run(() =>
            {
                try
                {
                    Settings.Default[key] = (attach ? (ReadSetting(key) ?? "") : "") + value;

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

        public static void WriteOnDisk(string filePath, string text)
        {
            try
            {
                ExpHandlerEngine.IsSelfException = true;

                var encodedText = Encoding.Unicode.GetBytes(text);

                using (var sourceStream = new FileStream(filePath,
                    FileMode.Create, FileAccess.Write, FileShare.ReadWrite,
                    bufferSize: 4096, useAsync: true))
                {

                    sourceStream.Write(encodedText, 0, encodedText.Length);

                    sourceStream.Close();
                }
            }
            finally
            {
                ExpHandlerEngine.IsSelfException = false;
            }
        }

        public static async Task<string> ReadFromDiskAsync(string filePath)
        {
            if (!File.Exists(filePath)) return "";

            using (var sourceStream = new FileStream(filePath,
                FileMode.Open, FileAccess.Read, FileShare.ReadWrite,
                bufferSize: 4096, useAsync: true))
            {
                var sb = new StringBuilder();

                var buffer = new byte[0x1000];
                int numRead;
                while ((numRead = await sourceStream.ReadAsync(buffer, 0, buffer.Length)) != 0)
                {
                    var text = Encoding.Unicode.GetString(buffer, 0, numRead);
                    sb.Append(text);
                }

                sourceStream.Close();

                return sb.ToString();
            }
        }

        public static void WriteLog(string text)
        {
            WriteOnDisk(ErrorLogFilePath, text);
        }

        public static async Task<string> ReadLogAsync()
        {
            return await ReadFromDiskAsync(ErrorLogFilePath);
        }

        public static async Task<string> SaveSnapshotImageOnDiskAsync(Error error)
        {
            return await Task.Run(() =>
            {
                var path = Path.Combine(SnapshotImagesPath, string.Format("ScreenCapture {0}.png", error.Id));

                if (!File.Exists(path) && error.GetSnapshot() != null)
                {
                    using (var img = error.GetSnapshot())
                    {
                        img.Save(path);
                    }
                }

                return path;
            });
        }

        public static async Task DeleteSnapshotImageOnDiskAsync(string imgAddress)
        {
            await Task.Run(() =>
            {
                try
                {
                    ExpHandlerEngine.IsSelfException = true;
                    if (File.Exists(imgAddress))
                        File.Delete(imgAddress);
                }
                finally { ExpHandlerEngine.IsSelfException = false; }
            });

        }


        #endregion

    }
}
