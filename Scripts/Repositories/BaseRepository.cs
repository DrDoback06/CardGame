using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

/// <summary>
/// Base class for easily reading JSON files containing game data
/// </summary>
/// <typeparam name="T"></typeparam>
public class BaseRepository<T>
{

    /// <summary>
    /// Generally used to load array based files, such as enemies, heroes, items etc...
    /// The array of items in the file must be called "gameData" to be intereprated correctly
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public List<T> GetGameData(string fileName)
    {
        var dataAsJson = LoadDataAsJson(fileName);
        if (!string.IsNullOrEmpty(dataAsJson))
        {
            GameData<T> loadedData = JsonUtility.FromJson<GameData<T>>(dataAsJson);
            return loadedData.gameData.ToList();
        }
        else
        {
            Debug.LogError($"Cannot load {fileName} data!");
        }

        return new List<T>();
    }

    /// <summary>
    /// Used for reading peristent files, such as UserData
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public T GetPersistentData(string fileName)
    {
        try
        {
            var dataAsJson = File.ReadAllText(Application.persistentDataPath + fileName);
            if (!string.IsNullOrEmpty(dataAsJson))
            {
                T loadedData = JsonUtility.FromJson<T>(dataAsJson);
                return loadedData;
            }
            else
            {
                Debug.LogError($"Cannot load {Application.persistentDataPath + fileName} data!");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Exception loading {Application.persistentDataPath + fileName} data! " + e.Message);
        }

        return default;
    }

    private string LoadDataAsJson(string fileName)
    {
        var data = Resources.Load<TextAsset>(fileName);
        return data.text;
    }

}

[Serializable]
public class GameData<T>
{
    public T[] gameData;
}