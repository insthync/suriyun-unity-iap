using UnityEngine.Networking;

namespace Suriyun.UnityIAP
{
    public class MsgResponseProductsFromServer : MessageBase
    {
        public const short MsgId = IAPNetworkMessageId.ToClientResponseProducts;
        public string jsonProducts;
    }
}
