using UnityEngine.Networking;

namespace Suriyun.UnityIAP
{
    public class MsgBuyProductFromClient : MessageBase
    {
        public const short MsgId = IAPNetworkMessageId.ToServerBuyProductMsgId;
        public string userId = string.Empty;
        public string productId = string.Empty;
        public IAPPlatform platform = IAPPlatform.Unknow;
        public string storeId = string.Empty;
        public string receipt = string.Empty;
        public string transactionId = string.Empty;
    }
}
