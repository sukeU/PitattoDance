using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class PlayerMagnet : MonoBehaviourPunCallbacks,IPunObservable
{
    [SerializeField]
    private TextMeshPro nameLabel = default;

    //�x���@Awake�ŃG���[���o�Ă�Ɛ����������ɂ��̃X�N���v�g����A�N�e�B�u�ɂȂ��Ă��܂��B�G���[���O�ɂ��łȂ����璍��
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
            Vector3 MagnetScreenPosition = Input.mousePosition;//�}�E�X���W���擾

            MagnetScreenPosition.x = Mathf.Clamp(MagnetScreenPosition.x, 0.0f, Screen.width);//Clamp�ŉ�ʊO�ɏo�Ȃ��悤��
            MagnetScreenPosition.y = Mathf.Clamp(MagnetScreenPosition.y, 0.0f, Screen.height);


            MagnetScreenPosition.z = 10.0f;//�摜��\�����邽�߂ɓ��ꂽ���́i���W���J�����Ɠ������Ǝʂ�Ȃ��j

            Camera gameCamera = Camera.main;
            Vector3 MagnetWorldPosition = gameCamera.ScreenToWorldPoint(MagnetScreenPosition);

            transform.position = MagnetWorldPosition;//�ړ�������
        }
    }
    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);//�ʒu�̍X�V
        }
        else
        {
            transform.position = (Vector3)stream.ReceiveNext();//�ʒu���󂯎��
        }
    }
}
