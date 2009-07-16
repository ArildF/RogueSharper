using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Moq;
using Reflector;
using Reflector.CodeModel;
using RogueSharper.ReflectorBrowseServicePlugin;
using ReflectorType = RogueSharper.ReflectorBrowseServicePlugin.Reflector;

namespace RogueSharper.Tests.ReflectorBrowseServicePlugin
{
    [TestFixture]
    public class ReflectorTest
    {
        private ReflectorType _reflector;

        private readonly MockFactory _factory =
            new MockFactory(MockBehavior.Loose)
                {DefaultValue = DefaultValue.Mock};

        private Mock<IAssemblyManager> _assemblyManager;
        private Mock<IAssemblyBrowser> _assemblyBrowser;
        private Mock<IWindowManager> _windowManager;

        [SetUp]
        public void SetUp()
        {
            _assemblyManager = _factory.Create<IAssemblyManager>();
            _assemblyBrowser = _factory.Create<IAssemblyBrowser>();
            _windowManager = _factory.Create<IWindowManager>();
            
            _reflector = new ReflectorType(_assemblyManager.Object, _assemblyBrowser.Object, _windowManager.Object);
        }

        [Test]
        public void HaveSuitcaseWillActivate()
        {
            _reflector.Browse(@"C:\assembly.dll", "Ding.Dong", "WitchIsDead");

            _windowManager.Verify(wm => wm.Activate());
        }

        [Test]
        public void AssemblyActivated()
        {
            string location = @"C:\assembly.dll";
            IAssembly assembly =
                _assemblyManager.Object.LoadFile(location);

            _reflector.Browse(location, "Ding.Dong", "WitchIsDead");

            _assemblyBrowser.VerifySet(ab => ab.ActiveItem, assembly);
        }
    }
}
