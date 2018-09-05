using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class PurchaseManager : MonoBehaviour, IStoreListener
{
    
    public static PurchaseManager _instance = null;
    public static PurchaseManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new PurchaseManager();
            }
            return _instance;
        }
    }
    
    PurchaseManager()
    {
       
    }

    private static IStoreController storeController;
    private static IExtensionProvider extensionProvider;

    #region 상품ID
    public const string productId1 = "1";
    public const string productId2 = "2";
    public const string productId3 = "3";
    public const string productId4 = "4";
    public const string productId5 = "5";
    #endregion


    private UIPurchaseSlot mPurchaseSlot;
    void Start()
    {
    }

 
    private bool IsInitialized()
    {
        return (storeController != null && extensionProvider != null);
    }

    public void InitializePurchasing()
    {
        
        if (IsInitialized())
            return;

        var module = StandardPurchasingModule.Instance();

        ConfigurationBuilder builder = ConfigurationBuilder.Instance(module);

        builder.AddProduct(productId1, ProductType.Consumable, new IDs
        {
            { CommonData.PURCHASE_ID_ARRAY[0], AppleAppStore.Name },
            { CommonData.PURCHASE_ID_ARRAY[0], GooglePlay.Name },
        });

        builder.AddProduct(productId2, ProductType.Consumable, new IDs
        {
            { CommonData.PURCHASE_ID_ARRAY[1], AppleAppStore.Name },
            { CommonData.PURCHASE_ID_ARRAY[2], GooglePlay.Name }, }
        );

        builder.AddProduct(productId3, ProductType.Consumable, new IDs
        {
            { CommonData.PURCHASE_ID_ARRAY[2], AppleAppStore.Name },
            { CommonData.PURCHASE_ID_ARRAY[2], GooglePlay.Name },
        });

        builder.AddProduct(productId4, ProductType.Consumable, new IDs
        {
            { CommonData.PURCHASE_ID_ARRAY[3], AppleAppStore.Name },
            { CommonData.PURCHASE_ID_ARRAY[3], GooglePlay.Name },
        });

        builder.AddProduct(productId5, ProductType.Consumable, new IDs
        {
            { CommonData.PURCHASE_ID_ARRAY[4], AppleAppStore.Name },
            { CommonData.PURCHASE_ID_ARRAY[4], GooglePlay.Name },
        });

        UnityPurchasing.Initialize(this, builder);
        
    }

    public void BuyProductID(UIPurchaseSlot slot)
    {
        try
        {
            if (IsInitialized())
            {
                mPurchaseSlot = slot;
                //Product p = storeController.products.WithID(productId);
                Product p = storeController.products.WithID(mPurchaseSlot.PurchaseID);

                if (p != null && p.availableToPurchase)
                {
                 
                    Debug.Log(string.Format("Purchasing product asychronously: '{0}'", p.definition.id));
                    storeController.InitiatePurchase(p);
                }
                else
                {
                    Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
                }
            }
            else
            {
                Debug.Log("BuyProductID FAIL. Not initialized.");
            }
        }
        catch (Exception e)
        {
            Debug.Log("BuyProductID: FAIL. Exception during purchase. " + e);
        }
    }

    public void RestorePurchase()
    {
        
        if (!IsInitialized())
        {
            Debug.Log("RestorePurchases FAIL. Not initialized.");
            return;
        }

        if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.OSXPlayer)
        {
            Debug.Log("RestorePurchases started ...");

            var apple = extensionProvider.GetExtension<IAppleExtensions>();

            apple.RestoreTransactions
                (
                    (result) => { Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore."); }
                );
        }
        else
        {
            Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
        }
        
    }

    public void OnInitialized(IStoreController sc, IExtensionProvider ep)
    {
        Debug.Log("OnInitialized : PASS");

        storeController = sc;
        extensionProvider = ep;
    }

    public void OnInitializeFailed(InitializationFailureReason reason)
    {
        Debug.Log("OnInitializeFailed InitializationFailureReason:" + reason);
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));

        PlayerData.Instance.PlusCoin(mPurchaseSlot.Reward);

        /*
        switch (args.purchasedProduct.definition.id)
        {
            case productId1:
                PlayerData.Instance.PlusCoin(mPurchaseSlot.Reward);
                break;

            case productId2:
                PlayerData.Instance.PlusCoin(SlotList[index].Reward);
                break;

            case productId3:
                PlayerData.Instance.PlusCoin(SlotList[index].Reward);
                break;

            case productId4:
                PlayerData.Instance.PlusCoin(SlotList[index].Reward);
                break;

            case productId5:
                PlayerData.Instance.PlusCoin(SlotList[index].Reward);
                break;
        }
        */
        return PurchaseProcessingResult.Complete;
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
    }


}
