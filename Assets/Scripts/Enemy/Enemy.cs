using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Enemy : MonoBehaviour
{
    Rigidbody2D rb;
    protected Animator anim;
    PhysicsCheck physicsCheck;
    [Header("基本参数")]
    public float normalSpeed;
    public float chassSpeed;

    public float currentSpeed;
    public Vector3 faceDir;

    [Header("计时器")]
    public float waitTime;
    public float waitTimeCounter;
    public bool wait;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        physicsCheck = GetComponent<PhysicsCheck>();
        currentSpeed = normalSpeed;
        waitTimeCounter = waitTime;
    }
    private void Update()
    {
        faceDir = new Vector3(-transform.localScale.x, 0, 0);

        if ((physicsCheck.touchLeftWall && faceDir.x < 0) || (physicsCheck.touchRightWall && faceDir.x > 0))
        {
            wait = true;
            anim.SetBool("walk", false);

        }
        TimeCounter();
    }
    private void FixedUpdate()
    {
        Move();
    }
    public virtual void Move()
    {
        rb.velocity = new Vector2(currentSpeed * faceDir.x * Time.deltaTime, rb.velocity.y);

    }
    public void TimeCounter()
    {
        if (wait)
        {
            waitTimeCounter -= Time.deltaTime;
            if (waitTimeCounter <= 0)
            {
                wait = false;
                waitTimeCounter = waitTime;
                transform.localScale = new Vector3(faceDir.x, 1, 1);
            }
        }
    }
}
