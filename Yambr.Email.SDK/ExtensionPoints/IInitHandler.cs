using Yambr.SDK.ComponentModel;

namespace Yambr.SDK.ExtensionPoints
{
    [ExtensionPoint]
    public interface IInitHandler
    {
        void Init();
        void InitComplete();
    }
}
