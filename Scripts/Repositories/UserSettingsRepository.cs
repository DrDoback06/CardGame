using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class UserSettingsRepository : Singleton<UserSettingsRepository>
{
    public UserData UserData;
    private BaseRepository<UserData> _baseRepo;

    private void Awake()
    {
        _baseRepo = new BaseRepository<UserData>();
        UserData = _baseRepo.GetPersistentData("/userData.json");
        if (UserData == default(UserData))
        {
            Debug.Log("Saving fresh data");
            UserData = new UserData() { coins = 0, coinsLastRound = 0, currentHero = "Knight", heroUpgrades = new List<HeroUpgrades>() { new HeroUpgrades() { heroName = "Knight", upgradeLevel = 0 } } };
            Save();
        }
    }

    public void Save()
    {
        var data = JsonUtility.ToJson(UserData);
        Debug.Log("Saving data " + data);
        File.WriteAllText(Application.persistentDataPath + "/userData.json", data);
    }

    public int GetUpgradesForHero(string name)
    {
        if (UserData.heroUpgrades.Any(a => a.heroName == name))
        {
            return UserData.heroUpgrades.Single(s => s.heroName == name).upgradeLevel;
        }

        return 0;
    }

    public void Upgrade(string name)
    {
        Debug.Log("Upgrading " + name);
        if (UserData.heroUpgrades.Any(a => a.heroName == name))
        {
            UserData.heroUpgrades.Single(s => s.heroName == name).upgradeLevel += 1;
        }
        else
        {
            UserData.heroUpgrades.Add(new HeroUpgrades() { heroName = name, upgradeLevel = 1 });
        }

        Save();
    }
}

[Serializable]
public class UserData
{
    public string currentHero;
    public int coins;
    public int coinsLastRound;
    public List<HeroUpgrades> heroUpgrades;
}

[Serializable]
public class HeroUpgrades
{
    public string heroName;
    public int upgradeLevel;
}