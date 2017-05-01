#if USE_IAP
using UnityEngine.Purchasing;
#endif

namespace Suriyun.UnityIAP
{
    [System.Serializable]
    public class InAppProductID
    {
        public string id;
        public IAPPlatform platform;

        public string GetPlatformName()
        {
#if USE_IAP
            switch (platform)
            {
                case IAPPlatform.AppleAppStore:
                    return AppleAppStore.Name;
                case IAPPlatform.GooglePlay:
                    return GooglePlay.Name;
                case IAPPlatform.WindowsStore:
                    return WindowsStore.Name;
                case IAPPlatform.TizenStore:
                    return TizenStore.Name;
                case IAPPlatform.MacAppStore:
                    return MacAppStore.Name;
            }
#endif
            return string.Empty;
        }
    }
}
