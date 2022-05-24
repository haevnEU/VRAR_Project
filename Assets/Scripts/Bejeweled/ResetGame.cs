using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetGame : MonoBehaviour
{
    [SerializeField] public GameObject game;
    public void resetGame()
    {
        if(game != null)
        {
            game.GetComponent<GameRenderer>().restart(); 
        }
    }
}
