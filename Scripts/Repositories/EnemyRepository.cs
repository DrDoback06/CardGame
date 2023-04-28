using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class EnemyRepository : Singleton<EnemyRepository>
{
    public List<EnemyData> Enemies = new List<EnemyData>();

    public string EnemiesFileName = "enemies";

    void Awake()
    {
        Enemies = new BaseRepository<EnemyData>().GetGameData(EnemiesFileName);
    }

    public List<EnemyData> GetEnemiesByTier(int tier)
    {
        return Enemies.Where(s => s.tier == tier).ToList();
    }
    
    public EnemyData GetEnemyByName(string name)
    {
        return Enemies.Single(s => s.name == name);
    }

    public EnemyData GetRandomEnemyByTier(int tier)
    {
        var enemiesInTier = Enemies.Where(s => s.tier == tier).ToList();
        var index = UnityEngine.Random.Range(0, enemiesInTier.Count);
        return enemiesInTier[index];
    }
}

[Serializable]
public class EnemyData
{
    public int health;
    public int[] healthDeviance;
    public string name;
    public int tier;
    public string sprite;
}
