using System;
using System.ServiceModel;
using System.Threading;

namespace RogueSharper.ReflectorBrowseServicePlugin
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ReflectorBrowseService : IReflectorBrowseService
    {
        private readonly IReflector _reflector;
        private readonly SynchronizationContext _synchronizationContext;

        public ReflectorBrowseService(IReflector reflector)
        {
            this._reflector = reflector;
            _synchronizationContext = SynchronizationContext.Current;
        }

        public void Ping()
        {
            // nothing
        }

        public void Browse(string assembly, string type, string member)
        {
            _synchronizationContext.Post(_ => 
                this._reflector.Browse(assembly, type, member), null);
        }
    }
}