using QFramework;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;
using UnityEngine.Purchasing.MiniJSON;
using UnityEngine.UI;
using Unity.Services.Core;
using Unity.Services.Core.Environments;

public class ShopManager : MonoBehaviour, IDetailedStoreListener, ICanGetUtility
{
    IStoreController m_StoreController; // The Unity Purchasing system.
    static public ShopManager Instance;
    public IArchitecture GetArchitecture()
    {
        return GameMainArc.Interface;
    }
    async void Start()
    {
        try
        {
            var options = new InitializationOptions()
                .SetEnvironmentName("rescue");

            await UnityServices.InitializeAsync(options);
        }
        catch (Exception exception)
        {
            // An error occurred during initialization.
        }
        Instance = this;
        InitializePurchasing();
    }


    void InitializePurchasing()
    {
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        
        //Add products that will be purchasable and indicate its type.
        //��ʼ����Ʒ�б�����Ҫ��IOS��Google��̨�Ĳ�Ʒ�б�һ��
        builder.AddProduct("unlimitedheart", ProductType.NonConsumable);
        Debug.Log("BeginInitialized");

        UnityPurchasing.Initialize(this, builder);
    }
    //�����õģ���ʽ�����ɾ��
    public void BuyHeart()
    {
        BuyProduct("unlimitedheart");
    }

    //����ʱ���õĽӿڣ��ⲿֻ�������һ���ӿڼ���
    public void BuyProduct(string pruductid)
    {
        //��ʼ����
        m_StoreController.InitiatePurchase(m_StoreController.products.WithID(pruductid));

        
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        //��ʼ���ɹ�
        Debug.Log("In-App Purchasing successfully initialized");
        m_StoreController = controller;
        //StartCoroutine(CheckReceipt());
    }

    IEnumerator CheckReceipt()
    {
        yield return new WaitForEndOfFrame();
        if (m_StoreController != null)
        {
            CheckSubscribeReceiptAndorid();
        }
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        //��ʼ��ʧ��
        OnInitializeFailed(error, null);
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        //��ʼ��ʧ��
        var errorMessage = $"Purchasing failed to initialize. Reason: {error}.";

        if (message != null)
        {
            errorMessage += $" More details: {message}";
        }

        Debug.Log(errorMessage);
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        //Retrieve the purchased product
        var product = args.purchasedProduct;

        LevelManager.Instance.NoCostVitality = true;
        this.GetUtility<SaveDataUtility>().SaveUnlock(1);

        //We return Complete, informing IAP that the processing on our side is done and the transaction can be closed.
        return PurchaseProcessingResult.Complete;
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        //����ʧ��
        Debug.Log($"Purchase failed - Product: '{product.definition.id}', PurchaseFailureReason: {failureReason}");
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
    {
        //����ʧ��
        Debug.Log($"Purchase failed - Product: '{product.definition.id}'," +
            $" Purchase failure reason: {failureDescription.reason}," +
            $" Purchase failure details: {failureDescription.message}");
    }
    //���ȸ趩��״̬�ķ������÷�����Ҫͬʱ��������һ���ű�GooglePurchaseData �����ȸ�֧����receipt
    public void CheckSubscribeReceiptAndorid()
    {
        if(m_StoreController.products != null)
        {
            Product p = m_StoreController.products.WithID("unlimitedheart");
            if (p != null && p.hasReceipt)
            {
                // Debug.Log("recepit all:" + p.receipt);
                GooglePurchaseData data = new GooglePurchaseData(p.receipt);
                if (data.json.purchaseState == "1")
                {
                    LevelManager.Instance.NoCostVitality = true;
                    this.GetUtility<SaveDataUtility>().SaveUnlock(1);
                }
                // Debug.Log("recepit autoRenewing:" + data.json.autoRenewing);
                // /*
                // Debug.Log("recepit orderId:" + data.json.orderId);
                // Debug.Log("recepit packageName:" + data.json.packageName);
                // Debug.Log("recepit productId:" + data.json.productId);
                // Debug.Log("recepit purchaseTime:" + data.json.purchaseTime);
                // Debug.Log("recepit purchaseState:" + data.json.purchaseState);
                // Debug.Log("recepit purchaseToken:" + data.json.purchaseToken);
                //*/
                // if (bool.Parse(data.json.autoRenewing))
                // {
                //     LevelManager.Instance.NoCostVitality = true;
                //     Debug.Log("sub is active");
                // }
                // else
                // {
                //     //CheckWrong();
                // }

            }
        }
       
    }
}

