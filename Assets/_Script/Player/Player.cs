using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UIElements;
using static UnityEngine.EventSystems.EventTrigger;

public class Player : MonoBehaviour, IBattler
{
    public enum SkillType : int
    {
        NormalAttack = 1,
        Skill_Q = 3,
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

    bool canMove = true;

    bool isMove;

    bool isRClicked = false;

    Coroutine eSkiilCoroutine;


    public GameObject RSkillObject;

    // ��ų ��Ÿ��
    bool normalAttack_IsCoolDown = false;
    bool skill_Q_IsCoolDown = false;
    bool skill_W_IsCoolDown = false;
    bool skill_E_IsCoolDown = false;
    bool skill_R_IsCoolDown = false;
    bool used_Skill = false;
    string[] normalAttacks = { "NormalAttack" , "NormalAtack_W"};
    string normalAttack;
    float skill_W_Duration = 10f;
    // ��ų ������       -------------------------------------------------------------------------------------------------------------------------------

    public float normalAttackDamage = 5f;
    public float qSkillDamage = 20f;
    public float wSkillDamage = 2f;
    public float eSkillDamage = 7f;

    // ��ų ��� ���� ����
    public bool rSkillActive = false;

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





    // ��������Ʈ -----------------------------------------------------------------------------------------------------
    public Action IceSkillLvUp;
    public Action IceSkillLvDown;
    private void Awake()
    {
        // �ʱⰪ ����
        maxHp = hp;
        maxMp = mp;
        currentSpeed = speed;
        movePoint = transform.position;
        normalAttack = normalAttacks[0];
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

        if (isRClicked && canMove)
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
        Vector3 thisUpdatePoint = (movePoint - transform.position).normalized * currentSpeed;
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
            //Debug.Log("Hit object: " + hit.collider.gameObject.name);
        }
        isMove = true;
    }

    private void SetRoation()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        LayerMask groundLayerMask = LayerMask.GetMask("Ground");
        Vector3 lookPoint = Vector3.zero;
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayerMask))
        {
            lookPoint = new Vector3(hit.point.x, transform.position.y, hit.point.z);
            //Debug.Log("Hit object: " + hit.collider.gameObject.name);
        }
        Vector3 thisUpdatePoint = (lookPoint - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(thisUpdatePoint);
    }


    // ��ų ����    -----------------------------------------------------------------------------------------------------------------------------------

    private void NormalAttack()
    {
        normalAttack_IsCoolDown = true;
        used_Skill = true;
        canMove = false;
        currentSpeed = 0.0f;
        isMove = false;
        StartCoroutine(NormalAttackAction());
    }

    private IEnumerator NormalAttackAction()
    {
        anim.SetTrigger(normalAttack);
        Collider[] enemies = Physics.OverlapSphere(transform.position + transform.forward + transform.up * 0.5f, 1f, LayerMask.GetMask("Enemy"));
        foreach (Collider enemy in enemies)
        {
            // ������ �������� ������ ����
            if (enemy.GetComponent<EnemyBase>() != null)
            {
                enemy.GetComponent<EnemyBase>().Defense(normalAttackDamage);
            }
        }
        yield return new WaitForSeconds(0.01f);

        float curAnimationTime = anim.GetCurrentAnimatorStateInfo(0).length;

        yield return new WaitForSeconds(curAnimationTime);
        canMove = true;
        currentSpeed = speed;
        isMove = true;
        used_Skill = false;
        yield return StartCoroutine(RefreshCoolTime(SkillType.NormalAttack));
    }

    private void QSkill()
    {
        skill_Q_IsCoolDown = true;
        used_Skill = true;
        canMove = false;
        currentSpeed = 0.0f;
        isMove = false;
        StartCoroutine(QSkillAction());
    }

    private IEnumerator QSkillAction()
    {
        anim.SetTrigger("QSkill");

        Collider[] enemies = Physics.OverlapSphere(transform.position + transform.forward + transform.up * 0.5f, 1f, LayerMask.GetMask("Enemy"));
        foreach (Collider enemy in enemies)
        {
            // ������ �������� ������ ����
            if (enemy.GetComponent<EnemyBase>() != null)
            {
                enemy.GetComponent<EnemyBase>().Defense(qSkillDamage);
            }
        }
        yield return new WaitForSeconds(0.01f);

        float curAnimationTime = anim.GetCurrentAnimatorStateInfo(0).length;

        yield return new WaitForSeconds(curAnimationTime);
        canMove = true;
        currentSpeed = speed;
        isMove = true;
        used_Skill = false;
        yield return StartCoroutine(RefreshCoolTime(SkillType.Skill_Q));
    }

    private void WSkill()
    {
        skill_W_IsCoolDown = true;
        StartCoroutine(WSkillAction());
    }

    private IEnumerator WSkillAction()
    {
        float originDamage = normalAttackDamage;
        skill_W_IsCoolDown = true;
        normalAttack = normalAttacks[1];
        normalAttackDamage *= wSkillDamage;
        yield return new WaitForSeconds(skill_W_Duration);
        normalAttackDamage = originDamage;
        normalAttack = normalAttacks[0];
        yield return StartCoroutine(RefreshCoolTime(SkillType.Skill_W));
    }

    private void ESklii()
    {
        skill_E_IsCoolDown = true;
        used_Skill = true;
        if (eSkiilCoroutine != null)
        {
            StopCoroutine(eSkiilCoroutine);
        }
        eSkiilCoroutine = StartCoroutine(ESkillAction());
    }

    IEnumerator ESkillAction()
    {
        anim.SetBool("IsEClicked", true);
        float elepsdTime = 0.0f;
        while (elepsdTime < 5.0f)
        {
            Collider[] enemies = Physics.OverlapSphere(transform.position + transform.up * 0.5f, 2f, LayerMask.GetMask("Enemy"));
            foreach (Collider enemy in enemies)
            {
                // ������ �������� ������ ����
                if (enemy.GetComponent<EnemyBase>() != null)
                {
                    enemy.GetComponent<EnemyBase>().Defense(eSkillDamage);
                }
            }
            Mp -= 10;
            elepsdTime += 1f;
            yield return new WaitForSeconds(1f);
        }
        anim.SetBool("IsEClicked", false);
        used_Skill = false;
        yield return StartCoroutine(RefreshCoolTime(SkillType.Skill_E));
    }

    private void RSkill()
    {
        skill_R_IsCoolDown = true;
        canMove = false;
        currentSpeed = 0.0f;
        used_Skill = true;
        isMove = false;
        StartCoroutine(RSkillAction());
    }

    private IEnumerator RSkillAction()
    {
        anim.SetTrigger("RSkill");
        GameObject skillObject = Instantiate(RSkillObject, transform.position + transform.forward * 3 + transform.up * 10, Quaternion.Euler(180, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z));
        Destroy(skillObject, 5f);
        
        yield return new WaitForSeconds(0.01f);

        float curAnimationTime = anim.GetCurrentAnimatorStateInfo(0).length;

        yield return new WaitForSeconds(curAnimationTime);
        canMove = true;
        currentSpeed = speed;
        isMove = true;
        used_Skill = false;
        yield return StartCoroutine(RefreshCoolTime(SkillType.Skill_R));
    }
    IEnumerator RefreshCoolTime(SkillType type)
    {
        float elepsedTime = 0.0f;
        
        while (elepsedTime < (float)type)
        {
            elepsedTime += Time.deltaTime;
            yield return null;
        }

        switch (type)
        {
            case SkillType.NormalAttack:
                normalAttack_IsCoolDown = false;
                break;
            case SkillType.Skill_Q:
                skill_Q_IsCoolDown = false;
                break;
            case SkillType.Skill_W:
                skill_W_IsCoolDown = false;
                break;
            case SkillType.Skill_E:
                skill_E_IsCoolDown = false;
                break;
            case SkillType.Skill_R:
                skill_R_IsCoolDown = false;
                break;
        }
        //���̽������� Ÿ�� ���ؼ� bool������ ó���ϰų� ������Ʈ���� ��Ÿ�� ó��
    }

    // ��ǲ �׼�        -------------------------------------------------------------------------------------------------------------------------------
    private void ActionLClick()
    {
        if(!normalAttack_IsCoolDown && !used_Skill)
        {
            SetRoation();
            NormalAttack();
        }
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
        if(!skill_Q_IsCoolDown && !used_Skill)
        {
            SetRoation();
            QSkill();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private void ActionSkill_W()
    {
        if(!skill_W_IsCoolDown && !used_Skill)
        {
            WSkill();
        }
    }

    /// <summary>
    /// ���� e���� ���ۺ��� ���� �Լ�
    /// </summary>
    private void ActionSkill_E()
    {
        if(!skill_E_IsCoolDown && !used_Skill)
        {
            ESklii();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private void ActionSkill_R()
    {
        if(!skill_R_IsCoolDown && !used_Skill && rSkillActive)
        {
            SetRoation();
            RSkill();
        }
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
    public void Attack(IBattler target)
    {

    }

    public void Defense(float attackPower)
    {
        Hp -= attackPower;
        Debug.Log("����HP" + Hp);
    }
#if UNITY_EDITOR
    public void TestCoolTimeRefresh()
    {
        skill_Q_IsCoolDown = false;
        skill_W_IsCoolDown = false;
        skill_E_IsCoolDown = false;
        skill_R_IsCoolDown = false;
    }

    public void TestSkillLvUp()
    {
        IceSkillLvUp.Invoke();
    }

    public void TestSkillLvDown()
    {
        IceSkillLvDown.Invoke();

    }
#endif
}
