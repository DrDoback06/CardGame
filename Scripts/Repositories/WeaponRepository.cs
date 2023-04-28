using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WeaponRepository : Singleton<WeaponRepository>
{
    public List<WeaponData> Weapons = new List<WeaponData>();
    public string WeaponsFileName = "weapons";

    void Awake()
    {
        Weapons = new BaseRepository<WeaponData>().GetGameData(WeaponsFileName);
    }

    /// <summary>
    /// Get weapon by name
    /// Generally use this when you know the weapon you want to retrieve
    /// For spawning purposes, use GetWeaponByTierWeight
    /// </summary>
    /// <param name="weaponName"></param>
    /// <returns></returns>
    public WeaponData GetWeapon(string weaponName)
    {
        return Weapons.Single(s => s.name.ToLowerInvariant() == weaponName.ToLowerInvariant()).Project();
    }

    /// <summary>
    /// Calculate what rarity we should use
    /// find a weapon with that rarity
    /// if there's no weapon with that rarity
    /// recursively reduce the rarity until we find one
    /// </summary>
    /// <returns></returns>
    public WeaponData GetWeaponByTierWeight(string[] weaponFilter = null)
    {
        var rarity = RarityEngine.Instance.CalculateRarityByTier();
        var weapons = new List<WeaponData>();
        while(!weapons.Any())
        {
            if (rarity < 0) return GetWeapon("sword");

            weapons = Weapons.Where(w => w.rarity == rarity).ToList();
            if (!weapons.Any()) rarity--;
        }
        
        if(weaponFilter != null && weaponFilter.Any())
        {
            weapons = weapons.Where(w => weaponFilter.Contains(w.name)).ToList();
        }

        // If after applying our filter we have no weapons
        // Default to sword. Change this based on how you want your filters to work
        // Otherwise ensure there's a valid item for each rarity
        if(!weapons.Any()) return GetWeapon("sword");

        return weapons[UnityEngine.Random.Range(0, weapons.Count)].Project();
    }
}

[Serializable]
public class WeaponData
{
    public string name;
    public int value;
    public int[] valueDeviance;
    public string effect;
    public int rarity;
    public string sprite;
    public string soundPrefix;

    /// <summary>
    /// Since we modify WeaponData (reduce damage when used)
    /// We create a new instance of the object to act on
    /// </summary>
    /// <returns></returns>
    public WeaponData Project()
    {
        return new WeaponData()
        {
            name = name,
            valueDeviance = valueDeviance,
            rarity = rarity,
            effect = effect,
            sprite = sprite,
            value = value,
            soundPrefix = soundPrefix,
        };
    }
}
