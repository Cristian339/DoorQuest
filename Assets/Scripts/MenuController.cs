using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [SerializeField] private GameObject menuCanvas;

    public void GoToGame()
        => SceneManager.LoadScene("GameScene");

    public void ExitGame()
        => Application.Quit();

    public void GoToMenu()
    {
        menuCanvas.SetActive(true);
    }
}
