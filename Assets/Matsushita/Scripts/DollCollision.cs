using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DollCollision : MonoBehaviour
{
    public string HitName; //�ڐG�����^�̕����̖��O��ێ�
    public string HitDollName; //�ڐG�����h�[���̕����̖��O��ێ�

    //���ׂĂ̔��肪���������ǂ���
    public bool checkedHit() 
    {
        if(CheckedHead() && CheckedRarm() && CheckedLarm())
        {
            Debug.Log("�S�����肳�ꂽ");
            return true;
        }
        else
        {
            return false;
        }
    }

    //����ɓ������ӏ��̖��O�Ɠ����Ă����h�[���̖��O��Ԃ��֐�
    public void checkHitName(string HDollName, string HName)
    {
        HitDollName = HDollName;
        HitName = HName;
    }

    //���肩��O�ꂽ�ӏ��̖��O�ƊO�ꂽ�h�[���̖��O���폜����֐�
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
