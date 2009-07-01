using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.Text;
using System.Threading;
using JetBrains.ActionManagement;
using JetBrains.ReSharper.Psi;
using RogueSharper.BrowseToReflector.Reflector;

namespace RogueSharper.BrowseToReflector
{
    [ActionHandler(new[] { "RogueSharper.BrowseToReflector.BrowseAction" })]
    class BrowseAction : IActionHandler
    {
        private static readonly int RetryCount = 10;

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
                return Browse(element, reflector);
            }

            return false;
        }

        private static bool Browse(Element element, ReflectorFacade reflector)
        {
            if (!EnsureReflectorRunning(reflector))
            {
                return false;
            }

            reflector.Browse(element.AssemblyFile, element.TypeName, element.MemberName);

            return true;
        }

        private static bool EnsureReflectorRunning(ReflectorFacade reflector)
        {
            for (int i = 0; !reflector.IsRunning() && i < RetryCount; i++ )
            {
                string configFile = GetConfigFile();
                string reflectorExe = GetReflectorExe();

                ReflectorRunner runner = new ReflectorRunner(configFile, reflectorExe);
                runner.Run();

                runner.WaitForIdle();

                if (!reflector.IsRunning())
                {
                    Debug.WriteLine("Reflector still not running at attempt #" + i + ". Sleeping for a second");
                    Thread.Sleep(1000);
                }
                else
                {
                    Debug.WriteLine(string.Format("Launched reflector after {0} retries.", i));
                    return true;
                }
            }

            return reflector.IsRunning();
        }

        private static string GetReflectorExe()
        {
            return
                @"C:\mydocs\vcsharp\RogueSharper\src\RogueSharper.ReflectorBrowseServicePlugin\bin\Debug\Reflector.exe";
        }

        private static string GetConfigFile()
        {
            string configFile =
                Path.GetDirectoryName(
                    Assembly.GetExecutingAssembly().ManifestModule.
                        FullyQualifiedName);

            configFile = Path.Combine(configFile, "Reflector.cfg");
            return configFile;
        }
    }
}
