using Newtonsoft.Json;
using Services;
using Services.Configs;
using System.IO;

namespace Scanner
{
    class Program
    {
        static void Main(string[] args)
        {
            var cfg = JsonConvert.DeserializeObject<Config>(File.ReadAllText("settings.json"));
            ConfigureService.Configure(cfg);
        }
    }
}
