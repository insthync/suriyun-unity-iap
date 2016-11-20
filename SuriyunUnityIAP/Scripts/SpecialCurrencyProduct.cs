using UnityEngine;
using UnityEngine.Purchasing;

namespace Suriyun.MultiplayerRPG
{
    public class SpecialCurrencyProduct : ScriptableObject
    {
        public string id;
        public InAppProductID[] storeIDs;
        public int currencyAmount;
    }
}
