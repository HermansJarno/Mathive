using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hive : MonoBehaviour {

  [SerializeField] int _value;

  [SerializeField] private int x;
  [SerializeField] private int y;
  private float scaleAnimation = 1.2f;
  private float animationSpeed = 2.5f;

  private Sprite normalSprite;
  private Sprite selectedSprite;
  private Sprite backgroundSprite;

  public Sprite redSprite;
  public Sprite blueSprite;
  public Sprite yellowSprite;
  public Sprite greenSprite;
  public Sprite magentaSprite;
  public Sprite cyanSprite;

  public Sprite redSelection;
  public Sprite blueSelection;
  public Sprite yellowSelection;
  public Sprite greenSelection;
  public Sprite magentaSelection;
  public Sprite cyanSelection;

  public Sprite redBackground;
  public Sprite blueBackground;
  public Sprite yellowBackground;
  public Sprite greenBackground;
  public Sprite magentaBackground;
  public Sprite cyanBackground;

  public GameObject backgroundAnimation;

  public Image imageHive;
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

  public void SetSelectedImage() {
    selected = true;
    Invoke("DelaySelectedHive", 0.1f);
    InstantiateBackground();
  }

  void DelaySelectedHive()
  {
    imageHive.sprite = selectedSprite;
  }

  public void SetNormalImage()
  {
    selected = false;
    imageHive.sprite = normalSprite;
  }

  public void OnValueChanged(int newValue)
  {
    _value = newValue;
    switch (_value)
    {
      case 1:
        normalSprite = redSprite;
        selectedSprite = redSelection;
        backgroundSprite = redBackground;
        break;
      case 2:
        normalSprite = greenSprite;
        selectedSprite = greenSelection;
        backgroundSprite = greenBackground;
        break;
      case 3:
        normalSprite = blueSprite;
        selectedSprite = blueSelection;
        backgroundSprite = blueBackground;
        break;
      case 4:
        normalSprite = yellowSprite;
        selectedSprite = yellowSelection;
        backgroundSprite = yellowBackground;
        break;
      case 5:
        normalSprite = cyanSprite;
        selectedSprite = cyanSelection;
        backgroundSprite = cyanBackground;
        break;
      case 6:
        normalSprite = magentaSprite;
        selectedSprite = magentaSelection;
        backgroundSprite = magentaBackground;
        break;
      default:
        break;
    }
    SetNormalImage();
    if (_value == 0)
    {
      gameObject.GetComponent<Image>().enabled = false;
      //gameObject.transform.Find("Text").GetComponent<Text>().text = "";
    }
    else
    {
      if (!gameObject.GetComponent<Image>().isActiveAndEnabled)
      {
        gameObject.GetComponent<Image>().enabled = true;
      }
      //gameObject.transform.Find("Text").GetComponent<Text>().text = _value.ToString();
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

  void InstantiateBackground()
  {
    GameObject prefabAnimation = Instantiate(backgroundAnimation, transform.localPosition, backgroundAnimation.transform.rotation) as GameObject;
    prefabAnimation.GetComponent<RectTransform>().localScale = new Vector3(transform.localScale.x, transform.localScale.x, transform.localScale.x);
    prefabAnimation.transform.SetParent(GameObject.Find("BGGrid").transform.GetChild(x), false);
    prefabAnimation.GetComponent<HiveAnimation>().BeginScale(scaleAnimation, animationSpeed, backgroundSprite);
  }
}
