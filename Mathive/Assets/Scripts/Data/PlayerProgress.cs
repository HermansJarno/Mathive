using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProgress : MonoBehaviour
{
    private int highestLevel = 1;
    private int numberOfLifes = 3;
    private int lastPlayedLevel = 1;
    private string playerName = "";

    private float lastPlayedTime = 0f;
    private List<List<int>> scores = new List<List<int>>();

    public int HighestLevel{
        get{return highestLevel;}
        set{
            highestLevel = value;
        }
    }

    public int NumberOfLifes{
        get{return numberOfLifes;}
        set{
            numberOfLifes = value;
        }
    }

    public int LastPlayedLevel{
        get{return lastPlayedLevel;}
        set{
            lastPlayedLevel = value;
        }
    }

    public float LastPlayedTime{
        get{return lastPlayedTime;}
        set{
            lastPlayedTime = value;
        }
    }

    public bool IsCurrentScoreHigherThanPrevious(int level, int score, int numberOfStars){
        bool higherScore = false;
        if(scores[level][0] < score){
            higherScore = true;
        }

        return higherScore;
    }
}
