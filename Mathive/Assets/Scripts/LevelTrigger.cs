using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    public void Trigger(){
        GameObject.Find("LevelChanger").GetComponent<LevelLoader>().LoadLevel("Level");
    }
}
