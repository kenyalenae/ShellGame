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
            if (value > highScore)
            {
                highScore = value;
            }
        }
    }

    public static void LoadHighScore()
    {
        highScore = PlayerPrefs.GetInt("HighScore", 0);
    }

    public static void SaveHighScore()
    {
        PlayerPrefs.SetInt("HighScore", highScore);
    }

}

