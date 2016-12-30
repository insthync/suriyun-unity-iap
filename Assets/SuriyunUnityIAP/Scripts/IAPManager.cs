using UnityEngine;
using UnityEngine.Purchasing;
using System.Collections.Generic;
namespace Suriyun.UnityIAP
{
    abstract public class IAPManager<T> : MonoBehaviour, IStoreListener where T : BaseIAPProduct
    {
        public T[] iapProducts;
        public delegate void OnInitializedEvent(IStoreController controller, IExtensionProvider extensions);
        public delegate void OnInitializeFailedEvent(InitializationFailureReason error);
        public delegate void OnPurchaseFailedEvent(Product i, PurchaseFailureReason p);
        public delegate PurchaseProcessingResult ProcessPurchaseEvent(PurchaseEventArgs e);
        public delegate void OnClientProductsResponse(List<T> products);
        public OnInitializedEvent onInitialized;
        public OnInitializeFailedEvent onInitializeFailed;
        public OnPurchaseFailedEvent onPurchaseFailed;
        public ProcessPurchaseEvent processPurchase;
        public OnClientProductsResponse onClientProductsResponse;
        public static IAPManager<T> Instance { get; protected set; }

        protected Dictionary<string, T> _iapProducts;
        public Dictionary<string, T> IAPProducts
        {
            get
            {
                if (_iapProducts == null)
                {
                    _iapProducts = new Dictionary<string, T>();
                    foreach (T iapProduct in iapProducts)
                    {
                        _iapProducts.Add(iapProduct.id, iapProduct);
                    }
                }
                return _iapProducts;
            }
        }

        void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        
        void SetupProducts()
        {
            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

            foreach (var product in iapProducts)
            {
                var storeIDs = new IDs();
                foreach (var storeId in product.storeIDs)
                {
                    storeIDs.Add(storeId.id, storeId.GetPlatformName());
                }
                builder.AddProduct(product.id, ProductType.Consumable, storeIDs);
            }

            UnityPurchasing.Initialize(this, builder);
        }

        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            if (onInitialized != null)
                onInitialized(controller, extensions);
        }

        public void OnInitializeFailed(InitializationFailureReason error)
        {
            if (onInitializeFailed != null)
                onInitializeFailed(error);
        }

        public void OnPurchaseFailed(Product i, PurchaseFailureReason p)
        {
            if (onPurchaseFailed != null)
                onPurchaseFailed(i, p);
        }

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs e)
        {
            if (processPurchase != null)
                return processPurchase(e);

            return PurchaseProcessingResult.Complete;
        }

        public void RaiseOnClientProductsResponse(List<T> productList)
        {
            if (onClientProductsResponse != null)
                onClientProductsResponse(productList);
        }
    }
}
