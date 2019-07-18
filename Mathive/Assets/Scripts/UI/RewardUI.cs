using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardUI : MonoBehaviour
{
    public Text scoreText;

    public GameObject star1, star2, star3;

    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        scoreText.text = gameManager.Score.ToString();
        if(gameManager.StarsEarned == 1){
            star1.SetActive(true);
        } else if(gameManager.StarsEarned == 2){
            star2.SetActive(true);
            star3.SetActive(true);
        } else if(gameManager.StarsEarned == 3){
            star1.SetActive(true);
            star2.SetActive(true);
            star3.SetActive(true);
        }
    }
}
