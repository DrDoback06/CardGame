using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardRepository : Singleton<CardRepository>
{
    private List<Card> _cards;

    private void Awake()
    {
        _cards = GameObject.FindGameObjectsWithTag("Card").Select(s => s.GetComponent<Card>()).ToList();
    }

    public Card GetCardAtPoint(Vector2 point)
    {
        return _cards.SingleOrDefault(s => s.MapPosition == point);
    }

    public void RemoveCardAtPoint(Vector2 point)
    {
        var card = _cards.SingleOrDefault(s => s.MapPosition == point);
        _cards.Remove(card);
    }

    public void AddCard(Card card)
    {
        _cards.Add(card);
    }

    public List<Card> GetCards()
    {
        return _cards;
    }
}
