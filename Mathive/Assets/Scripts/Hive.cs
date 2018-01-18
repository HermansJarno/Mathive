using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hive : MonoBehaviour {

  [SerializeField] int _value;

  [SerializeField] private int x;
  [SerializeField] private int y;

  public Sprite normalSprite;
  public Sprite selectedSprite;

  private Image imageHive;
  private bool selected = false;

  private void Start()
  {
    imageHive = gameObject.GetComponent<Image>();
  }

  // Implement that it holds image value

  public int Value
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

  public void SwitchState()
  {
    selected = !selected;
    if (selected)
    {
      imageHive.sprite = selectedSprite;
    }
    else
    {
      imageHive.sprite = normalSprite;
    }
  }

  public void SetNormalImage()
  {
    selected = false;
    imageHive.sprite = normalSprite;
  }

  public void OnValueChanged(int newValue)
  {
    _value = newValue;
    if (_value == 0)
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
      gameObject.transform.Find("Text").GetComponent<Text>().text = _value.ToString();
    }
  }

  public void SetHive(int setValue, int xValue, int yValue)
  {
    OnValueChanged(setValue);
    OnPositionChanged(xValue, yValue);
  }

  public void DestroyMyself()
  {
    Destroy(gameObject);
  }

}
