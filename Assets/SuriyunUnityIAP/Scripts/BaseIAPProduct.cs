using UnityEngine;

namespace Suriyun.UnityIAP
{
    [System.Serializable]
    public class BaseIAPProduct : ScriptableObject
    {
        public string id;
        public string title;
        public InAppProductID[] storeIDs;
    }
}