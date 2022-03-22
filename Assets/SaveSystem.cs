using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem
{
    public static void SavePlayer(PermanentStats permanentStats)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/Stats.bin";
        FileStream stream;

        PlayerData data = new PlayerData(permanentStats);

        using (stream = new FileStream(path, FileMode.Create))
        {
            formatter.Serialize(stream, data);
        }

        //stream.Close();
    }
    public static PlayerData LoadPlayer()
    {
        string path = Application.persistentDataPath + "/Stats.bin";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream;
            PlayerData data;
            using (stream = new FileStream(path, FileMode.Open))
            {
                data = formatter.Deserialize(stream) as PlayerData;
            }

            //PlayerData data = formatter.Deserialize(stream) as PlayerData;
            //stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }
}
