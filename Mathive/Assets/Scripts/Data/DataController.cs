using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataController : MonoBehaviour 
{
    private Hive[,] currentGrid;
    private PlayerProgress playerProgress;

    private string gameDataFileName = "data.json";

    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        //LoadGameData();

        LoadPlayerProgress();
    }

    public void SubmitNewHighestLevel(int currentLevel)
    {
        // If newScore is greater than playerProgress.highestScore, update playerProgress with the new value and call SavePlayerProgress()
        if(playerProgress.CurrentLevel < currentLevel)
        {
            playerProgress.CurrentLevel = currentLevel;
            SavePlayerProgress();
        }
    }

    public int GetCurrentLevel()
    {
        return playerProgress.CurrentLevel;
    }

    private void LoadLevelData(){

    }

    private void LoadGameData()
    {
        // Path.Combine combines strings into a file path
        // Application.StreamingAssets points to Assets/StreamingAssets in the Editor, and the StreamingAssets folder in a build
        string filePath = Path.Combine(Application.streamingAssetsPath, gameDataFileName);

        if(File.Exists(filePath))
        {
            // Read the json from the file into a string
            string dataAsJson = File.ReadAllText(filePath); 
            // Pass the json to JsonUtility, and tell it to create a GameData object from it
            GameData loadedData = JsonUtility.FromJson<GameData>(dataAsJson);

            // Retrieve the allRoundData property of loadedData
            currentGrid = loadedData.CurrentGrid;
        }
        else
        {
            Debug.LogError("Cannot load game data!");
        }
    }

    // This function could be extended easily to handle any additional data we wanted to store in our PlayerProgress object
    private void LoadPlayerProgress()
    {
        // Create a new PlayerProgress object
        playerProgress = new PlayerProgress();

        // If PlayerPrefs contains a key called "highestScore", set the value of playerProgress.highestScore using the value associated with that key
        if(PlayerPrefs.HasKey("currentLevel"))
        {
            playerProgress.CurrentLevel = PlayerPrefs.GetInt("currentLevel");
            Debug.Log(playerProgress.CurrentLevel);
        }
    }

    // This function could be extended easily to handle any additional data we wanted to store in our PlayerProgress object
    private void SavePlayerProgress()
    {
        // Save the value playerProgress.highestScore to PlayerPrefs, with a key of "highestScore"
        PlayerPrefs.SetInt("currentLevel", playerProgress.CurrentLevel);
        Debug.Log(playerProgress.CurrentLevel);
    }
}
