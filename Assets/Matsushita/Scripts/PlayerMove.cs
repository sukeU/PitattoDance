using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    
    private Vector3 _nowMousePosi; // ���݂̃}�E�X�̃��[���h���W
    [SerializeField]
    private GameObject rocket; //Unity��Ń��P�b�g�ɂ�������̂��h���b�O���h���b�v

    void Update()
    {
        Vector3 nowmouseposi;
        // ���݂̃}�E�X�̃��[���h���W���擾
        nowmouseposi = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        nowmouseposi.z = 10;
        // �J�n���̃I�u�W�F�N�g�̍��W�Ƀ}�E�X�̕ω��ʂ𑫂��ĐV�������W��ݒ�
        transform.position = nowmouseposi;
        //���΂��Ώۂ̂��̂������悤�ɉ�]����
        //this.transform.LookAt(rocket.transform.position);
    }
}