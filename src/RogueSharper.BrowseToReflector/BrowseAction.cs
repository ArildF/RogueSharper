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

            bool succeeded = TryBrowseElement(declared, reflector);

            nextExecute();
        }

        private static bool TryBrowseElement(IDeclaredElement declared, ReflectorFacade reflector)
        {
            var finder = new ElementFinder();
            Element element = finder.FindElement(declared);

            if (element != Element.NotFound)
            {
                reflector.Browse(element.AssemblyFile, element.TypeName, element.MemberName);

                return true;
            }

            return false;
        }
    }
}
