using System;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using Reflector;
using Reflector.CodeModel;

namespace RogueSharper.ReflectorBrowseServicePlugin
{
    class Reflector : IReflector
    {
        private readonly IAssemblyManager _manager;
        private readonly IAssemblyBrowser _browser;
        private readonly IWindowManager _windowManager;

        public Reflector(IAssemblyManager manager, IAssemblyBrowser browser, IWindowManager windowManager)
        {
            this._manager = manager;
            this._browser = browser;
            _windowManager = windowManager;
        }

        public void Browse(string assemblyPath, string typeName, string memberName)
        {
            var assembly = this._manager.LoadFile(assemblyPath);

            this._browser.ActiveItem = assembly;

            if (typeName != null)
            {
                Func<ITypeDeclaration, bool> pred =
                    CreatePredicate(typeName);

                var type = (from module in assembly.Modules.Cast<IModule>()
                            from t in module.Types.Cast<ITypeDeclaration>()
                            where pred(t)
                            select t).FirstOrDefault();

                if (type != null)
                {
                    this._browser.ActiveItem = type;

                    if (memberName != null)
                    {
                        var members = type.Methods.Cast<IMemberDeclaration>()
                            .Concat(type.Events.Cast<IMemberDeclaration>())
                            .Concat(type.Properties.Cast<IMemberDeclaration>())
                            .Concat(type.Fields.Cast<IMemberDeclaration>());

                        var member = (from m in members
                                      where m.Name == memberName
                                      select m).FirstOrDefault();
                        foreach (var enumerable in members)
                        {
                            Trace.WriteLine(enumerable.Name);
                        }
                        if (member != null)
                        {
                            this._browser.ActiveItem = member;
                        }
                    }
                }
            }

            this._windowManager.Activate();

        }

        private static Func<ITypeDeclaration,bool> CreatePredicate(string typeName)
        {
            Regex regex = new Regex(@"(.*)`(\d+)");
            Match match;
            if ((match = regex.Match(typeName)) != Match.Empty)
            {
                int numGenericArguments =
                    Convert.ToInt32(match.Groups[2].ToString());

                string typeNameWithoutGenerics = match.Groups[1].ToString();

                return
                    t =>
                    FullTypeName(t) == typeNameWithoutGenerics &&
                    t.GenericArguments.Count == numGenericArguments;
            }

            return t => FullTypeName(t) == typeName;

        }

        private static string FullTypeName(ITypeDeclaration t)
        {
            return t.Namespace + "." + t.Name;
        }
    }
}