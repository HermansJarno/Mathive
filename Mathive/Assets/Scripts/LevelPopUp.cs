using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelPopUp : MonoBehaviour
{
    // Start is called before the first frame update
    public LevelPopUp(int level, int goalScore, int numberOfMoves){
        if (GameObject.Find("LevelPopUp") == null){
            GameObject levelPopUp = Resources.Load("LevelPopUp") as GameObject;
            levelPopUp.transform.Find("LevelTitle").GetComponent<Text>().text = "LEVEL " + level;
            levelPopUp.transform.Find("NumberOfMoves").GetComponent<Text>().text = "MOVES: " + numberOfMoves;
            levelPopUp.transform.Find("Goal").GetComponent<Text>().text = "GOAL: " + goalScore;

            GameObject spawnedLevelPopUp = Instantiate(levelPopUp, levelPopUp.transform.position, levelPopUp.transform.rotation) as GameObject;
            spawnedLevelPopUp.transform.SetParent(GameObject.Find("CanvasLevels").transform, false);
        }
    }
}
