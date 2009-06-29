namespace RogueSharper.ReflectorBrowseServicePlugin
{
    public interface IReflector
    {
        void Browse(string assembly, string type, string member);
    }
}