using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using JetBrains.ActionManagement;
using RogueSharper.BrowseToReflector.Reflector;

namespace RogueSharper.BrowseToReflector
{
    [ActionHandler(new string[] { "RogueSharper.BrowseToReflector.BrowseAction" })]
    class BrowseAction : IActionHandler
    {
        public bool Update(IDataContext context, ActionPresentation presentation, DelegateUpdate nextUpdate)
        {
            return true;
        }

        public void Execute(IDataContext context, DelegateExecute nextExecute)
        {
            var binding = new NetNamedPipeBinding(NetNamedPipeSecurityMode.None);

            var endpoint =
                new EndpointAddress(
                    "net.pipe://localhost/ReflectorBrowseService");

            var client =
               new ReflectorBrowseServiceClient(binding, endpoint);

            client.Browse(
                this.GetType().Assembly.ManifestModule.FullyQualifiedName,
                this.GetType().FullName,
                "Execute");

            nextExecute();
        }
    }
}
