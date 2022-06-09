using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class DollSync : MonoBehaviourPunCallbacks, IOnPhotonViewOwnerChange//,IPunObservable
{
    public void ChangeOwner(Player NewOwner)
    {
        // ���L���̈ڏ�
        gameObject.GetComponent<PhotonView>().TransferOwnership(NewOwner);
    }

    void IOnPhotonViewOwnerChange.OnOwnerChange(Player newOwner, Player previousOwner)//���L�҂��ς�������Ƃ�m�点��֐�
    {
        string objectName = $"{photonView.name}({photonView.ViewID})";
        string oldName = previousOwner.NickName;
        string newName = newOwner.NickName;
        Debug.Log($"{objectName} �̏��L�҂� {oldName} ���� {newName} �ɕύX����܂���");
    }
}
