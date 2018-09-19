using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPurchaseSlot : MonoBehaviour
{
    public Button SlotButton;
    public Text FreeAdsSlot;
    public GameObject PurchaseSlot;
    public Text CostCount;
    public Image RewardIcon;
    public Text RewardCount;

    [System.NonSerialized]
    public CommonData.POINT_TYPE CostType;
    [System.NonSerialized]
    public int Cost;
    [System.NonSerialized]
    public CommonData.POINT_TYPE RewardType;
    [System.NonSerialized]
    public int Reward;
    [System.NonSerialized]
    public int BonusPercent;
    [System.NonSerialized]
    public bool AdsSlot;

    public string PurchaseID;
    public void SetPurchaseFreeSlot()
    {
        FreeAdsSlot.gameObject.SetActive(true);
        PurchaseSlot.gameObject.SetActive(false);
        AdsSlot = true;
        PurchaseID = "";
    }

    public void SetPurchaseSlot(CommonData.POINT_TYPE costType, int cost, CommonData.POINT_TYPE rewardType, int reward, int bonusPercent, string purchaseID = "")
    {
        FreeAdsSlot.gameObject.SetActive(false);
        PurchaseSlot.gameObject.SetActive(true);
        CostType = costType;
        Cost = cost;
        RewardType = rewardType;
        Reward = reward;
        BonusPercent = bonusPercent;
        SetValue(costType, ref CostCount, cost);
        SetIcon(rewardType, ref RewardIcon);
        SetValue(rewardType, ref RewardCount, reward);
        PurchaseID = purchaseID;
        AdsSlot = false;
    }

    private void SetIcon(CommonData.POINT_TYPE type, ref Image icon)
    {
        icon.gameObject.SetActive(true);
        switch (type)
        {
            case CommonData.POINT_TYPE.DDONG:
                CommonFunc.SetImageFile("Renewal/UI/icon_ddong", ref icon);
                break;
            case CommonData.POINT_TYPE.COIN:
                CommonFunc.SetImageFile("Renewal/UI/icon_gold", ref icon);
                break;
            default:
                icon.gameObject.SetActive(false);
                break;
        }
    }

    private void SetValue(CommonData.POINT_TYPE type, ref Text uiText, int value)
    {
        // <Localize id="PURCHASE_SLOT_BONUS_COST_CASH" kr="{0}원&#10;<color=#ff0000>({1}% 보너스)</color>"/>
        if (BonusPercent > 0)
        {
            switch (type)
            {
                case CommonData.POINT_TYPE.DDONG:
                    uiText.text = LocalizeData.Instance.GetLocalizeString("PURCHASE_SLOT_COST_DDONG", CommonFunc.ConvertNumber(value)) + string.Format("\n<color=#ffff00>({0}% 보너스)</color>", BonusPercent);
                    break;
                case CommonData.POINT_TYPE.COIN:
                    uiText.text = LocalizeData.Instance.GetLocalizeString("PURCHASE_SLOT_COST_COIN", CommonFunc.ConvertNumber(value)) + string.Format("\n<color=#ffff00>({0}% 보너스)</color>", BonusPercent);
                    break;
                case CommonData.POINT_TYPE.CASH:
                    uiText.text = LocalizeData.Instance.GetLocalizeString("PURCHASE_SLOT_COST_CASH", CommonFunc.ConvertNumber(value));
                    break;
            }
        }
        else
        {
            switch (type)
            {
                case CommonData.POINT_TYPE.DDONG:
                    uiText.text = LocalizeData.Instance.GetLocalizeString("PURCHASE_SLOT_COST_DDONG", CommonFunc.ConvertNumber(value));
                    break;
                case CommonData.POINT_TYPE.COIN:
                    uiText.text = LocalizeData.Instance.GetLocalizeString("PURCHASE_SLOT_COST_COIN", CommonFunc.ConvertNumber(value));
                    break;
                case CommonData.POINT_TYPE.CASH:
                    uiText.text = LocalizeData.Instance.GetLocalizeString("PURCHASE_SLOT_COST_CASH", CommonFunc.ConvertNumber(value));
                    break;
            }
        }
        
    }
}
