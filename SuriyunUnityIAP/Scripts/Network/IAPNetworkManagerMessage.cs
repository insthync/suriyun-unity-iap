using UnityEngine;
using UnityEngine.Networking;

namespace Suriyun.UnityIAP
{
    public class IAPNetworkManagerMessage
    {
        public enum ServerBuyProductFail
        {
            NoProduct,
            ValidateFail
        }
        public static System.Action<NetworkMessage> onBuyProductSuccess;
        public static System.Action<NetworkMessage, ServerBuyProductFail> onBuyProductFail;
        public static void OnServerBuyProduct(NetworkMessage netMsg)
        {
            MsgBuyProductFromClient msg = netMsg.ReadMessage<MsgBuyProductFromClient>();
            string playerId = msg.playerId;
            string productId = msg.productId;
            IAPPlatform platform = msg.platform;
            string storeId = msg.storeId;
            string receipt = msg.receipt;
            string transactionId = msg.transactionId;
            BaseIAPProduct iapProduct = null;
            if (IAPManager.Instance.IAPProducts.TryGetValue(productId, out iapProduct))
            {
                // TODO: May validate purchased data
                if (onBuyProductSuccess != null)
                    onBuyProductSuccess(netMsg);
            }
            else
            {
                if (onBuyProductFail != null)
                    onBuyProductFail(netMsg, ServerBuyProductFail.NoProduct);
            }
        }
    }
}
