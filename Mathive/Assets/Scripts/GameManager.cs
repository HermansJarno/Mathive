using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

  [SerializeField] private int level = 1;

  public int Level
  {
    get { return level; }
    set { level = value; }
  }


  // Use this for initialization
  void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
