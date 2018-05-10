using System;
using System.Collections.Generic;

namespace Config2Json
{
    public class Constants
    {
        public static ISet<string> SupportedExtensions { get; } = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            ".config",
        };

        public const string ExtendedHelpText = @"
Performs basic migration of an xml .config file to 
a JSON file. Uses the 'key' value as the key, and the
'value' as the value. Can optionally replace a given
character with the section marker (':').
";
    }
}
