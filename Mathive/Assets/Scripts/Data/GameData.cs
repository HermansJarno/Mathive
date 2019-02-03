using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    private Hive [,] currentGrid;
    bool darkMode = false;

    public Hive[,] CurrentGrid{
        get{return currentGrid;}
    }
}
