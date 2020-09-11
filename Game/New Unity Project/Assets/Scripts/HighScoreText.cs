using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class HighScoreText : MonoBehaviour
{
    Text highscore;

    void OnEnable()//Refreshed wverytime the Start Page is Activated
    {
        highscore = GetComponent<Text>();
        highscore.text = PlayerPrefs.GetInt("HighScore").ToString();
    }
}
 