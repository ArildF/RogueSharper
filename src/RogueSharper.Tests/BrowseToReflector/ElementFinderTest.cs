using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.ReSharper.Psi;
using Moq;
using NUnit.Framework;
using RogueSharper.BrowseToReflector;

namespace RogueSharper.Tests.BrowseToReflector
{
    [TestFixture]
    public class ElementFinderTest
    {
        private MockFactory _factory = new MockFactory(MockBehavior.Loose)
            {
                CallBase = true,
                DefaultValue = DefaultValue.Mock
            };

        private ElementFinder _finder;

        [SetUp]
        public void SetUp()
        {
            _finder = new ElementFinder();
        }

        [Test]
        public void FindTypeElement()
        {
            const string location = @"C:\assembly.dll";
            const string typename = "System.Ding.Dong";

            var elt = _factory.DeclaredElementAsTypeElementWithLocation(typename, location);

            Element found = _finder.FindElement(elt);
            Assert.That(found, Is.Not.SameAs(Element.NotFound));
            Assert.That(found.AssemblyFile, Is.SamePath(location));
            Assert.That(found.TypeName, Is.SameAs(typename));
        }

        [Test]
        public void FindTypeMember()
        {
            const string location = @"C:\assembly.dll";
            const string typename = "System.Ding.Dong";

            const string membername = "WitchIsDead";

            var elt = _factory.DeclaredElementAsTypeMember(typename, membername, location);

            Element found = _finder.FindElement(elt);
            Assert.That(found, Is.Not.SameAs(Element.NotFound));
            Assert.That(found.AssemblyFile, Is.SamePath(location));
            Assert.That(found.TypeName, Is.SameAs(typename));
            Assert.That(found.MemberName, Is.SameAs(membername));
        }

        [Test]
        public void CantFindAThing()
        {
            var elt = _factory.PlainDeclaredElement();
            Element found = _finder.FindElement(elt);

            Assert.That(found, Is.SameAs(Element.NotFound));
        }
    }
}
