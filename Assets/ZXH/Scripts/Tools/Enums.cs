public enum CardType
{
    Role,   // ��ɫ
    Intel,  // �鱨
    Book,   // �鼮
    Coin,    // ���
    Equip,   // װ��
}

/// <summary>
/// �¼����͡������������¼��Ĵ�����ʽ
/// </summary>
public enum EventType
{
    Fixed,   // �̶��¼�
    Timed    // ʱ���¼�
}

/// <summary>
/// ѡ�����͡������������¼��Ĵ�����ʽ
/// </summary>
public enum SelectType
{
    Card,    // ����ѡ��
    Option   // ѡ��ѡ��
}

/// <summary>
/// 事件触发类型——用于区分事件的触发方式
/// </summary>
public enum EventTriggerType
{
    AllDay,  // 一直存在
    RoleCondition,     // 角色触发
    ItemCondition,     // 物品触发
    TimeCondition,     // 时间触发
    AttributeCondition, // 属性触发
    EventCondition,    // 事件触发
    DirectCondition, // 直接触发
    RandomCondition,   // 随机触发
}
