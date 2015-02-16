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
    public class RoutingDataStoragePath
    {
        #region Properties

        public string ErrorLogFilePath;
        public string SnapshotImagesPath;

        #endregion

        #region Constructor

        public RoutingDataStoragePath()
        {
            var pathsChanged = false;

            #region Load Saved Path

            ErrorLogFilePath = (string)ReadFromSetting("ErrorLogPath");
            SnapshotImagesPath = (string)ReadFromSetting("SnapshotsPath");

            #endregion

            #region Check Error Log Path

            if (string.IsNullOrEmpty(ErrorLogFilePath) || !File.Exists(ErrorLogFilePath))
            {
                // LocalApplicationData: "C:\Users\[UserName]\AppData\Local"
                var appDataDir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

                // Application Name and Version 
                var appNameVer = String.Format("{0} v{1}",
                    AppDomain.CurrentDomain.FriendlyName.Substring(0, AppDomain.CurrentDomain.FriendlyName.IndexOf('.')),
                    Application.ProductVersion);

                // Storage Path LocalApplicationData\[AppName] v[AppVersion]\
                var storageDirPath = Path.Combine(appDataDir, appNameVer);

                // Check Directory Existence:
                if (!Directory.Exists(Path.Combine(storageDirPath, "Snapshots")))
                {
                    var dir = Directory.CreateDirectory(storageDirPath);
                    dir.CreateSubdirectory("Snapshots");
                    dir.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
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
                    dir.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
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

        private async void RegisterErrorPathsAsync()
        {
            // Add Error data path to [ErrorLogPath] of setting file:
            await WriteToSettingAsync("ErrorLogPath", ErrorLogFilePath);
            //
            // Add Error Snapshots path to [SnapshotsPath] of setting file:
            await WriteToSettingAsync("SnapshotsPath", SnapshotImagesPath);
        }

        public async Task<bool> WriteToSettingAsync(string key, string value, bool attach = false)
        {
            return await Task.Run(() =>
            {
                var successWrite = false;
                try
                {
                    Settings.Default[key] = (attach ? ((string)ReadFromSetting(key) ?? "") : "") + value;

                    Settings.Default.Save();

                    successWrite = true;
                }
                catch (Exception)
                {
                    successWrite = false;
                }

                return successWrite;
            });
        }

        public object ReadFromSetting(string key)
        {
            object result;
            try
            {
                result = Settings.Default[key];
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }

            return result;
        }

        public async Task WriteTextToDiskAsync(string filePath, string text)
        {
            try
            {
                Kernel.IsSelfException = true;

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
                Kernel.IsSelfException = false;
            }
        }

        public async Task<string> ReadTextFromDiskAsync(string filePath)
        {
            if (File.Exists(filePath))
            {
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

            return "";
        }

        public async Task WriteTextAsync(string text)
        {
            await WriteTextToDiskAsync(ErrorLogFilePath, text);
        }

        public async Task<string> ReadTextAsync()
        {
            return await ReadTextFromDiskAsync(ErrorLogFilePath);
        }

        public async Task<string> SaveSnapshotImageOnDiskAsync(Error error)
        {
            return await Task.Run(() =>
            {
                var path = Path.Combine(SnapshotImagesPath, string.Format("ScreenCapture_{0}.png", error.Id));

                using (var img = error.GetSnapshot())
                {
                    img.Save(path);
                }

                return path;
            });
        }

        public async Task DeleteSnapshotImageOnDiskAsync(string imgAddress)
        {

            await Task.Run(() =>
            {
                if (File.Exists(imgAddress))
                    try
                    {
                        Kernel.IsSelfException = true;
                        File.Delete(imgAddress);
                    }
                    finally { Kernel.IsSelfException = false; }
            });

        }

        public void WriteText(string text)
        {
            Kernel.IsSelfException = true;
            try
            {
                var encodedText = Encoding.Unicode.GetBytes(text);

                using (var sourceStream = new FileStream(ErrorLogFilePath,
                    FileMode.Create, FileAccess.Write , FileShare.ReadWrite,
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
            finally
            {
                Kernel.IsSelfException = false;
            }
        }
        #endregion

    }
}
