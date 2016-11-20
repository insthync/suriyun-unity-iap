using UnityEngine;
using UnityEngine.Purchasing;
using System;

namespace Suriyun.MultiplayerRPG
{
    public class IAPManager : MonoBehaviour, IStoreListener
    {
        public SpecialCurrencyProduct[] specialCurrencyProducts;
        public delegate void OnInitializedEvent(IStoreController controller, IExtensionProvider extensions);
        public delegate void OnInitializeFailedEvent(InitializationFailureReason error);
        public delegate void OnPurchaseFailedEvent(Product i, PurchaseFailureReason p);
        public delegate PurchaseProcessingResult ProcessPurchaseEvent(PurchaseEventArgs e);
        public OnInitializedEvent onInitialized;
        public OnInitializeFailedEvent onInitializeFailed;
        public OnPurchaseFailedEvent onPurchaseFailed;
        public ProcessPurchaseEvent processPurchase;
        public IAPManager instance { get; protected set; }
        
        void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
                return;
            }
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        void SetupProducts()
        {
            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

            foreach (var product in specialCurrencyProducts)
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
    }
}
