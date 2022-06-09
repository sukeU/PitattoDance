using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class CollisionDetection : MonoBehaviourPunCallbacks
{
    [SerializeField]
    [Tooltip("����������G�t�F�N�g(�p�[�e�B�N��)")]
    private ParticleSystem particle;
    [SerializeField]
    GameNetworkManager Manager=default;
    [SerializeField]
    List<string> TouchTags = new List<string>();
    [SerializeField]
    private string TargetTag;
    private float time;
    /*
    [SerializeField]
    private bool sendTagCount=false;
    [SerializeField]
    private bool sendExitTagCount = false;
    */

    public void SetTargetTag(string TargetTag)
    {
        this.TargetTag = TargetTag;
    }

    public void SetPoseObjColor()
    {
        switch (this.TargetTag)
        {
            case "Rarm":gameObject.GetComponent<Renderer>().material.color = new Color(1, 0, 0,0.25f);
                break;
            case "Body":
                gameObject.GetComponent<Renderer>().material.color = new Color(0, 1, 0, 0.25f);
                break;
            case "Larm":
                gameObject.GetComponent<Renderer>().material.color = new Color(0, 0, 1, 0.25f);
                break;
        }
    }

    private void Start()//�������ꂽ�u�ԂɃQ�[���V�[���̃l�b�g���[�N�}�l�[�W���[��{���Ď擾����
    {
        Manager = GameObject.FindGameObjectWithTag("GameNetworkObject").GetComponent<GameNetworkManager>();
    }

    private void FixedUpdate()
    {
        if (Manager.isReplay)
        {
            Destroy(gameObject);
        }
        if (Manager.timing)
        { //�^�C�~���O�������Ă���

          // �p�[�e�B�N���V�X�e���̃C���X�^���X�𐶐�����B
            ParticleSystem newParticle = Instantiate(particle);
            // �p�[�e�B�N���̔����ꏊ�����̃X�N���v�g���A�^�b�`���Ă���GameObject�̏ꏊ�ɂ���B
            newParticle.transform.position = transform.position;
            // �p�[�e�B�N���𔭐�������B
            newParticle.Play();
            Destroy(newParticle.gameObject, 5.0f);
            ScoreHandOver();//�X�R�A�̍X�V���s�����Ƃ�S���ɒʒm����;
            Destroy(gameObject);
        }
        /*
        if (!CheckTouchTag()) return;//�^�[�Q�b�g�^�O�ƐG���Ă���^�O����������Ȃ������烊�^�[�� 
        if (!PhotonNetwork.IsMasterClient) return; //�}�X�^�[�N���C�A���g����Ȃ������烊�^�[��
        */
       
    }

    private void OnTriggerEnter(Collider other)
    {
        //�葫�̃^�O�����Ă��Ȃ�������return����
        if (!(other.gameObject.tag == "Rarm" || other.gameObject.tag == "Larm" || other.gameObject.tag == "Rleg" || other.gameObject.tag == "Lleg"||other.gameObject.tag == "Body"))
        {
            return;
        }
        TouchTags.Add(other.gameObject.tag);
        if (!PhotonNetwork.IsMasterClient) return; //�}�X�^�[�N���C�A���g����Ȃ������烊�^�[��
        if (other.gameObject.tag == TargetTag)
        {
           
                photonView.RPC(nameof(TagHandOver), RpcTarget.All, 1);
           
           // sendTagCount = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //�葫�̃^�O�����Ă��Ȃ�������return����
        if (!(other.gameObject.tag == "Rarm" || other.gameObject.tag == "Larm" || other.gameObject.tag == "Rleg" || other.gameObject.tag == "Lleg" || other.gameObject.tag == "Body"))
        {
            return;
        }
        TouchTags.Remove(other.gameObject.tag);
        if (!PhotonNetwork.IsMasterClient) return; //�}�X�^�[�N���C�A���g����Ȃ������烊�^�[��
       // time = 0.0f;//����̒��̃^�C�}�[��������
        if (other.gameObject.tag == TargetTag)
        {
            photonView.RPC(nameof(TagHandOver), RpcTarget.All, -1);
            //sendTagCount = false;
        }
    }

    public void ScoreHandOver()//�X�R�A�̎󂯓n�����s��
    { 
        Manager.ScoreUpdate(1);//�e�X�g��1�|�C���g�󂯓n��������
    }

    [PunRPC]
    public void TagHandOver(int value)//����ɓ����Ă���^�O�̐���Manager�ɓn��
    {
        try
        {
        Manager.TagUpdate(value);
        }
        catch (NullReferenceException)
        {
            Debug.Log("tag���Z�b�g�����O�ɔ���ɓ����Ă����O");
        }
    }

    private bool CheckTouchTag()
    {
        foreach (var TouchTag in TouchTags)//�G���Ă���^�O�̒���S�Ėԗ����ă^�[�Q�b�g�^�O�Ƃ����Ă��邩�m�F����
        {
            if (TargetTag == TouchTag)
            {
                return true;
            }
        }
        return false;
    }
    
 
}
