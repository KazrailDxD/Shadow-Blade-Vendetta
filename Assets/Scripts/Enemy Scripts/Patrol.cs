using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : MonoBehaviour
{
    public float speed;
    public float rayDistance = 0;

    private bool movingRight = true;
    private bool running = true;
    public Transform groundDetection;

    float distanceToPlayer;

    public PlayerController player;

    //Animation Enemie
    Animator animator = null;

    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
        animator = GetComponent < Animator>();

        movingRight = Random.Range(0, 2) == 0;

        Turn();
    }

    void Update()
    {

        distanceToPlayer =(player.transform.position - transform.position).magnitude;

        running = distanceToPlayer > 0.4f;

        if (running)
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
        }

        animator.SetBool("IsRunning",running);
        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, rayDistance);
        if (groundInfo.collider == null)
        {
            Turn();
        }
    }

    private void Turn()
    {
        if (movingRight == true)
        {
            transform.eulerAngles = new Vector3(0, -180, 0);
            movingRight = false;
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            movingRight = true;
        }
    }
}
