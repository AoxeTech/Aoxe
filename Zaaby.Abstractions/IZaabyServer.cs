using System;
using System.Collections.Generic;

namespace Zaaby.Abstractions
{
    public interface IZaabyServer
    {
        IZaabyServer UseZaabyServer<TService>();

        IZaabyServer UseZaabyServer(Func<Type, bool> definition);
    }
}