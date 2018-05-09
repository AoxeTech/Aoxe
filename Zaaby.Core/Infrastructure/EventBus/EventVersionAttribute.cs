using System;

namespace Zaaby.Core.Infrastructure.EventBus
{
    public class EventVersionAttribute : Attribute
    {
        public EventVersionAttribute(string version)
        {
            Version = version;
        }

        public string Version { get; }
    }
}