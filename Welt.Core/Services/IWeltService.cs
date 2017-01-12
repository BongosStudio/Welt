using System.Reflection;

namespace Welt.Core.Services
{
    public interface IWeltService
    {
        void Load(Assembly assembly);
        void Unload();
    }
}