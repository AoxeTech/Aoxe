﻿using Zaaby;

namespace ShippingHost
{
    class Program
    {
        static void Main(string[] args)
        {
            ZaabyServer.GetInstance().UseDynamicProxy(null).Run();
        }
    }
}