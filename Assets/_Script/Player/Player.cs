using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.EventSystems.EventTrigger;

public class Player : MonoBehaviour
{
    public enum SkillType : int
    {
        Skill_Q = 5,
        Skill_W = 5,
        Skill_E = 8,
        Skill_R = 10,
    }

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

    bool isRClicked = false;

    Coroutine eSkiilCoroutine;
    Coroutine eSkiilCoolTimeCoroutine;


    // ��ų ������       -------------------------------------------------------------------------------------------------------------------------------

    float eSkillDamage = 1f;

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

        if (isRClicked)
        {
            SetMovePoint();
            // �̵�
        }
        if (isMove)
        {
            Move();
            isMove = (transform.position - movePoint).magnitude > 0.1f;
        }
        if (isRClicked || isMove)
        {
            anim.SetBool("IsMove", true);
        }
        else
        {
            anim.SetBool("IsMove", false);
        }

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

    /// <summary>
    /// �̵� ��ġ �����ϴ� �Լ�
    /// </summary>
    private void SetMovePoint()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        LayerMask groundLayerMask = LayerMask.GetMask("Ground");

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayerMask))
        {
            movePoint = new Vector3(hit.point.x, transform.position.y, hit.point.z);
            Debug.Log("Hit object: " + hit.collider.gameObject.name);
        }
        isMove = true;
    }



    // ��ų ����    -----------------------------------------------------------------------------------------------------------------------------------
    private void ESklii(bool isESkillOn)
    {
        if (isESkillOn && eSkiilCoolTimeCoroutine == null)
        {
            if (eSkiilCoroutine != null)
            {
                StopCoroutine(eSkiilCoroutine);
            }
            eSkiilCoroutine = StartCoroutine(ESkillAction());
        }
        else
        {
            if (eSkiilCoroutine != null)
            {
                StopCoroutine(eSkiilCoroutine);
                anim.SetBool("IsEClicked", false);
                eSkiilCoolTimeCoroutine = StartCoroutine(RefreshCoolTime(SkillType.Skill_E));
            }
        }
    }

    IEnumerator ESkillAction()
    {
        anim.SetBool("IsEClicked", true);
        float elepsdTime = 0.0f;
        while (elepsdTime < 5.0f)
        {
            Collider[] enemies = Physics.OverlapSphere(transform.position, 2f, LayerMask.GetMask("Enemy"));
            foreach (Collider enemy in enemies)
            {
                // ������ �������� ������ ����
                if (enemy.GetComponent<EnemyBase>() != null)
                {
                    enemy.GetComponent<EnemyBase>().TakeDamage(eSkillDamage);
                }
            }
            Mp -= 10;
            elepsdTime += Time.deltaTime;
            yield return new WaitForSeconds(1f);
        }
        anim.SetBool("IsEClicked", false);
        eSkiilCoolTimeCoroutine = StartCoroutine(RefreshCoolTime(SkillType.Skill_E));
    }

    IEnumerator RefreshCoolTime(SkillType type)
    {
        float elepsedTime = 0.0f;
        while (elepsedTime < (int)type)
        {
            elepsedTime += Time.deltaTime;
            yield return null;
            Debug.Log("��ٿ���!" + elepsedTime);
        }
        //���̽������� Ÿ�� ���ؼ� bool������ ó���ϰų� ������Ʈ���� ��Ÿ�� ó��
    }

    // ��ǲ �׼�        -------------------------------------------------------------------------------------------------------------------------------
    private void ActionLClick()
    {
    }

    private void ActionRClick(bool isClicked)
    {
        isRClicked = isClicked;
    }


    private void ActionSpace()
    {
    }

    /// <summary>
    /// 
    /// </summary>
    private void ActionSkill_Q()
    {
    }

    /// <summary>
    /// 
    /// </summary>
    private void ActionSkill_W()
    {
    }

    /// <summary>
    /// ���� e���� ���ۺ��� ���� �Լ�
    /// </summary>
    private void ActionSkill_E(bool isClicked)
    {
        ESklii(isClicked);
    }

    /// <summary>
    /// 
    /// </summary>
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
