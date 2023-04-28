using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Pawn
{
    public int Tier;

    public void Init(int health, int tier, string name, string spriteLoc, Card card)
    {
        Health = health;
        MaxHealth = Health;

        Tier = tier;
        Name = name;
        Card = card;
         
        Sprite = Resources.Load<Sprite>(spriteLoc);
    }

    public void RefreshValues()
    {
        Card.Init(this);
    }
}
