using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hexagon : MonoBehaviour {

    [SerializeField] int _value;

    [SerializeField] private int x;
    [SerializeField] private int y;

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
        gameObject.name = string.Format("{0}{1}", x + 1, y + 1);
        transform.SetSiblingIndex(newY);
    }
}
