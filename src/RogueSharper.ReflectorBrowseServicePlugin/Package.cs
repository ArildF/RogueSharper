using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using Reflector;
using Reflector.CodeModel;

namespace RogueSharper.ReflectorBrowseServicePlugin
{
    public class Package : IPackage
    {
        private ServiceHost _serviceHost;

        public void Load(IServiceProvider serviceProvider)
        {
            var assemblyManager = serviceProvider.GetService(typeof (IAssemblyManager)) as
                          IAssemblyManager;
            var assemblyBrowser = serviceProvider.GetService(typeof (IAssemblyBrowser)) as
                          IAssemblyBrowser;
            var windowManager =
                serviceProvider.GetService(typeof (IWindowManager)) as
                IWindowManager;
            var reflector = new Reflector(assemblyManager, assemblyBrowser, windowManager);
            var reflectorBrowseService = new ReflectorBrowseService(reflector);

            this._serviceHost = new ServiceHost(reflectorBrowseService, new Uri("http://localhost:9999/ReflectorBrowseService"));
            this._serviceHost.AddServiceEndpoint(typeof (IReflectorBrowseService),
                                            new NetNamedPipeBinding(NetNamedPipeSecurityMode.None),
                                            "net.pipe://localhost/ReflectorBrowseService");

            ServiceMetadataBehavior behavior = new ServiceMetadataBehavior();
            behavior.HttpGetEnabled = true;
            behavior.HttpGetUrl = new Uri("http://localhost:9999/ReflectorBrowseServiceMetadata");

            this._serviceHost.Description.Behaviors.Add(behavior);

            this._serviceHost.Open();
        }

        public void Unload()
        {
            this._serviceHost.Close();
        }
    }
}