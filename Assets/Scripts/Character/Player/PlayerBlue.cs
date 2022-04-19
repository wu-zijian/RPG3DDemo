using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlue : Player
{
    public Transform cam;
    private bool isAlt = false;

    #region 生命周期
    protected override void Start()
    {
        base.Start();
        cam = Camera.main.transform;
        // Cursor.lockState = CursorLockMode.Locked;
        fsmMrg.AddState(new BlueIdle(anim));
        fsmMrg.AddState(new BlueMove(anim));
        fsmMrg.AddState(new BlueJump(anim));
        fsmMrg.AddState(new BlueAttack(anim));
        fsmMrg.AddState(new BlueHurt(anim));
        fsmMrg.AddState(new BlueDeath(anim));
        if (FightSceneCtrl.Instance != null)
        {
            FightSceneCtrl.Instance.cam = cam.gameObject;
        }
    }
    private void FixedUpdate()
    {
        isAlt = Input.GetKey(KeyCode.LeftAlt);
        if (isDeath || isHurt) return;
        if (Input.GetButton("Fire1")) { isAttack = true; return; }
        if (Input.GetButtonDown("Jump")) { isJump = true; return; }
        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0) { isMoving = true; return; }
    }
    void Update()
    {
        fsmMrg.Update();
        playState = PlayState.Idle;
        if (isMoving) { playState = PlayState.Move; }
        if (isJump || isFall) { playState = PlayState.Jump; }
        if (isAttack) { playState = PlayState.Attack; }
        if (isHurt) { playState = PlayState.Hurt; }
        if (isDeath) { playState = PlayState.Death; }

        switch (playState)
        {
            case PlayState.Idle:
                Idle();
                break;
            case PlayState.Jump:
                Jump();
                break;
            case PlayState.Move:
                Move();
                break;
            case PlayState.Attack:
                Attack();
                break;
            case PlayState.Hurt:
                Hurt();
                break;
            case PlayState.Death:
                Death();
                break;
            default:
                break;
        }
        //模拟重力
        if (characterController.isGrounded || (isJump && (!isAttack || !isHurt || !isDeath)))
        {
            isFall = false;
            velocity.y = 0;
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
            move += transform.up * velocity.y;
        }
        characterController.Move(move);
        if (isAlt)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
    #endregion

    #region 行为
    protected override void Idle()
    {
        move = Vector3.zero;
        fsmMrg.ChangeState((int)PlayState.Idle);
    }

    private float turnSmoothTime = 0.1f, turnSmoothVelocity;//平滑过渡参数
    private void Move()
    {
        if (isAlt) return;
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        if (horizontal == 0 && vertical == 0) isMoving = false;
        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;//移动方向的单位向量

        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;//前进方向
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);//平滑过渡
        transform.rotation = Quaternion.Euler(0f, angle, 0f);//修改四元数

        Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        move = moveDir.normalized * speed * Time.deltaTime;
        fsmMrg.ChangeState((int)PlayState.Move);
    }

    private float jumpTime = 0;
    private float jumpCd = 0.2f;
    private Vector3 jumpDir = new Vector3(0, 0, 0);
    private void Jump()
    {
        if (characterController.isGrounded)
        {
            if (jumpTime < Time.time)
            {
                if (isJump)
                {
                    jumpTime = Time.time + jumpCd;
                    float horizontal = Input.GetAxisRaw("Horizontal");
                    float vertical = Input.GetAxisRaw("Vertical");
                    jumpDir = (cam.transform.right * horizontal
                    + new Vector3(cam.transform.forward.x, 0, cam.transform.forward.z)
                    * vertical).normalized * speed * Time.deltaTime;
                    move = transform.up * jumpSpeed * Time.deltaTime + jumpDir;
                    fsmMrg.ChangeState((int)PlayState.Jump);
                }
                else
                {
                    isJump = false;//一般不需要，个别情况需要；
                    isFall = false;
                    move = Vector3.zero;
                }
            }
        }
        else//在半空中时
        {
            if (jumpTime > Time.time)
            {
                move = transform.up * jumpSpeed * Time.deltaTime + jumpDir;
            }
            else
            {
                if (isJump)
                {
                    isJump = false;
                    isFall = true;
                }
            }
        }
    }

    protected void Attack()
    {
        move = Vector3.zero;
        fsmMrg.ChangeState((int)PlayState.Attack);
    }

    protected void Hurt()
    {
        if (hurt < Time.time)
        {
            isHurt = false;
        }
        fsmMrg.ChangeState((int)PlayState.Hurt);
    }
    #endregion

    public override void initState()
    {
        isAttack = false;
        isHurt = false;
        isJump = false;
        isMoving = false;
    }

}
