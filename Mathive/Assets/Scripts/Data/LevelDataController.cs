using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class LevelDataController : MonoBehaviour
{
    int numberOfColumns = 6;
    int numberOfRows = 6;
    int numberOfMoves = 30;
    List<string> emptyIndexes = new List<string>();
    List<string> iceIndexes = new List<string>();
    List<int> targets = new List<int>();

    // Start is called before the first frame update
    void Awake()
    {
        /* 
        emptyIndexes.Add("13");
        emptyIndexes.Add("14");
        emptyIndexes.Add("31");
        emptyIndexes.Add("33");
        emptyIndexes.Add("34");
        emptyIndexes.Add("36");
        emptyIndexes.Add("41");
        emptyIndexes.Add("43");
        emptyIndexes.Add("44");
        emptyIndexes.Add("46");
        emptyIndexes.Add("63");
        emptyIndexes.Add("64");

        for (int i = 0; i < 3; i++)
        {
            targets.Add(15);
        }
        for (int i = 0; i < 3; i++)
        {
            targets.Add(15);
        }
        SaveFile();
        */
    }

    public void SaveFile()
     {
         string destination = Application.persistentDataPath + "/level3.txt";
         FileStream file;
 
         if(File.Exists(destination)) file = File.OpenWrite(destination);
         else file = File.Create(destination);
 
         LevelData data = new LevelData(numberOfColumns, numberOfRows, emptyIndexes, iceIndexes, numberOfMoves, targets);
         BinaryFormatter bf = new BinaryFormatter();
         bf.Serialize(file, data);
         file.Close();
     }
 
     public LevelData LoadLevel(int levelNumber)
     {
         string destination = Application.persistentDataPath + "/level" + levelNumber.ToString() + ".dat";
         Debug.Log(destination);
         FileStream file;
 
         if(File.Exists(destination)) file = File.OpenRead(destination);
         else
         {
             Debug.LogError("File not found");
             return null;
         }
 
         BinaryFormatter bf = new BinaryFormatter();
         LevelData data = (LevelData) bf.Deserialize(file);
         file.Close();
 
         return data;
     }

     public LevelData LoadLevelFromResources(int levelNumber){
        string level = "Levels/level" + levelNumber.ToString();
        TextAsset textAsset = Resources.Load(level) as TextAsset;

        Stream stream = new MemoryStream(textAsset.bytes);
        BinaryFormatter formatter = new BinaryFormatter();                
        LevelData myInstance = formatter.Deserialize(stream) as LevelData;

        return myInstance;
     }
}
