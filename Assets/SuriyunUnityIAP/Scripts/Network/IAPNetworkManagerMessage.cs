using UnityEngine;
using UnityEngine.Networking;

namespace Suriyun.UnityIAP
{
    public class IAPNetworkManagerMessage
    {
        public enum ServerBuyProductFail
        {
            None,
            NoProduct,
            ValidateFail
        }
        public static System.Action<NetworkMessage, BaseIAPProduct> onBuyProductSuccess;
        public static System.Action<NetworkMessage, ServerBuyProductFail> onBuyProductFail;
        public static void OnServerBuyProduct(NetworkMessage netMsg)
        {
            BaseIAPProduct iapProduct = null;
            ServerBuyProductFail fail = ServerBuyProductFail.None;
            if (ValidateIAP(netMsg, out iapProduct, out fail))
            {
                if (onBuyProductSuccess != null)
                    onBuyProductSuccess(netMsg, iapProduct);
            }
            else
            {
                if (onBuyProductFail != null)
                    onBuyProductFail(netMsg, fail);
            }
        }

        public static bool ValidateIAP(NetworkMessage netMsg, out BaseIAPProduct iapProduct, out ServerBuyProductFail fail)
        {
            MsgBuyProductFromClient msg = netMsg.ReadMessage<MsgBuyProductFromClient>();
            iapProduct = null;
            fail = ServerBuyProductFail.None;
            // Variables from message
            string userId = msg.userId;
            string productId = msg.productId;
            IAPPlatform platform = msg.platform;
            string storeId = msg.storeId;
            string receipt = msg.receipt;
            string transactionId = msg.transactionId;
            if (IAPManager.Instance.IAPProducts.TryGetValue(productId, out iapProduct))
            {
                return true;
            }
            else
            {
                fail = ServerBuyProductFail.NoProduct;
            }
            return false;
        }
    }
}
