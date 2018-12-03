using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour {

  public void LoadLevel()
  {
    SceneManager.LoadScene("Level");
  }

  public void LoadLevel(string levelName)
  {
    if (levelName == "mainScene")
    {
      Destroy(GameObject.Find("LevelData"));
    }
    SceneManager.LoadScene(levelName);
  }

}
