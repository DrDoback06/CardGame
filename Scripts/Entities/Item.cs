using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public Sprite Sprite;
    public Card Card;
    public Vector2 MapPosition => Card.MapPosition;
    public int Value;
    public string Name;
    public string Effect;

    public void Init(int value, string name, string effect, string spriteLoc, Card card)
    {
        Value = value;
        Name = name;
        Effect = effect;
        Card = card;

        Sprite = Resources.Load<Sprite>(spriteLoc);
    }

}
