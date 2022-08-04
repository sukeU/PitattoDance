using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class State : MonoBehaviour
{

    ///���̃X�N���v�g���̂̓l�b�g���[�N����؂藣���ē��삳����

    [SerializeField]
    NewNetworkManager networkManager;
    [SerializeField]
    TMP_InputField inputField;//�v���C���[�l�[���̓��͗�
    [SerializeField]
    string nickname;//�v���C���[�l�[��
    [Space]
    [SerializeField]
    GameObject Title;//�^�C�g���I�u�W�F�N�g
    [SerializeField]
    GameObject Lobby;//���r�[�I�u�W�F�N�g
    [SerializeField]
    GameObject Room;//���[���I�u�W�F�N�g
    [SerializeField]
    TextMeshProUGUI RoomPlayerList;//���[���̖��O�����X�V���邽�߂ɕK�v
    [SerializeField]
    GameObject GameButton; //�Q�[���{�^���̃I�u�W�F�N�g
    private TextMeshProUGUI GameButtonText;

    


    void Start()
    {
        networkManager =GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<NewNetworkManager>();
        inputField = inputField.GetComponent<TMP_InputField>();//InputField�̏����擾
        GameButtonText = GameButton.GetComponentInChildren<TextMeshProUGUI>();
        networkManager.OnUpdateMember += roomSetup;

        if (networkManager.currentState == NewNetworkManager.NetworkState.Room)
        {
            Title.SetActive(false);
            Room.SetActive(true);
            networkManager.roomSetUp();//�l�b�g���[�N���̕����ݒ�
            roomSetup();//���̃X�N���v�g�̃��[�����Z�b�g����
        }
    }
    public void GoLobby()//�^�C�g����ʂ̓�������������ɌĂ�
    {
        nickname = inputField.text;
        networkManager.ChangeNickName(nickname);
        networkManager.ConnectSever();
        Title.SetActive(false);
        StartCoroutine("GoingLobby");

    }

    IEnumerator GoingLobby()//�ڑ�����܂ő҂�
    {
        yield return new WaitForSeconds(0.5f);
        Lobby.SetActive(true);
    }

    public void GoRoom()//���[���ɍs��
    {
        networkManager.CreateRoom1();
        StartCoroutine("GoingRoom");
        Lobby.SetActive(false);
    }
    IEnumerator GoingRoom()//�l�b�g���[�N�̒l�̎󂯓n���ɃR���[�`�����g��Ȃ���΂Ȃ�Ȃ�
    {
        yield return new WaitForSeconds(0.5f);
        networkManager.roomSetUp();//�l�b�g���[�N���̕����ݒ�
        roomSetup();
        Room.SetActive(true);
    }

    public void LeaveRoom()
    {
        networkManager.LeaveRoom();
        StartCoroutine("LeavingRoom");
        Room.SetActive(false);
    }

    IEnumerator LeavingRoom()
    {
        yield return new WaitForSeconds(0.5f);
        Lobby.SetActive(true);


    }

    public void LeaveLobby()
    {
        networkManager.LeaveLobby();
        StartCoroutine("LeavingLobby");
        Lobby.SetActive(false);
    }

    IEnumerator LeavingLobby()
    {
        yield return new WaitForSeconds(0.5f);
        Title.SetActive(true);

    }

    private void ChangeMemberList()
    {
        RoomPlayerList.text = networkManager.RoomPlayerList;
    }


    public void roomSetup()//�����{�^���Ȃǂ̏�����
    {
        if (networkManager.IsHost)//�z�X�g��������
        {
            GameButtonText.text = "�Q�[���X�^�[�g";
            GameButtonText.color = Color.black;
        }
        else//�z�X�g����Ȃ�������
        {
            GameButtonText.text = "������";
            GameButtonText.color = Color.red;
        }
        ChangeMemberList();
    }

    public void PressButton()
    {
        if (networkManager.IsHost)//�z�X�g��������
        {
            networkManager.PressedStartButton();
        }
        else//�z�X�g����Ȃ�������
        {
            networkManager.PressedReadyButton();
            if (networkManager.IsReady)
            {
                GameButtonText.text = "��������";
                GameButtonText.color = Color.green;
            }
            else
            {
                GameButtonText.text = "������";
                GameButtonText.color = Color.red;
            }
        }
    }
}
