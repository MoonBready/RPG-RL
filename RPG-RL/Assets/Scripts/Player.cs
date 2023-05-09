using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Player : ObjectMovement
{
    public int wallDamage = 1;
    public int pointPerRedPotion = 1;
    public int pointPerBluePotion = 2;
    public float restartLevelDelay = 1f;
    public TMPro.TMP_Text potionText;

    private Animator animator;
    private int potion;

    protected override void Start()
    {
        animator = GetComponent<Animator>();

        potion = GameManager.instance.playerPotionNb;

        potionText.text = "Potion " + potion;

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
        potionText.text = "Potion: " + potion;

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

            potionText.text = "+" + pointPerRedPotion + " Potion: " + potion;

            other.gameObject.SetActive(false);
        }
        else if(other.tag == "Blue Potion")
        {
            potion += pointPerBluePotion;

            potionText.text = "+" + pointPerBluePotion + " Potion: " + potion;

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
        potionText.text = "- " + "Potion: " + potion;
        CheckIfGameOver();
    }



    private void CheckIfGameOver()
    {
        if (potion <= 0)
            GameManager.instance.GameOver();
    }
}
