using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//ここのコードの大きな変更はPlayAreaの取得です。ここでArea上に存在する場合のみアイコンが追従するようにしてあります。　入野
public class TestMagnetControl: MonoBehaviour
{
    [SerializeField]
    private TextMeshPro nameLabel = default;
    public GameObject Area;

    //警告　Awakeでエラーが出てると生成した時にこのスクリプトが非アクティブになってしまう。エラーログにもでないから注意
    void Start()
    {
 
        var gamePlayerManager = GameObject.FindWithTag("GamePlayerManager");
        Area = GameObject.Find("PlayArea");

    }

    void FixedUpdate()
    {
        if (Area.GetComponent<PlayArea>()._OnPlayArea)
        {
            Vector3 MagnetScreenPosition = Input.mousePosition;//マウス座標を取得

            MagnetScreenPosition.x = Mathf.Clamp(MagnetScreenPosition.x, 0.0f, Screen.width);//Clampで画面外に出ないように
            MagnetScreenPosition.y = Mathf.Clamp(MagnetScreenPosition.y, 0.0f, Screen.height);


            MagnetScreenPosition.z = 10.0f;//画像を表示するために入れたもの（座標がカメラと同じだと写らない）

            Camera gameCamera = Camera.main;
            Vector3 MagnetWorldPosition = gameCamera.ScreenToWorldPoint(MagnetScreenPosition);

            transform.position = MagnetWorldPosition;//移動させる
        }
    }
    
}
