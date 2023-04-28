using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValueDevianceEngine : Singleton<ValueDevianceEngine>
{
    public int CalculateDeviance(int amount, int[] devianceLimits)
    {
        var newAmount = amount + Random.Range(devianceLimits[0], devianceLimits[1]+1);
        return newAmount;
    }
}
