using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class PlayerMagnet : MonoBehaviourPunCallbacks,IPunObservable
{
    [SerializeField]
    private TextMeshPro nameLabel = default;

    //警告　Awakeでエラーが出てると生成した時にこのスクリプトが非アクティブになってしまう。エラーログにもでないから注意
    void Start()
    {
        nameLabel.text = $"{photonView.Owner.NickName}";
        var gamePlayerManager = GameObject.FindWithTag("GamePlayerManager");
        transform.SetParent(gamePlayerManager.transform);
    }

    void FixedUpdate()
    {
        if (photonView.IsMine)
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
    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);//位置の更新
        }
        else
        {
            transform.position = (Vector3)stream.ReceiveNext();//位置を受け取る
        }
    }
}
