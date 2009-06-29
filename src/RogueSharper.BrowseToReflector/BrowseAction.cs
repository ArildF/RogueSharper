using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using JetBrains.ActionManagement;
using JetBrains.ReSharper.Psi;
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
            ReflectorFacade reflector = new ReflectorFacade();

            var declared =
                context.GetData(
                    JetBrains.ReSharper.Psi.Services.DataConstants.
                        DECLARED_ELEMENT);

            bool succeeded = TryFindElement(declared, reflector);


            //reflector.Browse(
            //    this.GetType().Assembly.ManifestModule.FullyQualifiedName,
            //    this.GetType().FullName,
            //    "Execute");

            nextExecute();
        }

        private static bool TryFindElement(IDeclaredElement declared, ReflectorFacade reflector)
        {
            bool instance;
            var type = declared.GetTypeElement(out instance);
            if (type != null)
            {
                var assemblyFile = type.GetAssemblyFile();

                if (assemblyFile != null)
                {
                    reflector.Browse(assemblyFile, type.CLRName, "");

                    return true;
                }
            }


            var member = declared.GetTypeMember();

            if (member != null)
            {
                var file = member.GetAssemblyFile();

                if (file != null)
                {
                    reflector.Browse(
                        file,
                        member.GetContainingType().CLRName,
                        member.ShortName
                        );

                    return true;
                }
                
            }

            return false;
        }
    }
}
