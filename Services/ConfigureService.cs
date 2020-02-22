using Services.Configs;
using Topshelf;

namespace Services
{
    public class ConfigureService
    {
        public static void Configure(Config cfg)
        {
            HostFactory.Run(configure =>
            {
                configure.Service<ServiceWS>(service =>
                {
                    service.ConstructUsing(s => new ServiceWS());
                    service.WhenStarted(s => s.Start(cfg));
                    service.WhenStopped(s => s.Stop());
                });

                configure.RunAsLocalSystem();
                configure.SetServiceName("Scanner_File");
                configure.SetDisplayName("Scanner_File");
                configure.SetDescription("Serviço de monitoramento de arquivos");
            });
        }
    }
}
