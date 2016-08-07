using UnityEngine;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using System.IO;

public static class GameSaver
{
    public static List<Game> savedGames = new List<Game>();

    public static void SaveGame()
    {
        savedGames.Add(Game.current);
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/savedGames.gd");
        bf.Serialize(file, GameSaver.savedGames);
        file.Close();
    }

    public static void LoadGame(int index)
    {
        if (File.Exists(Application.persistentDataPath + "/savedGames.gd") && index > 0 && index < GameSaver.savedGames.Count)
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/savedGames.gd", FileMode.Open);
            GameSaver.savedGames = (List<Game>)bf.Deserialize(file);
            file.Close();
        }
    }

}
