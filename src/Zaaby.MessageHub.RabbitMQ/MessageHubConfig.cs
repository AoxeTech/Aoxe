using System;

namespace Zaaby.MessageHub.RabbitMQ
{
    public class MessageHubConfig
    {
        public Type MessageHandlerInterfaceType { get; set; }
        public Type MessageInterfaceType { get; set; }
        public string HandleName { get; set; }

        private ushort _prefetch = 50;

        public ushort Prefetch
        {
            get => _prefetch;
            set => _prefetch = value == 0 ? _prefetch : value;
        }
    }
}