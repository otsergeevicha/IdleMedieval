﻿// using System;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;
// using UnityEngine.Purchasing;
// using System.Globalization;
//
// // Placing the Purchaser class in the CompleteProject namespace allows it to interact with ScoreManager, 
// // one of the existing Survival Shooter scripts.
// namespace CompleteProject
// {
//     // Deriving the Purchaser class from IStoreListener enables it to receive messages from Unity Purchasing.
//     public class Purchaser : MonoBehaviour, IStoreListener
//     {
//         private AppMetEvents _appMetEvents;
//         private static IStoreController m_StoreController;          // The Unity Purchasing system.
//         private static IExtensionProvider m_StoreExtensionProvider; // The store-specific Purchasing subsystems.
//         
//         // Product identifiers for all products capable of being purchased: 
//         // "convenience" general identifiers for use with Purchasing, and their store-specific identifier 
//         // counterparts for use with and outside of Unity Purchasing. Define store-specific identifiers 
//         // also on each platform's publisher dashboard (iTunes Connect, Google Play Developer Console, etc.)
//         
//         // General product identifiers for the consumable, non-consumable, and subscription products.
//         // Use these handles in the code to reference which product to purchase. Also use these values 
//         // when defining the Product Identifiers on the store. Except, for illustration purposes, the 
//         // kProductIDSubscription - it has custom Apple and Google identifiers. We declare their store-
//         // specific mapping to Unity Purchasing's AddProduct, below.
//         public static string imk_gold_099 = "imka_gold_099";
//         public static string imk_gold_299 = "imka_gold_299";
//         public static string imk_gold_999 = "imka_gold_999";
//         public static string imk_gold_3999 = "imka_gold_3999";
//         
//         [SerializeField] private Text[] _priceProductText;
//         [SerializeField] private Wallet _wallet;
//         
//         // Apple App Store-specific product identifier for the subscription product.
//         private static string kProductNameAppleSubscription = "com.unity3d.subscription.new";
//         
//         // Google Play Store-specific product identifier subscription product.
//         private static string kProductNameGooglePlaySubscription = "com.unity3d.subscription.original";
//         
//         
//         void Start()
//         {
//             _appMetEvents = FindObjectOfType<AppMetEvents>();
//             _wallet = FindObjectOfType<Wallet>();
//             
//             if (m_StoreController == null)
//             {
//                 InitializePurchasing();
//             }
//         
//             if (PlayerPrefs.HasKey("_priceProductText0"))
//             {
//                 for(int i=0; i<4; i++)
//                 {
//                     _priceProductText[i].text = PlayerPrefs.GetString("_priceProductText" + i);
//                 }
//             }
//         }
//         
//         
//         public void InitializePurchasing()
//         {
//             // If we have already connected to Purchasing ...
//             if (IsInitialized())
//             {
//                 // ... we are done here.
//                 return;
//             }
//         
//             // Create a builder, first passing in a suite of Unity provided stores.
//             var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
//         
//             // Add a product to sell / restore by way of its identifier, associating the general identifier
//             // with its store-specific identifiers.
//             builder.AddProduct(imk_gold_099, ProductType.Consumable);
//             builder.AddProduct(imk_gold_299, ProductType.Consumable);
//             builder.AddProduct(imk_gold_999, ProductType.Consumable);
//             builder.AddProduct(imk_gold_3999, ProductType.Consumable);
//         
//         
//             // Kick off the remainder of the set-up with an asynchrounous call, passing the configuration 
//             // and this class' instance. Expect a response either in OnInitialized or OnInitializeFailed.
//             UnityPurchasing.Initialize(this, builder);
//         }
//         
//         
//         private bool IsInitialized()
//         {
//             // Only say we are initialized if both the Purchasing references are set.
//             return m_StoreController != null && m_StoreExtensionProvider != null;
//         }
//         
//         public string GetPrice(string productID)
//         {
//             return m_StoreController.products.WithID(productID).metadata.localizedPriceString;
//         }
//         
//         public string GetPriceAnalythics(string productID)
//         {
//             if (m_StoreController == null) InitializePurchasing();
//             return m_StoreController.products.WithID(productID).metadata.localizedPrice.ToString();
//         }
//         
//         public string GetCurrency(string productID)
//         {
//             if (m_StoreController == null) InitializePurchasing();
//             return m_StoreController.products.WithID(productID).metadata.isoCurrencyCode;
//         }
//         
//         
//         
//         public void Buy_80()
//         {
//             BuyProductID(imk_gold_099);
//         }
//         
//         public void Buy_500()
//         {
//             BuyProductID(imk_gold_299);
//         }
//         
//         public void Buy_1200()
//         {
//             BuyProductID(imk_gold_999);
//         }
//         
//         public void Buy_2500()
//         {
//             BuyProductID(imk_gold_3999);
//         }
//         
//         
//         void BuyProductID(string productId)
//         {
//             // If Purchasing has been initialized ...
//             if (IsInitialized())
//             {
//                 // ... look up the Product reference with the general product identifier and the Purchasing 
//                 // system's products collection.
//                 Product product = m_StoreController.products.WithID(productId);
//         
//                 // If the look up found a product for this device's store and that product is ready to be sold ... 
//                 if (product != null && product.availableToPurchase)
//                 {
//                     Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
//                     // ... buy the product. Expect a response either through ProcessPurchase or OnPurchaseFailed 
//                     // asynchronously.
//                     m_StoreController.InitiatePurchase(product);
//                 }
//                 // Otherwise ...
//                 else
//                 {
//                     // ... report the product look-up failure situation  
//                     Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
//                 }
//             }
//             // Otherwise ...
//             else
//             {
//                 // ... report the fact Purchasing has not succeeded initializing yet. Consider waiting longer or 
//                 // retrying initiailization.
//                 Debug.Log("BuyProductID FAIL. Not initialized.");
//             }
//         }
//         
//         
//         // Restore purchases previously made by this customer. Some platforms automatically restore purchases, like Google. 
//         // Apple currently requires explicit purchase restoration for IAP, conditionally displaying a password prompt.
//         public void RestorePurchases()
//         {
//             // If Purchasing has not yet been set up ...
//             if (!IsInitialized())
//             {
//                 // ... report the situation and stop restoring. Consider either waiting longer, or retrying initialization.
//                 Debug.Log("RestorePurchases FAIL. Not initialized.");
//                 return;
//             }
//         
//             // If we are running on an Apple device ... 
//             if (Application.platform == RuntimePlatform.IPhonePlayer ||
//                 Application.platform == RuntimePlatform.OSXPlayer)
//             {
//                 // ... begin restoring purchases
//                 Debug.Log("RestorePurchases started ...");
//         
//                 // Fetch the Apple store-specific subsystem.
//                 var apple = m_StoreExtensionProvider.GetExtension<IAppleExtensions>();
//                 // Begin the asynchronous process of restoring purchases. Expect a confirmation response in 
//                 // the Action<bool> below, and ProcessPurchase if there are previously purchased products to restore.
//                 apple.RestoreTransactions((result) =>
//                 {
//                     // The first phase of restoration. If no more responses are received on ProcessPurchase then 
//                     // no purchases are available to be restored.
//                     Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
//                 });
//             }
//             // Otherwise ...
//             else
//             {
//                 // We are not running on an Apple device. No work is necessary to restore purchases.
//                 Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
//             }
//         }
//         
//         
//         //  
//         // --- IStoreListener
//         //
//         
//         public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
//         {
//             // Purchasing has succeeded initializing. Collect our Purchasing references.
//             Debug.Log("OnInitialized: PASS");
//         
//             // Overall Purchasing system, configured with products for this application.
//             m_StoreController = controller;
//             // Store specific subsystem, for accessing device-specific store features.
//             m_StoreExtensionProvider = extensions;
//         }
//         
//         public void OnInitializeFailed(InitializationFailureReason error)
//         {
//             // Purchasing set-up has not succeeded. Check error for reason. Consider sharing this reason with the user.
//             Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
//         }
//
//         public void OnInitializeFailed(InitializationFailureReason error, string message)
//         {
//             
//         }
//
//         public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
//         {
//             // A consumable product has been purchased by this user.
//             if (String.Equals(args.purchasedProduct.definition.id, imk_gold_099, StringComparison.Ordinal))
//             {
//                 Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
//                 _wallet.AddMoney(5000);
//                 _appMetEvents.IAP(args.purchasedProduct.definition.id, GetCurrency(args.purchasedProduct.definition.id), GetPrice(args.purchasedProduct.definition.id));
//                
//             }
//             else if (String.Equals(args.purchasedProduct.definition.id, imk_gold_299, StringComparison.Ordinal))
//             {
//                 Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
//                 _wallet.AddMoney(20000);
//                 _appMetEvents.IAP(args.purchasedProduct.definition.id, GetCurrency(args.purchasedProduct.definition.id), GetPrice(args.purchasedProduct.definition.id));
//                 
//             }
//             else if (String.Equals(args.purchasedProduct.definition.id, imk_gold_999, StringComparison.Ordinal))
//             {
//                 Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
//                 _wallet.AddMoney(100000);
//                 _appMetEvents.IAP(args.purchasedProduct.definition.id, GetCurrency(args.purchasedProduct.definition.id), GetPrice(args.purchasedProduct.definition.id));
//                 
//             }
//             else if (String.Equals(args.purchasedProduct.definition.id, imk_gold_3999, StringComparison.Ordinal))
//             {
//                 Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
//                 _wallet.AddMoney(1500000);
//                 _appMetEvents.IAP(args.purchasedProduct.definition.id, GetCurrency(args.purchasedProduct.definition.id), GetPrice(args.purchasedProduct.definition.id));               
//             }
//                        
//             // Or ... an unknown product has been purchased by this user. Fill in additional products here....
//             else
//             {
//                 Debug.Log(string.Format("ProcessPurchase: FAIL. Unrecognized product: '{0}'", args.purchasedProduct.definition.id));
//             }
//         
//             // Return a flag indicating whether this product has completely been received, or if the application needs 
//             // to be reminded of this purchase at next app launch. Use PurchaseProcessingResult.Pending when still 
//             // saving purchased products to the cloud, and when that save is delayed. 
//             return PurchaseProcessingResult.Complete;
//         }
//                
//         public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
//         {
//             // A product purchase attempt did not succeed. Check failureReason for more detail. Consider sharing 
//             // this reason with the user to guide their troubleshooting actions.
//             Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
//         }
//     }
// }