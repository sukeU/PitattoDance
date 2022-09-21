using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;
//using hashtable = ExitGames.Client.Photon.Hashtable;

public class NetworkManager : MonoBehaviourPunCallbacks
{

    [SerializeField]
    TMP_InputField inputField;//プレイヤーネームの入力欄
    [SerializeField]
    TextMeshProUGUI Nickname;//プレイヤーネーム
    [SerializeField]
    GameObject GameButtonObj; //ゲームスタートボタンのオブジェクト
    private TextMeshProUGUI GameButtonText;
    private bool _isReady;
    /// <summary>
    /// セットアクティブをスクリプト上で出来るようにするために必要
    /// </summary>
    [SerializeField]
    GameObject Title;//タイトルオブジェクト
    [SerializeField]
    GameObject Lobby;//ロビーオブジェクト
    [SerializeField]
    GameObject Room;//ルームオブジェクト
    [SerializeField]
    TextMeshProUGUI NameText;//ルームの名前欄を更新するために必要
   /* [SerializeField]
    TextMeshProUGUI ReadyText;//ルーム参加した人の準備ができているかどうか
   */
    private ExitGames.Client.Photon.Hashtable hashtable;//カスタムプロパティ用
    private const string KeyStartTime = "StartTime"; // ゲーム開始時刻のキーの文字列


    private void Start()
    {
       // SoundManager.Instance.PlayBgmByName("gamesound");//曲入れた時に使う
        inputField = inputField.GetComponent<TMP_InputField>();//InputFieldの情報を取得
        GameButtonText = GameButtonObj.GetComponentInChildren<TextMeshProUGUI>();
        if (PhotonNetwork.IsConnected)
        {
            foreach (var player in PhotonNetwork.PlayerList)
            {
                if (PhotonNetwork.LocalPlayer == player)
                {
                    PhotonNetwork.LocalPlayer.NickName = player.NickName;
                }
            }
            PhotonNetwork.IsMessageQueueRunning = true;//メッセージの送信を開始する
            Title.SetActive(false);
            roomSetUp();
        }
        else
        {
            Nickname = Nickname.GetComponent<TextMeshProUGUI>();//Textのコンポーネントを取得
        }
        _isReady = false;
    }

    public void PUN2Connect()
    {
        SoundManager.Instance.PlaySeByName("DecisionA");
        Title.SetActive(false);
        PhotonNetwork.ConnectUsingSettings();//Photonサーバーに接続するための関数
        PhotonNetwork.SendRate = 20; // 1秒間にメッセージ送信を行う回数
        PhotonNetwork.SerializationRate = 10; // 1秒間にオブジェクト同期を行う回数
    }

    public override void OnConnectedToMaster()//サーバーに接続できた時に呼ばれる関数
    {
        PhotonNetwork.LocalPlayer.NickName = Nickname.text;
        JoinLobby();
    }

    public void ChangeNickname()
    {
        //NicknameにinputFieldの内容を反映
        Nickname.text = inputField.text;
    }

    /// <summary>
    /// ここからロビー関係
    /// </summary>
    private void JoinLobby()
    {
        if (PhotonNetwork.IsConnected)//PhtonNetworkに接続できていたら
        {
            PhotonNetwork.JoinLobby();
        }
    }

    public override void OnJoinedLobby()//Lobbyに入った時の呼ばれる関数
    {
        Lobby.SetActive(true);
        Debug.Log("ロビーに入れたよ");
    }

    public void ClickLeaveLobby()//buttonで呼ばれることが前提のオブジェクト
    {
        SoundManager.Instance.PlaySeByName("DecisionC");
        PhotonNetwork.LeaveLobby();
        Lobby.SetActive(false);
    }

    public override void OnLeftLobby()//ロビーに戻った時に呼ばれる
    {
        PhotonNetwork.Disconnect();//PhotonNetworkから切断する
        Title.SetActive(true);
        Debug.Log("ロビーから戻ったよ");
    }
    /// <summary>
    /// ここからルーム関係
    /// </summary>
    public void ClickGoToRoom1()//Room1は最大3人の部屋
    {
        SoundManager.Instance.PlaySeByName("DecisionA");
        var room1Options = new RoomOptions();
        room1Options.MaxPlayers = 3;
        string displayName = $"{PhotonNetwork.NickName}の部屋";
        PhotonNetwork.JoinOrCreateRoom("room1", GameRoomProperty.CreateRoomOptions(displayName), TypedLobby.Default);//roomが無かったら新しくルームを作成して参加する
        Lobby.SetActive(false);
    }

    public void ClickGoToRoom2()//Room2は最大4人の部屋
    {
        SoundManager.Instance.PlaySeByName("DecisionA");
        var room2Options = new RoomOptions();
        room2Options.MaxPlayers = 4;
        string displayName = $"{PhotonNetwork.NickName}の部屋";
        PhotonNetwork.JoinOrCreateRoom("room2", GameRoomProperty.CreateRoomOptions(displayName), TypedLobby.Default);//roomが無かったら新しくルームを作成して参加する
        Lobby.SetActive(false);
    }

    public void ClickGoToRoom3()//Room3は最大5人の部屋
    {
        SoundManager.Instance.PlaySeByName("DecisionA");
        var room2Options = new RoomOptions();
        string displayName = $"{PhotonNetwork.NickName}の部屋";
        PhotonNetwork.JoinOrCreateRoom("room3", GameRoomProperty.CreateRoomOptions(displayName), TypedLobby.Default);//roomが無かったら新しくルームを作成して参加する
        Lobby.SetActive(false);
    }

    public void ClickLeaveRoom()
    {
        SoundManager.Instance.PlaySeByName("DecisionC");
        Debug.Log("部屋から出るよ");
        // ルームから退出する
        PhotonNetwork.LeaveRoom();//リロード以降これが呼ばれなくなる？？？？？ 原因：PhotonNetwork.IsMessageQueueRunning = true;がなかった
        Room.SetActive(false);
    }

    public override void OnLeftRoom()
    {
        Lobby.SetActive(true);
        Debug.Log("ルームから戻ったよ");
    }

    // ルームリストが更新された時に呼ばれるコールバック
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (var info in roomList)
        {
            if (!info.RemovedFromList)
            {
                Debug.Log($"ルーム更新: {info.Name}({info.PlayerCount}/{info.MaxPlayers})");
            }
            else
            {
                Debug.Log($"ルーム削除: {info.Name}");
            }
        }
    }

    public override void OnJoinedRoom()//ルームに参加できた時に呼ばれる
    {
        roomSetUp();
    }

    /// <summary>
    /// supportLoggerをオーバライドしてプレイヤーの入室を検知かつメンバーリストの更新をできるようにした
    /// </summary>
    public override void OnPlayerEnteredRoom(Player player)
    {
        Debug.Log(player.NickName + "が部屋に入ってきたよ");
        ChangeMemberList();
    }

    /// <summary>
    /// supportLoggerをオーバライドしてプレイヤーの退出を検知かつメンバーリストの更新をできるようにした
    /// </summary>
    public override void OnPlayerLeftRoom(Player player)
    {
        Debug.Log(player.NickName + "が退出していったよ");
        ChangeMemberList();
    }

    public void ChangeMemberList()
    {
        NameText.text = "";//文字列の初期化
        foreach (var player in PhotonNetwork.PlayerList)//PhotonNetworkでプレイヤーリストは取得出来ているのでそこから持ってくる
        {
            NameText.text += player.NickName + "\n";
        }
    }
   
    public override void OnJoinRoomFailed(short returnCode, string message)//ルームへの参加に失敗した時
    {
        Lobby.SetActive(true);
        Debug.Log($"ルームへの参加に失敗したよ: {message}");
    }

    public void GameStart()
    {
        if (!PhotonNetwork.IsMasterClient) { return; }//マスタークライアントじゃないならreturnする
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.SetStartTime(PhotonNetwork.ServerTimestamp);
        }
        Debug.Log("ゲームスタート");
        //↓これが見つからないらしい…。原因はPhotonViewスクリプトがついてないことによるエラーでした
        photonView.RPC(nameof(SceneChange), RpcTarget.All);//同じルームのリモートプレイヤーをシーン遷移させる
    }

    //送信者以外もシーン移動させるためにRPCが必要
    [PunRPC]
    private void SceneChange()
    {
        SoundManager.Instance.PlaySeByName("DecisionD");
        PhotonNetwork.IsMessageQueueRunning = false;//シーンを跨ぐため一度サーバーに情報を送るのを停止する。これを使った後は早めに解除しないといけない
        SceneManager.LoadScene("GameScene");
        //SceneManager.LoadScene("Tester");
    }

    public void ReadyButton() //Readyボタン
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            hashtable["Ready"] = ((bool)hashtable["Ready"] == true) ? false : true;
            GameButtonText.text = ((bool)hashtable["Ready"] == true) ? "準備完了" : "準備中";
            GameButtonText.color = ((bool)hashtable["Ready"] == true) ? Color.green : Color.red;
            PhotonNetwork.LocalPlayer.SetCustomProperties(hashtable);
            //SoundManager.Instance.PlaySeByName();//効果音が決まったら
        }
        else
        {
            hashtable["Ready"] = true;
            GameButtonText.color = Color.black;
            PhotonNetwork.LocalPlayer.SetCustomProperties(hashtable);
            if (CheckReady())
                GameStart();
            //SoundManager.Instance.PlaySeByName();//効果音が決まったら
        }
    }

    public bool CheckReady()//ホスト用チェック
    {
        bool check=false;
        var allReady = true;
        //全員が準備完了かの確認
        foreach (var player in PhotonNetwork.PlayerList)
        {
            
            if ((bool)player.CustomProperties["Ready"] == false)
            {
                Debug.Log("メンバーが準備中です。");
                allReady = false;
               
            }
        }
        Debug.Log("ゲームの準備ができました!");

        if (allReady)
        {
            check = true;
        }
        return check;
    }

    public void roomSetUp()
    {
        hashtable = new ExitGames.Client.Photon.Hashtable();//hashtableの初期化
        if (PhotonNetwork.IsMasterClient)//部屋の作成主かどうか
        {
            hashtable["Ready"] = true;//Readyキーをtrueに設定
        }
        else
        {
            hashtable["Ready"] = false;//Readyキーをfalseに設定
        }
        PhotonNetwork.LocalPlayer.SetCustomProperties(hashtable);
        GameButtonText.text = PhotonNetwork.IsMasterClient ? "ゲームスタート" : "準備中";//ゲームボタンのテキストを変更
        GameButtonText.color = PhotonNetwork.IsMasterClient ? Color.black : Color.red;
        ChangeMemberList();
        Room.SetActive(true);

    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        base.OnMasterClientSwitched(newMasterClient);
        GameButtonText.text = PhotonNetwork.IsMasterClient ? "ゲームスタート" : "準備中";//ゲームボタンのテキストを変更
        GameButtonText.color = PhotonNetwork.IsMasterClient ? Color.black : Color.red;
        if (PhotonNetwork.IsMasterClient)
        {
            hashtable["Ready"] = true;//Readyキーをtrueに設定
        }
        else
        {
            hashtable["Ready"] = false;//Readyキーをfalseに設定
        }
        PhotonNetwork.LocalPlayer.SetCustomProperties(hashtable);
    }

}
