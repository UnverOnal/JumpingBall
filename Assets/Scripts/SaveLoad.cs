using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveLoad  {

    public static GameDatas gameDatas = new GameDatas();

    public static void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream dataFile = File.Create(Application.persistentDataPath + "/datasInfo.gd");

        bf.Serialize(dataFile, gameDatas);
        dataFile.Close();
    }

    public static void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/datasInfo.gd"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream dataFile = File.Open(Application.persistentDataPath + "/datasInfo.gd", FileMode.Open);

            gameDatas = (GameDatas)bf.Deserialize(dataFile);
            dataFile.Close();
        }
    }
}
