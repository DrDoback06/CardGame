using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pawn : MonoBehaviour
{
    public Sprite Sprite;
    public Card Card;
    public Vector2 MapPosition => Card.MapPosition;
    public int Health;
    public int MaxHealth;
    public string Name;

    public bool IsAlive => Health > 0;

    public void TakeDamage(int damage)
    {
        Health = Mathf.Max(0, Health - damage);
        Card.SetHealth(Health);
    }

    public void Heal(int value)
    {
        Health = Mathf.Min(MaxHealth, Health + value);
        Card.SetHealth(Health);
    }
}
