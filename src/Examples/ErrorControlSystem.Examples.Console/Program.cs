namespace ErrorControlSystem.Examples.Console
{
    using System;

    using ErrorControlSystem;

    class Program
    {
        static void Main(string[] args)
        {
            // Start ErrorControlSystem Engine for console applications
            ExceptionHandler.Engine.Start(".", "UsersManagements");

            //
            // Throw Test Exception
            Console.Write("Throw a System.Exception (y/n): ");

            if (Console.ReadKey().Key == ConsoleKey.Y)
            {
                throw new Exception("This is an exception thrown from console sample application.");
            }
        }
    }
}
