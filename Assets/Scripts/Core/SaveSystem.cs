using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem
{
    public static void SavePermanent(PermanentStats permanentStats)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/Permanent_Stats.bin";
        FileStream stream;

        PlayerDataPermanent data = new PlayerDataPermanent();

        using (stream = new FileStream(path, FileMode.Create))
        {
            formatter.Serialize(stream, data);
        }

        stream.Close();
    }
    public static void SavePrestige(PrestigeStats prestigeStats)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/Prestige_Stats.bin";
        FileStream stream;

        PlayerDataPrestige data = new PlayerDataPrestige(prestigeStats);

        using (stream = new FileStream(path, FileMode.Create))
        {
            formatter.Serialize(stream, data);
        }

        stream.Close();
    }
    public static void SavePermanentCache(PermanentCache permanentCache)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/Permanent_Cache.bin";
        FileStream stream;

        PlayerDataPermanentCache data = new PlayerDataPermanentCache(permanentCache);

        using (stream = new FileStream(path, FileMode.Create))
        {
            formatter.Serialize(stream, data);
        }

        stream.Close();
    }
    public static PlayerDataPermanentCache LoadPermanentCache()
    {
        string path = Application.persistentDataPath + "/Permanent_Cache.bin";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream;
            PlayerDataPermanentCache data;
            using (stream = new FileStream(path, FileMode.Open))
            {
                data = formatter.Deserialize(stream) as PlayerDataPermanentCache;
            }

            //PlayerData data = formatter.Deserialize(stream) as PlayerData;
            //stream.Close();

            return data;
        }
        else
        {
            Debug.Log("Save file not found in " + path);
            return null;
        }
    }
    public static PlayerDataPermanent LoadPermanentStats()
    {
        string path = Application.persistentDataPath + "/Permanent_Stats.bin";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream;
            PlayerDataPermanent data;
            using (stream = new FileStream(path, FileMode.Open))
            {
                data = formatter.Deserialize(stream) as PlayerDataPermanent;
            }

            //PlayerData data = formatter.Deserialize(stream) as PlayerData;
            //stream.Close();

            return data;
        }
        else
        {
            Debug.Log("Save file not found in " + path);
            return null;
        }
    }
    public static PlayerDataPrestige LoadPrestigeStats()
    {
        string path = Application.persistentDataPath + "/Prestige_Stats.bin";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream;
            PlayerDataPrestige data;
            using (stream = new FileStream(path, FileMode.Open))
            {
                data = formatter.Deserialize(stream) as PlayerDataPrestige;
            }

            //PlayerData data = formatter.Deserialize(stream) as PlayerData;
            //stream.Close();

            return data;
        }
        else
        {
            Debug.Log("Save file not found in " + path);
            return null;
        }
    }
    public static void ClearPrestigeData()
    {
        string path = Application.persistentDataPath + "/Prestige_Stats.bin";

        using (var FileWriter = new StreamWriter(path, false))
        {
            FileWriter.WriteLine("");
        }
    }
}
