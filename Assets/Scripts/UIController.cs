using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText1;
    [SerializeField] private TMP_Text scoreText2;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TMP_Text highScore1;
    [SerializeField] private TMP_Text highScore2;
    [SerializeField] private GameObject key;

    public void UpdateScore(int score)
    {
        scoreText1.text = $"Puntaje: {score}";
        scoreText2.text = $"Puntaje: {score}";
    }

    public void ShowGameOver(int score)
    {
        highScore1.text = $"Puntaje maximo: {score}";
        highScore2.text = $"Puntaje maximo: {score}";
        gameOverPanel.SetActive(true);
    }

    public void ShowKeyUI()
        => key.SetActive(true);
}
