using System;

namespace Zaaby.Core.Infrastructure.EventBus
{
    public class MessageVersionAttribute : Attribute
    {
        public MessageVersionAttribute(string version)
        {
            Version = version;
        }

        public string Version { get; }
    }
}