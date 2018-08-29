using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPurchaseSlot : MonoBehaviour
{
    public Button SlotButton;
    public Image CostIcon;
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

    public void SetPurchaseSlot(CommonData.POINT_TYPE costType, int cost, CommonData.POINT_TYPE rewardType, int reward)
    {
        CostType = costType;
        Cost = cost;
        RewardType = rewardType;
        Reward = reward;
        SetIcon(costType, ref CostIcon);
        SetValue(costType, ref CostCount, cost);
        SetIcon(rewardType, ref RewardIcon);
        SetValue(rewardType, ref RewardCount, reward);
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
        switch (type)
        {
            case CommonData.POINT_TYPE.DDONG:
                uiText.text = LocalizeData.Instance.GetLocalizeString("PURCHASE_SLOT_COST_DDONG", value);
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
