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
    private TextMeshProUGUI GameButtonText;

    


    void Start()
    {
        networkManager =GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<NewNetworkManager>();
        inputField = inputField.GetComponent<TMP_InputField>();//InputFieldの情報を取得
        GameButtonText = GameButton.GetComponentInChildren<TextMeshProUGUI>();
        networkManager.OnUpdateMember += roomSetup;

        if (networkManager.currentState == NewNetworkManager.NetworkState.Room)
        {
            Title.SetActive(false);
            Room.SetActive(true);
            networkManager.roomSetUp();//ネットワーク側の部屋設定
            roomSetup();//このスクリプトのルームをセットする
        }
    }
    public void GoLobby()//タイトル画面の入場を押した時に呼ぶ
    {
        nickname = inputField.text;
        networkManager.ChangeNickName(nickname);
        networkManager.ConnectSever();
        Title.SetActive(false);
        StartCoroutine("GoingLobby");

    }

    IEnumerator GoingLobby()//接続するまで待つ
    {
        yield return new WaitForSeconds(0.5f);
        Lobby.SetActive(true);
    }

    public void GoRoom()//ルームに行く
    {
        networkManager.CreateRoom1();
        StartCoroutine("GoingRoom");
        Lobby.SetActive(false);
    }
    IEnumerator GoingRoom()//ネットワークの値の受け渡しにコルーチンを使わなければならない
    {
        yield return new WaitForSeconds(0.5f);
        networkManager.roomSetUp();//ネットワーク側の部屋設定
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


    public void roomSetup()//準備ボタンなどの初期化
    {
        if (networkManager.IsHost)//ホストだったら
        {
            GameButtonText.text = "ゲームスタート";
            GameButtonText.color = Color.black;
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
