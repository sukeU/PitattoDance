using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class State : MonoBehaviour
{
    ///このスクリプト自体はネットワークから切り離して動作させる
    [SerializeField]
    NewNetworkManager networkManager;
    [SerializeField]
    TMP_InputField inputField;//プレイヤーネームの入力欄
    [SerializeField]
    string nickname;//プレイヤーネーム
    [Space]
    [SerializeField]
    GameObject Title;//タイトルオブジェクト
    [SerializeField]
    GameObject Lobby;//ロビーオブジェクト
    [SerializeField]
    GameObject Room;//ルームオブジェクト
    [SerializeField]
    TextMeshProUGUI RoomPlayerList;//ルームの名前欄を更新するために必要
    [SerializeField]
    GameObject GameButton; //ゲームボタンのオブジェクト
    [SerializeField] GameObject loadingText;
    [SerializeField]
    GameObject RoomDollObjects; //ルームオブジェクトの人形と背景
    private TextMeshProUGUI GameButtonText;

    void Start()
    {
        SoundManager.Instance.PlayBgmByName("Title");
        networkManager =GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<NewNetworkManager>();
        inputField = inputField.GetComponent<TMP_InputField>();//InputFieldの情報を取得
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
            networkManager.roomSetUp();//ネットワーク側の部屋設定
            roomSetup();//このスクリプトのルームをセットする
        }
    }
    public void GoLobby()//タイトル画面の入場を押した時に呼ぶ
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

    IEnumerator GoingLobby()//接続するまで待つ
    {
        yield return new WaitForSeconds(3.0f);
       // Lobby.SetActive(true);
    }

    public void GoRoom1()//ルームに行く
    {
        //SoundManager.Instance.PlaySeByName("DecisionA");
        networkManager.CreateRoom1();
       // Lobby.SetActive(false);
    }

    public void GoRoom2()//ルームに行く
    {
        SoundManager.Instance.PlaySeByName("DecisionA");
        networkManager.CreateRoom2();
        StartCoroutine("GoingRoom");
        Lobby.SetActive(false);
    }

    public void GoRoom3()//ルームに行く
    {
        SoundManager.Instance.PlaySeByName("DecisionA");
        networkManager.CreateRoom3();
        StartCoroutine("GoingRoom");
        Lobby.SetActive(false);
    }
    void GoingRoom()//ネットワークの値の受け渡しにコルーチンを使わなければならない
    {
        networkManager.roomSetUp();//ネットワーク側の部屋設定
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


    public void roomSetup()//準備ボタンなどの初期化
    {
        if (networkManager.IsHost)//ホストだったら
        {
            GameButtonText.text = "ゲームスタート";
            GameButtonText.color = Color.white;
        }
        else//ホストじゃなかったら
        {
            GameButtonText.text = "準備中";
            GameButtonText.color = Color.red;
        }
        ChangeMemberList();
    }

    public void PressButton()
    {
        if (networkManager.IsHost)//ホストだったら
        {
            networkManager.PressedStartButton();
        }
        else//ホストじゃなかったら
        {
            networkManager.PressedReadyButton();
            if (networkManager.IsReady)
            {
                GameButtonText.text = "準備完了";
                GameButtonText.color = Color.green;
            }
            else
            {
                GameButtonText.text = "準備中";
                GameButtonText.color = Color.red;
            }
        }
    }
}
