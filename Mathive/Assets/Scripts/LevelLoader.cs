using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour {

  string levelToLoad = "mainScene";

  public Animator animator;

  public void FadeScene(){
      animator.SetTrigger("FadeOut");
  }

  public void LoadLevel(string levelName)
  {
    if (levelName == "mainScene")
    {
      Destroy(GameObject.Find("LevelData"));
    }
    animator.SetTrigger("FadeOut");
    levelToLoad = levelName;
  }

  public void LoadLevelAfterFade(){
      SceneManager.LoadScene(levelToLoad);
  }

}
