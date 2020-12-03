using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class UnitBase : MonoBehaviour
{
    public enum eTribleType
    {
        HUMAN=0,
        UNDEAD=1,
        ALIEN,
        ROBOT,
    }

    #region [Stat]
    string name;
    int powAtt;
    int powDef;
    int currentLife;
    int maxLife;
    eTribleType trib;

    int powAgi;
    int powDex;
    #endregion

    #region [Peoperty]
    //초기화 함수 
    /// <summary>
    /// 
    /// </summary>
    /// <param name="_name">이름</param>
    /// <param name="_att">공격력</param>
    /// <param name="_def">방어력</param>
    /// <param name="_life">생명력</param>
    /// <param name="_agi">명중률</param>
    /// <param name="_dex">회피율</param>
    public virtual void initBase(string _name,eTribleType type,int _att, int _def, int _life,int _agi=0,int _dex=0)
    {
        name = _name;
        trib = type;
        powAtt = _att;
        powDef = _def;
        currentLife = maxLife = _life;

        powAgi = _agi;
        powDex = _dex;
    }

    //상태값 프로퍼티 이름/체력/체력 비율
    public string myname { get { return name; } }   

    public int maxHP { get{ return maxLife; } }

    public int currentHP { get { return currentLife; } }

    public float hpRate { get { return (float)currentLife / maxLife; } }
    /// <summary>
    /// 기본공격력
    /// </summary>
    protected int baseAtt { get { return powAtt; } }
    /// <summary>
    /// 기본 방어력
    /// </summary>
    protected int baseDef { get { return powDef; } }
    #endregion

    /// <summary>
    /// 최종데미지 계산해서 캐릭터 사망 여부 판단
    /// </summary>
    /// <param name="damage"></param>
    /// <returns></returns>
    protected bool calcHit(int damage=1)
    {
        currentLife -= damage;

        if (currentLife <= 0)
        {
            currentLife = 0;
            return true;
        }
        else
            return false;
            
    }

   
}
