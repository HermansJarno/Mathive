using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProgress : MonoBehaviour
{
    private int currentLevel = 1;
    private List<List<int>> scores = new List<List<int>>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int CurrentLevel{
        get{return currentLevel;}
        set{
            currentLevel = value;
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
