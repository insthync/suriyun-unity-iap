using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Networking;
using System.Collections.Generic;

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

        public static bool ValidateIAP<T>(NetworkMessage netMsg, out T iapProduct, out ServerBuyProductFail fail) where T : BaseIAPProduct
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
            if (IAPManager<T>.Instance.IAPProducts.TryGetValue(productId, out iapProduct))
                return true;
            else
                fail = ServerBuyProductFail.NoProduct;
            return false;
        }

        public static void OnServerProductsRequest<T>(NetworkMessage netMsg) where T : BaseIAPProduct
        {
            MsgRequestProductsFromClient msg = netMsg.ReadMessage<MsgRequestProductsFromClient>();
            // TODO: May receive type of list via message
            List<T> iapProducts = new List<T>();
            var keyValueList = IAPManager<T>.Instance.IAPProducts.GetEnumerator();
            while (keyValueList.MoveNext())
                iapProducts.Add(keyValueList.Current.Value as T);

            IAPProducts<T> productsList = new IAPProducts<T>();
            productsList.iapProducts = iapProducts;

            MsgResponseProductsFromServer resMsg = new MsgResponseProductsFromServer();
            resMsg.jsonProducts = JsonUtility.ToJson(productsList);
            netMsg.conn.Send(MsgResponseProductsFromServer.MsgId, resMsg);
        }

        public static void OnClientProductsResponse<T>(NetworkMessage netMsg) where T : BaseIAPProduct
        {
            MsgResponseProductsFromServer msg = netMsg.ReadMessage<MsgResponseProductsFromServer>();
            IAPProducts<T> productsList = JsonUtility.FromJson<IAPProducts<T>>(msg.jsonProducts);
            IAPManager<T>.Instance.RaiseOnClientProductsResponse(productsList.iapProducts);
        }

        [System.Serializable]
        public struct IAPProducts<T> where T : BaseIAPProduct
        {
            public List<T> iapProducts;
        }
    }
}
