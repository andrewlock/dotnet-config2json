using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace Config2Json
{
    internal static class ConfigurationProviderExtensions
    {
        public static HashSet<string> GetFullKeyNames(this IConfigurationProvider provider, string rootKey, HashSet<string> initialKeys)
        {
            foreach (var key in provider.GetChildKeys(Enumerable.Empty<string>(), rootKey))
            {
                string surrogateKey = key;
                if (rootKey != null)
                {
                    surrogateKey = rootKey + ":" + key;
                }

                GetFullKeyNames(provider, surrogateKey, initialKeys);

                if (!initialKeys.Any(k => k.StartsWith(surrogateKey)))
                {
                    initialKeys.Add(surrogateKey);
                }
            }

            return initialKeys;
        }
    }
}
