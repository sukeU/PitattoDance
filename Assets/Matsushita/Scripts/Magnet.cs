using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Linq;

public class Magnet : MonoBehaviour
{
    Rigidbody target_body; // オブジェクトのRigidbody
    [SerializeField]
    private GameObject target; // 磁石に引かれるオブジェクト

    PhotonView photonView;
    //lineRenderer用
    Vector3[] positions; //マグネットと引っ張る箇所の間の座標
    LineRenderer lineRenderer;
    public GameObject magnet;
    private GameObject RightHand;
    private GameObject LeftHand;
    private GameObject Head;
    NetworkManager networkManager;

    private void Start()
    {
        networkManager = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<NetworkManager>();
        //lineRenderer用の初期設定
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
        //マウスが押されて、かつ、リプレイ時ではないとき、
        if (Input.GetMouseButton(0) && networkManager.currentGameState==NetworkManager.GameState.Playing)
        {
            if (!photonView.IsMine) return;
            target = closestDoll();//ターゲットに最も近い人形を代入する
            lineRenderer.enabled = true;
            if (target != null)//ターゲットがnullじゃなかったら
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
        else//ボタンが押されていないときに初期化
        {
            target = null;
            target_body = null;
            lineRenderer.enabled = false;
    
        }

        //オブジェクト間の線の表示
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


    public void UseMagnet()//磁石を利用する
    {    
        Vector3 vec_direction = (this.transform.position - target.transform.position).normalized;
        Vector3 move = vec_direction*Mathf.Lerp(0.0f, Vector3.Distance(this.transform.position, target.transform.position), 0.1f);//移動する方向に距離に応じた速度を返す
        target_body.velocity = move*10;
        sendOwner(target);
    }

    GameObject closestDoll()//最も磁石に近い人形オブジェクトを返す
    {
        var hits = Physics.SphereCastAll(
            transform.position,//磁石の場所から
            3f,//sphereの大きさ
            transform.forward,//前方
            0.01f
            ).Select(h => h.transform.gameObject).ToList();//Selectで当たったゲームオブジェクトのtransformを選び、リスト化する

        if (0 < hits.Count())//1つでも当たっていたら
        {
            float min_target_distance = float.MaxValue;//最小距離をfloat型の最大値で初期化
            GameObject target = null;

            foreach (var hit in hits)//hitsの中から
            {
                if (hit.layer != 6) continue ;
                float target_distance = Vector3.Distance(transform.position, hit.transform.position);//磁石とhitの距離を求める

                if (target_distance < min_target_distance)//最も近いオブジェクトが更にあったら
                {
                    min_target_distance = target_distance;
                    target = hit.transform.gameObject;
                }
            }
            return target;//最も近いオブジェクトを返す
        }
        else
        {
            return null;//何も返さない
        }
    }

    private void sendOwner(GameObject obj)//人形に所有者を渡すための関数
    {
        DollSync dollSync = obj.GetComponent<DollSync>();//スクリプトを取得する
        if (dollSync == null)return;//人形用のスクリプトを持っていなかったらreturnする
        dollSync.ChangeOwner(PhotonNetwork.LocalPlayer);
    }

}
