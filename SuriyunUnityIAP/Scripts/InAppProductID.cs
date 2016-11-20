using UnityEngine.Purchasing;

namespace Suriyun.MultiplayerRPG
{
    [System.Serializable]
    public class InAppProductID
    {
        public enum IAPPlatform
        {
            GooglePlay,
            AppleAppStore,
            WindowsStore
        }

        public string id;
        public IAPPlatform platform;

        public string GetPlatformName()
        {
            switch (platform)
            {
                case IAPPlatform.AppleAppStore:
                    return AppleAppStore.Name;
                case IAPPlatform.GooglePlay:
                    return GooglePlay.Name;
                case IAPPlatform.WindowsStore:
                    return WindowsStore.Name;
            }
            return string.Empty;
        }
    }
}