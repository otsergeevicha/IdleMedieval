using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace webgl
{
    public class InApps : MonoBehaviour
    {
        [SerializeField] private TMP_Text _price5000;
        [SerializeField] private TMP_Text _price20000;
        [SerializeField] private TMP_Text _price100000;
        [SerializeField] private TMP_Text _price1500000;
        
        private void Start()
        {
            StartCoroutine(LoadPrices());
        }

        public void BuyGold5000()
        {
           // MirraSDK.Payments.Purchase(ID_GOLD_5000);
        }
        
        public void BuyGold20000()
        {
            //MirraSDK.Payments.Purchase(ID_GOLD_20000);
        }
        
        public void BuyGold100000()
        {
            //MirraSDK.Payments.Purchase(ID_GOLD_100000);
        }
        
        public void BuyGold1500000()
        {
            //MirraSDK.Payments.Purchase(ID_GOLD_1500000);
        }

        private void FillPrice()
        {
            // _price5000.text = MirraSDK.Payments.GetProductPrice(ID_GOLD_5000);
            // _price20000.text = MirraSDK.Payments.GetProductPrice(ID_GOLD_20000);
            // _price100000.text = MirraSDK.Payments.GetProductPrice(ID_GOLD_100000);
            // _price1500000.text = MirraSDK.Payments.GetProductPrice(ID_GOLD_1500000);
        }

        private IEnumerator LoadPrices()
        {
            //while (!MirraSDK.Payments.IsProductsReady)
            {
                yield return null;
            }
            FillPrice();
        }
        
        public const string ID_GOLD_5000 = "gold_5000";
        public const string ID_GOLD_20000 = "gold_20000";
        public const string ID_GOLD_100000 = "gold_100000";
        public const string ID_GOLD_1500000 = "gold_1500000";
        
        public static event Action OnGold5000;
        public static event Action OnGold20000;
        public static event Action OnGold100000;
        public static event Action OnGold1500000;
        
        // BaseProductsModule.

        // Inaccessible fields.
        private readonly Dictionary<string, Action> products = new() {
            [ID_GOLD_5000] = () => {
                OnGold5000?.Invoke();
            },
            [ID_GOLD_20000] = () => {
                OnGold20000?.Invoke();
            },
            [ID_GOLD_100000] = () => {
                OnGold100000?.Invoke();
            },
            [ID_GOLD_1500000] = () => {
                OnGold1500000?.Invoke();
            }
        };
    }
}
