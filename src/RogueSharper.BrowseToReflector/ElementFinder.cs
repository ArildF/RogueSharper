using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.ReSharper.Psi;

namespace RogueSharper.BrowseToReflector
{
    class ElementFinder
    {
        public Element FindElement(IDeclaredElement declared)
        {
            ITypeElement type = declared.GetTypeElement();
            if (type != null)
            {
                var assemblyFile = type.GetAssemblyFile();

                if (assemblyFile != null)
                {
                    return new Element(assemblyFile, type.CLRName, "");
                }
            }

            var member = declared.GetTypeMember();

            if (member != null)
            {
                var file = member.GetAssemblyFile();

                if (file != null)
                {
                    return new Element(
                        file,
                        member.GetContainingType().CLRName,
                        member.ShortName
                        );

                }
            }


            return Element.NotFound;

        }
    }
}
