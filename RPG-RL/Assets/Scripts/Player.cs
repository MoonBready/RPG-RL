using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Movement
{
    public int wallDamage = 1;
    public int pointPerRedPotion = 1;
    public int pointPerBluePotion = 2;
    public float restartLevelDelay = 1f;

    private Animator animator;
    private int potion;

    void Start()
    {
        transform.position = new Vector3(0, 0, 0);

        animator = GetComponent<Animator>();

        potion = GameManager.instance.playerPotionNb;
    }


    private void OnDisable()
    {
        GameManager.instance.playerPotionNb = potion;
    }


    // Update is called once per frame
    void Update()
    {
        
    }


    protected void AttemptMove<T>(int xDir, int yDir)
    {
        potion--;

        CheckIfGameOver();

        GameManager.instance.playersTurn = false;
    }

    protected void OnCantMove<T>(T component)
    {
        Wall hitWall = component as Wall;

        hitWall.DamageWall(wallDamage);

        animator.SetTrigger("PlayerAttack");
    }



    private void CheckIfGameOver()
    {
        if (potion <= 0)
            GameManager.instance.GameOver();
    }
}
