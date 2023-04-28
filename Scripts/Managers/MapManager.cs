using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapManager : MonoBehaviour
{
    [SerializeField]
    private MapInitializerEngine _mapInitializer;
    [SerializeField]
    private CombatEngine _combatEngine;
    [SerializeField]
    private MovementEngine _movementEngine;

    [SerializeField]
    private KeyCode _up = KeyCode.UpArrow;
    [SerializeField]
    private KeyCode _down = KeyCode.DownArrow;
    [SerializeField]
    private KeyCode _left = KeyCode.LeftArrow;
    [SerializeField]
    private KeyCode _right = KeyCode.RightArrow;

    public Hero Hero;

    // Boolean used to decide if we should act in the Update() method.
    private bool _busy;

    // Start is called before the first frame update
    void Start()
    {
        Hero = GetComponent<Hero>();
        _mapInitializer.GetComponent<MapInitializerEngine>().InitializeMap(Hero, 2);
    }

    // Update is called once per frame
    void Update()
    {
        if (!_busy)
        {
            try
            {
                var touchPos = GetTouch();
                var keyboardInputDetected = KeyboardInputDetected();
                if (touchPos.HasValue || keyboardInputDetected)
                {
                    Card cardTouched = null;
                    if (touchPos.HasValue)
                    {
                        cardTouched = GetCardTouched(touchPos.Value);
                    }
                    else if (keyboardInputDetected)
                    {
                        cardTouched = GetCardFromKeyboardInput();
                    }

                    // If we can move to the card touched
                    if (cardTouched != null && Hero.MapPosition.CanMoveTo(cardTouched.MapPosition))
                    {
                        // Get what type of entity the card is
                        var cardEntity = cardTouched.GetEntity();

                        switch (cardEntity)
                        {
                            // No entity, so just move
                            case ActiveEntity.None:
                                StartCoroutine(MoveHeroToCardCoroutine(cardTouched));
                                break;
                            // Enemy, so fight
                            case ActiveEntity.Enemy:
                                FightEnemy(cardTouched);
                                break;
                            // Item, so move to item and grab it
                            case ActiveEntity.Item:
                                ConsumeItem(cardTouched);
                                break;
                            // Weapon, so move to the weapon and grab it
                            case ActiveEntity.Weapon:
                                GrabWeapon(cardTouched);
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.Log($"{e.InnerException}");
            }
        }
    }

    /// <summary>
    /// Gets a card according to keyboard input
    /// i.e. if the player presses the up arrow we try and get the card above the hero
    /// </summary>
    /// <returns>The card relative to keyboard input, or null if none</returns>
    private Card GetCardFromKeyboardInput()
    {
        Vector2? cardPos = null;
        if (Input.GetKeyDown(_up))
        {
            cardPos = new Vector2(Hero.MapPosition.x - 1, Hero.MapPosition.y);
        }
        else if (Input.GetKeyDown(_down))
        {
            cardPos = new Vector2(Hero.MapPosition.x + 1, Hero.MapPosition.y);
        }
        else if (Input.GetKeyDown(_left))
        {
            cardPos = new Vector2(Hero.MapPosition.x, Hero.MapPosition.y - 1);
        }
        else if (Input.GetKeyDown(_right))
        {
            cardPos = new Vector2(Hero.MapPosition.x, Hero.MapPosition.y + 1);
        }

        if (cardPos.HasValue)
        {
            return CardRepository.Instance.GetCardAtPoint(cardPos.Value);
        }

        return null;
    }

    /// <summary>
    /// Check for keyboard input relative to our mapped keys
    /// </summary>
    /// <returns></returns>
    private bool KeyboardInputDetected()
    {
        return (Input.GetKeyDown(_up) ||
            Input.GetKeyDown(_down) ||
            Input.GetKeyDown(_left) ||
            Input.GetKeyDown(_right));
    }

    /// <summary>
    /// Get any touch registered, can default to mouse click if on PC
    /// </summary>
    /// <returns></returns>
    private Vector2? GetTouch()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            return Input.GetTouch(0).position;
        }
        else if (Input.GetMouseButtonDown(0))
        {
            return Input.mousePosition;
        }

        return null;
    }

    /// <summary>
    /// Grab's the weapon on the card we just touched
    /// </summary>
    /// <param name="cardTouched"></param>
    private void GrabWeapon(Card cardTouched)
    {
        var weapon = cardTouched.GetWeapon();
        _combatEngine.GrabWeapon(Hero, weapon);
        StartCoroutine(MoveHeroToCardCoroutine(cardTouched));
    }

    /// <summary>
    /// Consumes the item on the card we just touched.
    /// </summary>
    /// <param name="cardTouched"></param>
    private void ConsumeItem(Card cardTouched)
    {
        var item = cardTouched.GetItem();
        _combatEngine.ConsumeItem(Hero, item);
        StartCoroutine(MoveHeroToCardCoroutine(cardTouched));
    }

    private void FightEnemy(Card cardTouched)
    {
        var enemy = cardTouched.GetEnemy();
        // Fight enemy
        _combatEngine.BattleEntities(Hero, enemy);

        // If we died during, then save and do death coroutine
        if (!Hero.IsAlive)
        {
            CoinManager.Instance.SaveCoins();
            UserSettingsRepository.Instance.Save();

            StartCoroutine(DeathCoroutine());
        }
        else if (enemy.Name == "Chest")
        {
            StartCoroutine(OpenChestCoroutine(cardTouched));
        }
        // If we are still alive but the enemy isn't, drop loot
        else if (!enemy.IsAlive)
        {
            StartCoroutine(DropLootCoroutine(cardTouched, enemy.MaxHealth));
        }
    }

    private IEnumerator OpenChestCoroutine(Card cardTouched)
    {
        _busy = true;
        cardTouched.PlayEffect("shake");
        yield return new WaitForSeconds(.3f);
        cardTouched.SetSprite(Resources.Load<Sprite>("Sprites/chest_open"));
        yield return new WaitForSeconds(.3f);
        StartCoroutine(DropLootCoroutine(cardTouched, 25));
    }

    /// <summary>
    /// Iterate through the cards and scale them to 0, then transition to game over
    /// </summary>
    /// <returns></returns>
    private IEnumerator DeathCoroutine()
    {
        yield return new WaitForSeconds(1);
        foreach (var card in CardRepository.Instance.GetCards())
        {
            card.transform.DOScale(0, .4f);

            yield return new WaitForSeconds(.25f);
        }

        yield return new WaitForSeconds(.5f);
        SceneManager.LoadScene("GameOver");
    }

    /// <summary>
    /// Switch the card to a coin item.
    /// </summary>
    /// <param name="card"></param>
    /// <returns></returns>
    private IEnumerator DropLootCoroutine(Card card, int coinAmount)
    {
        _busy = true;

        card.FlipAnimation();
        yield return new WaitForSeconds(.5f);
        card.ResetCard();
        _mapInitializer.CreateCoin(card, card.MapPosition, coinAmount);
        yield return new WaitForSeconds(.1f);

        _busy = false;

        yield return true;
    }

    /// <summary>
    /// Move hero to card
    /// Then back fill missing card
    /// </summary>
    /// <param name="card"></param>
    /// <returns></returns>
    private IEnumerator MoveHeroToCardCoroutine(Card card)
    {
        _busy = true;
        var cardPos = card.transform.position;
        var cardMapPos = card.MapPosition;

        CardRepository.Instance.RemoveCardAtPoint(cardMapPos);
        card.DestroyCard();
        yield return new WaitForSeconds(.25f);

        _movementEngine.MoveAndFill(Hero.Card, cardPos, cardMapPos);
        yield return new WaitForSeconds(.4f);

        _busy = false;

        yield return true;
    }

    /// <summary>
    /// Simple raycast to get card touched by user.
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    private Card GetCardTouched(Vector2 pos)
    {
        Ray raycast = Camera.main.ScreenPointToRay(pos);
        RaycastHit raycastHit;
        if (Physics.Raycast(raycast, out raycastHit))
        {
            if (raycastHit.collider.CompareTag("Card"))
            {
                return raycastHit.transform.gameObject.GetComponent<Card>();
            }
        }

        return null;
    }
}
