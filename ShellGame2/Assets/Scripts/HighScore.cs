using System;
using UnityEngine;

public static class HighScore
{
    private static int highScore;

    public static int Value
    {
        get { return highScore; }
        set
        {
            if (value > highScore) // if value is greater than current high score change it
            {
                highScore = value;
            }
        }
    }

    public static void LoadHighScore() // When you start up the game, will load the highest score ever got 
    {
        //                                         \/ if game never yet be played, load this default 
        highScore = PlayerPrefs.GetInt("HighScore", 0);
    }

    public static void SaveHighScore() // Will save high score when game shut down 
    {
        PlayerPrefs.SetInt("HighScore", highScore);
    }

}

