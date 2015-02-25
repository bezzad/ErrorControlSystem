using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using ErrorHandlerEngine.Properties;
using ErrorHandlerEngine.ServerUploader;

namespace ErrorHandlerEngine.CacheHandledErrors
{
    /// <summary>
    /// Routing Where the data must be saved
    /// </summary>
    public static class StorageRouter
    {
        #region Properties

        public static string ErrorLogFilePath;

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

            CheckLogPath();

            SdfFileManager.CreateSdf(ErrorLogFilePath);
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
        }

        private static async void RegisterErrorPathsAsync()
        {
            // Add Error data path to [ErrorLogPath] of setting file:
            await WriteSettingAsync("ErrorLogPath", ErrorLogFilePath);
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
        
        #endregion

    }
}
