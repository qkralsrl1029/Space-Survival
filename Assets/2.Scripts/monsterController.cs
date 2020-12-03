using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using UnityEngine;
using UnityEngine.AI;

public class monsterController : UnitBase
{
    Transform player;
    NavMeshAgent monsterNav;
    [SerializeField] Animator anim;
    [SerializeField] MonsterUI ui;
   

    List<Vector3> Points = new List<Vector3>(); //몬스터 이동 목적지 열 개
    public Transform root;
    Vector3 goalPos;
    Vector3 battleStartPos; //전투종료시 돌아올 위치
                            //백런 만들어서 도착 위치로 돌아가고 체력 회복

    [SerializeField] float walkSpeed;
    [SerializeField] float runSpeed;

    bool isDead = false;
    bool isSelect = false;

    float timeCheck = 0;

    [SerializeField] float sightRange = 10f;
    [SerializeField] float attackRange = 2;

    [SerializeField] BoxCollider attackZone;

    #region [AddStat]
    int addAtt;
    int addDef;
    #endregion

    public int finalDamage
    {
        get { return baseAtt + addAtt; }
    }
    public int finalDefense
    {
        get { return baseDef + addDef; }
    }


    public enum MonsterState
    {
        IDLE=0,
        WALK=1,
        RUN,
        ATTACK1,
        ATTACK2,
        DIE,
        BACKRUN,
    }
    MonsterState currentState = MonsterState.IDLE;


    private void Awake()
    {
        goalPos = transform.position;
        monsterNav = GetComponent<NavMeshAgent>();
        player = FindObjectOfType<PlayerController>().transform;
    }
    // Start is called before the first frame update
    void Start()
    {               
        setDestinations();
        changeDestination();
        disableAttackZone();
        GetComponentInChildren<playerDetect>().setMonster(this);
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case MonsterState.WALK:
                ChangeAction(MonsterState.WALK);
                if (Vector3.Distance(transform.position, goalPos) < 0.1f)
                    isSelect = false;
                break;
            case MonsterState.IDLE:
                ChangeAction(MonsterState.IDLE);
                timeCheck += Time.deltaTime;
                if (timeCheck > 3)
                    isSelect = false;
                break;
            case MonsterState.RUN:
                if (Vector3.Distance(transform.position, player.position) < attackRange) 
                    ChangeAction(MonsterState.ATTACK1);
                break;
            case MonsterState.ATTACK1:
            case MonsterState.ATTACK2:
                if (Vector3.Distance(transform.position, player.position) < attackRange)
                {
                    transform.LookAt(player);
                    ChangeAction(MonsterState.ATTACK1);
                }
                else
                {
                    goalPos = player.position;
                    ChangeAction(MonsterState.RUN);
                    monsterNav.destination = goalPos;
                }
                break;
            case MonsterState.DIE:
                return;
                break;
        }
        selectAI();
    }

    public void ChangeAction(MonsterState state)
    {
        if (isDead)
            return;

        switch(state)
        {
            case MonsterState.IDLE:
                monsterNav.speed = 0;
                break;
            case MonsterState.WALK:              
                monsterNav.speed = walkSpeed;
                monsterNav.stoppingDistance = 0;
                ChangeAnim(MonsterState.WALK);
                break;
            case MonsterState.RUN:
                monsterNav.speed = runSpeed;
                disableAttackZone();
                monsterNav.stoppingDistance = attackRange;    //목표 달성 거리 설정
                break;
            case MonsterState.BACKRUN:
                disableAttackZone();
                break;
            case MonsterState.ATTACK1:
                
                break;
            case MonsterState.ATTACK2:
                break;
            case MonsterState.DIE:
                anim.SetTrigger("Dead");
                monsterNav.enabled=false;
                GetComponent<BoxCollider>().enabled = false;
                isDead = true;
                
                break;
        }
        anim.SetInteger("state",(int)state);
        currentState = state;
    }

    void setDestinations()
    {
        for (int i = 0; i < root.childCount; i++)
        {
            Vector3 pos = root.GetChild(i).transform.position;
            Points.Add(pos);
            Debug.Log(Points.Count);
        }
    }

    void changeDestination()
    {
        int tempPos = Random.Range(0, Points.Count);

        currentState = MonsterState.WALK;
        goalPos = Points[tempPos];
        monsterNav.destination = goalPos;
    }

    void ChangeAnim(MonsterState state)
    {
        anim.SetInteger("state", (int)state);
    }

    void selectAI()
    {
        if(!isSelect)
        {
            int rd = Random.Range(0, 4);
            if(rd==2||rd==3)
            {
                timeCheck = 0;
                Debug.Log("대기");
                ChangeAction(MonsterState.IDLE);
                float waitTime = Random.Range(3, 7);
            }
            else
            {
                changeDestination();
            }
            isSelect = true;
        }
    }

    /// <summary>
    /// 유닛 베이스 상속받아서 기본값 할당
    /// </summary>
    /// <param name="_name"></param>
    /// <param name="type"></param>
    /// <param name="_att"></param>
    /// <param name="_def"></param>
    /// <param name="_life"></param>
    /// <param name="_agi"></param>
    /// <param name="_dex"></param>
    public override void initBase(string _name,eTribleType type, int _att, int _def, int _life, int _agi = 0, int _dex = 0)
    {
        base.initBase(_name, type,_att, _def, _life, _agi, _dex);
        switch(type)
        {
            case eTribleType.HUMAN:
                addAtt = 5;
                addDef = 2;
                break;
            case eTribleType.UNDEAD:
                addAtt = 4;
                addDef = 3;
                break;
            case eTribleType.ALIEN:
                addAtt = 6;
                addDef = 2;
                break;
            case eTribleType.ROBOT:
                addAtt = 5;
                addDef = 3;
                break;
        }
        ui.initInfo(currentHP, myname);
    }

    void enableAttackZone()
    {
        attackZone.enabled = true;
    }

    void disableAttackZone()
    {
        attackZone.enabled = false;
    }


    /// <summary>
    /// 피격 연산, 데미지가 음수면 최저 데미지로 조정
    /// </summary>
    /// <param name="_damage"></param>
    void HitMe(int _damage)
    {
        int finishDamage = _damage - finalDefense;
        if (finishDamage <= 0)
            finishDamage = 1;
        

        if(calcHit(finishDamage))
        {
            Destroy(gameObject,3f);
            ChangeAction(MonsterState.DIE);
        }

        ui.setHP(currentHP);
    }

    private void OnTriggerEnter(Collider other)    
    {
        if(other.CompareTag("Laser"))
        {
            laserBullet lb = other.GetComponent<laserBullet>();
            HitMe(lb.finalDamage);

            Debug.Log("attaked");
            Destroy(other.gameObject);
            //총알맞으면 플레이어 추격
            goalPos = player.position;
            monsterNav.destination = goalPos;
            ChangeAction(MonsterState.RUN);
        }      
    }

    
    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        player = other.transform;
    //        goalPos = player.position;

    //        if (Vector3.Distance(transform.position, goalPos) < attackRange)
    //            ChangeAction(MonsterState.ATTACK1);
    //        else
    //        {
    //            monsterNav.destination = goalPos;
    //            ChangeAction(MonsterState.RUN);
    //        }
    //    }
    //}

    public void setGoalPos(Vector3 pos)
    {
        goalPos = pos;
    }

    public void tracePlayer()
    {
        if (Vector3.Distance(transform.position, goalPos) < attackRange)
            ChangeAction(MonsterState.ATTACK1);
        else
        {
            monsterNav.destination = goalPos;
            ChangeAction(MonsterState.RUN);
        }
    }

    public void stopTrace()
    {
        changeDestination();
        ChangeAction(MonsterState.WALK);
    }

    
    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {

    //        changeDestination();
    //        ChangeAction(MonsterState.WALK);
    //    }
    //}
}
