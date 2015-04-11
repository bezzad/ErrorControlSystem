using ErrorControlSystem.Properties;

namespace ErrorControlSystem.CacheErrors
{
    using System.IO;

    using ErrorControlSystem.Shared;

    /// <summary>
    /// Routing Where the data must be saved
    /// </summary>
    internal static class StorageRouter
    {
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
            if (File.Exists(ErrorHandlingOption.ErrorLogPath))
            {
                SqlCompactEditionManager.CheckSdf(ErrorHandlingOption.ErrorLogPath);
            }
            else
            {
                await SqlCompactEditionManager.CreateSdfAsync(ErrorHandlingOption.ErrorLogPath);
            }
        }

        private static async void LoadLogPath()
        {
            ErrorHandlingOption.ErrorLogPath = Settings.Default.ErrorLogPath;

            CheckLogPath();

            await SqlCompactEditionManager.CreateSdfAsync(ErrorHandlingOption.ErrorLogPath);
        }

        private static void CheckLogPath()
        {
            if (!string.IsNullOrEmpty(ErrorHandlingOption.ErrorLogPath) && File.Exists(ErrorHandlingOption.ErrorLogPath)) return; // That path's is correct.

            // Storage Path\[AppName] v[AppMajorVersion]\
            var storageDirPath = StoragePathBuilder.GetPath(ErrorHandlingOption.StoragePath);

            var dir = Directory.CreateDirectory(storageDirPath);
            dir.Attributes = FileAttributes.Directory;

            ErrorHandlingOption.ErrorLogPath = Path.Combine(storageDirPath, "Errors.log");
        }

        private static async void RegisterErrorPathsAsync()
        {
            // Add Error data path to [ErrorLogPath] of setting file:
            await ErrorHandlingOption.WriteSettingAsync("ErrorLogPath", ErrorHandlingOption.ErrorLogPath);
        }

        #endregion

    }
}
