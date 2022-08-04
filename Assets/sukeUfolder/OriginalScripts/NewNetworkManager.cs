using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
//using hashtable = ExitGames.Client.Photon.Hashtable;

public class NewNetworkManager : MonoBehaviourPunCallbacks
{
  //�l�b�g���[�N�����𕪗������X�N���v�g

    public enum NetworkState
    {
        NotConnect,
        Lobby,
        Room,
    }

    public NetworkState currentState = NetworkState.NotConnect;




  //�}�b�`���C�L���O��
    [SerializeField]
    string Nickname;//�v���C���[�l�[��
    [SerializeField]
    public string RoomPlayerList { get; private set; }//���[���̖��O��
    public bool IsHost { get; private set; }//�z�X�g���ǂ���
    public bool IsReady { get; private set; }//���[���ҋ@���ŏ����������ǂ���

    public event System.Action  OnUpdateMember;//State�Ő��l�̕ω����󂯎�邽�߂�Action

    private ExitGames.Client.Photon.Hashtable hashtable;//�J�X�^���v���p�e�B�p

    //�Q�[����
    public int delayTime { get; private set; }
    public int limitTime { get; private set; }


    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += Loaded;
    }
    /// <summary>
    /// �T�[�o�[�ڑ��֌W
    /// </summary>
    public void ConnectSever()
    {
        PhotonNetwork.ConnectUsingSettings();//Photon�T�[�o�[�ɐڑ����邽�߂̊֐�
    }
    public override void OnConnectedToMaster()//�T�[�o�[�ɐڑ��ł������ɌĂ΂��֐�
    {
        PhotonNetwork.SendRate = 20; // 1�b�ԂɃ��b�Z�[�W���M���s����
        PhotonNetwork.SerializationRate = 10; // 1�b�ԂɃI�u�W�F�N�g�������s����
        PhotonNetwork.LocalPlayer.NickName = Nickname;
        currentState = NetworkState.Lobby;
    }

    /// <summary>
    /// ���r�[�֌W
    /// </summary>
    public void ConnectLobby()//Lobby�ɎQ�����鎞�ɌĂ�
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinLobby();
        }
    }
    /// <summary>
    /// �����ɓ��ꂽ�j�b�N�l�[���ɕς���
    /// </summary>
    /// <param name="str"></param>
    public void ChangeNickName(string str) => Nickname = str;

    public void LeaveLobby() => PhotonNetwork.Disconnect();

    /// �������烋�[���֌W
    public void CreateRoom1()
    {
        var room1Options = new RoomOptions();
        room1Options.MaxPlayers = 3;
        try
        {
            PhotonNetwork.JoinOrCreateRoom("room4", room1Options, TypedLobby.Default);//room������������V�������[�����쐬���ĎQ������
        }
        catch //�������쐬�o���Ȃ������Ƃ��̏���
        {
            Debug.Log("�����̍쐬�Ɏ��s���܂���");
        }
    }
    public void LeaveRoom()=>PhotonNetwork.LeaveRoom();



    void RefleshList()//�����̃����o�[���X�g���Ď擾
    {
        RoomPlayerList = "";//������̏�����
        foreach (var player in PhotonNetwork.PlayerList)//PhotonNetwork�Ńv���C���[���X�g�͎擾�o���Ă���̂ł������玝���Ă���
        {
            RoomPlayerList += player.NickName + "\n";
        }
       
    }

    public void CheckHost() //Host�������łȂ������m�F
    {
        if (PhotonNetwork.IsMasterClient)//�z�X�g��������
        {
            IsHost = true;
        }
        else//�z�X�g����Ȃ�������
        {
            IsHost = false;
        }

    }

    public override void OnJoinedRoom()//���[���ɎQ���ł������ɌĂ΂��
    {
        currentState = NetworkState.Room;
        roomSetUp();

    }

    /// <summary>
    /// supportLogger���I�[�o���C�h���ăv���C���[�̓��������m�������o�[���X�g�̍X�V���ł���悤�ɂ���
    /// </summary>
    public override void OnPlayerEnteredRoom(Player player)
    {
        Debug.Log(player.NickName + "�������ɓ����Ă�����");
        RefleshList();
        OnUpdateMember?.Invoke();
    }

    /// <summary>
    /// supportLogger���I�[�o���C�h���ăv���C���[�̑ޏo�����m�������o�[���X�g�̍X�V���ł���悤�ɂ���
    /// </summary>
    public override void OnPlayerLeftRoom(Player player)
    {
        Debug.Log(player.NickName + "���ޏo���Ă�������");
        RefleshList();
        OnUpdateMember?.Invoke();
    }

    public override void OnJoinRoomFailed(short returnCode, string message)//���[���ւ̎Q���Ɏ��s������
    {
        Debug.Log($"���[���ւ̎Q���Ɏ��s������: {message}");
    }
    
    public void GameStart()
    {
        if (!PhotonNetwork.IsMasterClient) { return; }//�}�X�^�[�N���C�A���g����Ȃ��Ȃ�return����
        photonView.RPC(nameof(SceneChange), RpcTarget.All);//�������[���̃����[�g�v���C���[���V�[���J�ڂ�����
    }

    //���M�҈ȊO���V�[���ړ������邽�߂�RPC���K�v
    [PunRPC]
    private void SceneChange()
    {
        PhotonNetwork.IsMessageQueueRunning = false;//�V�[�����ׂ����߈�x�T�[�o�[�ɏ��𑗂�̂��~����B������g������͑��߂ɉ������Ȃ��Ƃ����Ȃ�
        SceneManager.LoadScene("GameScene 1");
    }
    
    public void PressedReadyButton() //Ready�{�^���������ꂽ�Ƃ��̏���
    {
        if ((bool)hashtable["Ready"] == true)
        {
            hashtable["Ready"] = false;
            IsReady = false;
        }
        else
        {
            hashtable["Ready"] = true;
            IsReady = true;
        }
        // hashtable["Ready"] = ((bool)hashtable["Ready"] == true) ? false : true;
         PhotonNetwork.LocalPlayer.SetCustomProperties(hashtable);
    }
    public void PressedStartButton() //�Q�[���X�^�[�g�{�^���������ꂽ�Ƃ��̏���
    {
        hashtable["Ready"] = true;
        PhotonNetwork.LocalPlayer.SetCustomProperties(hashtable);
        if (CheckReady())GameStart();
    }
    
    public bool CheckReady()//�z�X�g�p�`�F�b�N
    {
        bool check = false;
        var allReady = true;
        //�S���������������̊m�F
        foreach (var player in PhotonNetwork.PlayerList)
        {
            if ((bool)player.CustomProperties["Ready"] == false)
            {
                Debug.Log("�����o�[���������ł��B");
                allReady = false;

            }
        }

        if (allReady)
        {
            check = true;
            Debug.Log("�Q�[���̏������ł��܂���!");
        }
        return check;
    }
    

    public void roomSetUp()
    {
        hashtable = new ExitGames.Client.Photon.Hashtable();//hashtable�̏�����
        if (PhotonNetwork.IsMasterClient)//�����̍쐬�傩�ǂ���
        {
            hashtable["Ready"] = true;//Ready�L�[��true�ɐݒ�
        }
        else
        {
            hashtable["Ready"] = false;//Ready�L�[��false�ɐݒ�
        }
        PhotonNetwork.LocalPlayer.SetCustomProperties(hashtable);
        RefleshList();
        CheckHost();
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        base.OnMasterClientSwitched(newMasterClient);
        if (PhotonNetwork.IsMasterClient)
        {
            hashtable["Ready"] = true;//Ready�L�[��true�ɐݒ�
        }
        else
        {
            hashtable["Ready"] = false;//Ready�L�[��false�ɐݒ�
        }
        PhotonNetwork.LocalPlayer.SetCustomProperties(hashtable);
        CheckHost();
    }

    private void Loaded(Scene nextScene, LoadSceneMode mode)
    {
        PhotonNetwork.IsMessageQueueRunning = true;//���b�Z�[�W�̑��M���J�n����
        if (nextScene.name == "GameScene 1")
        {
            GameSetUp();
        }

    }


    //�Q�[���V�[��
    public enum GameState
    {
        None,
        Ready,
        Playing,
        Replay,
        Finish,
    }
    public GameState currentGameState = GameState.None;

    public void GameSetUp()//�Q�[���̏�����
    {
        currentGameState = GameState.Ready;
        delayTime = unchecked(PhotonNetwork.ServerTimestamp);//0�b��ɗ����悤�ɂ��Ă���
        limitTime = unchecked(PhotonNetwork.ServerTimestamp + 9000);//9�b��ɏI��
    }

     

     void Update()
    {if (currentState != NetworkState.Room) return;
        switch (currentGameState)
        {
            case GameState.Ready:
                if (unchecked(PhotonNetwork.ServerTimestamp - delayTime) > 0)//�T�[�o�[���Ԃ���delayTime�b��Ɏn�߂�
                {
                    PhotonNetwork.Instantiate("PlayerMagnet", new Vector2(0f, 0f), Quaternion.identity);//magnet�𐶐�����
                 
                    currentGameState = GameState.Playing;
                }
                break;
            case GameState.Playing:
                if (unchecked(PhotonNetwork.ServerTimestamp - limitTime) > 0)//�I�����Ԃ𖞂����Ă�����
                {
                    Debug.Log("���v���C���");
                    currentGameState = GameState.Replay;
                }
                    break;
            case GameState.Replay:
        
                if (unchecked(PhotonNetwork.ServerTimestamp - (limitTime+9000)) > 0)//���v���C�I�����Ԃ𖞂����Ă�����
                {
                    currentGameState = GameState.Finish;
                }
                break;
            case GameState.Finish:
                ReturnCall();
                currentGameState = GameState.None;
                break;

        }
          
           
        
    }

    public void ReturnCall()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC(nameof(ReturnRoom), RpcTarget.All);

            PhotonNetwork.DestroyAll();
        }
    }

        [PunRPC]
    public void ReturnRoom()
    {
        currentState = NetworkState.Room;
        SceneManager.LoadScene("MatchMaking 1");
        PhotonNetwork.IsMessageQueueRunning = false;//���b�Z�[�W�̑��M���~����

    }
    

}