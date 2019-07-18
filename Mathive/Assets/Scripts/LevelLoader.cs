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

  public void LoadRewardScene(){
    levelToLoad = "rewardScene";
    DontDestroyOnLoad(GameObject.Find("GameManager").gameObject);
    animator.SetTrigger("FadeOut");
  }

  public void LoadNextLevel(){
    if(GameObject.Find("LevelData").GetComponent<LevelController>().level < GameObject.Find("LevelData").GetComponent<LevelController>().MaxLevel){
      levelToLoad = "Level";
      Destroy(GameObject.Find("GameManager").gameObject);
      GameObject.Find("LevelData").GetComponent<LevelController>().level++;
      animator.SetTrigger("FadeOut");
    }else{
      LoadLevel("mainScene");
    }
  }

  public void LoadLevelAfterFade(){
      SceneManager.LoadScene(levelToLoad);
  }

}
