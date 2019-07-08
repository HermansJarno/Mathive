using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartUI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        UpdateNumberOfLifes();
    }   

    public void UpdateNumberOfLifes(){
        int numberOfLifes = GameObject.Find("LevelData").GetComponent<DataController>().GetNumberOfLives();
        gameObject.transform.Find("Number").GetComponent<Text>().text = numberOfLifes.ToString();
    } 
}
