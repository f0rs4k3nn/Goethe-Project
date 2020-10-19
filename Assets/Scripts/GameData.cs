using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[Serializable]
public class SaveData
{
    public int lastUnlockedLevel;
    public int lastPlayedLevel;
    public int lastScore;
}

public class GameData : MonoBehaviour
{
    public static GameData gameData;
    public SaveData saveData;

    void Awake()
    {
        if (gameData == null)
        {
            DontDestroyOnLoad(this.gameObject);
            gameData = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        Load();
    }

    public void Save()
    {
        BinaryFormatter formatter = new BinaryFormatter();

        FileStream file = File.Open(Application.persistentDataPath + "/player.data", FileMode.Create);

        SaveData data = new SaveData();
        data = saveData;

        formatter.Serialize(file, data);

        file.Close();

        Debug.Log("file saved succesfully");
    }

    public void Load()
    {

        if(File.Exists(Application.persistentDataPath+ "/player.data"))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/player.data", FileMode.Open);
            saveData = formatter.Deserialize(file) as SaveData;

            file.Close();
            Debug.Log("Loaded");
        }
        else
        {
            saveData = new SaveData();
            Save();
        }
    }
}
