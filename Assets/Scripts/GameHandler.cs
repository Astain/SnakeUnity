using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    [SerializeField]
    private Snake snake;
    private LevelGrid levelGrid;

    private static GameHandler instance;

    private static int score;
    private void Awake()
    {
        instance = this;
        score = 0;
    }
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("GameHandler.Start");

        levelGrid = new LevelGrid(20, 20);
        
        snake.Setup(levelGrid);
        levelGrid.Setup(snake);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
    }
    public static int GetScore()
    {
        return score;
    }

    public static void AddScore()
    {
        score += 100;
    }
    public static void PauseGame()
    {
        PauseWindow.ShowStatic();
        //"Stop" the time
        Time.timeScale = 0f;
    }
    public static void ResumeGame()
    {
        PauseWindow.HideStatic();
        //Resume the timescale to 1
        Time.timeScale = 1f;
    }
    public static void SnakeDied()
    {
        GameOverWindow.ShowStatic();
    }
}
