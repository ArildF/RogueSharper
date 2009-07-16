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
        private const string _location = @"C:\assembly.dll";
        private const string _typename = "System.Ding.Dong";
        private const string _membername = "WitchIsDead";

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
            var elt = _factory.DeclaredElementAsTypeMember(_typename, _membername, _location);

            Element found = _finder.FindElement(elt);
            Assert.That(found, Is.Not.SameAs(Element.NotFound));
            Assert.That(found.AssemblyFile, Is.SamePath(_location));
            Assert.That(found.TypeName, Is.SameAs(_typename));
            Assert.That(found.MemberName, Is.SameAs(_membername));
        }

        [Test]
        public void FindTypeOwner()
        {
            var elt = _factory.DeclaredElementAsTypeOwner(_typename, _location);

            Element found = _finder.FindElement(elt);
            Assert.That(found, Is.Not.SameAs(Element.NotFound));
            Assert.That(found.AssemblyFile, Is.SamePath(_location));
            Assert.That(found.TypeName, Is.SameAs(_typename));
            Assert.That(found.MemberName, Is.Empty);
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
