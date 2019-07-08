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
        
        LoadPlayerProgress();
        LoadNumberOfLifes();
        LoadlastTimeLostLife();
        
        if(playerProgress.NumberOfLifes < 3){
            int minutes = getDifferenceBetweenTwoDatesInMinutes(playerProgress.LastTimeLostLife, buildTimeStamp());
            if(minutes >= 30){
                float times = Mathf.Floor(minutes / 30);
                float lifesToAdd = Mathf.Clamp(times, 0, 3 - playerProgress.NumberOfLifes);

                if(lifesToAdd > 0){
                    for (int i = 0; i < lifesToAdd; i++)
                    {
                        IncreaseNumberOfLifes();  
                    }
                }

                Debug.Log("lifes to add: " + lifesToAdd);

            }else{
                Debug.Log("still have to wait for new life " + (30 - minutes) + " minutes.");
            }
        }

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

    public int GetNumberOfLives()
    {
        return playerProgress.NumberOfLifes;
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
        playerProgress.HighestLevel = PlayerPrefs.GetInt("currentLevel", 1);
    }


    private void SavePlayerProgress()
    {
        PlayerPrefs.SetInt("currentLevel", playerProgress.HighestLevel);
    }

    private void LoadNumberOfLifes()
    {
        playerProgress.NumberOfLifes = PlayerPrefs.GetInt("numberOfLifes", 3);
    }

    private void SaveNumberOfLifes()
    {
        PlayerPrefs.SetInt("numberOfLifes", playerProgress.NumberOfLifes);
    }

    public void LowerNumberOfLifes()
    {
        if(playerProgress.NumberOfLifes == 3) SavelastTimeLostLife();
        if(playerProgress.NumberOfLifes > 0) playerProgress.NumberOfLifes--;
        PlayerPrefs.SetInt("numberOfLifes", playerProgress.NumberOfLifes);
        Debug.Log(playerProgress.NumberOfLifes);
    }

    public void IncreaseNumberOfLifes()
    {
        if(playerProgress.NumberOfLifes < 2) SavelastTimeLostLife();
        if(playerProgress.NumberOfLifes < 3) playerProgress.NumberOfLifes++;
        PlayerPrefs.SetInt("numberOfLifes", playerProgress.NumberOfLifes);
    }

    private void LoadlastTimeLostLife()
    {
        playerProgress.LastTimeLostLife = PlayerPrefs.GetString("lastTimeLostLife");
    }

    private void SavelastTimeLostLife()
    {
        PlayerPrefs.SetString("lastTimeLostLife", buildTimeStamp());
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

    private string buildTimeStamp(){
        string timestamp = "";
        float year = (float)DateTime.Now.Year;
        float month = (float)DateTime.Now.Month;
        float day = (float)DateTime.Now.Day;
        float minute = (float)DateTime.Now.Minute;
        float hour = (float)DateTime.Now.Hour;

        timestamp = year.ToString() + "-" + month.ToString() + "-" + day.ToString() + "-" + hour.ToString() + "-" + minute.ToString();

        return timestamp;
    }

    private int getDifferenceBetweenTwoDatesInMinutes(string previousTimeStamp, string newTimeStamp){
        Debug.Log(previousTimeStamp);

        string[] splittedPreviousTimeStamp = previousTimeStamp.Split('-');
        string[] splittedNewTimeStamp = newTimeStamp.Split('-');
        
        DateTime previousDate = new DateTime(
            Int32.Parse(splittedPreviousTimeStamp[0]), 
            Int32.Parse(splittedPreviousTimeStamp[1]), 
            Int32.Parse(splittedPreviousTimeStamp[2]), 
            Int32.Parse(splittedPreviousTimeStamp[3]), 
            Int32.Parse(splittedPreviousTimeStamp[4]), 0);
        DateTime newDate = new DateTime(
            Int32.Parse(splittedNewTimeStamp[0]), 
            Int32.Parse(splittedNewTimeStamp[1]), 
            Int32.Parse(splittedNewTimeStamp[2]), 
            Int32.Parse(splittedNewTimeStamp[3]), 
            Int32.Parse(splittedNewTimeStamp[4]), 0);

        TimeSpan span = newDate.Subtract (previousDate);

        return (int)span.TotalMinutes;
    }
}
