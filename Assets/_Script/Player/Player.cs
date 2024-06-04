using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            if(hp != value)
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
            if(mp != value)
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
    /// 이동방향
    /// </summary>
    Vector3 inputDir = Vector3.zero;

    /// <summary>
    /// 바닥 체크용 레이어 마스크
    /// </summary>
    public LayerMask groundLayer;

    /// <summary>
    /// 바닥 체크용 트랜스폼
    /// </summary>
    Transform groundCheckPosition;


    /// <summary>
    /// 중력값
    /// </summary>
    public float gravityForce = 9.81f;

    /// <summary>
    /// 중력적용할 백터
    /// </summary>
    Vector3 gravityDir;
    
    /// <summary>
    /// 현재 중력값
    /// </summary>
    float gravityY;

    //  컴포넌트        --------------------------------------------------------------------------------------------------------------------------------

    /// <summary>
    /// 플레이어 인풋액션
    /// </summary>
    PlayerInput input;

    /// <summary>
    /// 캐릭터 컨트롤러
    /// </summary>
    CharacterController characterController;


    private void Awake()
    {
        // 초기값 설정
        maxHp = hp;
        maxMp = mp;
        currentSpeed = speed;
        groundCheckPosition = transform.GetChild(0);

        // 컴포넌트 찾기
        input = GetComponent<PlayerInput>();
        characterController = GetComponent<CharacterController>();

        // 인풋 액션 델리게이트 연결하기
        input.onMove += MoveInput;
        input.onJump += JumpInput;
        input.onDash += DashInput;
        input.onAttack += AttackInput;

    }

    private void Update()
    {

        ApplyGravity();
        Move();
        transform.Rotate(0f, Input.GetAxis("Mouse X"), 0f, Space.World);
    }






    /// <summary>
    /// 이동관련 함수
    /// </summary>
    private void Move()
    {
        Vector3 moveDirection = Vector3.forward * inputDir.z + transform.right * inputDir.x;
        characterController.Move(moveDirection * currentSpeed * Time.deltaTime);
    }


    /// <summary>
    /// 현재 지금 땅 위인지 확인하는 함수
    /// </summary>
    /// <returns></returns>
    bool IsGrounded()
    {
        // 캐릭터의 아래에 레이캐스트를 쏴서 바닥에 닿았는지 확인
        return Physics.Raycast(groundCheckPosition.position, Vector3.down, 0.1f, layerMask: groundLayer);
    }

    /// <summary>
    /// 공중일때 중력 적용하는 함수
    /// </summary>
    void ApplyGravity()
    {
        if (!IsGrounded())
        {
            gravityY += gravityForce * Time.deltaTime;
            gravityDir.y = -gravityY;
        }
        else
        {
            gravityY = 0;
        }
        characterController.Move(Time.deltaTime * gravityDir);
    }




    // 인풋 액션        -------------------------------------------------------------------------------------------------------------------------------
    private void MoveInput(Vector2 input, bool isPress)
    {
        // 입력 방향 저장
        inputDir.x = input.x;
        inputDir.y = 0;
        inputDir.z = input.y;
    }

    private void JumpInput()
    {
        if (IsGrounded())
        {
            gravityDir.y = 100f;
        }
    }

    private void DashInput()
    {
    }

    private void AttackInput()
    {
    }

}
