using System;
using McMaster.Extensions.CommandLineUtils;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.ConfigFile;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Config2Json
{
    public class FileMigrator
    {
        public IEnumerable<string> FilesToSquash { get; }
        public string SectionDelimiter { get; }
        public string Prefix { get; }
        public IConsole Console { get; }

        public FileMigrator(IEnumerable<string> filesToSquash, IConsole console, string sectionDelimiter, string prefix)
        {
            FilesToSquash = filesToSquash;
            Console = console;
            SectionDelimiter = sectionDelimiter;
            Prefix = prefix;
        }

        public async Task MigrateFiles()
        {
            // migrate all sequentially

            foreach (var file in FilesToSquash.Where(file => Constants.SupportedExtensions.Contains(Path.GetExtension(file))))
            {
                await MigrateFile(file);
            }
        }

        async Task MigrateFile(string file)
        {
            var fileName = Path.GetFileName(file);
            try
            {
                //based on https://github.com/aspnet/Entropy/tree/7c027069b715a4b2ffd126f58def04c6111925c3/samples/Config.CustomConfigurationProviders.Sample
                Console.WriteLine($"Migrating {fileName}...");

                var parsersToUse = new List<IConfigurationParser> {
                    new KeyValueParser(),
                    new KeyValueParser("name", "connectionString")
                };

                var provider = new ConfigFileConfigurationProvider(file, loadFromFile: true, optional: false, Console, parsersToUse);
                provider.Load();
                var keyValues = provider.GetFullKeyNames(null, new HashSet<string>())
                    .Select(key =>
                    {
                        provider.TryGet(key, out var value);

                        var newKey = string.IsNullOrEmpty(SectionDelimiter)
                            ? key
                            : key.Replace(SectionDelimiter, ":", StringComparison.OrdinalIgnoreCase);
                        return new KeyValuePair<string, string>(newKey, value);
                    });

                var config = new ConfigurationBuilder()
                    .AddInMemoryCollection(keyValues)
                    .Build();

                var jsonObject = GetConfigAsJObject(config);

                if (!string.IsNullOrEmpty(Prefix))
                {
                    jsonObject = new JObject { { Prefix, jsonObject } };
                }

                //write to file 
                var newPath = Path.ChangeExtension(file, "json");
                var contents = JsonConvert.SerializeObject(jsonObject, Formatting.Indented);
                await File.WriteAllTextAsync(newPath, contents);

                Console.WriteLine($"Migration of {fileName} to {Path.GetFileName(newPath)} complete");
            }
            catch (System.Exception ex)
            {
                Console.WriteLine($"An error occurred migrating {fileName}: ");
                Console.WriteLine(ex);
            }
        }

        static JObject GetConfigAsJObject(IConfiguration config)
        {
            var root = new JObject();

            foreach (var child in config.GetChildren())
            {
                //not strictly correct, but we'll go with it.
                var isSection = child.Value == null;
                if (isSection)
                {
                    root.Add(child.Key, GetConfigAsJObject(child));
                }
                else
                {
                    root.Add(child.Key, child.Value);
                }
            }

            return root;
        }
    }
}
