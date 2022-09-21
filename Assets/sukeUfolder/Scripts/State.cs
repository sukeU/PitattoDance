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
    [SerializeField] GameObject loadingText;
    [SerializeField]
    GameObject RoomDollObjects; //���[���I�u�W�F�N�g�̐l�`�Ɣw�i
    private TextMeshProUGUI GameButtonText;

    void Start()
    {
        SoundManager.Instance.PlayBgmByName("Title");
        networkManager =GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<NewNetworkManager>();
        inputField = inputField.GetComponent<TMP_InputField>();//InputField�̏����擾
        GameButtonText = GameButton.GetComponentInChildren<TextMeshProUGUI>();
        networkManager.OnUpdateMember += roomSetup;
        networkManager.OnJoinLobby += GoRoom1;
        networkManager.OnJoinRoom += GoingRoom;
        networkManager.OnLeftRoomA += LeavingRoom;
        if (networkManager.currentState == NewNetworkManager.NetworkState.Room)
        {
            Title.SetActive(false);
            Room.SetActive(true);
            RoomDollObjects.SetActive(true);
            networkManager.roomSetUp();//�l�b�g���[�N���̕����ݒ�
            roomSetup();//���̃X�N���v�g�̃��[�����Z�b�g����
        }
    }
    public void GoLobby()//�^�C�g����ʂ̓�������������ɌĂ�
    {
        SoundManager.Instance.PlaySeByName("DecisionA");
        nickname = inputField.text;
        networkManager.ChangeNickName(nickname);
        networkManager.ConnectSever();
        Title.SetActive(false);
        loadingText.SetActive(true);
        StartCoroutine("GoingLobby");
    }

    private void OnDisable()
    {
        networkManager.OnUpdateMember -= roomSetup;
        networkManager.OnJoinLobby -= GoRoom1;
        networkManager.OnJoinRoom -= GoingRoom;
        networkManager.OnLeftRoomA -= LeavingRoom;
    }

    IEnumerator GoingLobby()//�ڑ�����܂ő҂�
    {
        yield return new WaitForSeconds(3.0f);
       // Lobby.SetActive(true);
    }

    public void GoRoom1()//���[���ɍs��
    {
        //SoundManager.Instance.PlaySeByName("DecisionA");
        networkManager.CreateRoom1();
       // Lobby.SetActive(false);
    }

    public void GoRoom2()//���[���ɍs��
    {
        SoundManager.Instance.PlaySeByName("DecisionA");
        networkManager.CreateRoom2();
        StartCoroutine("GoingRoom");
        Lobby.SetActive(false);
    }

    public void GoRoom3()//���[���ɍs��
    {
        SoundManager.Instance.PlaySeByName("DecisionA");
        networkManager.CreateRoom3();
        StartCoroutine("GoingRoom");
        Lobby.SetActive(false);
    }
    void GoingRoom()//�l�b�g���[�N�̒l�̎󂯓n���ɃR���[�`�����g��Ȃ���΂Ȃ�Ȃ�
    {
        networkManager.roomSetUp();//�l�b�g���[�N���̕����ݒ�
        roomSetup();
        Room.SetActive(true);
        loadingText.SetActive(false);
        RoomDollObjects.SetActive(true);
    }

    public void LeaveRoom()
    {
        SoundManager.Instance.PlaySeByName("DecisionC");
        networkManager.LeaveRoom();
        StartCoroutine("LeavingRoom");
        Room.SetActive(false);
        RoomDollObjects.SetActive(false);
        loadingText.SetActive(true);
    }

    public void LeavingRoom()
    {
       // Lobby.SetActive(true);
        LeaveLobby();
    }

    public void LeaveLobby()
    {
        //SoundManager.Instance.PlaySeByName("DecisionC");
        networkManager.LeaveLobby();
        StartCoroutine("LeavingLobby");
        //Lobby.SetActive(false);
    }

    IEnumerator LeavingLobby()
    {
        yield return new WaitForSeconds(0.5f);
        Title.SetActive(true);
        loadingText.SetActive(false);
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
            GameButtonText.color = Color.white;
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
