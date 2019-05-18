using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSpecs : MonoBehaviour
{
    public Canvas _canvas;
    public RectTransform _rectCanvas;
    public float _scale = 0.82f;
    public float _refWidth = 800f;
    public float _refHeight = 1200f;

    public float getScale(){
		float widthScreen = _rectCanvas.rect.width;
		float heightScreen = _rectCanvas.rect.height;

		// Calculate scale
		float scaleX = (widthScreen / _refWidth) * _scale;
		float scaleY = (heightScreen / _refHeight) * _scale;

		if(scaleX > scaleY){
			scaleX = scaleY;
		}
        return scaleX;
    }
}
