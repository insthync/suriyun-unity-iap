using UnityEngine.Networking;

namespace Suriyun.UnityIAP
{
    public class IAPNetworkMessageId
    {
        // Developer can changes these Ids to avoid hacking while hosting
        public const short ToServerBuyProductMsgId = MsgType.Highest + 201;
        public const short ToServerRequestProducts = MsgType.Highest + 202;
        public const short ToClientResponseProducts = MsgType.Highest + 203;
    }
}
