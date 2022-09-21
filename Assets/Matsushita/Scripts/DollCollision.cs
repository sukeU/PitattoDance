using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DollCollision : MonoBehaviour
{
    public string HitName; //接触した型の部分の名前を保持
    public string HitDollName; //接触したドールの部分の名前を保持

    //すべての判定が入ったかどうか
    public bool checkedHit() 
    {
        if(CheckedHead() && CheckedRarm() && CheckedLarm())
        {
            Debug.Log("全部判定された");
            return true;
        }
        else
        {
            return false;
        }
    }

    //判定に入った箇所の名前と入ってきたドールの名前を返す関数
    public void checkHitName(string HDollName, string HName)
    {
        HitDollName = HDollName;
        HitName = HName;
    }

    //判定から外れた箇所の名前と外れたドールの名前を削除する関数
    public void checkoutHitName()
    {
        HitDollName = null;
        HitName = null;
    }

    public bool CheckedHead()
    {
        if(HitDollName == "Head" && HitName == "colHead")
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool CheckedRarm()
    {
        if (HitDollName == "Rarm" && HitName == "colRarm")
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool CheckedLarm()
    {
        if (HitDollName == "Larm" && HitName == "colLarm")
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
