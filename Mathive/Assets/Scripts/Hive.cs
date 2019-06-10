using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hive : Hexagon
{
	
	private float scaleAnimation = 1.2f;
	private float animationSpeed = 2.5f;
	public HiveType hiveType;

	private Sprite normalSprite;
	private Sprite selectedSprite;
	private Sprite backgroundSprite;

	public Sprite blockSprite;

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
	private bool stillSelecting = false;

	private void Start()
	{
		imageHive = gameObject.GetComponent<Image>();
	}

	// Implement that it holds image value

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

	public void SetSelectedImage()
	{
		selected = true;
		stillSelecting = true;
		Invoke("DelaySelectedHive", 0.1f);
		InstantiateBackground();
	}

	void DelaySelectedHive()
	{
		if (stillSelecting)
		{
			imageHive.overrideSprite = selectedSprite;
			stillSelecting = false;
		}
	}

	public void SetNormalImage()
	{
		if (stillSelecting) stillSelecting = false;

		selected = false;
		imageHive.overrideSprite = normalSprite;
	}

	public void setIceHiveImage(){
		imageHive.sprite = blockSprite;
	}

	public void OnValueChanged(HiveType hiveTypeValue)
	{
		if(gameObject.tag != "Blockage"){
			hiveType = hiveTypeValue;
			Value = (int)hiveTypeValue;
		}
		switch (hiveTypeValue)
		{
			case HiveType.background:
				SetAsBackground();
				normalSprite = Resources.Load<Sprite>("Sprites/greyBackground");
				break;
			case HiveType.blockage:
				gameObject.tag = "Blockage";
				OnValueChanged((HiveType)UnityEngine.Random.Range(1, 7));
				InstantiateIceHive();
				break;
			case HiveType.red:
				normalSprite = redSprite;
				selectedSprite = redSelection;
				backgroundSprite = redBackground;
				break;
			case HiveType.green:
				normalSprite = greenSprite;
				selectedSprite = greenSelection;
				backgroundSprite = greenBackground;
				break;
			case HiveType.blue:
				normalSprite = blueSprite;
				selectedSprite = blueSelection;
				backgroundSprite = blueBackground;
				break;
			case HiveType.yellow:
				normalSprite = yellowSprite;
				selectedSprite = yellowSelection;
				backgroundSprite = yellowBackground;
				break;
			case HiveType.cyan:
				normalSprite = cyanSprite;
				selectedSprite = cyanSelection;
				backgroundSprite = cyanBackground;
				break;
			case HiveType.magenta:
				normalSprite = magentaSprite;
				selectedSprite = magentaSelection;
				backgroundSprite = magentaBackground;
				break;
			case HiveType.empty:
				gameObject.GetComponent<Image>().enabled = false;
				break;
			default:
				break;
		}
		SetNormalImage();

		if(!hiveTypeValue.Equals(HiveType.empty)){
			if (!gameObject.GetComponent<Image>().isActiveAndEnabled)
			{
				gameObject.GetComponent<Image>().enabled = true;
			}
		}
	}

	public void SetVisible(){
		if (!gameObject.GetComponent<Image>().isActiveAndEnabled)
		{
			gameObject.GetComponent<Image>().enabled = true;
		}
	}

	public void SetHive(int setValue, int xValue, int yValue)
	{
		OnValueChanged((HiveType)setValue);
		OnPositionChanged(xValue, yValue);
	}

	public void DestroyMyself()
	{
		Destroy(gameObject);
	}

	public void SetAsBackground(){
		Destroy(gameObject.GetComponent<PolygonCollider2D>());
	}

	void InstantiateBackground()
	{
		GameObject prefabAnimation = Instantiate(backgroundAnimation, transform.localPosition, backgroundAnimation.transform.rotation) as GameObject;
		prefabAnimation.GetComponent<RectTransform>().localScale = new Vector3(transform.localScale.x, transform.localScale.x, transform.localScale.x);
		prefabAnimation.transform.SetParent(GameObject.Find("BGGrid").transform.GetChild(X), false);
		prefabAnimation.GetComponent<HiveAnimation>().BeginScale(scaleAnimation, animationSpeed, backgroundSprite);
	}

	public HiveType GetHiveType{
		get{
			return hiveType;
		}
	}

	Hive InstantiateIceHive()
	{
		GameObject prefabHive = Instantiate(Resources.Load("IceHive"), Vector3.zero, transform.rotation) as GameObject;
		prefabHive.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
		prefabHive.tag = "Blockage";
		prefabHive.transform.SetParent(gameObject.transform, false);
		Hive tempHive = prefabHive.GetComponent<Hive>();
		tempHive.setIceHiveImage();
		gameObject.AddComponent<IceHive>();
		return tempHive;
	}
}

public enum HiveType
{
	background = -2,
	blockage = -1,
	empty = 0,
	red = 1,
	blue = 2,
	yellow = 3,
	green = 4,
	magenta = 5,
	cyan = 6
}