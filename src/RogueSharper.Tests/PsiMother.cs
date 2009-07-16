using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;
using JetBrains.Util;
using Moq;

namespace RogueSharper.Tests
{
    static class PsiMother
    {
        public static IDeclaredElement DeclaredElementAsTypeElementWithLocation(this MockFactory factory, string typeName, 
            string location)
        {
            var elt = factory.Create<IDeclaredElement>();
            var typeElement = elt.As<ITypeElement>();
            var module = factory.Create<IAssemblyPsiModule>();

            var file = factory.Create<IAssemblyFile>();

            var path = new FileSystemPath(location);

            typeElement.Setup(te => te.CLRName).Returns(typeName);

            file.Setup(f => f.Location).Returns(path);

            module.Setup(m => m.Assembly.GetFiles()).Returns(
                new[] {file.Object});

            typeElement.Setup(te => te.Module).Returns(module.Object);


            return elt.Object;
        }

        public static IDeclaredElement DeclaredElementAsTypeMember(this MockFactory factory, string typename, string member, 
            string location)
        {
            var elt = factory.Create<IDeclaredElement>();
            var typeMember = elt.As<ITypeMember>();

            var module = factory.Create<IAssemblyPsiModule>();

            var file = factory.Create<IAssemblyFile>();

            var path = new FileSystemPath(location);
            var type = factory.Create<ITypeElement>();

            typeMember.Setup(tm => tm.GetContainingType()).Returns(type.Object);
            typeMember.Setup(tm => tm.ShortName).Returns(member);

            type.Setup(t => t.CLRName).Returns(typename);
                
            file.Setup(f => f.Location).Returns(path);

            module.Setup(m => m.Assembly.GetFiles()).Returns(
                new[] { file.Object });

            typeMember.Setup(te => te.Module).Returns(module.Object);


            return elt.Object;
        }

        public static IDeclaredElement DeclaredElementAsTypeOwner(this MockFactory factory, string typename, 
           string location)
        {
            var elt = factory.Create<IDeclaredElement>();
            var typeOwner = elt.As<ITypeOwner>();
            var type = factory.Create<IType>();
            var declaredType = type.As<IDeclaredType>();
            var typeElement = factory.Create<ITypeElement>();

            typeOwner.SetupGet(tm => tm.Type).Returns(type.Object);



            var module = factory.Create<IAssemblyPsiModule>();

            var file = factory.Create<IAssemblyFile>();

            var path = new FileSystemPath(location);

            declaredType.Setup(dt => dt.GetTypeElement()).Returns(
                typeElement.Object);

            typeElement.Setup(dt => dt.CLRName).Returns(typename);


            file.Setup(f => f.Location).Returns(path);

            module.Setup(m => m.Assembly.GetFiles()).Returns(
                new[] { file.Object });

            typeElement.Setup(dt => dt.Module).Returns(module.Object);


            return elt.Object;
        }

        public static IDeclaredElement PlainDeclaredElement(this MockFactory factory)
        {
            return factory.Create<IDeclaredElement>().Object;
        }
    }
}
