using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : UnitBase
{
    [SerializeField] Transform weapomPos;   //무기위치(오른손)
    [SerializeField] GameObject goGun;
    launcherController launcher;

    Animator anim;

    [SerializeField] float rotateSpeed;
    float moveSpeed;
    [SerializeField] float runSpeed;    //앞뒤로 이동시 이동속도 차이
    [SerializeField] float walkSpeed;

    public int finishdamage = 10;
    int finalDefense = 1;

    public bool isSniping = false;      //조준상태일때만 총알 발사
    public bool isFire = false;
    public static bool isDead = false;
    //애니메이션 이벤트 호출로 발사 종료시 다시 원상태


    public enum playerState //플레이어 상태
    {
        IDLE=0,
        WALK=1,
        RUN,
        SNIPE,
        FIRE,
        DEAD,
    }

    playerState currentState;

    // Start is called before the first frame update
    void Start()
    {
        initBase("player", eTribleType.ROBOT, 10, 3, 100);

        anim = GetComponent<Animator>();

        //무기 장착
        GameObject go = Instantiate(goGun,weapomPos);
        go.transform.Rotate(90, 0, 0);

        launcher = go.GetComponent<launcherController>();
        launcher.InitData(this);

        currentState = playerState.IDLE;

    }

    // Update is called once per frame
    void Update()
    {
        float dirZ = Input.GetAxis("Vertical");     //이동
        float dirX = Input.GetAxis("Horizontal");   //회전

        Vector3 mV = new Vector3(0, 0, dirZ);
        

        if (Input.GetMouseButton(0) && !isSniping)
            currentState = playerState.SNIPE;

        switch (currentState)
        {
            case playerState.IDLE:
                ChangeAnim(playerState.IDLE);
                moveSpeed = 0;
                if(dirZ>0)                                   
                    currentState = playerState.RUN;               
                else if(dirZ<0)                                  
                    currentState = playerState.WALK;               
                break;
            case playerState.WALK:
                moveSpeed = walkSpeed;
                isSniping = false;
                ChangeAnim(playerState.WALK);
                if (dirZ == 0)
                    currentState = playerState.IDLE;
                else if (dirZ > 0)
                    currentState = playerState.RUN;
                break;
            case playerState.RUN:
                moveSpeed = runSpeed;
                isSniping = false;
                ChangeAnim(playerState.RUN);
                if (dirZ == 0)
                    currentState = playerState.IDLE;
                else if (dirZ < 0)
                    currentState = playerState.WALK;
                break;
            case playerState.SNIPE:
                ChangeAnim(playerState.SNIPE);
                if (!isFire)
                {
                    if (dirZ > 0)
                        currentState = playerState.RUN;
                    else if (dirZ < 0)
                        currentState = playerState.WALK;

                    if (Input.GetMouseButtonDown(0)&&isSniping)
                    {
                        isFire = true;
                        anim.SetTrigger("Fire"); //발사
                        launcher.bulletCreate();  //총알 생성                   
                    }
                }

                break;
            case playerState.DEAD:
                return;              
        }

        if (!isSniping)
        {
            mV = (mV.magnitude > 1f) ? mV.normalized : mV;      //그래서 작은값은 정규화시키지않음,magnitude는 벡터길이 반환
            transform.Translate(mV * moveSpeed * Time.deltaTime);
            
        }
        if(!isFire)
            transform.Rotate(Vector3.up * dirX * rotateSpeed * Time.deltaTime);

    }


    void ChangeAnim(playerState state)
    {
        anim.SetInteger("StateAnim", (int)state);
    }


    void startSnipe()
    {
        isSniping = true;
    }
    public void startShot()
    {
        //Debug.Log("shot");
    }
    void endShot()
    {
        isFire = false;
    }



    void HitMe(int _damage)
    {
        int finishDamage = _damage - finalDefense;
        if (finishDamage <= 0)
            finishDamage = 1;


        if (calcHit(finishDamage))
        {
            ChangeAnim(playerState.DEAD);
        }
        Debug.Log("enemy attack : "+currentHP);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            
            monsterController mCtrl = other.GetComponentInParent<monsterController>();
            HitMe(mCtrl.finalDamage);
        }
    }
}
