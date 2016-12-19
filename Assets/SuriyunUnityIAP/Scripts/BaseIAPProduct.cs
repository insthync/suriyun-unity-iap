using UnityEngine;

namespace Suriyun.UnityIAP
{
    public class BaseIAPProduct : ScriptableObject
    {
        public string id;
        public string title;
        public InAppProductID[] storeIDs;
    }
}