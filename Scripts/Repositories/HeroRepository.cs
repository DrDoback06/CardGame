using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class HeroRepository : Singleton<HeroRepository>
{
    public List<HeroData> Heroes = new List<HeroData>();
    public string HeroesFileName = "heroes";

    void Awake()
    {
        Heroes = new BaseRepository<HeroData>().GetGameData(HeroesFileName);
    }

    public List<HeroData> GetHeroesByPrecedence()
    {
        return Heroes.OrderBy(o => o.precedence).ToList();
    }

    public HeroData GetHeroWithUpgradesByName(string name)
    {
        var hero = ApplyUpgrades(Heroes.Single(s => s.name.ToLowerInvariant() == name.ToLowerInvariant()).Project());
        return hero;
    }

    public HeroData GetHeroWithUpgradesByPrecedence(int precedence)
    {
        var hero = ApplyUpgrades(Heroes.Single(s => s.precedence == precedence).Project());
        return hero;
    }

    public HeroData GetCurrentActiveHero()
    {
        var hero = UserSettingsRepository.Instance.UserData.currentHero;
        return Heroes.Single(s => s.name == hero);
    }

    private HeroData ApplyUpgrades(HeroData hero)
    {
        var heroUpgradeLevel = UserSettingsRepository.Instance.GetUpgradesForHero(hero.name);
        hero.health += heroUpgradeLevel;
        hero.nextUpgradeCost = hero.upgradeCosts[Mathf.Min(heroUpgradeLevel, hero.upgradeCosts.Count() - 1)];
        return hero;
    }

    public int GetMaxPrecidence()
    {
        return Heroes.Max(m => m.precedence);
    }

}

[Serializable]
public class HeroData
{
    public int health;
    public string name;
    public string sprite;
    public string weapon;
    public string effect;
    public string description;
    public int[] upgradeCosts;
    public string[] weaponFilter;
    public string[] itemFilter;
    public int nextUpgradeCost;
    public int precedence;

    public HeroData Project()
    {
        return new HeroData()
        {
            health = health,
            name = name,
            sprite = sprite,
            weapon = weapon,
            effect = effect,
            description = description,
            upgradeCosts = upgradeCosts,
            weaponFilter = weaponFilter,
            itemFilter = itemFilter,
            nextUpgradeCost = nextUpgradeCost,
            precedence = precedence
        };
    }
}