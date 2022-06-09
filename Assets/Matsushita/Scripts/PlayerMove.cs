using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    
    private Vector3 _nowMousePosi; // 現在のマウスのワールド座標
    [SerializeField]
    private GameObject rocket; //Unity上でロケットにあたるものをドラッグ＆ドロップ

    void Update()
    {
        Vector3 nowmouseposi;
        // 現在のマウスのワールド座標を取得
        nowmouseposi = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        nowmouseposi.z = 10;
        // 開始時のオブジェクトの座標にマウスの変化量を足して新しい座標を設定
        transform.position = nowmouseposi;
        //磁石が対象のものを向くように回転する
        //this.transform.LookAt(rocket.transform.position);
    }
}