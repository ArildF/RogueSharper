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
            if (File.Exists(_path))
            {
#if !DEBUG
                return;
#endif
            }

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
            
            string contents = String.Format(@"[AddInManager]
""{0}""",
                                            reflectorPlugin);

            File.WriteAllText(_path, contents);
        }
    }
}
