// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.



//TODO: why didn't this stuff get pulled in????
//I can fix this when 2.1 is officially released.

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;
 
namespace Microsoft.AspNetCore.Http.Connections.Features
{
    public interface IHttpTransportFeature
    {
        HttpTransportType TransportType { get; }
    }
}

namespace Microsoft.AspNetCore.Http.Connections
{
    public static class HttpTransports
    {
        // Note that this is static readonly instead of const so it is not baked into a DLL when referenced
        // Updating package without recompiling will automatically pick up new transports added here
        public static readonly HttpTransportType All = HttpTransportType.WebSockets | HttpTransportType.ServerSentEvents | HttpTransportType.LongPolling;
    }
}

namespace Microsoft.AspNetCore.Http.Connections
{
    [Flags]
    public enum HttpTransportType
    {
        None = 0,
        WebSockets = 1,
        ServerSentEvents = 2,
        LongPolling = 4,
    }
}