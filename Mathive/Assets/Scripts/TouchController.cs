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

  Stack<GameObject> selectedHives = new Stack<GameObject>();
  List<GameObject> lineHives = new List<GameObject>();
  List<Hive> listHives = new List<Hive>();
  string currentHive = "";
  GameObject lastHive;
  Grid grid;
  LineRenderController LRController;

  private void Start()
  {
    grid = GameObject.Find("Scripts").GetComponent<Grid>();
    LRController = GameObject.Find("LineRenderer").GetComponent<LineRenderController>();
  }

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
        Hive resultH = result.gameObject.GetComponent<Hive>();
        if (resultHive.tag == "Hive")
        {
          // Is there a hive selected?
          if (selectedHives.Count > 0)
          {
            //Is it the same as last frame?
            if (currentHive != resultHive.name)
            {
              GameObject tempHive = null;
              if (selectedHives.Count > 1)
              {
                 tempHive = selectedHives.Pop();
              }

              // Are we deselecting?
              if (selectedHives.Peek() == resultHive)
              {
                lineHives.Remove(tempHive);
                listHives.Remove(tempHive.GetComponent<Hive>());
                if (tempHive != null) tempHive.GetComponent<Image>().sprite = normalSprite;

              }
              else
              {
                // If not deselecting, repush
                if (tempHive != null)
                {
                  selectedHives.Push(tempHive);
                }
                // If selected hive is already in the Stack, do nothing
                if (!selectedHives.Contains(resultHive))
                {
                  if (grid.IsHiveNear(lastHive, resultHive))
                  {
                    if (lastHive.transform.GetChild(0).GetComponent<Text>().text == resultHive.transform.GetChild(0).GetComponent<Text>().text)
                    {
                      selectedHives.Push(resultHive);
                      lastHive = resultHive;
                      resultHive.GetComponent<Image>().sprite = selectedSprite;
                      lineHives.Add(resultHive);
                      listHives.Add(resultH);
                    }
                  }
                }
              }
              LRController.UpdatePoints(lineHives);
            }
          }
          else
          {
            // Push the first hive
            selectedHives.Push(resultHive);
            lineHives.Add(resultHive);
            listHives.Add(resultH);
            lastHive = resultHive;
            resultHive.GetComponent<Image>().sprite = selectedSprite;
          }
          currentHive = resultHive.name;
          
        }
      }
    }
    // Deselect all hives on touch end.
    // Check if it's the right result.
    if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
    {
      if (listHives.Count > 1)
      {
        grid.CalculateScore(listHives);

      }

      foreach (GameObject Hive in GameObject.FindGameObjectsWithTag("Hive"))
      {
        Hive.GetComponent<Image>().sprite = normalSprite;
      }
      selectedHives.Clear();
      lineHives.Clear();
      listHives.Clear();
      LRController.UpdatePoints(lineHives);
    }
  }
}
