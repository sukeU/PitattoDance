using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionReplay1 : MonoBehaviour
{
    //���̃R�[�h�͓������Č��������I�u�W�F�N�g�ɂ����t���Ă�������
    private bool isInReplayMode;//���v���C���[�h���ۂ��̃t���O�ł�
    [SerializeField]
    private bool flag = true;//���v���C���[�h�ɂ���ׂ̔����1�񂾂��s�����߂����̃t���O�ł�
    //public bool Flag { set { flag = value; } get { return flag; } }
    private float ReplaySpeed = 3.0f;//���v���C�̑����̕ϐ��ł��B1�Ńv���C���Ɠ��������ōĐ����܂��B
    private float currentReplayIndex;
    private Rigidbody rb;
    private List<ReplayRecorder> replayRecorders = new List<ReplayRecorder>();//�������i�[���Ă��郊�X�g�ł�
    [SerializeField]private GamePlayManager Manager;
    private bool oneTime=false;

    // Start is called before the first frame update
    void Start()
    {
        Manager=GameObject.FindGameObjectWithTag("GamePlayManager").GetComponent<GamePlayManager>();
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Manager.isReplay)
        {

            if (!oneTime) 
            {
                oneTime = true;
                isInReplayMode = true;


                if (isInReplayMode)
                {
                    SetTransform(0);
                    rb.isKinematic = true;//�������Z�̉e����off��
                    rb.constraints= RigidbodyConstraints.FreezePosition;
                    if (gameObject.GetComponent<Magnet>() != null)
                    {
                        gameObject.GetComponent<Magnet>().enabled = false;//�����ő����off�ɂ��Ă��܂�<>�̒��ɑ���Ɋ֌W����X�N���v�g�����Ă�������
                    }
                }
            }
        }

    }

    private void FixedUpdate()//���v���C�Ɋւ��鏔�X�̏����@
    {
        if (isInReplayMode == false)//���v���C���[�h�łȂ��Ƃ��͋L�^
        {
            replayRecorders.Add(new ReplayRecorder { position = transform.position, rotation = transform.rotation });
        }
        else
        {
            float nextIndex = currentReplayIndex + ReplaySpeed;//���v���C���[�h�̎��͍Đ�
            if (nextIndex < replayRecorders.Count && nextIndex >= 0)
            {
                SetTransform(nextIndex);
            }
        }
    }
    private void SetTransform(float index)
    {
        currentReplayIndex = index;
        ReplayRecorder replayRecorder = replayRecorders[(int)index];

        transform.position = replayRecorder.position;
        transform.rotation = replayRecorder.rotation;
    }

}
