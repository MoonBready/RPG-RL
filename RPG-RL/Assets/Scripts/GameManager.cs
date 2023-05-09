using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public float levelStartDelay = 2f;
    public float turnDelay = 1f;
    public static GameManager instance = null;
    private BoardManager boardScript;

    public int playerPotionNb = 10;
    [HideInInspector] public bool playersTurn = true;

    private TMPro.TMP_Text stageText;
    private GameObject stageImage;
    private int level = 0;
    private List<Enemy> enemies;
    private bool enemiesMoving;
    private bool setup;


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


    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        level++;

        InitGame();
    }


    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }


    void InitGame()
    {
        setup = true;

        stageImage = GameObject.Find("StageImage");
        stageText = GameObject.Find("StageText").GetComponent<TMPro.TMP_Text>();
        stageText.text = "Stage " + level;

        stageImage.SetActive(true);

        Invoke("HideLevelImage", levelStartDelay);

        enemies.Clear();
        boardScript.SetupScene(level);
    }

    void HideLevelImage()
    {
        stageImage.SetActive(false);

        setup = false;
    }

    void Update()
    {
        if (playersTurn || enemiesMoving || setup)
            return;

        StartCoroutine(MoveEnemies());
    }



    public void AddEnemy(Enemy script)
    {
        enemies.Add(script);
    }


    public void GameOver()
    {
        stageText.text = "You have lasted for " + level + " stages and lost all your potions.";

        stageImage.SetActive(true);

        enabled = false;
    }


    IEnumerator MoveEnemies()
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
