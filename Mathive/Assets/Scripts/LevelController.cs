using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour {

  private int oldLevel = 1;
  private int _level = 1;

  private int maxLevel = 6;

  DataController dataController;
  LevelLoader levelLoader = new LevelLoader();

  private void Start()
  {
    dataController = gameObject.GetComponent<DataController>();
    _level = dataController.GetCurrentLevel();
    oldLevel = _level;
    DontDestroyOnLoad(gameObject);
  }

  public void LoadLevel(GameObject go)
  { 
    int levelNr = int.Parse(go.name.Replace("Level ", ""));
    if(levelNr <= _level) {
      Debug.Log("loadLevel: " + levelNr);
      _level = levelNr;
      levelLoader.LoadLevel();
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
}
