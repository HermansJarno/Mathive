using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class DataController : MonoBehaviour
{
    private Hive[,] currentGrid;
    private PlayerProgress playerProgress = new PlayerProgress();

    private string gameDataFileName = "data.json";

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        //Debug.Log(((float)DateTime.Now.Hour + ((float)DateTime.Now.Minute * 0.01f)));

        //LoadGameData();

        LoadPlayerProgress();
    }

    public void SubmitNewHighestLevel(int currentLevel)
    {
        if (playerProgress.HighestLevel < currentLevel)
        {
            playerProgress.HighestLevel = currentLevel;
            SavePlayerProgress();
        }
    }

    public int GetCurrentLevel()
    {
        return playerProgress.HighestLevel;
    }

    private void LoadLevelData()
    {

    }

    private void LoadGameData()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, gameDataFileName);

        if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);
            GameData loadedData = JsonUtility.FromJson<GameData>(dataAsJson);
            currentGrid = loadedData.CurrentGrid;
        }
        else
        {
            Debug.LogError("Cannot load game data!");
        }
    }

    private void LoadPlayerProgress()
    {
        if (PlayerPrefs.HasKey("currentLevel"))
        {
            playerProgress.HighestLevel = PlayerPrefs.GetInt("currentLevel");
        }
    }

    private void SavePlayerProgress()
    {
        PlayerPrefs.SetInt("currentLevel", playerProgress.HighestLevel);
    }

    private void LoadNumberOfLifes()
    {
        if (PlayerPrefs.HasKey("numberOfLifes"))
        {
            playerProgress.NumberOfLifes = PlayerPrefs.GetInt("numberOfLifes");
        }
    }

    private void SaveNumberOfLifes()
    {
        PlayerPrefs.SetInt("numberOfLifes", playerProgress.NumberOfLifes);
    }

    private void LoadLastPlayedTime()
    {
        if (PlayerPrefs.HasKey("lastPlayedTime"))
        {
            playerProgress.LastPlayedTime = PlayerPrefs.GetFloat("lastPlayedTime");
        }
    }

    private void SaveLastPlayedTime()
    {
        PlayerPrefs.SetFloat("lastPlayedTime", ((float)DateTime.Now.Hour + ((float)DateTime.Now.Minute * 0.01f)));
    }

    private void LoadLastPlayedLevel()
    {
        if (PlayerPrefs.HasKey("lastPlayedLevel"))
        {
            playerProgress.LastPlayedLevel = PlayerPrefs.GetInt("lastPlayedLevel");
        }
    }

    private void SaveLastPlayedLevel()
    {
        PlayerPrefs.SetInt("lastPlayedLevel", playerProgress.LastPlayedLevel);
    }
}
