using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RarityEngine : Singleton<RarityEngine>
{

    /// <summary>
    /// How
    /// </summary>
    [SerializeField]
    private int _tierClimbCeiling = 2;
    [SerializeField]
    private int _tierClimbFloor = 1;

    public int CalculateRarityByTier()
    {
        var tier = TierEngine.Instance.GetCurrentTier();

        var randomInt = Random.Range(1, 10);
        if (randomInt >= 9) return tier + _tierClimbCeiling;
        if (randomInt >= 5) return tier + _tierClimbFloor;
        if (randomInt == 1) return tier - _tierClimbFloor;

        return tier;
    }

}
