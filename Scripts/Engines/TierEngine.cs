using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TierEngine : Singleton<TierEngine>
{
    private int _tierIncrement = 100;

    public int GetCurrentTier()
    {
        var coins = CoinManager.Instance.GetCurrentCoins();
        if (coins == 0) return 0;

        var tier = coins / _tierIncrement;
        return tier;
    }
}
