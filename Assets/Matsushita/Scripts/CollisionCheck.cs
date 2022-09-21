using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionCheck : MonoBehaviour
{
    DollCollision dollCollision;
    // Start is called before the first frame update
    void Start()
    {
        dollCollision = GameObject.FindGameObjectWithTag("Pose").GetComponent<DollCollision>();
    }

    //�G��Ă���Ƃ��A�Ă΂��
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("�G�ꂽ");
        var HDollName = other.gameObject.name; //�G�ꂽ�h�[���̉ӏ��̖��O���擾
        var HName = this.gameObject.name; //�G���ꂽ�^�̉ӏ��̖��O���擾
        dollCollision.checkHitName(HDollName, HName);
    }

    //���ꂽ�Ƃ��ɌĂ΂��
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("���ꂽ");
        dollCollision.checkoutHitName();
    }
}