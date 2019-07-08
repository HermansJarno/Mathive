using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour {

  private int oldLevel = 1;
  private int _level = 1;

  private int maxLevel = 10;

  DataController dataController;

  private void Start()
  {
    dataController = gameObject.GetComponent<DataController>();
    _level = dataController.GetCurrentLevel();
    oldLevel = _level;
    DontDestroyOnLoad(gameObject);
  }

//Used by level objects in editor.
  public void LoadLevel(GameObject go)
  { 
    int levelNr = int.Parse(go.name.Replace("Level ", ""));
    if(levelNr <= dataController.GetCurrentLevel()) {
      Debug.Log("loadLevel: " + levelNr);
      _level = levelNr;
      LevelData levelData = new LevelDataController().LoadLevelFromResources(_level);
      new LevelPopUp(_level, levelData.ScoreTargets[0], levelData.NumberOfMoves);
    } 
  }

  public int level
  {
    get { return _level; }
  }

  public int OldLevel{
    get{return oldLevel;}
    set{oldLevel = value;}
  }

  public void UpdateHighestLevel(int level){
    if(level <= maxLevel){
      dataController.SubmitNewHighestLevel(level);
    }
  }

  public void LowerNumberOfLifes(){
    dataController.LowerNumberOfLifes();
  }
}
