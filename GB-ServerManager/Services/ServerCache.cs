using GB_ServerManager.Models;
using Microsoft.Extensions.Caching.Memory;

namespace GB_ServerManager.Services
{
    internal static class ServerCache
    {
        public static MemoryCache _ServerCache;

        public static ServerList _ServerList;
    }
}
