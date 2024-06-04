using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            if(hp != value)
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
            if(mp != value)
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
    Vector3 inputDir = Vector3.zero;

    /// <summary>
    /// �ٴ� üũ�� ���̾� ����ũ
    /// </summary>
    public LayerMask groundLayer;

    /// <summary>
    /// �ٴ� üũ�� Ʈ������
    /// </summary>
    Transform groundCheckPosition;


    /// <summary>
    /// �߷°�
    /// </summary>
    public float gravityForce = 9.81f;

    /// <summary>
    /// �߷������� ����
    /// </summary>
    Vector3 gravityDir;
    
    /// <summary>
    /// ���� �߷°�
    /// </summary>
    float gravityY;

    //  ������Ʈ        --------------------------------------------------------------------------------------------------------------------------------

    /// <summary>
    /// �÷��̾� ��ǲ�׼�
    /// </summary>
    PlayerInput input;

    /// <summary>
    /// ĳ���� ��Ʈ�ѷ�
    /// </summary>
    CharacterController characterController;


    private void Awake()
    {
        // �ʱⰪ ����
        maxHp = hp;
        maxMp = mp;
        currentSpeed = speed;
        groundCheckPosition = transform.GetChild(0);

        // ������Ʈ ã��
        input = GetComponent<PlayerInput>();
        characterController = GetComponent<CharacterController>();

        // ��ǲ �׼� ��������Ʈ �����ϱ�
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
    /// �̵����� �Լ�
    /// </summary>
    private void Move()
    {
        Vector3 moveDirection = Vector3.forward * inputDir.z + transform.right * inputDir.x;
        characterController.Move(moveDirection * currentSpeed * Time.deltaTime);
    }


    /// <summary>
    /// ���� ���� �� ������ Ȯ���ϴ� �Լ�
    /// </summary>
    /// <returns></returns>
    bool IsGrounded()
    {
        // ĳ������ �Ʒ��� ����ĳ��Ʈ�� ���� �ٴڿ� ��Ҵ��� Ȯ��
        return Physics.Raycast(groundCheckPosition.position, Vector3.down, 0.1f, layerMask: groundLayer);
    }

    /// <summary>
    /// �����϶� �߷� �����ϴ� �Լ�
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




    // ��ǲ �׼�        -------------------------------------------------------------------------------------------------------------------------------
    private void MoveInput(Vector2 input, bool isPress)
    {
        // �Է� ���� ����
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
