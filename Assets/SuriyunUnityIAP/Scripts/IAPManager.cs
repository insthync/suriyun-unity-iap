using UnityEngine;
using UnityEngine.Purchasing;
using System.Collections.Generic;
namespace Suriyun.UnityIAP
{
    public abstract class IAPManager<T> : MonoBehaviour, IStoreListener where T : BaseIAPProduct
    {
        public T[] consumableProductList;
        public delegate void OnInitializedEvent(IStoreController controller, IExtensionProvider extensions);
        public delegate void OnInitializeFailedEvent(InitializationFailureReason error);
        public delegate void OnPurchaseFailedEvent(Product i, PurchaseFailureReason p);
        public delegate PurchaseProcessingResult ProcessPurchaseEvent(PurchaseEventArgs args);
        public delegate void OnProcessPurchaseEvent(T product, PurchaseEventArgs args, PurchaseProcessingResult result);
        public OnInitializedEvent onInitialized;
        public OnInitializeFailedEvent onInitializeFailed;
        public OnPurchaseFailedEvent onPurchaseFailed;
        public ProcessPurchaseEvent processPurchase;
        public OnProcessPurchaseEvent onProcessPurchase;
        public static IAPManager<T> Instance { get; protected set; }
        protected IStoreController storeController;
        protected Dictionary<string, T> consumableProducts;
        public Dictionary<string, T> ConsumableProducts
        {
            get
            {
                if (consumableProducts == null)
                {
                    consumableProducts = new Dictionary<string, T>();
                    foreach (T product in consumableProductList)
                    {
                        consumableProducts.Add(product.id, product);
                    }
                }
                return consumableProducts;
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
        
        public void SetupProducts()
        {
            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

            foreach (var product in consumableProductList)
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

        public void BuyProduct(T product)
        {
            storeController.products.WithID(product.id);
        }

        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            if (onInitialized != null)
                onInitialized(controller, extensions);
            storeController = controller;
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

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
        {
            if (processPurchase != null)
                return processPurchase(args);

            PurchaseProcessingResult result = PurchaseProcessingResult.Complete;

            T product = null;
            ConsumableProducts.TryGetValue(args.purchasedProduct.definition.id, out product);

            if (onProcessPurchase != null)
                onProcessPurchase(product, args, result);

            return result;
        }

        public static IAPPlatform GetCurrentPlatform()
        {
#if UNITY_ANDROID
            return IAPPlatform.GooglePlay;
#elif UNITY_IOS
            return IAPPlatform.AppleAppStore;
#elif UNITY_WSA
            return IAPPlatform.WindowsStore;
#elif UNITY_TIZEN
            return IAPPlatform.TizenStore;
#elif UNITY_STANDALONE_OSX
            return IAPPlatform.MacAppStore;
#else
            return IAPPlatform.Unknow;
#endif
        }
    }
}
