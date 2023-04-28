using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Sprite Sprite;
    public Card Card;
    public Vector2 MapPosition => Card.MapPosition;
    public int Damage;
    public string Name;
    public string Effect;
    public string SoundPrefix;

    public void Init(int value, string name, string effect, string spriteLoc, string soundPrefix, Card card)
    {
        Damage = value;
        Name = name;
        Effect = effect;
        Card = card;
        SoundPrefix = soundPrefix;

        Sprite = Resources.Load<Sprite>(spriteLoc);
    }
}
