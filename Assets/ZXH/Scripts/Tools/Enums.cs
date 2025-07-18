
public enum CardType
{
    Role,   // 角色
    Intel,  // 情报
    Book,   // 书籍
    Coin,    // 金币
    Equip,   // 装备
}

/// <summary>
/// 事件类型——用于区分事件的触发方式
/// </summary>
public enum EventType
{
    Fixed,   // 固定事件
    Timed    // 时间事件
}

/// <summary>
/// 选择类型——用于区分事件的处理方式
/// </summary>
public enum SelectType
{
    Card,    // 卡牌选择
    Option   // 选项选择
}
