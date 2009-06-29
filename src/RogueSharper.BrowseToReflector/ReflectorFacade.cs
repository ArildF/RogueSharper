using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using RogueSharper.BrowseToReflector.Reflector;

namespace RogueSharper.BrowseToReflector
{
    class ReflectorFacade
    {
        public void Browse(string assemblyFile, string typeName, string memberName)
        {
            var client = CreateClient();

            client.Browse(assemblyFile, typeName, memberName);
        }

        private static ReflectorBrowseServiceClient CreateClient()
        {
            var binding = new NetNamedPipeBinding(NetNamedPipeSecurityMode.None);

            var endpoint =
                new EndpointAddress(
                    "net.pipe://localhost/ReflectorBrowseService");

            return
               new ReflectorBrowseServiceClient(binding, endpoint);
        }
    }
}
