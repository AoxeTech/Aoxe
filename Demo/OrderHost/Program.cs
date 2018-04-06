using Zaaby;

namespace OrderHost
{
    class Program
    {
        static void Main(string[] args)
        {
            ZaabyServer.GetInstance().UseDynamicProxy(null).Run();
        }
    }
}