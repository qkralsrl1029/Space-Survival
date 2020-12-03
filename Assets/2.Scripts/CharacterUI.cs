using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterUI : MonoBehaviour
{
    [SerializeField] Text txtName;
    [SerializeField] Slider hpBar;
    [SerializeField] Text txtAtk;
    [SerializeField] Text txtDef;
    [SerializeField] RawImage icon;

    public void initData(string name, float hpRate,int dmg=0, int def=0)
    {
        enableWindow(true);
        txtName.text = name;
        setHpRate(hpRate);
        //txtAtk.text = dmg.ToString();
        //txtDef.text = def.ToString();
    }

    public void setHpRate(float rate)
    {
        hpBar.value = rate;
    }

    public void enableWindow(bool isView)
    {
        gameObject.SetActive(isView);
    }
}
