namespace ErrorControlSystem.CacheErrors
{
    public enum StoragePaths
    {
        /// <summary>
        /// %Temp% directory is the default storage. Usually set to 'C:\Documents and Settings\Username\AppData\Local\AppName AppVersion'.
        /// </summary>
        LocalApplicationData,

        /// <summary>
        /// %Temp% directory is the default storage. Usually set to 'C:\Documents and Settings\Username\Local Settings\Temp\AppName AppVersion'.
        /// </summary>
        WindowsTemp,

        /// <summary>
        /// Initial working directory, i.e. where the executing assembly (MyProduct.exe) is located.
        /// </summary>
        CurrentDirectory,

        /// <summary>
        /// Custom path should be a full path like 'C:\Documents and Settings\MyUser\Local Settings\Temp'.
        /// </summary>
        /// <remarks>Path should not have a trailing slash. If the directory doesn't exist, it is created first.</remarks>
        Custom,

        /// <summary>
        /// The internet cache path
        /// </summary>
        InternetCache
    }
}
