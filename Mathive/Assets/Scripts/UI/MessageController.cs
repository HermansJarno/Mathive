using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageController
{
    string[] normalCompliments = new string[] {"GOOD THINKING", "COOL!", "THAT'S IT", "KEEP IT UP!"};
    string[] midCompliments = new string[] {"EXCELLENT!", "NICE!", "GREAT!"};
    string[] highCompliments = new string[] {"AWESOME!", "FANTASTIC!", "TREMENDOUS!"};
    string[] higherCompliments = new string[] {"MARVELOUS!", "SUPERB!", "BEAUTIFUL!"};
    string[] highestCompliments = new string[] {"MINDBLOWING!", "GODLIKE!", "INSANE!"};

    // Update is called once per frame
    public void processScore(int score)
    {
        if(score >= 16 && score < 32){
            new Message(normalCompliments[Random.Range(0, normalCompliments.Length)]);
        }
        if(score >= 32 && score < 64){
            new Message(midCompliments[Random.Range(0, midCompliments.Length)]);
        }
        if(score >= 64 && score < 128){
            new Message(highCompliments[Random.Range(0, highCompliments.Length)]);
        } 
        if(score >= 128 && score < 256){
            new Message(highCompliments[Random.Range(0, highCompliments.Length)]);
        }
        if(score >= 256){
            new Message(highestCompliments[Random.Range(0, highestCompliments.Length)]);
        }
    }

    public void showOutOfMoves(){
        new Message("NO OPTIONS LEFT SHUFFLING GRID");
    }
}
