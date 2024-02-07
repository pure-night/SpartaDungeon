using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlotUI : MonoBehaviour
{
    public Button button;
    public Image icon;
    public TextMeshProUGUI quantityText;
    private ItemSlot _curSlot;
    public GameObject equipMark;

    public int index;
    public bool equipped;

    private void OnEquip()
    {
        equipMark.SetActive(true);
    }

    public void Set(ItemSlot slot)
    {
        _curSlot = slot;
        icon.gameObject.SetActive(true);
        icon.sprite = slot.item.icon;
        quantityText.text = slot.quantity > 1 ? slot.quantity.ToString() : string.Empty;

        equipMark.SetActive(equipped);
    }

    public void Clear()
    {
        _curSlot = null;
        icon.gameObject.SetActive(false);
        equipMark.SetActive(false);
        quantityText.text = string.Empty;
    }

    public void OnButtonClick()
    {
        Inventory.Instance.SelectItem(index);
    }
}
