using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionReplay1 : MonoBehaviour
{
    //このコードは動きを再現したいオブジェクトにくっ付けてください
    private bool isInReplayMode;//リプレイモードか否かのフラグです
    [SerializeField]
    private bool flag = true;//リプレイモードにする為の判定を1回だけ行うためだけのフラグです
    //public bool Flag { set { flag = value; } get { return flag; } }
    private float ReplaySpeed = 3.0f;//リプレイの速さの変数です。1でプレイ時と同じ速さで再生します。
    private float currentReplayIndex;
    private Rigidbody rb;
    private List<ReplayRecorder> replayRecorders = new List<ReplayRecorder>();//動きを格納しているリストです
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
                    rb.isKinematic = true;//物理演算の影響をoffに
                    rb.constraints= RigidbodyConstraints.FreezePosition;
                    if (gameObject.GetComponent<Magnet>() != null)
                    {
                        gameObject.GetComponent<Magnet>().enabled = false;//ここで操作をoffにしています<>の中に操作に関係するスクリプトを入れてください
                    }
                }
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

}
