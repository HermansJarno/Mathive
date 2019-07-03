using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Message : MonoBehaviour
{
    private string messageText = "";
    GameObject spawnedMessageBox;

    public Message(string text){
        GameObject messageBox = Resources.Load("MessageBox") as GameObject;
        messageBox.GetComponent<Text>().text = text;

        spawnedMessageBox = Instantiate(messageBox, messageBox.transform.position, messageBox.transform.rotation) as GameObject;
        spawnedMessageBox.AddComponent<MessageAnimation>();
		spawnedMessageBox.transform.SetParent(GameObject.Find("MidPanel").transform, false);
        
        Destroy(spawnedMessageBox, 2.0f);
        Destroy(this, 2.1f);
    }
}
