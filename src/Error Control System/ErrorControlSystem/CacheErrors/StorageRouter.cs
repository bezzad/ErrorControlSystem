namespace ErrorControlSystem.CacheErrors
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using ErrorControlSystem.DbConnectionManager;


    /// <summary>
    /// Routing Where the data must be saved
    /// </summary>
    public static class StorageRouter
    {
        private const string LogFileName = "Errors.log";


        #region Methods

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

        /// <summary>
        /// Gets the <see cref="System.String"/> path of the given <see cref="ErrorControlSystem.CacheErrors.StoragePaths"/> path.
        /// </summary>
        /// <param name="path">The <see cref="ErrorControlSystem.CacheErrors.StoragePaths"/> path.</param>
        /// <returns>Converted path + app caching folders</returns>
        public static String GetPath(this StoragePaths path)
        {
            string dataDir;

            switch (path)
            {
                case StoragePaths.LocalApplicationData: // LocalApplicationData: "C:\Users\[UserName]\AppData\Local"
                    dataDir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                    break;

                case StoragePaths.InternetCache:
                    dataDir = Environment.GetFolderPath(Environment.SpecialFolder.InternetCache);
                    break;

                case StoragePaths.CurrentDirectory: // CurrentDirectory: "App Executable File Path\"
                    dataDir = Environment.CurrentDirectory;
                    break;

                case StoragePaths.WindowsTemp: // WindowsTemp: "App Executable File Path\"
                    dataDir = Path.GetTempPath();
                    break;

                case StoragePaths.Custom:
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
        
        internal static async void CheckLogFileHealthy(SqlCompactEditionManager sdfManager)
        {
            #region Check Log File Existance

            if (File.Exists(ErrorHandlingOption.ErrorLogPath)) // File exist, so check log file healthy
            {
                sdfManager.CheckSdf(ErrorHandlingOption.ErrorLogPath);
            }
            else // File not found, so create that ...
            {
                await sdfManager.CreateSdfAsync();
            }

            #endregion
        }

        internal static void CheckLogPathAndName()
        {
            #region Check Log File Path and Name

            //
            // Check just log file path and name
            //
            if (String.IsNullOrEmpty(ErrorHandlingOption.ErrorLogPath) ||
                !ErrorHandlingOption.ErrorLogPath.EndsWith(LogFileName))
            {
                // Storage Path\[AppName] v[AppMajorVersion]\
                var storageDirPath = GetPath(ErrorHandlingOption.StoragePath);

                var dir = Directory.CreateDirectory(storageDirPath);
                dir.Attributes = FileAttributes.Directory;

                ErrorHandlingOption.ErrorLogPath = Path.Combine(storageDirPath, LogFileName);
            }

            #endregion
        }

        #endregion
    }

}
