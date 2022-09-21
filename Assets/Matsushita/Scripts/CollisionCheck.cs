using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionCheck : MonoBehaviour
{
    DollCollision dollCollision;
    // Start is called before the first frame update
    void Start()
    {
        dollCollision = GameObject.FindGameObjectWithTag("Pose").GetComponent<DollCollision>();
    }

    //触れているとき、呼ばれる
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("触れた");
        var HDollName = other.gameObject.name; //触れたドールの箇所の名前を取得
        var HName = this.gameObject.name; //触れられた型の箇所の名前を取得
        dollCollision.checkHitName(HDollName, HName);
    }

    //離れたときに呼ばれる
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("離れた");
        dollCollision.checkoutHitName();
    }
}