using UnityEngine;

namespace Suriyun.UnityIAP
{
    [System.Serializable]
    public class BaseIAPProduct : ScriptableObject
    {
        public string id;
        public string title;
        public InAppProductID[] storeIDs;

        public string GetStoreIdByPlatform(IAPPlatform platform)
        {
            foreach (var id in storeIDs)
            {
                if (id.platform == platform)
                    return id.id;
            }
            return string.Empty;
        }
    }
}