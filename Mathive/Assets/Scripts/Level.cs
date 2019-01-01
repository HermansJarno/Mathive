using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour {

  public int _level = 1;

  private void Awake()
  {
    DontDestroyOnLoad(gameObject);
  }

  public void setLevel(GameObject go)
  { 
    string levelNr = go.name.Replace("Level ", "");
    _level = int.Parse(levelNr);
  }

  public int _Level
  {
    get { return _level; }
  }

}
