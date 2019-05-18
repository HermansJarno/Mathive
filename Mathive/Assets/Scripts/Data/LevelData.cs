using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class LevelData 
{
    int numerOfRows;
    int numberOfColumns;
    int numberOfMoves;
    List<int> targets = new List<int>();
    int[,] hives;
    List<int> scoreTargets = new List<int>();

    public LevelData(int initNumberOfMoves, List<int> initTargets, int[,] initHives, List<int> initScoreTargets){
        numerOfRows = initHives.GetLength(0);
        numberOfColumns = initHives.GetLength(1);

        numberOfMoves = initNumberOfMoves;
        targets = initTargets;
        hives = initHives;
        scoreTargets = initScoreTargets;
    }

    public int NumberOfMoves{
        get{return numberOfMoves;}
    }

    public List<int> Targets{
        get{return targets;}
    }

    public int NumberOfColumns{
        get{return numberOfColumns;}
    }

    public int NumberOfRows{
        get{return numerOfRows;}
    }

    public int[,] Hives{
        get{return hives;}
    }

    public List<int> ScoreTargets{
        get{return scoreTargets;}
    }
}
