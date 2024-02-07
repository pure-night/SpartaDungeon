using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ItemSlot
{
    public ItemData item;
    public int quantity;
}

public class Inventory : MonoBehaviour
{
    public ItemSlotUI[] uiSlots;
    public ItemSlot[] slots;
    public int gold;

    public GameObject inventoryWindow;
    public GameObject itemPopupWindow;
    public Transform dropPosition;
    private CharacterStatsHandler _playerStatsHandler;

    [Header("Selected Item")]
    private ItemSlot _selectedItem;
    private int _selectedItemIndex;
    public TextMeshProUGUI selectedItemName;
    public TextMeshProUGUI selectedItemDescription;
    public GameObject selectedItemAtkStat;
    public GameObject selectedItemDefStat;
    public GameObject selectedItemSpdStat;
    public Image selectedItemIcon;
    public TextMeshProUGUI selectedItemAtkStatValues;
    public TextMeshProUGUI selectedItemDefStatValues;
    public TextMeshProUGUI selectedItemSpdStatValues;

    private int _curWeaponEquipIndex = -1;
    private int _curArmorEquipIndex = -1;

    [Header("Events")]
    public UnityEvent onOpenInventory;
    public UnityEvent onCloseInventory;

    public static Inventory Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
    
    private void Start()
    {
        _playerStatsHandler = GameManager.Instance.player.GetComponent<CharacterStatsHandler>();
        
        inventoryWindow.SetActive(false);
        slots = new ItemSlot[uiSlots.Length];

        for (var i = 0; i < slots.Length; i++)
        {
            slots[i] = new ItemSlot();
            uiSlots[i].index = i;
            uiSlots[i].Clear();
        }

        AddItem(GameManager.Instance.itemDB.GetItemData("pistol"));
        ClearSelectedItemWindow();
    }

    public void OnInventoryButton()
    {
        Toggle();
    }
    
    private void Toggle()
    {
        if (inventoryWindow.activeInHierarchy)
        {
            inventoryWindow.SetActive(false);
            onCloseInventory?.Invoke();
        }
        else
        {
            inventoryWindow.SetActive(true);
            onOpenInventory?.Invoke();
        }
    }
    
    public bool IsOpen()
    {
        return inventoryWindow.activeInHierarchy;
    }

    public void AddItem(ItemData item)
    {
        if (item == null)
            return;
        
        if(item.canStack)
        {
            var slotToStackTo = GetItemStack(item);
            if(slotToStackTo != null)
            {
                slotToStackTo.quantity++;
                UpdateUI();
                return;
            }
        }

        var emptySlot = GetEmptySlot();
        if(emptySlot != null)
        {
            emptySlot.item = item;
            emptySlot.quantity = 1;
            UpdateUI();
            return;
        }

        ThrowItem(item);
    }

    private void ThrowItem(ItemData item)
    {
        var temp = Instantiate(item.dropPrefab, dropPosition);
        temp.transform.SetParent(null);
    }

    private void UpdateUI()
    {
        for (var i = 0; i < slots.Length; i++)
        {
            if (slots[i].item != null)
                uiSlots[i].Set(slots[i]);
            else
                uiSlots[i].Clear();
        }
    }

    private ItemSlot GetItemStack(ItemData item)
    {
        for (var i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == item && slots[i].quantity < item.maxStackAmount)
                return slots[i];
        }

        return null;
    }

    private ItemSlot GetEmptySlot()
    {
        for (var i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
                return slots[i];
        }

        return null;
    }

    public void SelectItem(int index)
    {
        if (slots[index].item == null)
            return;

        itemPopupWindow.SetActive(true);
        
        _selectedItem = slots[index];
        _selectedItemIndex = index;

        selectedItemName.text = _selectedItem.item.displayName;
        selectedItemDescription.text = _selectedItem.item.description;
        selectedItemIcon.sprite = _selectedItem.item.icon;
        
        foreach (var itemData in _selectedItem.item.additionalItemStats)
        {
            switch (itemData.type)
            {
                case ValueType.Power:
                    selectedItemAtkStat.SetActive(true);
                    selectedItemAtkStatValues.text = itemData.value.ToString();
                    break;
                case ValueType.Defense:
                    selectedItemDefStat.SetActive(true);
                    selectedItemDefStatValues.text = itemData.value.ToString();
                    break;
                case ValueType.MoveSpeed:
                    selectedItemSpdStat.SetActive(true);
                    selectedItemSpdStatValues.text = itemData.value.ToString();
                    break;
            }
        }
    }

    public void ClearSelectedItemWindow()
    {
        itemPopupWindow.SetActive(false);
        
        _selectedItem = null;

        selectedItemAtkStat.SetActive(false);
        selectedItemDefStat.SetActive(false);
        selectedItemSpdStat.SetActive(false);
    }

    public void OnConfirmButton()
    {
        switch (_selectedItem.item.type)
        {
            case ItemType.Consumable:
                Consume();
                break;
            case ItemType.Weapon:
            case ItemType.Armor:
                if (_selectedItemIndex == _curWeaponEquipIndex || _selectedItemIndex == _curArmorEquipIndex)
                {
                    UnEquip(_selectedItemIndex);
                    if (_selectedItem.item.type == ItemType.Weapon)
                        _curWeaponEquipIndex = -1;
                    else
                        _curArmorEquipIndex = -1;
                }
                else
                    Equip();
                break;
        }

        ClearSelectedItemWindow();
    }

    public void Consume()
    {
        for (var i = 0; i < _selectedItem.item.additionalItemStats.Length; i++)
        {
            switch (_selectedItem.item.additionalItemStats[i].type)
            {
                
            }
        }
    }

    private void Equip()
    {
        if (_curWeaponEquipIndex >= 0 && _selectedItem.item.type == ItemType.Weapon && uiSlots[_curWeaponEquipIndex].equipped)
        {
            UnEquip(_curWeaponEquipIndex);
        }
        else if (_curArmorEquipIndex >= 0 && _selectedItem.item.type == ItemType.Armor && uiSlots[_curArmorEquipIndex].equipped)
        {
            UnEquip(_curArmorEquipIndex);
        }

        uiSlots[_selectedItemIndex].equipped = true;
        _playerStatsHandler.AddStatModifier(_selectedItem.item.statMultiplier);
        if (_selectedItem.item.type == ItemType.Weapon)
            _curWeaponEquipIndex = _selectedItemIndex;
        else if (_selectedItem.item.type == ItemType.Armor)
            _curArmorEquipIndex = _selectedItemIndex;
        
        UpdateUI();
    }

    private void UnEquip(int index)
    {
        uiSlots[index].equipped = false;
        _playerStatsHandler.RemoveStatModifier(slots[index].item.statMultiplier);
        
        UpdateUI();
    }

    public void OnDropButton()
    {
        ThrowItem(_selectedItem.item);
        RemoveSelectedItem();
    }

    private void RemoveSelectedItem()
    {
        _selectedItem.quantity--;

        if (_selectedItem.quantity <= 0)
        {
            if (uiSlots[_selectedItemIndex].equipped)
            {
                UnEquip(_selectedItemIndex);
            }

            _selectedItem.item = null;
        }

        UpdateUI();
    }

    public void RemoveItem(ItemData item)
    {

    }

    public bool HasItems(ItemData item, int quantity)
    {
        return false;
    }
}
