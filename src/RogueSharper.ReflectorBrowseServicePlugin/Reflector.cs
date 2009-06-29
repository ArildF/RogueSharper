using System.Linq;
using Reflector;
using Reflector.CodeModel;

namespace RogueSharper.ReflectorBrowseServicePlugin
{
    class Reflector : IReflector
    {
        private readonly IAssemblyManager _manager;
        private readonly IAssemblyBrowser _browser;

        public Reflector(IAssemblyManager manager, IAssemblyBrowser browser)
        {
            this._manager = manager;
            this._browser = browser;
        }

        public void Browse(string assemblyPath, string typeName, string memberName)
        {
            var assembly = this._manager.LoadFile(assemblyPath);

            this._browser.ActiveItem = assembly;

            if (typeName != null)
            {
                var type = (from module in assembly.Modules.Cast<IModule>()
                            from t in module.Types.Cast<ITypeDeclaration>()
                            where (t.Namespace + "." + t.Name) == typeName
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
                        if (member != null)
                        {
                            this._browser.ActiveItem = member;
                        }
                    }
                }
            }

        }
    }
}