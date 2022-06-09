using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;
using Photon.Pun;


public class ActionReplay : MonoBehaviourPunCallbacks
{
    //このコードは動きを再現したいオブジェクトにくっ付けてください
    private bool isInReplayMode;//リプレイモードか否かのフラグです
    [SerializeField]
    private bool flag = false;//リプレイモードにする為の判定を1回だけ行うためだけのフラグです
    //public bool Flag { set { flag = value; } get { return flag; } }
    private float ReplaySpeed = 2.5f;//リプレイの速さの変数です。1でプレイ時と同じ速さで再生します。
    private float currentReplayIndex;
    private Rigidbody rb;
    private List<ReplayRecorder> replayRecorders = new List<ReplayRecorder>();//動きを格納しているリストです
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
                        photonView.RPC(nameof(CallFlag), RpcTarget.All);//NullReferenceExceptionが出てるので原因捜し中
                    }
                    catch (NullReferenceException)
                    {
                        Debug.Log("この例外処理は後から考える");
                    }
                }
                oneTime = true;
            }
        }
        if (flag == false)//1回だけ発動させたいのでflagを使ってます
         //テスト環境下ではGameRoomHUDというスクリプトで時間を管理していました
         //GameRoomHUD内の時間が設定された時間を超えたときにGameRoom内にあるgameTimeがfalseになるという仕組みです
         //ゲームに組み込むときは上のif文の中のコメントアウトされた箇所に時間による条件を入れてください
        {
            flag = true;
            isInReplayMode = !isInReplayMode;
            

            if (isInReplayMode)
            {
                SetTransform(0);
                rb.isKinematic = true;//物理演算の影響をoffに
                if (gameObject.GetComponent<Magnet>() != null) { 
                gameObject.GetComponent<Magnet>().enabled = false;//ここで操作をoffにしています<>の中に操作に関係するスクリプトを入れてください
                    }
                //このテスト環境ではControlが操作に関係するスクリプトでした
                //本当はインスペクターで参照するスクリプトを切り替えられるようにしたいのですがやり方を調べているところなので手間を駆けさせます　申し訳ないです。
            }

        }

    }

    private void FixedUpdate()//リプレイに関する諸々の処理　
    {
        if (isInReplayMode == false)//リプレイモードでないときは記録
        {
            replayRecorders.Add(new ReplayRecorder { position = transform.position, rotation = transform.rotation });
        }
        else
        {
            float nextIndex = currentReplayIndex + ReplaySpeed;//リプレイモードの時は再生
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
