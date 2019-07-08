using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDestroyer : MonoBehaviour
{
    public GameObject objectToDestroy;

    public void Destroy(){
        Destroy(objectToDestroy);
    }
}
