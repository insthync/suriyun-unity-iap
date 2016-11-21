using UnityEngine;

namespace Suriyun.UnityIAP
{
    public class SpecialCurrencyProduct : ScriptableObject
    {
        public string id;
        public InAppProductID[] storeIDs;
        public int currencyAmount;
    }
}
