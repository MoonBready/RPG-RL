using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : ObjectMovement
{
    public int wallDamage = 1;
    public int pointPerRedPotion = 1;
    public int pointPerBluePotion = 2;
    public float restartLevelDelay = 1f;

    private Animator animator;
    private int potion;

    protected override void Start()
    {

        animator = GetComponent<Animator>();

        potion = GameManager.instance.playerPotionNb;

        base.Start();
    }


    private void OnDisable()
    {
        GameManager.instance.playerPotionNb = potion;
    }

    private void Update()
    {
        if (!GameManager.instance.playersTurn) return;

        int horizontal = 0;
        int vertical = 0;

        horizontal = (int)Input.GetAxisRaw("Horizontal");
        vertical = (int)Input.GetAxisRaw("Vertical");

        if (horizontal != 0)
            vertical = 0;

        if (horizontal != 0 || vertical != 0)
            AttemptMove<Wall>(horizontal, vertical);
    }



    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        potion--;

        base.AttemptMove<T>(xDir, yDir);

        CheckIfGameOver();

        GameManager.instance.playersTurn = false;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Exit")
        {
            Invoke("Restart", restartLevelDelay);
            enabled = false;
        }
        else if(other.tag == "Red Potion")
        {
            potion += pointPerRedPotion;
            other.gameObject.SetActive(false);
        }
        else if(other.tag == "Blue Potion")
        {
            potion += pointPerBluePotion;
            other.gameObject.SetActive(false);
        }
    }


    protected override void cantMove<T>(T component)
    {
        Wall hitWall = component as Wall;
        hitWall.DamageWall(wallDamage);
        animator.SetTrigger("PlayerAttack");
    }


    private void Restart()
    {
        SceneManager.LoadScene(0);

    }


    public void LosePotion(int loss)
    {
        animator.SetTrigger("PlayerHit");
        potion -= loss;
        CheckIfGameOver();
    }



    private void CheckIfGameOver()
    {
        if (potion <= 0)
            GameManager.instance.GameOver();
    }
}
