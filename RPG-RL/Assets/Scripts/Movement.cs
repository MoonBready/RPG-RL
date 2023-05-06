using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float moveSpeed;
    private bool isMoving;
    private Vector2 input;

    private void Start()
    {
        transform.position = new Vector3(0, 0, 0);
    }

    private void Update()
    {
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");

        if (input.x != 0) input.y = 0;

        var targetPos = transform.position;
        targetPos.x += input.x;
        targetPos.y += input.y;

        transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
    }

    IEnumerator Move(Vector3 targetPos)
    {
        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPos;

        isMoving = false;
    }
}
