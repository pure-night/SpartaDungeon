using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDB : MonoBehaviour
{
    [SerializeField] private List<ItemData> itemValue;
    private Dictionary<string, ItemData> _itemDictionary;

    public void InitDict()
    {
        _itemDictionary = new Dictionary<string, ItemData>();

        foreach (var item in itemValue)
        {
            _itemDictionary.Add(item.itemKey, item);
        }
    }

    public ItemData GetItemData(string key)
    {
        return _itemDictionary.TryGetValue(key, out var itemData) ? itemData : null;
    }
}
