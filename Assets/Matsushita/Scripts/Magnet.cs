using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;
using Photon.Realtime;

public class Magnet : MonoBehaviour//, IPointerDownHandler, IPointerUpHandler
{
    public const float G = 6.67259f; // ���L���͒萔
    Rigidbody obj_body; // �I�u�W�F�N�g��Rigidbody
    private GameObject obj; // ���΂Ɉ������I�u�W�F�N�g
    Rigidbody magnet_body; // ���΂�Rigidbody
    private bool _isPushed = false; // �}�E�X��������Ă��邩������Ă��Ȃ���
    private bool _OnCollision = false; //���΂ƕ��̂��G��Ă��邩�ǂ���
   // private bool _OnTrigger = false;//���Γ��ߎ��ɕ��̂��̈���ɂ��邩�ǂ���
    private float dis = 3.0f; // ���΂̔������鋗��

    // Start is called before the first frame update
    void Start()
    {
        //obj_body = obj.GetComponent<Rigidbody>();
        magnet_body = this.GetComponent<Rigidbody>();
        //obj = GameObject.FindGameObjectWithTag("Target");
      
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            _isPushed = true;
        }
        else
        {
            _isPushed = false;
        }
        /*
        if (_OnTrigger)
        {
            gameObject.GetComponent<BoxCollider>().isTrigger = true;//�̈���ɃI�u�W�F�N�g������Ƃ��̓����蔻����Ȃ���
        }
        else
        {
            gameObject.GetComponent<BoxCollider>().isTrigger = false;//�����蔻���߂�

        
        }*/


       
            //���N���b�N��������āA���A���̂ɐG��Ă��Ȃ��A
            if (_isPushed&&_OnCollision == false)
            {
             
           
                if (WithinDisRarm())
                {
                    obj = GameObject.FindGameObjectWithTag("Rarm");
                    obj_body = obj.GetComponent<Rigidbody>();
                    UniGravitation();
                }

                if (WithinDisLarm())
                {
                    obj = GameObject.FindGameObjectWithTag("Larm");
                    obj_body = obj.GetComponent<Rigidbody>();
                    UniGravitation();
                }

                if (WithinDisRleg())
                {
                    obj = GameObject.FindGameObjectWithTag("Rleg");
                    obj_body = obj.GetComponent<Rigidbody>();
                    UniGravitation();
                }

                if (WithinDisLleg())
                {
                    obj = GameObject.FindGameObjectWithTag("Lleg");
                    obj_body = obj.GetComponent<Rigidbody>();
                    UniGravitation();
                }

            if (WithinDisBody())
            {
                obj = GameObject.FindGameObjectWithTag("Body");
                obj_body = obj.GetComponent<Rigidbody>();
                UniGravitation();
            }
            }
            else
            {
            
                if (!(obj == null)) obj_body.velocity = Vector3.zero;
                obj = null;
            }
        
      
    }


    public void UniGravitation()
    {
        /*
          Vector3 vec_direction = this.transform.position - obj.transform.position; //���P�b�g���猩���f���̈ʒu
         Vector3 Univ_gravity = G * vec_direction.normalized * (magnet_body.mass * obj_body.mass) / (vec_direction.sqrMagnitude); //���L���͂̌v�Z
         float gravityX = Mathf.Clamp(Univ_gravity.x, -10.0f, 10.0f);//�X�C���O�o�C���~�߂邽�߂ɍő�l�����߂�
         float gravityY = Mathf.Clamp(Univ_gravity.y, -10.0f, 10.0f);//�X�C���O�o�C���~�߂邽�߂ɍő�l�����߂�
         Univ_gravity.z = 10;
         obj_body.AddForce(gravityX,gravityY,0.0f); //���P�b�g�ɖ��L���͂��|����
         */
      
        
        Vector3 vec_direction = (this.transform.position - obj.transform.position).normalized;
        Vector3 move = vec_direction*Mathf.Lerp(0.0f, Vector3.Distance(this.transform.position, obj.transform.position), 0.1f);//�ړ���������ɋ����ɉ��������x��Ԃ�
        //Vector3 move = vec_direction * (3-Vector3.Distance(this.transform.position, obj.transform.position));//�ړ���������ɋ����ɉ��������x��Ԃ�
        obj_body.velocity = move*20;
        Debug.Log(move);
        sendOwner(obj);
    }
    /*
    public void OnPointerDown(PointerEventData eventData)
    {
        // �����J�n�@�t���O�𗧂Ă�
        _isPushed = true;
        gameObject.GetComponent<BoxCollider>().isTrigger = true;//�{�^���������Ă���Ԃ̓����蔻����Ȃ���
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // �����I���@�t���O�𗎂Ƃ�
        _isPushed = false;
        gameObject.GetComponent<BoxCollider>().isTrigger = false;//�����蔻���߂�
    }
    */

    private void OnCollisionEnter(Collision collision)
    {
        //�葫�̃^�O�����Ă��Ȃ�������return����
        if (!(collision.gameObject.tag == "Rarm" || collision.gameObject.tag == "Larm" || collision.gameObject.tag == "Rleg" || collision.gameObject.tag == "Lleg"||collision.gameObject.tag=="Body"))
        {
            return;
        }
        _OnCollision = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        //�葫�̃^�O�����Ă��Ȃ�������return����
        if (!(collision.gameObject.tag == "Rarm" || collision.gameObject.tag == "Larm" || collision.gameObject.tag == "Rleg" || collision.gameObject.tag == "Lleg" || collision.gameObject.tag == "Body"))
        {
            return;
        }
        _OnCollision = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!(other.gameObject.tag == "Rarm" || other.gameObject.tag == "Larm" || other.gameObject.tag == "Rleg" || other.gameObject.tag == "Lleg" || other.gameObject.tag == "Body"))
        {
            return;
        }
      //  _OnTrigger = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!(other.gameObject.tag == "Rarm" || other.gameObject.tag == "Larm" || other.gameObject.tag == "Rleg" || other.gameObject.tag == "Lleg" || other.gameObject.tag == "Body"))
        {
            return;
        }
      //  _OnTrigger = false;
    }


    //��苗�����ɂ��邩�ǂ���
    private bool WithinDisRarm()
    {
        
        GameObject Rarm = GameObject.FindGameObjectWithTag("Rarm");
        float d = Vector3.Distance(this.transform.position, Rarm.transform.position);
        if(d < dis)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool WithinDisLarm()
    {
        GameObject Larm = GameObject.FindGameObjectWithTag("Larm");
     
        float d = Vector3.Distance(this.transform.position, Larm.transform.position);
        if (d < dis)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool WithinDisRleg()
    {
        GameObject Rleg = GameObject.FindGameObjectWithTag("Rleg");
       
        float d = Vector3.Distance(this.transform.position, Rleg.transform.position);
        if (d < dis)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool WithinDisLleg()
    {
        GameObject Lleg = GameObject.FindGameObjectWithTag("Lleg");
 
        float d = Vector3.Distance(this.transform.position, Lleg.transform.position);
        if (d < dis)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool WithinDisBody()
    {
        GameObject Body = GameObject.FindGameObjectWithTag("Body");
        float d = Vector3.Distance(this.transform.position, Body.transform.position);
        if (d < dis)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    private void sendOwner(GameObject obj)//�l�`�ɏ��L�҂�n�����߂̊֐�
    {
        DollSync dollSync = obj.GetComponent<DollSync>();//�X�N���v�g���擾����
        if (dollSync == null)return;//�l�`�p�̃X�N���v�g�������Ă��Ȃ�������return����
        dollSync.ChangeOwner(PhotonNetwork.LocalPlayer);
       
    }

}
