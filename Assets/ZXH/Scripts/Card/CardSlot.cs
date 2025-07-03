// CardSlot.cs
using UnityEngine;
using UnityEngine.EventSystems;

public class CardSlot : MonoBehaviour, IDropHandler
{
    [Header("���Ʋ�����")]
    public CardType acceptedCardType;
    public int level;
    public Card child; // ��ǰ�����ڵĿ��ƣ�����еĻ���

    // �������屻��ק���˿����ϲ��ͷ�ʱ����
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("OnDrop event triggered on " + gameObject.name);

        // 1. ��ȡ����ק������
        GameObject droppedObject = eventData.pointerDrag;
        if (droppedObject == null) return;

        // 2. ��ȡ�������ϵ� Card �ű�
        Card card = droppedObject.GetComponent<Card>();
        if (card == null) return; // ��������Ĳ��ǿ��ƣ��򲻴���

        // 3. �����߼����жϿ��������Ƿ�ƥ��
        if (card.cardData.cardType == acceptedCardType)
        {
            // ����ƥ�䣬�������
            Debug.Log($"�ɹ����ÿ���: {card.cardData.cardName} ������: {gameObject.name}");

            // �����������Ѿ��п����ˣ�����ʵ�ֽ����߼�����ѡ��
            // �������������򵥵ģ�ֱ�����¿��Ƴ�Ϊ������
            if (transform.childCount > 0)
            {
                // ���Խ��ɿ��Ƶ���ԭ�������ߺ��������ƽ���λ��
                // Ϊ�򻯣��˴��ݲ�����
                Debug.Log("�����ѱ�ռ�ã�");
                return; // ����ִ�н����߼�
            }

            // 4. ��������
            // �����Ƶĸ���������Ϊ��ǰ����
            card.transform.SetParent(this.transform);
            // �����Ƶ�λ�����õ���������
            card.transform.localPosition = Vector3.zero;

            // ֪ͨ�������Ѿ����ɹ�����
            card.OnPlacedSuccessfully(this.transform);
        }
        else
        {
            // ���Ͳ�ƥ�䣬���ƻ��Զ�����ԭλ����Card.cs��ʵ�֣�
            Debug.LogWarning($"���Ͳ�ƥ��! ������Ҫ {acceptedCardType}, ���������� {card.cardData.cardType}.");
        }
    }
}