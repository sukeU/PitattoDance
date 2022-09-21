using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetEffect : MonoBehaviour
{
    private Vector3 mouse;
    private Vector3 target;
    void Update()
    {
        //マウスの座標を受け取る
        mouse = Input.mousePosition;
        target = Camera.main.ScreenToWorldPoint(new Vector3(mouse.x, mouse.y, 10));
        this.transform.position = target;
        if (Input.GetMouseButton(0))
        {
            GetComponent<SpriteRenderer>().color = new Color(255, 0, 0, 0.5f);
        }
        else
        {
            GetComponent<SpriteRenderer>().color = new Color(255, 0, 0, 0);
        }
    }
}
