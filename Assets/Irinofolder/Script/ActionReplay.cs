using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;
using Photon.Pun;


public class ActionReplay : MonoBehaviourPunCallbacks
{
    //���̃R�[�h�͓������Č��������I�u�W�F�N�g�ɂ����t���Ă�������
    private bool isInReplayMode;//���v���C���[�h���ۂ��̃t���O�ł�
    [SerializeField]
    private bool flag = false;//���v���C���[�h�ɂ���ׂ̔����1�񂾂��s�����߂����̃t���O�ł�
    //public bool Flag { set { flag = value; } get { return flag; } }
    private float ReplaySpeed = 2.5f;//���v���C�̑����̕ϐ��ł��B1�Ńv���C���Ɠ��������ōĐ����܂��B
    private float currentReplayIndex;
    private Rigidbody rb;
    private List<ReplayRecorder> replayRecorders = new List<ReplayRecorder>();//�������i�[���Ă��郊�X�g�ł�
    private GameNetworkManager Manager;
    private bool oneTime=false;

    // Start is called before the first frame update
    void Start()
    {
        Manager=GameObject.FindGameObjectWithTag("GameNetworkObject").GetComponent<GameNetworkManager>();
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Manager.isReplay)
        {
            if (!oneTime) 
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    try
                    {
                        photonView.RPC(nameof(CallFlag), RpcTarget.All);//NullReferenceException���o�Ă�̂Ō����{����
                    }
                    catch (NullReferenceException)
                    {
                        Debug.Log("���̗�O�����͌ォ��l����");
                    }
                }
                oneTime = true;
            }
        }
        if (flag == false)//1�񂾂��������������̂�flag���g���Ă܂�
         //�e�X�g�����ł�GameRoomHUD�Ƃ����X�N���v�g�Ŏ��Ԃ��Ǘ����Ă��܂���
         //GameRoomHUD���̎��Ԃ��ݒ肳�ꂽ���Ԃ𒴂����Ƃ���GameRoom���ɂ���gameTime��false�ɂȂ�Ƃ����d�g�݂ł�
         //�Q�[���ɑg�ݍ��ނƂ��͏��if���̒��̃R�����g�A�E�g���ꂽ�ӏ��Ɏ��Ԃɂ����������Ă�������
        {
            flag = true;
            isInReplayMode = !isInReplayMode;
            

            if (isInReplayMode)
            {
                SetTransform(0);
                rb.isKinematic = true;//�������Z�̉e����off��
                if (gameObject.GetComponent<Magnet>() != null) { 
                gameObject.GetComponent<Magnet>().enabled = false;//�����ő����off�ɂ��Ă��܂�<>�̒��ɑ���Ɋ֌W����X�N���v�g�����Ă�������
                    }
                //���̃e�X�g���ł�Control������Ɋ֌W����X�N���v�g�ł���
                //�{���̓C���X�y�N�^�[�ŎQ�Ƃ���X�N���v�g��؂�ւ�����悤�ɂ������̂ł��������𒲂ׂĂ���Ƃ���Ȃ̂Ŏ�Ԃ��삯�����܂��@�\����Ȃ��ł��B
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
    [PunRPC]
    private void CallFlag()
    {
        flag = !flag;
    }
}
