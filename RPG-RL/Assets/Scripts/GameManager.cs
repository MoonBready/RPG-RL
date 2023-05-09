using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public float turnDelay = 1f;
    public static GameManager instance = null;
    private BoardManager boardScript;

    public int playerPotionNb = 10;
    [HideInInspector] public bool playersTurn = true;

    private int level = 3;
    private List<Enemy> enemies;
    private bool enemiesMoving;


    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        enemies = new List<Enemy>();
        boardScript = GetComponent<BoardManager>();
        InitGame();
    }

    void InitGame()
    {
        enemies.Clear();
        boardScript.SetupScene(level);
    }

    public void GameOver()
    {
        enabled = false;
    }

    IEnumerable MoveEnemies()
    {
        enemiesMoving = true;
        yield return new WaitForSeconds(turnDelay);

        if(enemies.Count == 0)
        {
            yield return new WaitForSeconds(turnDelay);
        }

        for(int i = 0; i < enemies.Count; i++)
        {
            enemies[i].MoveEnemy();

            yield return new WaitForSeconds(enemies[i].moveTime);
        }

        playersTurn = true;

        enemiesMoving = false;
    }
}
