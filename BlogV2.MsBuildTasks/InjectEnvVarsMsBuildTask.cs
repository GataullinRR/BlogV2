using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
//using System.Threading.Tasks;
using System.Security;
using System.Collections;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System.Text.RegularExpressions;
using System.Text;

namespace BlogV2.MsBuildTasks
{
    public class InjectEnvVarsMsBuildTask : Task
    {
        public static Regex ConfigMatcher = new Regex("appsettings.*\\.json", RegexOptions.Compiled);

        public override bool Execute()
        {
            Print("Starting configs rewrite task...");

            var directory = Directory.GetCurrentDirectory();
            Print("Base directory: " + directory);
            var entries = Environment
                .GetEnvironmentVariables()
                .OfType<DictionaryEntry>()
                .Select(e => new KeyValuePair<string, string>("${" + e.Key + "}", e.Value.ToString()));
            var variablesInserted = 0;
            foreach (var configPath in ListConfigs(directory))
            {
                Print($"Found config file: {configPath}");

                var config = File.ReadAllText(configPath);
                foreach (var entry in entries)
                {
                    if (config.Contains(entry.Key))
                    {
                        Print($"Replacing {entry.Key} with {entry.Value}");
                        config = config.Replace(entry.Key, entry.Value);
                        variablesInserted++;
                    }
                }
                File.WriteAllText(configPath, config, Encoding.UTF8);
            }

            Print($"Task finished with {variablesInserted} variables inserted");

            return false;
        }

        private IEnumerable<string> ListConfigs(string root)
        {
            var parts = root.Split(Path.DirectorySeparatorChar);
            if (parts.Last() == "wwwroot" && parts.Contains("bin"))
            {
                foreach (var file in Directory.GetFiles(root).Select(f => Path.GetFileName(f)))
                {
                    if (ConfigMatcher.IsMatch(file))
                    {
                        var path = Path.Combine(root, file);

                        yield return path;
                    }
                }
            }

            foreach (var innerDir in Directory.GetDirectories(root))
            {
                foreach (var configPath in ListConfigs(innerDir))
                {
                    yield return configPath;
                }
            }
        }

        private void Print(string message)
        {
            // By default, MSBuild logs at minimal verbosity which will prevent these messages from being seen.
            Log.LogMessage(MessageImportance.High, message);
        }
    }
}
