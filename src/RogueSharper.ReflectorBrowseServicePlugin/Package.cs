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

            this._serviceHost = new ServiceHost(reflectorBrowseService);
            this._serviceHost.AddServiceEndpoint(typeof (IReflectorBrowseService),
                                            new NetNamedPipeBinding(NetNamedPipeSecurityMode.None),
                                            "net.pipe://localhost/ReflectorBrowseService");

#if DEBUG
            ServiceMetadataBehavior behavior = new ServiceMetadataBehavior();
            behavior.HttpGetEnabled = true;
            behavior.HttpGetUrl = new Uri("http://localhost:9999/ReflectorBrowseServiceMetadata");

            //this._serviceHost.Description.Behaviors.Add(behavior);
#endif

            try
            {
                this._serviceHost.Open();
            }
            catch (AddressAlreadyInUseException)
            {
                // someone else running with this plugin, not much we can do
            }
        }

        public void Unload()
        {
            if (this._serviceHost.State == CommunicationState.Opened)
            {
                this._serviceHost.Close();
            }
            else
            {
                this._serviceHost.Abort();
            }
            
        }
    }
}