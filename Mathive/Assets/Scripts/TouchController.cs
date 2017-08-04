using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TouchController : MonoBehaviour {

  public Sprite normalSprite;
  public Sprite selectedSprite;

  //Change me to change the touch phase used.
  TouchPhase touchPhase = TouchPhase.Moved;
  public GraphicRaycaster myGRaycaster;

  List<GameObject> selectedHives = new List<GameObject>();
  List<GameObject> currentHives = new List<GameObject>();
  Stack<GameObject> stackHives = new Stack<GameObject>();
  string currentHive = "";

  void Update()
  {
    //We check if we have more than one touch happening.
    //We also check if the first touches phase is Ended (that the finger was lifted)
    if (Input.touchCount > 0 && (Input.GetTouch(0).phase == touchPhase || Input.GetTouch(0).phase == TouchPhase.Began))
    {
      List<RaycastResult> rayResults = new List<RaycastResult>();
      PointerEventData ped = new PointerEventData(null);
      ped.position = Input.GetTouch(0).position;
      myGRaycaster.Raycast(ped, rayResults);
      foreach (RaycastResult result in rayResults)
      {
        GameObject resultHive = result.gameObject;
        if (resultHive.tag == "Hive")
        {
          if (stackHives.Count > 0)
          {
            if (currentHive != resultHive.name)
            {
              GameObject tempHive = null;
              if (stackHives.Count > 1)
              {
                 tempHive = stackHives.Pop();
              }

              if (stackHives.Peek() == resultHive)
              {
                tempHive.GetComponent<Image>().sprite = normalSprite;
              }
              else
              {
                if (tempHive != null)
                {
                  stackHives.Push(tempHive);
                }

                stackHives.Push(resultHive);
                resultHive.GetComponent<Image>().sprite = selectedSprite;
              }
            }
          }
          else
          {
            stackHives.Push(resultHive);
            resultHive.GetComponent<Image>().sprite = selectedSprite;
          }
          currentHive = resultHive.name;
        }
      }
    }
    if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
    {
      foreach (GameObject Hive in GameObject.FindGameObjectsWithTag("Hive"))
      {
        Hive.GetComponent<Image>().sprite = normalSprite;
        if (selectedHives.Contains(Hive))
        {
          selectedHives.Remove(Hive);
        }
        if (currentHives.Contains(Hive))
        {
          currentHives.Remove(Hive);
        }
      }
      stackHives.Clear();
    }
  }
}
