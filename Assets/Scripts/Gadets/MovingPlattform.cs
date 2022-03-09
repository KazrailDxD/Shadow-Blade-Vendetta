using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlattform : MonoBehaviour
{
    [SerializeField] GameObject Player;

    private Vector3 startPos;
    private Vector3 newPos;

    private int speed = CONSTANTS.PLATFORM_SPEED;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        newPos = startPos;
        newPos.x = newPos.x + Mathf.PingPong(Time.time * speed, 4) - 2;

        transform.position = newPos;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject == Player) 
        {
            Player.transform.parent = transform;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == Player)
        {
            Player.transform.parent = null;
        }
    }
}
