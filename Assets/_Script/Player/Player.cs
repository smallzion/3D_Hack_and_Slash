using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    //  ����          ---------------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// ü��
    /// </summary>
    public float hp = 100.0f;

    /// <summary>
    /// �ִ� ü��
    /// </summary>
    float maxHp;

    /// <summary>
    /// ���� ü��
    /// </summary>
    public float Hp
    {
        get => hp;
        private set
        {
            if (hp != value)
            {
                hp = Mathf.Clamp(value, 0, maxHp);
            }
        }
    }

    /// <summary>
    /// ����
    /// </summary>
    public float mp = 100.0f;

    /// <summary>
    /// �ִ� ����
    /// </summary>
    float maxMp;

    /// <summary>
    /// ���� ����
    /// </summary>
    public float Mp
    {
        get => mp;
        set
        {
            if (mp != value)
            {
                mp = Mathf.Clamp(value, 0, maxMp);
            }
        }
    }

    /// <summary>
    /// �ӵ�
    /// </summary>
    public float speed = 5.0f;

    /// <summary>
    /// ���� �ӵ�
    /// </summary>
    float currentSpeed;

    /// <summary>
    /// �̵�����
    /// </summary>
    Vector3 movePoint;

    Vector3 destPos;

    bool isMove;

    //  ������Ʈ        --------------------------------------------------------------------------------------------------------------------------------

    /// <summary>
    /// �÷��̾� ��ǲ�׼�
    /// </summary>
    PlayerInput input;

    /// <summary>
    /// ĳ���� ��Ʈ�ѷ�
    /// </summary>
    CharacterController characterController;

    /// <summary>
    /// �ִϸ�����
    /// </summary>
    Animator anim;

    /// <summary>
    /// ����ī�޶�
    /// </summary>
    Camera mainCamera;
    private void Awake()
    {
        // �ʱⰪ ����
        maxHp = hp;
        maxMp = mp;
        currentSpeed = speed;
        movePoint = transform.position;
        // ������Ʈ ã��
        input = GetComponent<PlayerInput>();
        characterController = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();


        // ��ǲ �׼� ��������Ʈ �����ϱ�
        input.onLClick += ActionLClick;
        input.onRClick += ActionRClick;
        input.onSpace += ActionSpace;
        input.onSkill_Q += ActionSkill_Q;
        input.onSkill_W += ActionSkill_W;
        input.onSkill_E += ActionSkill_E;
        input.onSkill_R += ActionSkill_R;
        input.onSkill_A += ActionSkill_A;
        input.onSkill_S += ActionSkill_S;
        input.onSkill_D += ActionSkill_D;
        input.onSkill_F += ActionSkill_F;
    }

    private void Start()
    {
        mainCamera = Camera.main;
    }


    private void Update()
    {
        isMove = (transform.position - movePoint).magnitude > 0.1f;
        if (isMove)
        {
            // �̵�
            Move();
        }
        anim.SetBool("IsMove", isMove);

    }

    /// <summary>
    /// �̵����� �Լ�
    /// </summary>
    private void Move()
    {
        Vector3 thisUpdatePoint = (movePoint - transform.position).normalized * speed;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(thisUpdatePoint), 0.25f);
        characterController.SimpleMove(thisUpdatePoint);
    }

    // ��ǲ �׼�        -------------------------------------------------------------------------------------------------------------------------------
    private void ActionLClick()
    {
    }

    private void ActionRClick()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            movePoint = new Vector3 (hit.point.x, transform.position.y, hit.point.z);
            Debug.Log("movePoint : " + movePoint.ToString());
        }

    }


    private void ActionSpace()
    {
    }

    private void ActionSkill_Q()
    {
    }

    private void ActionSkill_W()
    {
    }

    private void ActionSkill_E()
    {
    }

    private void ActionSkill_R()
    {
    }

    private void ActionSkill_A()
    {
    }

    private void ActionSkill_S()
    {
    }

    private void ActionSkill_D()
    {
    }

    private void ActionSkill_F()
    {
    }


}
