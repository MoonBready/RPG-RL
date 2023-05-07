using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;
    private Vector2 input;
    public LayerMask Blocking;

    private Rigidbody2D rb;
    private Collider2D coll;

    private void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
    }

    void FixedUpdate()
    {
        {
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");

            var targetPos = transform.position;
            targetPos.x += input.x;
            targetPos.y += input.y;

            Vector2 movement = new Vector2(input.x, input.y).normalized * moveSpeed;

            rb.MovePosition(rb.position + movement * Time.fixedDeltaTime);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Blocking")
        {
            rb.velocity = Vector2.zero;
        }
    }
}