using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceHive : MonoBehaviour
{
	private int numberOfTimesLeftToHit = 3;

	public void LowerNumberOfHitsLeft(){
		if(numberOfTimesLeftToHit > 0) numberOfTimesLeftToHit--;
	}

	public int NumberOfTimesLeftToHit{
		get {
			return numberOfTimesLeftToHit;
		}
	}
}
