using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementEngine : MonoBehaviour
{
    [SerializeField]
    private MapInitializerEngine _mapInitializer;
    [SerializeField]
    private GameObject _cardPrefab;

    /// <summary>
    /// Recursively move's and fills all entities along a particular axis
    /// </summary>
    /// <param name="cardToMove"></param>
    /// <param name="dest"></param>
    /// <param name="mapPosDest"></param>
    public void MoveAndFill(Card cardToMove, Vector3 dest, Vector2 mapPosDest)
    {
        var startingPos = cardToMove.transform.position;
        var startingMapPos = cardToMove.MapPosition;
        cardToMove.MoveTo(dest, mapPosDest);

        // Try moving on xAxis first
        if (mapPosDest.x > startingMapPos.x)
        {
            var cardToFill = CardRepository.Instance.GetCardAtPoint(new Vector2(startingMapPos.x - 1, startingMapPos.y));
            if (cardToFill != null)
            {
                MoveAndFill(cardToFill, startingPos, startingMapPos);
            }
            else
            {
                CreateNewCard(startingPos, startingMapPos);
            }
        }
        else if (mapPosDest.x < startingMapPos.x)
        {
            var cardToFill = CardRepository.Instance.GetCardAtPoint(new Vector2(startingMapPos.x + 1, startingMapPos.y));
            if (cardToFill != null)
            {
                MoveAndFill(cardToFill, startingPos, startingMapPos);
            }
            else
            {
                CreateNewCard(startingPos, startingMapPos);
            }
        }
        else if (mapPosDest.y > startingMapPos.y)
        {
            var cardToFill = CardRepository.Instance.GetCardAtPoint(new Vector2(startingMapPos.x, startingMapPos.y - 1));
            if (cardToFill != null)
            {
                MoveAndFill(cardToFill, startingPos, startingMapPos);
            }
            else
            {
                CreateNewCard(startingPos, startingMapPos);
            }
        }
        else if (mapPosDest.y < startingMapPos.y)
        {
            var cardToFill = CardRepository.Instance.GetCardAtPoint(new Vector2(startingMapPos.x, startingMapPos.y + 1));
            if (cardToFill != null)
            {
                MoveAndFill(cardToFill, startingPos, startingMapPos);
            }
            else
            {
                CreateNewCard(startingPos, startingMapPos);
            }
        }
    }
    private void CreateNewCard(Vector3 startingPos, Vector2 startingMapPos)
    {
        var card = Instantiate(_cardPrefab, startingPos, Quaternion.identity).GetComponent<Card>();
        card.gameObject.transform.localScale = new Vector3(0, 0, 0);
        card.MapPosition = startingMapPos;
        CardRepository.Instance.AddCard(card);
        _mapInitializer.GenerateCard(card, startingMapPos);

        card.transform.DOScale(new Vector3(2,2.75f,0.1f), .2f);
    }
}

