using System;
using UnityEngine;

public class Hero : Pawn
{
    public WeaponData Weapon;
    public int Damage => Weapon != null ? Weapon.value : 0;
    public string Effect;

    public void Init(int health, string name, string spriteLoc, string effect, string weaponString, Card card)
    {
        Health = health;
        MaxHealth = Health;

        Effect = effect;
        Name = name;
        Card = card;

        AddWeapon(WeaponRepository.Instance.GetWeapon(weaponString));
        Sprite = Resources.Load<Sprite>(spriteLoc);
    }

    public void SetDamage(int damage)
    {
        Weapon.value = damage;
        Card.SetDamage(Weapon.value);
    }

    public void AddWeapon(WeaponData weapon)
    {
        Weapon = weapon;
        Card.SetWeaponSprite(Resources.Load<Sprite>(weapon.sprite));
        Card.SetDamage(Weapon.value);
    }

    public void AddWeapon(string name, string effect, int damage, int rarity, Sprite sprite, string soundPrefix)
    {
        Weapon = new WeaponData()
        {
            name = name,
            effect = effect,
            value = damage,
            rarity = rarity,
            soundPrefix = soundPrefix
        };
        Card.SetWeaponSprite(sprite);
        Card.SetDamage(Weapon.value);
    }

    public void RemoveWeapon()
    {
        Card.SetWeaponSprite(null);
        Card.SetDamage(0);
        Weapon = null;
    }

}
