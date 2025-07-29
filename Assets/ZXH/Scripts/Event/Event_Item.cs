using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

/// <summary>
/// 需要物品的事件类
/// </summary>
public class Event_Item : EventBase
{
    [Header("事件属性")]
    [SerializeField] protected List<string> RequiredItems; // 需要的物品列表
    [SerializeField] private TMP_Dropdown itemDropdown;// 下拉框
    [SerializeField] private TMP_InputField countInput;// 数量输入框
    [SerializeField] private Button consumeButton;// 消费按钮——测试用的
    [SerializeField] private TMP_Text errorText; // 用于显示提示信息
    [SerializeField] private bool isConsumable;// 物品是否够消费

    private Inventory inventory;
    private int selectedIndex = -1;
    private int consumeAmount = 0;
    private ItemInfo? selectedItemInfo;// 当前选中的物品信息

    protected override void Awake()
    {
        base.Awake();

        inventory = Character.Instance.inventory;
    }

    protected override void Start()
    {
        base.Start();

        PopulateDropdown();
        itemDropdown.onValueChanged.AddListener(OnDropdownChanged);
        countInput.onValueChanged.AddListener(OnCountChanged);
    }

    private void OnDestroy()
    {
        countInput.onValueChanged.RemoveListener(OnCountChanged);
        itemDropdown.onValueChanged.RemoveListener(OnDropdownChanged);
    }

    public override void Initialize(EventData eventData)
    {
        base.Initialize(eventData);

        RequiredItems = eventData.RequiredItems;
    }

    protected override void ExecutionEvent(EventData eventData)
    {
        isEventActive = true;

        //属性和文本都过关
        if (RollTheDice_CharacterStat(eventData, successProbability) && Character.Instance.HasAllEquipByDefinitionNames(RequiredItems))
        {
            // 成功逻辑
            Result_Story.text = eventData.SuccessfulResults;
            Result_Dice.text = $"成功骰子的个数：{numberOfSuccesses}";
            Reward_Card.text = $"获得：{eventData.RewardItemIDs}"; // 这里可以替换为实际的奖励逻辑

            isSuccess_Event = true;
            GameManager.Instance.RegisterChoice(eventData.SuccessEvent); // 注册成功事件
            GiveRewards_CharacterStat(eventData); // 发放奖励
        }
        //属性过关但物品不满足要求
        else if (RollTheDice_CharacterStat(eventData, successProbability))
        {
            // 成功但没有满足物品要求
            Result_Story.text = eventData.FailedResults + "骰子成功，但没有满足所有物品要求。";
            Result_Dice.text = $"成功骰子的个数：{numberOfSuccesses}";
            Reward_Card.text = "没有奖励";

            GameManager.Instance.RegisterChoice(eventData.FailedEvent); // 注册成功事件
            isSuccess_Event = false;
        }
        //物品满足但属性不满足
        else if (Character.Instance.HasAllEquipByDefinitionNames(RequiredItems))
        {
            // 失败逻辑
            Result_Story.text = eventData.FailedResults + "装备满足，但骰子不满足要求。";
            Result_Dice.text = $"成功骰子的个数：{numberOfSuccesses}";
            Reward_Card.text = "没有奖励";

            GameManager.Instance.RegisterChoice(eventData.FailedEvent); // 注册失败事件
            isSuccess_Event = false;
        }
        //都不满足
        else
        {
            // 失败逻辑
            Result_Story.text = eventData.FailedResults + "骰子和装备都不满足";
            Result_Dice.text = $"成功骰子的个数：{numberOfSuccesses}";
            Reward_Card.text = "没有奖励";

            GameManager.Instance.RegisterChoice(eventData.FailedEvent); // 注册失败事件
            isSuccess_Event = false;
        }

        // 展开Three面板
        ExpandThree();

        //消耗掉所有卡槽中的卡牌
        foreach (var cardSlot in CardSlots)
        {
            var card = cardSlot.GetComponentInChildren<Card>();
            if (card != null)
            {
                Inventory_ZXH.Instance.Backpack.RemoveCard(card.cardData);
            }

        }
    }
}
