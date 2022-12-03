using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Linq;

public class Magnet : MonoBehaviour
{
    Rigidbody target_body; // �I�u�W�F�N�g��Rigidbody
    [SerializeField]
    private GameObject target; // ���΂Ɉ������I�u�W�F�N�g

    PhotonView photonView;
    //lineRenderer�p
    Vector3[] positions; //�}�O�l�b�g�ƈ�������ӏ��̊Ԃ̍��W
    LineRenderer lineRenderer;
    public GameObject magnet;
    private GameObject RightHand;
    private GameObject LeftHand;
    private GameObject Head;
    NetworkManager networkManager;

    private void Start()
    {
        networkManager = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<NetworkManager>();
        //lineRenderer�p�̏����ݒ�
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        RightHand = GameObject.FindWithTag("Rarm");
        LeftHand = GameObject.FindWithTag("Larm");
        Head = GameObject.FindWithTag("Head");
        positions = new Vector3[]
        {
            magnet.transform.position,
            RightHand.transform.position
        };
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        photonView = GetComponent<PhotonView>();
    }
    void Update()
    {
        //�}�E�X��������āA���A���v���C���ł͂Ȃ��Ƃ��A
        if (Input.GetMouseButton(0) && networkManager.currentGameState==NetworkManager.GameState.Playing)
        {
            if (!photonView.IsMine) return;
            target = closestDoll();//�^�[�Q�b�g�ɍł��߂��l�`��������
            lineRenderer.enabled = true;
            if (target != null)//�^�[�Q�b�g��null����Ȃ�������
            {
                if (target_body == null)
                {
                    target_body = target.GetComponent<Rigidbody>();
                }
                else
                {
                    UseMagnet();
                }
            }
        }
        else//�{�^����������Ă��Ȃ��Ƃ��ɏ�����
        {
            target = null;
            target_body = null;
            lineRenderer.enabled = false;
    
        }

        //�I�u�W�F�N�g�Ԃ̐��̕\��
        if (target == null)
        {
            lineRenderer.enabled = false;
            return;
        }
        switch (target.name)
        {
            case "RightHand":
                positions[0] = magnet.transform.position;
                positions[1] = RightHand.transform.position;
                lineRenderer.SetPositions(positions);
                break;
            case "LeftHand":
                positions[0] = magnet.transform.position;
                positions[1] = LeftHand.transform.position;
                lineRenderer.SetPositions(positions);
                break;
            case "Head":
                positions[0] = magnet.transform.position;
                positions[1] = Head.transform.position;
                lineRenderer.SetPositions(positions);
                break;
        }
    }


    public void UseMagnet()//���΂𗘗p����
    {    
        Vector3 vec_direction = (this.transform.position - target.transform.position).normalized;
        Vector3 move = vec_direction*Mathf.Lerp(0.0f, Vector3.Distance(this.transform.position, target.transform.position), 0.1f);//�ړ���������ɋ����ɉ��������x��Ԃ�
        target_body.velocity = move*10;
        sendOwner(target);
    }

    GameObject closestDoll()//�ł����΂ɋ߂��l�`�I�u�W�F�N�g��Ԃ�
    {
        var hits = Physics.SphereCastAll(
            transform.position,//���΂̏ꏊ����
            3f,//sphere�̑傫��
            transform.forward,//�O��
            0.01f
            ).Select(h => h.transform.gameObject).ToList();//Select�œ��������Q�[���I�u�W�F�N�g��transform��I�сA���X�g������

        if (0 < hits.Count())//1�ł��������Ă�����
        {
            float min_target_distance = float.MaxValue;//�ŏ�������float�^�̍ő�l�ŏ�����
            GameObject target = null;

            foreach (var hit in hits)//hits�̒�����
            {
                if (hit.layer != 6) continue ;
                float target_distance = Vector3.Distance(transform.position, hit.transform.position);//���΂�hit�̋��������߂�

                if (target_distance < min_target_distance)//�ł��߂��I�u�W�F�N�g���X�ɂ�������
                {
                    min_target_distance = target_distance;
                    target = hit.transform.gameObject;
                }
            }
            return target;//�ł��߂��I�u�W�F�N�g��Ԃ�
        }
        else
        {
            return null;//�����Ԃ��Ȃ�
        }
    }

    private void sendOwner(GameObject obj)//�l�`�ɏ��L�҂�n�����߂̊֐�
    {
        DollSync dollSync = obj.GetComponent<DollSync>();//�X�N���v�g���擾����
        if (dollSync == null)return;//�l�`�p�̃X�N���v�g�������Ă��Ȃ�������return����
        dollSync.ChangeOwner(PhotonNetwork.LocalPlayer);
    }

}
