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
            bool instance;
            var type = declared.GetTypeElement(out instance);
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

    internal class Element
    {
        public static readonly Element NotFound = new Element("", "", "");

        public string AssemblyFile { get; private set; }
        public string TypeName { get; private set; }
        public string MemberName { get; private set; }

        public Element(string assemblyFile, string typeName, string memberName)
        {
            AssemblyFile = assemblyFile;
            TypeName = typeName;
            MemberName = memberName;
        }
    }
}
