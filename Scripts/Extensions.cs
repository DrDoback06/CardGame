using UnityEngine;

/// <summary>
/// General extensions to help write cleaner code
/// </summary>
public static class Extensions
{
    public static bool CanMoveTo(this Vector2 currentPos, Vector2 intendedPos)
    {
        var validMoves = new[] {
            new Vector2(currentPos.x+1, currentPos.y), new Vector2(currentPos.x - 1, currentPos.y),
            new Vector2(currentPos.x, currentPos.y+1), new Vector2(currentPos.x, currentPos.y -1)
        };

        foreach (var move in validMoves)
        {
            if (intendedPos == move) return true;
        }

        return false;
    }

    public static bool IsBetween(this int evalNumber, int floor, int ceiling)
    {
        return evalNumber >= floor && evalNumber <= ceiling;
    }
}
