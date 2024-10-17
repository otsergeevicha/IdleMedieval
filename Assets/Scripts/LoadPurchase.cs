// using System;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.Purchasing;
// using System.Globalization;
//
// // Placing the Purchaser class in the CompleteProject namespace allows it to interact with ScoreManager, 
// // one of the existing Survival Shooter scripts.
// namespace CompleteProject
// {
//   // Deriving the Purchaser class from IStoreListener enables it to receive messages from Unity Purchasing.
//   public class LoadPurchase : MonoBehaviour, IStoreListener
//   {
//     private static IStoreController m_StoreController; // The Unity Purchasing system.
//     private static IExtensionProvider m_StoreExtensionProvider; // The store-specific Purchasing subsystems.
//
//     // Product identifiers for all products capable of being purchased: 
//     // "convenience" general identifiers for use with Purchasing, and their store-specific identifier 
//     // counterparts for use with and outside of Unity Purchasing. Define store-specific identifiers 
//     // also on each platform's publisher dashboard (iTunes Connect, Google Play Developer Console, etc.)
//
//     // General product identifiers for the consumable, non-consumable, and subscription products.
//     // Use these handles in the code to reference which product to purchase. Also use these values 
//     // when defining the Product Identifiers on the store. Except, for illustration purposes, the 
//     // kProductIDSubscription - it has custom Apple and Google identifiers. We declare their store-
//     // specific mapping to Unity Purchasing's AddProduct, below.
//     public static string imk_gold_099 = "imka_gold_099";
//     public static string imk_gold_299 = "imka_gold_299";
//     public static string imk_gold_999 = "imka_gold_999";
//     public static string imk_gold_3999 = "imka_gold_3999";
//     private string[] _priceProductText = new string[4];
//
//     // Apple App Store-specific product identifier for the subscription product.
//     private static string kProductNameAppleSubscription = "com.unity3d.subscription.new";
//
//     // Google Play Store-specific product identifier subscription product.
//     private static string kProductNameGooglePlaySubscription = "com.unity3d.subscription.original";
//
//
//     void Start()
//     {
//       if (m_StoreController == null)
//       {
//         InitializePurchasing();
//       }
//     }
//
//
//     public void InitializePurchasing()
//     {
//       // If we have already connected to Purchasing ...
//       if (IsInitialized())
//       {
//         // ... we are done here.
//         return;
//       }
//
//       // Create a builder, first passing in a suite of Unity provided stores.
//       var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
//
//       // Add a product to sell / restore by way of its identifier, associating the general identifier
//       // with its store-specific identifiers.
//       builder.AddProduct(imk_gold_099, ProductType.Consumable);
//       builder.AddProduct(imk_gold_299, ProductType.Consumable);
//       builder.AddProduct(imk_gold_999, ProductType.Consumable);
//       builder.AddProduct(imk_gold_3999, ProductType.Consumable);
//
//
//       // Kick off the remainder of the set-up with an asynchrounous call, passing the configuration 
//       // and this class' instance. Expect a response either in OnInitialized or OnInitializeFailed.
//       UnityPurchasing.Initialize(this, builder);
//     }
//
//
//     private bool IsInitialized()
//     {
//       // Only say we are initialized if both the Purchasing references are set.
//       return m_StoreController != null && m_StoreExtensionProvider != null;
//     }
//
//     public string GetPrice(string productID)
//     {
//       return m_StoreController.products.WithID(productID).metadata.localizedPriceString;
//     }
//
//     //  
//     // --- IStoreListener
//     //
//
//     public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
//     {
//       // Purchasing has succeeded initializing. Collect our Purchasing references.
//       Debug.Log("OnInitialized: PASS");
//
//       // Overall Purchasing system, configured with products for this application.
//       m_StoreController = controller;
//       // Store specific subsystem, for accessing device-specific store features.
//       m_StoreExtensionProvider = extensions;
//
//       _priceProductText[0] = GetPrice(imk_gold_099);
//       _priceProductText[1] = GetPrice(imk_gold_299);
//       _priceProductText[2] = GetPrice(imk_gold_999);
//       _priceProductText[3] = GetPrice(imk_gold_3999);
//
//       for (int i = 0; i < 4; i++)
//       {
//         PlayerPrefs.SetString("_priceProductText" + i, _priceProductText[i]);
//       }
//
//       LevelLoading._purchaseIsDone = true;
//     }
//
//     public void OnInitializeFailed(InitializationFailureReason error)
//     {
//       // Purchasing set-up has not succeeded. Check error for reason. Consider sharing this reason with the user.
//       Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
//     }
//
//     public void OnInitializeFailed(InitializationFailureReason error, string message)
//     {
//       Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
//     }
//
//     public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
//     {
//       return PurchaseProcessingResult.Complete;
//     }
//
//     public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
//     {
//       // A product purchase attempt did not succeed. Check failureReason for more detail. Consider sharing 
//       // this reason with the user to guide their troubleshooting actions.
//       Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}",
//         product.definition.storeSpecificId, failureReason));
//     }
//   }
// }