using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CardDetailPanel : MonoBehaviour
{
    public static CardDetailPanel Instance { get; private set; }

    [Header("CardDetailPanel����")]
    public GameObject cardDetailPanel;
    public TextMeshProUGUI cardNameText;//
    public TextMeshProUGUI idText;
    public TextMeshProUGUI typeText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI attributesText;
    public Image artworkImage;


    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        // ֻ����弤��ʱ����Ӧ
        if (cardDetailPanel.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Hide();
            }
        }
    }


    public void Show(CardData data, Vector3 position)
    {
        if (data == null) return;

        cardDetailPanel.SetActive(true);

        cardNameText.text = $"����: {data.cardName}";
        idText.text = $"ID: {data.id}";
        //typeText.text = $"����: {data.cardType}";
        descriptionText.text = $"����: {data.description}";
        attributesText.text = GetAttributesString(data.GetAttributes());
        if (artworkImage != null && data.artwork != null)
            artworkImage.sprite = data.artwork;

        // �������λ�ã��ɸ���UI�������ê��/ƫ�ƣ�
        transform.position = position;
    }

    public void Hide()
    {
        cardDetailPanel.SetActive(false);
    }

    private string GetAttributesString(Attributes attr)
    {
        return $"����:{attr.physique} �罻:{attr.social} ����:{attr.survival} �ǻ�:{attr.intelligence} ����:{attr.charm} ս��:{attr.combat} ֧��:{attr.support}";
    }
}
