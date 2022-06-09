using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class CollisionDetection : MonoBehaviourPunCallbacks
{
    [SerializeField]
    [Tooltip("発生させるエフェクト(パーティクル)")]
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

    private void Start()//生成された瞬間にゲームシーンのネットワークマネージャーを捜して取得する
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
        { //タイミングがあってたら

          // パーティクルシステムのインスタンスを生成する。
            ParticleSystem newParticle = Instantiate(particle);
            // パーティクルの発生場所をこのスクリプトをアタッチしているGameObjectの場所にする。
            newParticle.transform.position = transform.position;
            // パーティクルを発生させる。
            newParticle.Play();
            Destroy(newParticle.gameObject, 5.0f);
            ScoreHandOver();//スコアの更新を行うことを全員に通知する;
            Destroy(gameObject);
        }
        /*
        if (!CheckTouchTag()) return;//ターゲットタグと触っているタグが同じじゃなかったらリターン 
        if (!PhotonNetwork.IsMasterClient) return; //マスタークライアントじゃなかったらリターン
        */
       
    }

    private void OnTriggerEnter(Collider other)
    {
        //手足のタグがついていなかったらreturnする
        if (!(other.gameObject.tag == "Rarm" || other.gameObject.tag == "Larm" || other.gameObject.tag == "Rleg" || other.gameObject.tag == "Lleg"||other.gameObject.tag == "Body"))
        {
            return;
        }
        TouchTags.Add(other.gameObject.tag);
        if (!PhotonNetwork.IsMasterClient) return; //マスタークライアントじゃなかったらリターン
        if (other.gameObject.tag == TargetTag)
        {
           
                photonView.RPC(nameof(TagHandOver), RpcTarget.All, 1);
           
           // sendTagCount = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //手足のタグがついていなかったらreturnする
        if (!(other.gameObject.tag == "Rarm" || other.gameObject.tag == "Larm" || other.gameObject.tag == "Rleg" || other.gameObject.tag == "Lleg" || other.gameObject.tag == "Body"))
        {
            return;
        }
        TouchTags.Remove(other.gameObject.tag);
        if (!PhotonNetwork.IsMasterClient) return; //マスタークライアントじゃなかったらリターン
       // time = 0.0f;//判定の中のタイマーを初期化
        if (other.gameObject.tag == TargetTag)
        {
            photonView.RPC(nameof(TagHandOver), RpcTarget.All, -1);
            //sendTagCount = false;
        }
    }

    public void ScoreHandOver()//スコアの受け渡しを行う
    { 
        Manager.ScoreUpdate(1);//テストで1ポイント受け渡しをする
    }

    [PunRPC]
    public void TagHandOver(int value)//判定に入っているタグの数をManagerに渡す
    {
        try
        {
        Manager.TagUpdate(value);
        }
        catch (NullReferenceException)
        {
            Debug.Log("tagがセットされる前に判定に入っている例外");
        }
    }

    private bool CheckTouchTag()
    {
        foreach (var TouchTag in TouchTags)//触っているタグの中を全て網羅してターゲットタグとあっているか確認する
        {
            if (TargetTag == TouchTag)
            {
                return true;
            }
        }
        return false;
    }
    
 
}
