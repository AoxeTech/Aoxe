using System;

namespace Zaaby.MessageHub.RabbitMQ
{
    public class MessageHubConfig
    {
        public Type MessageHandlerInterfaceType { get; set; }
        public Type MessageInterfaceType { get; set; }
        public string HandleName { get; set; }
        public ushort Prefetch { get; set; }
    }
}