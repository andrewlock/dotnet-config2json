using McMaster.Extensions.CommandLineUtils;
using System;
using System.Threading.Tasks;

namespace Config2Json
{
    class Program
    {
        // Return codes
        public const int EXCEPTION = 2;
        public const int ERROR = 1;
        public const int OK = 0;

        public static async Task<int> Main(string[] args)
        {
            try
            {
                return await CommandLineApplication.ExecuteAsync<Migrator>(args);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Error.WriteLine("Unexpected error: " + ex.ToString());
                Console.ResetColor();
                return EXCEPTION;
            }
        }
    }
}
