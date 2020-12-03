using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    [SerializeField] Transform RootPoint;
    [SerializeField] float spawnTime = 5;       //리스폰 시간
    [SerializeField] int limitSpawnCount = 10;  //최대생성 개수
    [SerializeField] int maxCount = 4;          //동시에 최대 생성 수

    List<monsterController> monsters = new List<monsterController>();   //생성되는 몬스터들 저장할 리스트
    float timePass = 0;     //리스폰 카운트 
    int currentSpawnCount = 0;


    
    // Update is called once per frame
    void Update()
    {
        //최대생성 개수를 제한으로 해서 그 이상을 넘지 않게
        if(currentSpawnCount<limitSpawnCount)
        {
            //현재 존재하는 몬스터가 최대 수용 한을 넘지 않으면
            if(monsters.Count<maxCount)
            {
                //리스폰 시간 체크후  한마리 생성
                timePass += Time.deltaTime;
                if(timePass>spawnTime)
                {
                    timePass = 0;
                    currentSpawnCount++;
                    //몬스터 생성
                    //파일 경로 따라서 게임오브젝트 접근
                    //시리얼라이즈 필드보다 메모리 관리 효율적, 디렉토리내에 참조하는 방식
                    GameObject prefab = Resources.Load("prefab/Unit/reptile") as GameObject;
                    GameObject go = Instantiate(prefab, transform.position, transform.rotation);
                    monsterController mCtrl = go.GetComponent<monsterController>();
                    //초기화
                    mCtrl.initBase("monster", UnitBase.eTribleType.ALIEN, 6, 2, 55);
                    //루트 설정
                    mCtrl.root = RootPoint;
                    //리스트에 추가
                    monsters.Add(mCtrl);
                }
            }
        }
    }

    private void LateUpdate()
    {
        //리스트 내 없어진 몬스터들은 리스트에서 삭제
        for (int i = 0; i < monsters.Count; i++)
        {
            if (monsters[i] == null)
            {
                monsters.RemoveAt(i);
                break;
            }

        }
    }
}
