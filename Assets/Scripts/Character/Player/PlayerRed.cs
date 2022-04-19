using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRed : Player
{
    public GameObject cam;
    private bool isAlt = false;
    public int bulletNum = 35;

    [Header("准星的长度")]
    public float width;
    [Header("准星的高度")]
    public float height;
    [Header("上下（左右）两条准星之间的距离")]
    public float distance;
    [Header("准星背景图")]
    public Texture2D crosshairTexture;

    private GUIStyle lineStyle;     //  GUI自定义参数
    private Texture tex;            //  准星背景辅助参数

    #region 生命周期
    protected override void Start()
    {
        base.Start();
        cam = transform.Find("Character/Camera").gameObject;
        lineStyle = new GUIStyle();                         //  游戏开始实例化背景图
        lineStyle.normal.background = crosshairTexture;     //  将背景图默认背景设为准星背景
        // Cursor.lockState = CursorLockMode.Locked;s
        fsmMrg.AddState(new RedIdle(anim));
        fsmMrg.AddState(new RedMove(anim));
        fsmMrg.AddState(new RedJump(anim));
        fsmMrg.AddState(new RedAttack(anim));
        fsmMrg.AddState(new RedHurt(anim));
        fsmMrg.AddState(new RedDeath(anim));
        if (FightSceneCtrl.Instance != null)
        {
            FightSceneCtrl.Instance.cam = cam;
        }
    }
    private void FixedUpdate()
    {
        isAlt = Input.GetKey(KeyCode.LeftAlt);
        isAttack = Input.GetButton("Fire1");
        if (isDeath) return;
        // if (Input.GetButton("Fire1")) { isAttack = true; attack = Time.time + 0.01f; return; }
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
        diversion();
    }

    private void OnGUI()
    {
        //  左准星
        GUI.Box(new Rect(Screen.width / 2 - distance / 2 - width, Screen.height / 2 - height / 2, width, height), tex, lineStyle);
        //  右准星
        GUI.Box(new Rect(Screen.width / 2 + distance / 2, Screen.height / 2 - height / 2, width, height), tex, lineStyle);
        //  上准星
        GUI.Box(new Rect(Screen.width / 2 - height / 2, Screen.height / 2 - distance / 2 - width, height, width), tex, lineStyle);
        //  下准星
        GUI.Box(new Rect(Screen.width / 2 - height / 2, Screen.height / 2 + distance / 2, height, width), tex, lineStyle);
    }
    #endregion

    #region 行为
    protected override void Idle()
    {
        move = Vector3.zero;
        fsmMrg.ChangeState((int)PlayState.Idle);
    }


    private float mouseSensitivity = 100f;//平滑过渡参数
    private float xRotation = 0f;
    private void Move()
    {
        if (isAlt) return;
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        if (horizontal == 0 && vertical == 0) isMoving = false;
        move = (transform.right * horizontal + transform.forward * vertical) * speed * Time.deltaTime;
        if (playState == PlayState.Move)
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
                    jumpDir = (transform.right * horizontal + transform.forward * vertical).normalized * speed * Time.deltaTime;
                    move = transform.up * jumpSpeed * Time.deltaTime + jumpDir;
                    fsmMrg.ChangeState((int)PlayState.Jump);
                }
                else//一般不需要，个别情况需要；
                {
                    isJump = false;
                    isFall = false;
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
        Move();
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

    protected void diversion()//角色方向
    {
        if (isAlt) return;
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90);

        cam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }
    public override void initState()//初始化
    {
        isAttack = false;
        isHurt = false;
        isJump = false;
        isMoving = false;
    }

    public void StopAttack()
    {
        isAttack = false;
    }
}
