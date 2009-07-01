using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace RogueSharper.ReflectorBrowseServicePlugin
{
    [ServiceContract]
   
    public interface IReflectorBrowseService
    {
        [OperationContract]
        void Ping();

        [OperationContract]
        void Browse(string assembly, string type, string member);
    }
}