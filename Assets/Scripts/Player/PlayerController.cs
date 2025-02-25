using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.Remoting.Messaging;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    public PlayerInputControl inputControl;
    private PhysicsCheck physicsCheck;
    private PlayerAnimation playerAnimation;
    public Vector2 inputDirection;
    private Rigidbody2D rb;
    private CapsuleCollider2D coll;



    [Header("基本参数")]

    public float speed;
    private float runSpeed;
    private float walkSpeed => speed / 2.5f;
    public float jumpForce;

    private Vector2 originalOffset;
    private Vector2 OriginalSize;
    [Header("物理材质")]
    public PhysicsMaterial2D normal;
    public PhysicsMaterial2D wall;

    public float hurtForce;
    [Header("状态")]
    public bool isCrouch;
    public bool isHurt;
    public bool isDead;
    public bool isAttack;
    private void Awake()
    {

        rb = GetComponent<Rigidbody2D>();
        physicsCheck = GetComponent<PhysicsCheck>();
        coll = GetComponent<CapsuleCollider2D>();
        playerAnimation = GetComponent<PlayerAnimation>();

        originalOffset = coll.offset;
        OriginalSize = coll.size;
        inputControl = new PlayerInputControl();
        inputControl.Gameplay.Jump.started += Jump;
        #region 强制走路
        runSpeed = speed;
        inputControl.Gameplay.WalkButton.performed += ctx =>
        {
            if (physicsCheck.isGround)
                speed = walkSpeed;
        };

        inputControl.Gameplay.WalkButton.canceled += ctx =>
        {
            if (physicsCheck.isGround)
                speed = runSpeed;
        };
        #endregion
        //攻击
        inputControl.Gameplay.Attack.started += PlayerAttack;
    }



    private void OnEnable()
    {
        inputControl.Enable();
    }

    private void OnDisable()
    {
        inputControl.Disable();
    }

    private void Update()
    {

        inputDirection = inputControl.Gameplay.Move.ReadValue<Vector2>();
        CheckState();
    }

    private void FixedUpdate()
    {
        if (!isHurt && !isAttack)
            Move();

    }

    public void Move()
    {
        //人物移动
        if (!isCrouch)
            rb.velocity = new Vector2(inputDirection.x * speed * Time.deltaTime, rb.velocity.y);

        int faceDir = (int)transform.localScale.x;

        if (inputDirection.x > 0)
            faceDir = 1;
        if (inputDirection.x < 0)
            faceDir = -1;
        // 人物翻转
        transform.localScale = new Vector3(faceDir, 1, 1);

        //人物下蹲
        isCrouch = inputDirection.y < -0.5f && physicsCheck.isGround;
        if (isCrouch)
        {
            //改变碰撞体
            coll.offset = new Vector2(-0.05f, 0.85f);
            coll.size = new Vector2(0.7f, 1.7f);
        }
        else
        {
            //恢复原来
            coll.offset = originalOffset;
            coll.size = OriginalSize;
        }
    }
    private void Jump(InputAction.CallbackContext context)
    {
        if (physicsCheck.isGround)
            rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
    }
    private void PlayerAttack(InputAction.CallbackContext context)
    {
        if (physicsCheck.isGround)
        {
            playerAnimation.PlayerAttack();
            isAttack = true;
        }
    }


    #region UnityEvent    
    public void GetHurt(Transform attacker)
    {
        isHurt = true;
        rb.velocity = Vector2.zero;//速度置零
        Vector2 dir = new Vector2((transform.position.x - attacker.position.x), 0).normalized;//只要个正负

        rb.AddForce(dir * hurtForce, ForceMode2D.Impulse);
    }

    public void PlayerDead()
    {
        isDead = true;
        inputControl.Gameplay.Disable();//不能控制行动。
    }
    #endregion

    private void CheckState()
    {
        coll.sharedMaterial = physicsCheck.isGround ? normal : wall;
    }

}
