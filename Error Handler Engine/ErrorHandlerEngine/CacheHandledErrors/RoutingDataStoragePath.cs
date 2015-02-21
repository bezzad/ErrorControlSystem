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
    public static class RoutingDataStoragePath
    {
        #region Properties

        public static string ErrorLogFilePath;
        public static string SnapshotImagesPath;

        #endregion

        #region Constructor

        static RoutingDataStoragePath()
        {
            var pathsChanged = false;

            #region Load Saved Path

            ErrorLogFilePath = ReadFromSetting("ErrorLogPath");
            SnapshotImagesPath = ReadFromSetting("SnapshotsPath");

            #endregion

            #region Check Error Log Path

            if (string.IsNullOrEmpty(ErrorLogFilePath) || !File.Exists(ErrorLogFilePath))
            {
                // LocalApplicationData: "C:\Users\[UserName]\AppData\Local"
                var appDataDir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

                // Application Name and Major Version 
                var appNameVer = String.Format("{0} v{1}",
                    AppDomain.CurrentDomain.FriendlyName.Substring(0,
                    AppDomain.CurrentDomain.FriendlyName.IndexOf('.')),
                    Version.Parse(Application.ProductVersion).Major);

                // Storage Path LocalApplicationData\[AppName] v[AppMajorVersion]\
                var storageDirPath = Path.Combine(appDataDir, appNameVer);

                // Check Directory Existence:
                if (!Directory.Exists(Path.Combine(storageDirPath, "Snapshots")))
                {
                    var dir = Directory.CreateDirectory(storageDirPath);
                    dir.CreateSubdirectory("Snapshots");
                    dir.Attributes = FileAttributes.Directory;
                }

                ErrorLogFilePath = Path.Combine(storageDirPath, "Errors.log");

                pathsChanged = true;
            }

            #endregion

            #region Check Snapshots Path

            if (string.IsNullOrEmpty(SnapshotImagesPath) || !Directory.Exists(SnapshotImagesPath))
            {
                SnapshotImagesPath =
                    Path.Combine(
                        ErrorLogFilePath.Substring(0,
                            ErrorLogFilePath.LastIndexOf(@"\", StringComparison.OrdinalIgnoreCase)), "Snapshots");

                // Check Directory Existence:
                if (!Directory.Exists(SnapshotImagesPath))
                {
                    var dir = Directory.CreateDirectory(SnapshotImagesPath);
                    dir.Attributes = FileAttributes.Directory;
                }

                pathsChanged = true;
            }

            #endregion

            #region Save Paths

            if (pathsChanged)
            {
                RegisterErrorPathsAsync();
            }

            #endregion
        }

        #endregion

        #region Methods

        private static async void RegisterErrorPathsAsync()
        {
            // Add Error data path to [ErrorLogPath] of setting file:
            await WriteToSettingAsync("ErrorLogPath", ErrorLogFilePath);
            //
            // Add Error Snapshots path to [SnapshotsPath] of setting file:
            await WriteToSettingAsync("SnapshotsPath", SnapshotImagesPath);
        }

        public static async Task<bool> WriteToSettingAsync(string key, string value, bool attach = false)
        {
            return await Task.Run(() =>
            {
                try
                {
                    Settings.Default[key] = (attach ? (ReadFromSetting(key) ?? "") : "") + value;

                    Settings.Default.Save();

                    return true;
                }
                catch
                {
                    return false;
                }
            });
        }

        public static string ReadFromSetting(string key)
        {
            try { return (string)Settings.Default[key]; }
            catch { return ""; }
        }

        public static async Task WriteTextToDiskAsync(string filePath, string text)
        {
            try
            {
                HandleExceptions.IsSelfException = true;

                var encodedText = Encoding.Unicode.GetBytes(text);

                using (var sourceStream = new FileStream(filePath,
                    FileMode.Create, FileAccess.Write, FileShare.ReadWrite,
                    bufferSize: 4096, useAsync: true))
                {

                    await sourceStream.WriteAsync(encodedText, 0, encodedText.Length);

                    sourceStream.Close();
                }
            }
            finally
            {
                HandleExceptions.IsSelfException = false;
            }
        }

        public static async Task<string> ReadTextFromDiskAsync(string filePath)
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

        public static async Task WriteTextAsync(string text)
        {
            await WriteTextToDiskAsync(ErrorLogFilePath, text);
        }

        public static async Task<string> ReadLogAsync()
        {
            return await ReadTextFromDiskAsync(ErrorLogFilePath);
        }

        public static async Task<string> SaveSnapshotImageOnDiskAsync(Error error)
        {
            return await Task.Run(() =>
            {
                var path = Path.Combine(SnapshotImagesPath, string.Format("ScreenCapture_{0}.png", error.Id));

                if (!File.Exists(path))
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
                    HandleExceptions.IsSelfException = true;
                    if (File.Exists(imgAddress))
                        File.Delete(imgAddress);
                }
                finally { HandleExceptions.IsSelfException = false; }
            });

        }

        public static void WriteTextToLog(string text)
        {
            try
            {
                var encodedText = Encoding.Unicode.GetBytes(text);

                using (var sourceStream = new FileStream(ErrorLogFilePath,
                    FileMode.Create, FileAccess.Write, FileShare.ReadWrite,
                    bufferSize: 4096, useAsync: true))
                {
                    sourceStream.Write(encodedText, 0, encodedText.Length);

                    sourceStream.Close();
                }
            }
            catch (IOException exp)
            {
                MessageBox.Show(exp.Message, "Can not to Write Text");
            }
        }
        #endregion

    }
}
