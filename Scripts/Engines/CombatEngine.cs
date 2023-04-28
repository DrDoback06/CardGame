using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatEngine : MonoBehaviour
{
    public void BattleEntities(Hero hero, Enemy enemy)
    {
        if (enemy.Card.Name == "Chest")
        {
            AudioManager.Instance.PlaySound("chest");
            Debug.Log("Chest fight");
            return;
        }

        if (hero.Damage != 0)
        {

            var baseDamage = Mathf.Max(0, Mathf.Min(hero.Damage, enemy.Health));
            var damageToApply = baseDamage;
            switch (hero.Weapon.effect)
            {
                case "Gold":
                    CoinManager.Instance.AddCoins(damageToApply);
                    break;
                case "Leech":
                    hero.Heal(damageToApply);
                    break;
                default:
                    break;
            }
            switch (hero.Effect)
            {
                default:
                    break;
            }

            AudioManager.Instance.PlaySound(hero.Weapon.soundPrefix);

            enemy.TakeDamage(damageToApply);
            EffectManager.Instance.PlaySlashEffect(enemy.Card.transform.position);

            hero.SetDamage(hero.Damage - baseDamage);

            if (hero.Damage <= 0)
            {
                hero.RemoveWeapon();
            }

        }
        else if (hero.Damage == 0)
        {
            AudioManager.Instance.PlaySound("hero_hurt");
            var heroDamage = hero.Health;
            var enemyDamage = enemy.Health;

            hero.TakeDamage(enemyDamage);
            enemy.TakeDamage(heroDamage);
        }
    }

    public void ConsumeItem(Hero hero, Item item)
    {
        switch (item.Effect)
        {
            case "Heal":
                hero.Heal(item.Value);
                AudioManager.Instance.PlaySound("bottle");
                break;
            case "Coin":
                CoinManager.Instance.AddCoins(item.Value);
                AudioManager.Instance.PlaySound("coin_pickup");
                break;
            default:
                break;
        }
    }

    public void GrabWeapon(Hero hero, Weapon weapon)
    {
        if (weapon.Damage > hero.Damage)
        {
            AudioManager.Instance.PlaySound("weapon_pickup");
            hero.AddWeapon(weapon.Name, weapon.Effect, weapon.Damage, 1, weapon.Sprite, weapon.SoundPrefix);
        }
        else
        {
            AudioManager.Instance.PlaySound("coin_pickup");
            CoinManager.Instance.AddCoins(weapon.Damage);
        }
    }
}
