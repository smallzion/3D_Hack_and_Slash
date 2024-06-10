using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    //  변수          ---------------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// 체력
    /// </summary>
    public float hp = 100.0f;

    /// <summary>
    /// 최대 체력
    /// </summary>
    float maxHp;

    /// <summary>
    /// 현재 체력
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
    /// 마나
    /// </summary>
    public float mp = 100.0f;

    /// <summary>
    /// 최대 마나
    /// </summary>
    float maxMp;

    /// <summary>
    /// 현재 마나
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
    /// 속도
    /// </summary>
    public float speed = 5.0f;

    /// <summary>
    /// 현재 속도
    /// </summary>
    float currentSpeed;

    /// <summary>
    /// 이동지점
    /// </summary>
    Vector3 movePoint;

    Vector3 destPos;

    bool isMove;

    //  컴포넌트        --------------------------------------------------------------------------------------------------------------------------------

    /// <summary>
    /// 플레이어 인풋액션
    /// </summary>
    PlayerInput input;

    /// <summary>
    /// 캐릭터 컨트롤러
    /// </summary>
    CharacterController characterController;

    /// <summary>
    /// 애니메이터
    /// </summary>
    Animator anim;

    /// <summary>
    /// 메인카메라
    /// </summary>
    Camera mainCamera;
    private void Awake()
    {
        // 초기값 설정
        maxHp = hp;
        maxMp = mp;
        currentSpeed = speed;
        movePoint = transform.position;
        // 컴포넌트 찾기
        input = GetComponent<PlayerInput>();
        characterController = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();


        // 인풋 액션 델리게이트 연결하기
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
            // 이동
            Move();
        }
        anim.SetBool("IsMove", isMove);

    }

    /// <summary>
    /// 이동관련 함수
    /// </summary>
    private void Move()
    {
        Vector3 thisUpdatePoint = (movePoint - transform.position).normalized * speed;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(thisUpdatePoint), 0.25f);
        characterController.SimpleMove(thisUpdatePoint);
    }

    // 인풋 액션        -------------------------------------------------------------------------------------------------------------------------------
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
