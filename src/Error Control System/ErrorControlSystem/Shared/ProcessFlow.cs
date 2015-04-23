namespace ErrorControlSystem.Shared
{
    public enum ProcessFlow
    {
        /// <summary>
        /// This will handle all unhandled exceptions to be able to continue execution.
        /// </summary>
        Continue,

        /// <summary>
        /// This will handle all unhandled exceptions and exit the application.
        /// </summary>
        Exit,
    }
}
