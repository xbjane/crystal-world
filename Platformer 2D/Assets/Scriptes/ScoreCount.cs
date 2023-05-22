using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreCount : MonoBehaviour
{
    [SerializeField] TMP_Text scoreText;
    [SerializeField] int score;
    private void Start()
    {
        score = 0;
        scoreText.text = score.ToString();
    }
    public void AddCrystal(int count)
    {
        score += count;
        Debug.Log(score);
        scoreText.text = score.ToString();
    }
}
