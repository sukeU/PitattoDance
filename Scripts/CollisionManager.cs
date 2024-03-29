using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CollisionManager : MonoBehaviour
{
    [SerializeField]
    bool matchPose = false;
    GamePlayManager manager;
    CollisionCheckT[] collisions = new CollisionCheckT[3];
    bool reset = false;

    private void Start()
    {
        manager = GameObject.FindGameObjectWithTag("GamePlayManager").GetComponent<GamePlayManager>();
        collisions[0] = transform.GetChild(0).GetChild(0).GetComponent<CollisionCheckT>();//頭
        collisions[1] = transform.GetChild(0).GetChild(1).GetComponent<CollisionCheckT>();//左手
        collisions[2] = transform.GetChild(0).GetChild(2).GetComponent<CollisionCheckT>();//右手

        collisions[0].SetTargetTag("Head");
        collisions[1].SetTargetTag("Larm");
        collisions[2].SetTargetTag("Rarm");

        Debug.Log("初期化");
        foreach (var col in collisions)
        {
            col.ClearTotchTags();
        }

    }
    private void FixedUpdate()
    {
        if (collisions[0].CheckTouchTag() && collisions[1].CheckTouchTag() && collisions[2].CheckTouchTag())//全部合致していたら
        {
            Debug.Log("初期化");

            foreach (var col in collisions)
            {
                col.ClearTotchTags();
            }
            manager.MatchPose();
        }
    }     
}

