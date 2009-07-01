using System;
using System.ServiceModel;

namespace RogueSharper.ReflectorBrowseServicePlugin
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ReflectorBrowseService : IReflectorBrowseService
    {
        private readonly IReflector _reflector;

        public ReflectorBrowseService(IReflector reflector)
        {
            this._reflector = reflector;
        }

        public void Ping()
        {
            // nothing
        }

        public void Browse(string assembly, string type, string member)
        {
            this._reflector.Browse(assembly, type, member);
        }
    }
}