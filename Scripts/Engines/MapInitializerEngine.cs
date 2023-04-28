using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class MapInitializerEngine : MonoBehaviour
{
    private Dictionary<Vector2, Card> _cards;
    [SerializeField]
    private GameObject _enemyGameObject;

    [SerializeField]
    private GameObject _itemGameObject;

    public void InitializeMap(Hero hero, int tier)
    {
        var gameHero = HeroRepository.Instance.GetHeroWithUpgradesByName(UserSettingsRepository.Instance.UserData.currentHero);

        foreach (var cardOnMap in CardRepository.Instance.GetCards())
        {
            if (cardOnMap.IsHeroCard)
            {
                hero.Init(gameHero.health, gameHero.name, gameHero.sprite, gameHero.effect, gameHero.weapon, cardOnMap);
                cardOnMap.Init(hero);
            }
            else
            {
                GenerateCard(cardOnMap, cardOnMap.MapPosition);
            }
        }
    }

    public void CreateItem(Card cardOnMap, Vector2 mapPosition, string itemName)
    {
        var itemFilter = HeroRepository.Instance.GetCurrentActiveHero().itemFilter;

        var randomItem = ItemRepository.Instance.GetItemByName(itemName, itemFilter);
        var itemGo = Instantiate<GameObject>(_itemGameObject);
        var item = itemGo.GetComponent<Item>();
        item.Init(randomItem.value,
            randomItem.name,
            randomItem.effect,
            randomItem.sprite,
            cardOnMap);

        cardOnMap.Init(item);
    }

    public void CreateCoin(Card cardOnMap, Vector2 mapPosition, int coinAmount = 5)
    {
        var coin = ItemRepository.Instance.GetCoin(coinAmount);
        var itemGo = Instantiate<GameObject>(_itemGameObject);
        var item = itemGo.GetComponent<Item>();
        item.Init(coin.value,
            coin.name,
            coin.effect,
            coin.sprite,
            cardOnMap);

        cardOnMap.Init(item);
    }

    public void CreateEnemy(Card cardOnMap, Vector2 mapPos)
    {
        var randomEnemy = EnemyRepository.Instance.GetRandomEnemyByTier(TierEngine.Instance.GetCurrentTier());
        var enemyGo = Instantiate(_enemyGameObject);
        var enemy = enemyGo.GetComponent<Enemy>();
        enemy.Init(
            ValueDevianceEngine.Instance.CalculateDeviance(randomEnemy.health, randomEnemy.healthDeviance),
            randomEnemy.tier,
            randomEnemy.name,
            randomEnemy.sprite,
            cardOnMap);

        cardOnMap.Init(enemy);
    }

    public void CreateWeapon(Card cardOnMap, Vector2 mapPosition)
    {
        var weaponFilter = HeroRepository.Instance.GetCurrentActiveHero().weaponFilter;

        var weapon = WeaponRepository.Instance.GetWeaponByTierWeight(weaponFilter);
        var weaponGo = Instantiate(_itemGameObject);
        var weaponComponent = weaponGo.GetComponent<Weapon>();
        weaponComponent.Init(weapon.value,
            weapon.name,
            weapon.effect,
            weapon.sprite,
            weapon.soundPrefix,
            cardOnMap);

        cardOnMap.Init(weaponComponent);
    }

    private void CreateChest(Card cardOnMap, Vector2 mapPosition)
    {
        var chest = EnemyRepository.Instance.GetEnemyByName("Chest");
        var enemyGo = Instantiate(_enemyGameObject);
        var enemy = enemyGo.GetComponent<Enemy>();
        enemy.Init(
            ValueDevianceEngine.Instance.CalculateDeviance(chest.health, chest.healthDeviance),
            chest.tier,
            chest.name,
            chest.sprite,
            cardOnMap);

        cardOnMap.Init(enemy);
    }

    /// <summary>
    /// Contains the values to determine what we spawn
    /// </summary>
    /// <param name="cardOnMap"></param>
    /// <param name="mapPosition"></param>
    public void GenerateCard(Card cardOnMap, Vector2 mapPosition)
    {
        var randomInt = Random.Range(1, 100);

        if (randomInt.IsBetween(1,5))
        {
            CreateItem(cardOnMap, cardOnMap.MapPosition, "random");
            return;
        }
        else if (randomInt.IsBetween(5, 10))
        {
            CreateChest(cardOnMap, cardOnMap.MapPosition);
            return;
        }
        else if (randomInt.IsBetween(10,20))
        {
            CreateWeapon(cardOnMap, cardOnMap.MapPosition);
            return;
        }


        CreateEnemy(cardOnMap, cardOnMap.MapPosition);
        return;
    }
}