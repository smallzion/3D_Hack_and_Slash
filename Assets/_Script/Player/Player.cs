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

    bool canMove = true;

    bool isMove;

    bool isRClicked = false;

    Coroutine eSkiilCoroutine;


    public GameObject RSkillObject;

    // 스킬 쿨타임
    bool normalAttack_IsCoolDown = false;
    bool skill_Q_IsCoolDown = false;
    bool skill_W_IsCoolDown = false;
    bool skill_E_IsCoolDown = false;
    bool skill_R_IsCoolDown = false;
    bool used_Skill = false;
    string[] normalAttacks = { "NormalAttack" , "NormalAtack_W"};
    string normalAttack;
    float skill_W_Duration = 10f;
    // 스킬 데미지       -------------------------------------------------------------------------------------------------------------------------------

    public float normalAttackDamage = 5f;
    public float qSkillDamage = 20f;
    public float wSkillDamage = 2f;
    public float eSkillDamage = 7f;

    // 스킬 사용 가능 여부
    public bool rSkillActive = false;

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





    // 델리게이트 -----------------------------------------------------------------------------------------------------
    public Action IceSkillLvUp;
    public Action IceSkillLvDown;
    private void Awake()
    {
        // 초기값 설정
        maxHp = hp;
        maxMp = mp;
        currentSpeed = speed;
        movePoint = transform.position;
        normalAttack = normalAttacks[0];
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

        if (isRClicked && canMove)
        {
            SetMovePoint();
            // 이동
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
    /// 이동관련 함수
    /// </summary>
    private void Move()
    {
        Vector3 thisUpdatePoint = (movePoint - transform.position).normalized * currentSpeed;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(thisUpdatePoint), 0.25f);
        characterController.SimpleMove(thisUpdatePoint);
    }

    /// <summary>
    /// 이동 위치 저장하는 함수
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


    // 스킬 관련    -----------------------------------------------------------------------------------------------------------------------------------

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
            // 적에게 데미지를 입히는 로직
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
            // 적에게 데미지를 입히는 로직
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
                // 적에게 데미지를 입히는 로직
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
        //케이스문으로 타입 비교해서 bool변수로 처리하거나 업데이트에서 쿨타임 처리
    }

    // 인풋 액션        -------------------------------------------------------------------------------------------------------------------------------
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
    /// 가렌 e마냥 빙글빙글 도는 함수
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
        Debug.Log("남은HP" + Hp);
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
