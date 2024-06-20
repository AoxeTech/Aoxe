namespace Aoxe.ServiceRegistration.Consul;

public class AoxeConsulOptions
{
    public Uri ConsulAddress { get; set; }
    public string Datacenter { get; set; }
    public string Token { get; set; }
    public TimeSpan? WaitTime { get; set; }
    public AgentServiceRegistration AgentServiceRegistration { get; set; }
}
