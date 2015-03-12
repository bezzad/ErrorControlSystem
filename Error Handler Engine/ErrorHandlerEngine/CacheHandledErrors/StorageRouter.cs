using System;
using System.IO;
using ErrorHandlerEngine.ModelObjecting;

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

        private static async void LoadLogPath()
        {
            ErrorLogFilePath = DiskHelper.ReadSetting("ErrorLogPath");

            CheckLogPath();

            await SdfFileManager.CreateSdfAsync(ErrorLogFilePath);
        }

        private static void CheckLogPath()
        {
            if (!string.IsNullOrEmpty(ErrorLogFilePath) && File.Exists(ErrorLogFilePath)) return; // That path's is correct.

            // LocalApplicationData: "C:\Users\[UserName]\AppData\Local"
            var appDataDir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

            // Application Name and Major Version 
            var appNameVer = String.Format(ConnectionsManager.Connection.GetRunningAppNameVersion());

            // Storage Path LocalApplicationData\[AppName] v[AppMajorVersion]\
            var storageDirPath = Path.Combine(appDataDir, appNameVer);

            var dir = Directory.CreateDirectory(storageDirPath);
            dir.Attributes = FileAttributes.Directory;

            ErrorLogFilePath = Path.Combine(storageDirPath, "Errors.log");
        }

        private static async void RegisterErrorPathsAsync()
        {
            // Add Error data path to [ErrorLogPath] of setting file:
            await DiskHelper.WriteSettingAsync("ErrorLogPath", ErrorLogFilePath);
        }
        
        #endregion

    }
}
