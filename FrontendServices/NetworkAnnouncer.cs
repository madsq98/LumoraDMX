using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Makaretu.Dns;
using Zeroconf;

namespace FrontendServices
{
    public static class NetworkAnnouncer
    {
        public const string ANNOUNCE_TYPE = "_lumoradmx._tcp";
        public const string DISCOVER_TYPE = "_lumoradmx._tcp.local.";

        private static MulticastService? _mdns;
        private static ServiceDiscovery? _serviceDiscovery;

        /// <summary>
        /// Starts advertising the service on the local network.
        /// </summary>
        /// <param name="instanceName">Instance name (e.g. "LumoraDMX")</param>
        /// <param name="serviceType">Service type (e.g. "_lumoradmx._tcp")</param>
        /// <param name="port">The port number this service listens on</param>
        public static void Announce(string instanceName, string serviceType, int port)
        {
            _mdns = new MulticastService();
            _serviceDiscovery = new ServiceDiscovery(_mdns);

            _mdns.Start();

            var service = new ServiceProfile(instanceName, serviceType, (ushort)port);
            _serviceDiscovery.Advertise(service);

            Console.WriteLine($"Now announcing {instanceName} - {serviceType} on port {port}");
        }

        /// <summary>
        /// Stops advertising the service.
        /// </summary>
        public static void StopAnnouncing()
        {
            _serviceDiscovery?.Dispose();
            _mdns?.Dispose();
        }

        /// <summary>
        /// Discovers other services of the given type on the local network.
        /// </summary>
        /// <param name="type">The type of service to search for (e.g., "_lumoradmx._tcp.local.").</param>
        /// 
        public static async Task<List<(string InstanceName, string IPAddress, int Port)>> DiscoverAsync(string serviceType)
        {
            var results = new List<(string InstanceName, string IPAddress, int Port)>();
            IReadOnlyList<IZeroconfHost> zeroConfResults = await ZeroconfResolver.ResolveAsync(serviceType, TimeSpan.FromSeconds(5), 5);
            foreach (var zeroConfResult in zeroConfResults)
            {
                var instanceName = zeroConfResult.DisplayName;
                var ipAddress = zeroConfResult.IPAddress;
                var port = zeroConfResult.Services.First().Value.Port;
                Console.WriteLine($"Found: {instanceName} - {ipAddress} : {port}");
                results.Add((instanceName, ipAddress, port));
            }
            return results;
        }
    }
}
