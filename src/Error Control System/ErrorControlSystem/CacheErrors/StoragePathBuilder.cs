namespace ErrorControlSystem.CacheErrors
{
    using System;
    using System.IO;

    using ErrorControlSystem.DbConnectionManager;


    public static class StoragePathBuilder
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


    }
}
