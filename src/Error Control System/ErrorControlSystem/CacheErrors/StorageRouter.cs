using System;
using System.IO;
using ErrorControlSystem.DbConnectionManager;
using ErrorControlSystem.Shared;

namespace ErrorControlSystem.CacheErrors
{
    /// <summary>
    /// Routing Where the data must be saved
    /// </summary>
    internal static class StorageRouter
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

        public static async void ReadyCache()
        {
            if (File.Exists(ErrorLogFilePath))
            {
                SqlCompactEditionManager.CheckSdf(ErrorLogFilePath);
            }
            else
            {
                await SqlCompactEditionManager.CreateSdfAsync(ErrorLogFilePath);
            }
        }

        private static async void LoadLogPath()
        {
            ErrorLogFilePath = DiskHelper.ReadSetting("ErrorLogPath");

            CheckLogPath();

            await SqlCompactEditionManager.CreateSdfAsync(ErrorLogFilePath);
        }

        private static void CheckLogPath()
        {
            if (!string.IsNullOrEmpty(ErrorLogFilePath) && File.Exists(ErrorLogFilePath)) return; // That path's is correct.

            // Storage Path\[AppName] v[AppMajorVersion]\
            var storageDirPath = StoragePathBuilder.GetPath(ErrorHandlingOption.StoragePath);

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
