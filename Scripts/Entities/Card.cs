using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Card : MonoBehaviour
{
    [SerializeField]
    public Vector2 MapPosition;

    public bool IsHeroCard = false;

    private SpriteRenderer _entitySpriteRenderer;
    private SpriteRenderer _weaponSpriteRenderer;

    [SerializeField]
    private GameObject _weaponSprite;
    [SerializeField]
    private GameObject _entitySprite;

    [SerializeField]
    private GameObject _damageTextObject;
    private TextMeshProUGUI _damageText;

    [SerializeField]
    private GameObject _heartImage;
    [SerializeField]
    private GameObject _healthTextObject;
    private TextMeshProUGUI _healthText;
    [SerializeField]
    private GameObject _nameTextObject;
    private TextMeshProUGUI _nameText;
    [SerializeField]
    private GameObject _valueTextObject;
    private TextMeshProUGUI _valueText;

    [SerializeField]
    private GameObject _heroBorder;

    [SerializeField]
    private ActiveEntity _activeEntity;

    [SerializeField]
    private Enemy _activeEnemy;
    [SerializeField]
    private Item _activeItem;
    [SerializeField]
    private Weapon _activeWeapon;

    public string Name;
    public int? Damage;
    public int? Value;
    public int? Health;
    public int? MaxHealth;

    /// <summary>
    /// Get all the components on the Card
    /// </summary>
    public void Awake()
    {
        _entitySpriteRenderer = _entitySprite.GetComponent<SpriteRenderer>();
        _weaponSpriteRenderer = _weaponSprite.GetComponent<SpriteRenderer>();

        _damageText = _damageTextObject.GetComponent<TextMeshProUGUI>();
        _healthText = _healthTextObject.GetComponent<TextMeshProUGUI>();
        _nameText = _nameTextObject.GetComponent<TextMeshProUGUI>();
        _valueText = _valueTextObject.GetComponent<TextMeshProUGUI>();
    }

    /// <summary>
    /// Used to represent an enemy on the card.
    /// </summary>
    /// <param name="enemy"></param>
    public void Init(Enemy enemy)
    {
        SetHealth(enemy.Health);
        SetName(enemy.Name);
        SetSprite(enemy.Sprite);
        _activeEntity = ActiveEntity.Enemy;
        _activeEnemy = enemy;
    }

    /// <summary>
    /// Used to represent a hero on the card.
    /// </summary>
    /// <param name="hero"></param>
    public void Init(Hero hero)
    {
        SetDamage(hero.Weapon.value);
        SetHealth(hero.Health);
        SetName(hero.Name);
        SetSprite(hero.Sprite);
        _activeEntity = ActiveEntity.Hero;
        _heroBorder.SetActive(true);
    }

    /// <summary>
    /// Used to represent an item on the card.
    /// </summary>
    /// <param name="item"></param>
    public void Init(Item item)
    {
        SetName(item.Name);
        SetSprite(item.Sprite);
        SetValue(item.Value);
        _activeEntity = ActiveEntity.Item;
        _activeItem = item;
    }

    /// <summary>
    /// Used to represent a weapon on the card.
    /// </summary>
    /// <param name="weapon"></param>
    public void Init(Weapon weapon)
    {
        SetName(weapon.Name);
        SetSprite(weapon.Sprite);
        SetValue(weapon.Damage);
        _activeEntity = ActiveEntity.Weapon;
        _activeWeapon = weapon;
    }

    /// <summary>
    /// Used to remove all data of the entity on the card.
    /// </summary>
    public void ResetCard()
    {
        SetDamage(null);
        SetHealth(null);
        SetName(null);
        SetSprite(null);
        SetValue(null);
        ResetActiveEntity();
        _heroBorder.SetActive(false);
    }

    /// <summary>
    /// Only one entity can be active at a time, so we use this to reset all of the entities at once.
    /// </summary>
    private void ResetActiveEntity()
    {
        _activeEntity = ActiveEntity.None;
        _activeWeapon = null;
        _activeEnemy = null;
        _activeItem = null;
    }

    /// <summary>
    /// Set the value of damage on the card.
    /// If null or 0, sets damage and damageText to null.
    /// </summary>
    /// <param name="damage"></param>
    public void SetDamage(int? damage)
    {
        if (damage.HasValue && damage.Value != 0)
        {
            Damage = damage.Value;
            _damageText.text = Damage.ToString();
            _damageText.transform.DOScale(1.5f, .1f).OnComplete(() => _damageText.transform.DOScale(1, .1f));
        }
        else
        {
            Damage = null;
            _damageText.text = string.Empty;
        }

    }

    /// <summary>
    /// Flip's the card, can be used to swap entity in a cool looking way.
    /// </summary>
    public void FlipAnimation()
    {
        gameObject.transform.DORotate(new Vector3(0, 180, 0), .5f).onComplete = () =>
        {
            gameObject.transform.DORotate(new Vector3(0, 0, 0), .5f);
        };
    }

    /// <summary>
    /// Sets the health of the card
    /// If null, sets health, healthText and the heart image to null
    /// Also tweens the healthText to visually show change
    /// </summary>
    /// <param name="health"></param>
    /// <param name="maxHealth"></param>
    public void SetHealth(int? health, int? maxHealth = null)
    {
        if (health == Health) return;

        if (maxHealth.HasValue) MaxHealth = maxHealth.Value;

        if (health.HasValue && health > 0)
        {
            Health = health;
            _healthText.text = Health.ToString();
            _heartImage.SetActive(true);
        }
        else
        {
            Health = null;
            _healthText.text = string.Empty;
            _heartImage.SetActive(false);
        }

        _healthText.DOColor(Color.yellow, .1f).OnComplete(() => _healthText.DOColor(Color.white, .1f));
        _healthText.transform.DOScale(1.5f, .1f).OnComplete(() => _healthText.transform.DOScale(1, .1f));
    }

    /// <summary>
    /// Sets the name on the card
    /// </summary>
    /// <param name="name"></param>
    public void SetName(string name)
    {
        Name = name;
        _nameText.text = name;
    }

    /// <summary>
    /// Sets the 'Value' on the card
    /// </summary>
    /// <param name="value"></param>
    public void SetValue(int? value)
    {
        if (value.HasValue)
        {
            Value = value;
            _valueText.text = value.ToString();
        }
        else
        {
            Value = value;
            _valueText.text = string.Empty;
        }
    }

    /// <summary>
    /// Sets the sprite image on the card
    /// </summary>
    /// <param name="newSprite"></param>
    public void SetSprite(Sprite newSprite)
    {
        _entitySpriteRenderer.sprite = newSprite;
    }

    /// <summary>
    /// Gets the sprite image on the card
    /// </summary>
    /// <returns></returns>
    public Sprite GetSprite()
    {
        return _entitySpriteRenderer.sprite;
    }

    /// <summary>
    /// Sets the weapon sprite on the card
    /// </summary>
    /// <param name="weaponSprite"></param>
    public void SetWeaponSprite(Sprite weaponSprite)
    {
        _weaponSpriteRenderer.sprite = weaponSprite;
    }

    /// <summary>
    /// Gets the active entity on the card
    /// </summary>
    /// <returns></returns>
    public ActiveEntity GetEntity()
    {
        return _activeEntity;
    }

    /// <summary>
    /// Gets the active enemy on the card
    /// </summary>
    /// <returns></returns>
    public Enemy GetEnemy()
    {
        return _activeEnemy;
    }

    /// <summary>
    /// Gets the active Item on the card
    /// </summary>
    /// <returns></returns>
    public Item GetItem()
    {
        return _activeItem;
    }

    /// <summary>
    /// Gets the active weapon on the card
    /// </summary>
    /// <returns></returns>
    public Weapon GetWeapon()
    {
        return _activeWeapon;
    }

    /// <summary>
    /// Tweens the card to the target position.
    /// </summary>
    /// <param name="cardPos"></param>
    /// <param name="cardMapPos"></param>
    public void MoveTo(Vector3 cardPos, Vector2 cardMapPos)
    {
        MapPosition = cardMapPos;
        gameObject.transform.DOMove(cardPos, .5f);
    }

    /// <summary>
    /// Shrinks the card then destroys it.
    /// </summary>
    /// <returns></returns>
    public Tweener DestroyCard()
    {
        var tween = gameObject.transform.DOScale(0, .2f);
        tween.onComplete = () => { Destroy(gameObject); };
        return tween;
    }


    public Tweener PlayEffect(string effectName)
    {
        switch (effectName)
        {
            case "shake":
                return _entitySprite.transform.DOShakePosition(.5f, .1f, 20);
            default:
                break;
        }

        return null;
    }

}

public enum ActiveEntity
{
    None,
    Enemy,
    Hero,
    Item,
    Weapon
}
