using UnityEngine.Networking;

namespace Suriyun.UnityIAP
{
    public class MsgRequestProductsFromClient : MessageBase
    {
        public const short MsgId = IAPNetworkMessageId.ToServerRequestProducts;
    }
}
