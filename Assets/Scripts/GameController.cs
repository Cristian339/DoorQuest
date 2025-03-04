using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class GameController : MonoBehaviour
{
    private static float playerHealth;
    private static int rounds = 1;
    private static int score = 0;
    private static int nextLevelEnemyIndex = 0;
    private static int nextLevelEnemy = 0;
    private static int[] enemiesByLevel;
    private bool inGameOver;
    private bool isGamePause;

    [SerializeField] private Player player;

    [Header("Door Configuration")]
    [SerializeField] private Transform[] doorPoints;
    [SerializeField] private Door doorPrefab;

    [Header("Key Configuration")]
    [SerializeField] private Transform[] keyPoints;
    [SerializeField] private Key keyPrefab;

    [Header("Others")]
    [SerializeField] private EnemiesSpawner spawner;
    [SerializeField] private UIController uiController;

    [SerializeField] private GameObject GameUI;
    [SerializeField] private GameObject PauseUI;

    // Start is called before the first frame update
    void Start()
    {
        if (rounds == 1)
        {
            playerHealth = player.GetLives();
            spawner.Initialize();
        }

        player.SetLives(Mathf.Min(playerHealth, player.GetLives()));

        if (rounds > 1) spawner.EnemiesByLevel = enemiesByLevel;

        uiController.UpdateScore(score);

        Transform actualDoorPoint = doorPoints[Random.Range(0, doorPoints.Length)];
        var door = Instantiate(doorPrefab, actualDoorPoint.position, Quaternion.identity);

        door.OnDoorOpen += RestartLevel;

        Transform actualKeyPoint = keyPoints[Random.Range(0, keyPoints.Length)];
        var key = Instantiate(keyPrefab, actualKeyPoint.position, Quaternion.identity);

        player.OnDie += GameOver;
        player.OnGetKey += uiController.ShowKeyUI;
    }

    private void RestartLevel()
    {
        if ((rounds + 1) % 2 == 0)
        {
            enemiesByLevel = spawner.EnemiesByLevel;
            nextLevelEnemyIndex = Random.Range(0, enemiesByLevel.Length);
            nextLevelEnemy = ++enemiesByLevel[nextLevelEnemyIndex];
        }

        score += 100;
        rounds++;
        playerHealth = player.GetLives();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void GameOver()
    {
        inGameOver = true;
        uiController.ShowGameOver(score);
        StartCoroutine(WaitForGameOverRestart());
    }

    private IEnumerator WaitForGameOverRestart()
    {
        yield return new WaitForSeconds(1.25f);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && !inGameOver)
        {
            Time.timeScale = 0f;
            GameUI.SetActive(false);
            PauseUI.SetActive(true);
            isGamePause = true;
            player.Pause = true;
        }
#if DEBUG
        if (Input.GetKeyDown(KeyCode.N))
        {
            RestartLevel();
        }
#endif
    }

    public void ContinueGame()
    {
        Time.timeScale = 1;
        PauseUI.SetActive(false);
        GameUI.SetActive(true);
        isGamePause = false;
        player.Pause = false;
    }

    public void ExitGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MenuScene");
    }

    public void RestartGame()
    {
        score = 0;
        rounds = 1;
        playerHealth = 0;
        spawner.RestartSpawner();
        SceneManager.LoadScene("GameScene");
    }
}
