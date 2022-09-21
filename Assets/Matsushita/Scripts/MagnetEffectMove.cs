using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetEffectMove : MonoBehaviour
{
    private Vector3 mouse;
    private Vector3 target;
    // Update is called once per frame
    void Update()
    {
        //マウスの座標を受け取る
        mouse = Input.mousePosition;
        target = Camera.main.ScreenToWorldPoint(new Vector3(mouse.x, mouse.y, 10));
        this.transform.position = target;

    }
}
