using UnityEngine;
using Photon.Pun;
using System.Linq;

public class TestMagnet : MonoBehaviour
{
    Rigidbody target_body; // �I�u�W�F�N�g��Rigidbody
    [SerializeField]
    private GameObject target; // ���΂Ɉ������I�u�W�F�N�g
    private bool isPushed = false; // �}�E�X��������Ă��邩������Ă��Ȃ���

    //lineRenderer�p
    Vector3[] positions; //�}�O�l�b�g�ƈ�������ӏ��̊Ԃ̍��W
    LineRenderer lineRenderer;
    public GameObject magnet;
    private GameObject rightHand;
    private GameObject leftHand;
    private GameObject head;
    private void Start()
    {
        //lineRenderer�p�̏����ݒ�
        lineRenderer = GetComponent<LineRenderer>();
        rightHand = GameObject.FindWithTag("Rarm");
        leftHand = GameObject.FindWithTag("Larm");
        head = GameObject.FindWithTag("Head");
        positions = new Vector3[]
        {
            magnet.transform.position,
            rightHand.transform.position
        };
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
    }
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            isPushed = true;
        }
        else
        {
            isPushed = false;
        }
        //�}�E�X��������āA���A���v���C���ł͂Ȃ��Ƃ��A
        if (isPushed)
        {

            target = roomClosestDoll();//�^�[�Q�b�g�ɍł��߂��l�`��������
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
        if (target == null) return;
        switch (target.name)
        {
            case "RightHand":
                positions[0] = magnet.transform.position;
                positions[1] = rightHand.transform.position;
                lineRenderer.SetPositions(positions);
                break;
            case "LeftHand":
                positions[0] = magnet.transform.position;
                positions[1] = leftHand.transform.position;
                lineRenderer.SetPositions(positions);
                break;
            case "Head":
                positions[0] = magnet.transform.position;
                positions[1] = head.transform.position;
                lineRenderer.SetPositions(positions);
                break;
            default:
                break;

        }
    }


    public void UseMagnet()//���΂𗘗p����
    {
        Vector3 vec_direction = (this.transform.position - target.transform.position).normalized;
        Vector3 move = vec_direction * Mathf.Lerp(0.0f, Vector3.Distance(this.transform.position, target.transform.position), 0.1f);//�ړ���������ɋ����ɉ��������x��Ԃ�
        target_body.velocity = move * 10;
    }

    GameObject roomClosestDoll()//�ł����΂ɋ߂��l�`�I�u�W�F�N�g��Ԃ�
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
                if (hit.layer != 6) continue;
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
}
