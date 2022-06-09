using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;
using Photon.Realtime;

public class Magnet : MonoBehaviour//, IPointerDownHandler, IPointerUpHandler
{
    public const float G = 6.67259f; // 万有引力定数
    Rigidbody obj_body; // オブジェクトのRigidbody
    private GameObject obj; // 磁石に引かれるオブジェクト
    Rigidbody magnet_body; // 磁石のRigidbody
    private bool _isPushed = false; // マウスが押されているか押されていないか
    private bool _OnCollision = false; //磁石と物体が触れているかどうか
   // private bool _OnTrigger = false;//磁石透過時に物体が領域内にいるかどうか
    private float dis = 3.0f; // 磁石の反応する距離

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
            gameObject.GetComponent<BoxCollider>().isTrigger = true;//領域内にオブジェクトがあるときの当たり判定をなくす
        }
        else
        {
            gameObject.GetComponent<BoxCollider>().isTrigger = false;//当たり判定を戻す

        
        }*/


       
            //左クリックが押されて、かつ、物体に触れていない、
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
          Vector3 vec_direction = this.transform.position - obj.transform.position; //ロケットから見た惑星の位置
         Vector3 Univ_gravity = G * vec_direction.normalized * (magnet_body.mass * obj_body.mass) / (vec_direction.sqrMagnitude); //万有引力の計算
         float gravityX = Mathf.Clamp(Univ_gravity.x, -10.0f, 10.0f);//スイングバイを止めるために最大値を決めた
         float gravityY = Mathf.Clamp(Univ_gravity.y, -10.0f, 10.0f);//スイングバイを止めるために最大値を決めた
         Univ_gravity.z = 10;
         obj_body.AddForce(gravityX,gravityY,0.0f); //ロケットに万有引力を掛ける
         */
      
        
        Vector3 vec_direction = (this.transform.position - obj.transform.position).normalized;
        Vector3 move = vec_direction*Mathf.Lerp(0.0f, Vector3.Distance(this.transform.position, obj.transform.position), 0.1f);//移動する方向に距離に応じた速度を返す
        //Vector3 move = vec_direction * (3-Vector3.Distance(this.transform.position, obj.transform.position));//移動する方向に距離に応じた速度を返す
        obj_body.velocity = move*20;
        Debug.Log(move);
        sendOwner(obj);
    }
    /*
    public void OnPointerDown(PointerEventData eventData)
    {
        // 押下開始　フラグを立てる
        _isPushed = true;
        gameObject.GetComponent<BoxCollider>().isTrigger = true;//ボタンを押している間の当たり判定をなくす
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // 押下終了　フラグを落とす
        _isPushed = false;
        gameObject.GetComponent<BoxCollider>().isTrigger = false;//当たり判定を戻す
    }
    */

    private void OnCollisionEnter(Collision collision)
    {
        //手足のタグがついていなかったらreturnする
        if (!(collision.gameObject.tag == "Rarm" || collision.gameObject.tag == "Larm" || collision.gameObject.tag == "Rleg" || collision.gameObject.tag == "Lleg"||collision.gameObject.tag=="Body"))
        {
            return;
        }
        _OnCollision = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        //手足のタグがついていなかったらreturnする
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


    //一定距離内にあるかどうか
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


    private void sendOwner(GameObject obj)//人形に所有者を渡すための関数
    {
        DollSync dollSync = obj.GetComponent<DollSync>();//スクリプトを取得する
        if (dollSync == null)return;//人形用のスクリプトを持っていなかったらreturnする
        dollSync.ChangeOwner(PhotonNetwork.LocalPlayer);
       
    }

}
