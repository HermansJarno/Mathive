using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageController
{
    string[] normalCompliments = new string[] {"GOOD THINKING", "NICE!", "THAT'S IT", "KEEP IT UP!", "GREAT!"};
    string[] midCompliments = new string[] {"EXCELLENT!", "NEAT!", "AMAZING!", "BEAUTIFUL!", "AWESOME!"};
    string[] highCompliments = new string[] {"FANTASTIC!", "TREMENDOUS!", "GODLIKE!", "MARVELOUS!", "SUPERB!"};

    public void processScore(int score)
    {
        if(score >= 32 && score < 64){
            new Message(normalCompliments[Random.Range(0, normalCompliments.Length)]);
        }
        if(score >= 64 && score < 128){
            new Message(midCompliments[Random.Range(0, midCompliments.Length)]);
        }
        if(score >= 128){
            new Message(highCompliments[Random.Range(0, highCompliments.Length)]);
        } 
    }

    public void showRemainingMoves() {
        new Message("5 MOVES LEFT!");
    }

    public void showOutOfMoves(){
        new Message("NO OPTIONS LEFT SHUFFLING GRID");
    }
}
