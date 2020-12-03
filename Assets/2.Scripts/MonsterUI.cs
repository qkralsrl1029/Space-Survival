using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using UnityEngine;
using UnityEngine.UI;

public class MonsterUI : MonoBehaviour
{
    [SerializeField] Slider hpBar;
    [SerializeField] Text monsterName;
   
    public void initInfo(float rate,string name)
    {
        setHP(rate);
        setName(name);
    }

    // Update is called once per frame
    void Update()
    {
        //월드스페이스로 만든 몬스터 ui 캔버스를 매 프레임마다 메인 카메라를 바라보게 함
        //transform.LookAt(Camera.main.transform.position);

    }

    public void setHP(float rate)
    {
        hpBar.value = rate;
    }
    public void setName(string name)
    {
        monsterName.text = name;
    }
}
