using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace RogueSharper.BrowseToReflector
{
    class ReflectorConfigFile
    {
        private readonly string _path;

        public ReflectorConfigFile()
        {
            _path =
                System.IO.Path.Combine(
                    Environment.GetFolderPath(
                        Environment.SpecialFolder.ApplicationData),
                    @"RogueSharper.BrowseToReflector\Reflector.cfg");
        }

        public string Path
        {
            get
            {
                EnsureExists();
                return _path;
            }
        }

        private void EnsureExists()
        {

            string dir = System.IO.Path.GetDirectoryName(_path);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            string currentAssemblyDir = System.IO.Path.GetDirectoryName(
                Assembly.GetExecutingAssembly().ManifestModule.
                    FullyQualifiedName);
            string reflectorPlugin = System.IO.Path.Combine(currentAssemblyDir,
                                                            "RogueSharper.ReflectorBrowseServicePlugin.dll");
            
            string contents = String.Format(@"
[AddInManager]
""{0}""

[AssemblyCache]
""%SystemRoot%\Microsoft.net""
""%ProgramFiles%\Reference Assemblies""
""%ProgramFiles%\Microsoft.net""
""%ProgramFiles%\Microsoft Silverlight""

[AssemblyManager]
""C:\Windows\Microsoft.NET\Framework\v2.0.50727\Microsoft.Build.Engine.dll""
""%SystemRoot%\Microsoft.net\Framework\v2.0.50727\mscorlib.dll""
""%SystemRoot%\Microsoft.net\Framework\v2.0.50727\System.dll""
""%SystemRoot%\Microsoft.net\Framework\v2.0.50727\System.Xml.dll""
""%SystemRoot%\Microsoft.net\Framework\v2.0.50727\System.Data.dll""
""%SystemRoot%\Microsoft.net\Framework\v2.0.50727\System.Web.dll""
""%SystemRoot%\Microsoft.net\Framework\v2.0.50727\System.Drawing.dll""
""%SystemRoot%\Microsoft.net\Framework\v2.0.50727\System.Windows.Forms.dll""
""%ProgramFiles%\Reference Assemblies\Microsoft\Framework\v3.5\System.Core.dll""
""%ProgramFiles%\Reference Assemblies\Microsoft\Framework\v3.0\System.ServiceModel.dll""
""%ProgramFiles%\Reference Assemblies\Microsoft\Framework\v3.0\System.Workflow.ComponentModel.dll""
""%ProgramFiles%\Reference Assemblies\Microsoft\Framework\v3.0\System.Workflow.Runtime.dll""
""%ProgramFiles%\Reference Assemblies\Microsoft\Framework\v3.0\System.Workflow.Activities.dll""
""%ProgramFiles%\Reference Assemblies\Microsoft\Framework\v3.0\WindowsBase.dll""
""%ProgramFiles%\Reference Assemblies\Microsoft\Framework\v3.0\PresentationCore.dll""
""%ProgramFiles%\Reference Assemblies\Microsoft\Framework\v3.0\PresentationFramework.dll""",
                                            reflectorPlugin);

            File.WriteAllText(_path, contents);
        }
    }
}
