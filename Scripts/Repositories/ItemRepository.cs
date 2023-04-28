using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class ItemRepository : Singleton<ItemRepository>
{
    public List<ItemData> Items = new List<ItemData>();

    public string ItemsFileName = "items";

    void Awake()
    {
        Items = new BaseRepository<ItemData>().GetGameData(ItemsFileName);
    }

    public ItemData GetItemByName(string itemName, string[] itemFilter = null)
    {
        if (itemName == "random")
        {
            // If random item (which we usually use for spawning items)
            // Filter the item list if there's any filter.
            var items = (itemFilter.Any() && itemFilter != null)
                ? Items.Where(i => itemFilter.Contains(i.name)).ToList()
                : Items;

            // If there's an issue with our filter, just default to coin
            if (!items.Any())
            {
                // Remove if this is a valid situation.
                Debug.LogWarning("No items after filtering, make sure this is intended");
                return GetCoin(5);
            }

            return items[UnityEngine.Random.Range(0, Items.Count - 1)];
        }

        return Items.Single(s => s.name.ToLowerInvariant() == itemName.ToLowerInvariant());
    }

    public ItemData GetCoin(int coinAmount)
    {
        var coin = Items.Single(s => s.name.ToLowerInvariant() == "coin").Project();

        coin.value = coinAmount + UnityEngine.Random.Range(1, 9);

        return coin;
    }

}

[Serializable]
public class ItemData
{
    public string name;
    public int value;
    public string effect;
    public int rarity;
    public string sprite;

    public ItemData Project()
    {
        return new ItemData()
        {
            name = name,
            value = value,
            effect = effect,
            rarity = rarity,
            sprite = sprite
        };
    }
}
