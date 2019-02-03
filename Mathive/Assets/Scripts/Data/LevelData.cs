using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class LevelData 
{
    int numerOfRows;
    int numberOfColumns;
    List<string> emptyIndexes = new List<string>();
    List<string> iceIndexes = new List<string>();
    int numberOfMoves;
    List<int> targets = new List<int>();

    public LevelData(int initNumberOfcolumns, int initNumberOfRows, List<string> initEmptyIndexes, List<string> initIceIndexes, int initNumberOfMoves, List<int> initTargets){
        numerOfRows = initNumberOfRows;
        numberOfColumns = initNumberOfcolumns;
        emptyIndexes = initEmptyIndexes;
        iceIndexes = initIceIndexes;
        numberOfMoves = initNumberOfMoves;
        targets = initTargets;
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

    public List<string> EmptyIndexes{
        get{return emptyIndexes;}
        set{emptyIndexes = value;}
    }

    public List<string> IceIndexes{
        get{return iceIndexes;}
        set{iceIndexes = value;}
    }
}
