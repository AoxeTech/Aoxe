using Consul;

namespace Zaaby.Consul
{
    public class ZaabeeConsulOptions
    {
        public string ConsulAddress { get; set; }
        public AgentServiceRegistration AgentServiceRegistration { get; set; }
    }
}