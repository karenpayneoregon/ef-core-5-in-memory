using System;
using System.IO;

namespace EntityFrameworkCoreDbExtensions.Classes.QuestionAnswer
{

    //var configuration = ConfigurationOperations.ReadConfiguration();
    //configuration.ConnectionStrings.DevConnection = "Server=.\\sqlexpress;Database=North;Trusted_Connection=True;MultipleActiveResultSets=True;";
    //ConfigurationOperations.SaveChanges(configuration);
    public class ConfigurationOperations
    {
        private static string FileName => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json");
        public static Configuration ReadConfiguration()
        {
            var json = File.ReadAllText(FileName);
            return json.JSonToItem<Configuration>();
        }

        public static void SaveChanges(Configuration configuration)
        {
            configuration.JsonToFile(FileName);
        }
    }


    public class Configuration
    {
        public Logging Logging { get; set; }
        public string AllowedHosts { get; set; }
        public Connectionstrings ConnectionStrings { get; set; }
    }

    public class Logging
    {
        public Loglevel LogLevel { get; set; }
    }

    public class Loglevel
    {
        public string Default { get; set; }
    }

    public class Connectionstrings
    {
        public string DevConnection { get; set; }
    }

}
