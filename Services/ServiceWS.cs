using Services.Configs;

namespace Services
{
    class ServiceWS
    {
        public void Start(Config cfg)
        {
            Scanner.Init(cfg);
        }
        public void Stop()
        {

        }
    }
}
