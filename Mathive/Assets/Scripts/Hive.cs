using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hive : MonoBehaviour {

  [SerializeField] string _value;

  [SerializeField] private int x;
  [SerializeField] private int y;

  public string Value
  {
    get { return _value; }
    set { _value = value; }
  }

  public int X
  {
    get { return x; }
    set { x = value; }
  }

  public int Y
  {
    get { return y; }
    set { y = value; }
  }

  public void OnPositionChanged(int newX, int newY)
  {
    x = newX;
    y = newY;
    gameObject.name = string.Format("{0}{1}", x+1, y+1);
    transform.SetSiblingIndex(newY);
  }

  public void OnValueChanged(string newValue)
  {
    _value = newValue;
    if ((int.Parse(_value) == 0))
    {
      gameObject.GetComponent<Image>().enabled = false;
      gameObject.transform.Find("Text").GetComponent<Text>().text = "";
    }
    else
    {
      if (!gameObject.GetComponent<Image>().isActiveAndEnabled)
      {
        gameObject.GetComponent<Image>().enabled = true;
      }
      gameObject.transform.Find("Text").GetComponent<Text>().text = _value;
    }
  }

  public void SetHive(string setValue, int xValue, int yValue)
  {
    OnValueChanged(setValue);
    x = xValue;
    y = yValue;
  }

  public void DestroyMyself()
  {
    Destroy(gameObject);
  }

}
