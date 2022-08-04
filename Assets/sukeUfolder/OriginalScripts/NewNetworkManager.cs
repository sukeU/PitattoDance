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
  //ネットワーク部分を分離したスクリプト

    public enum NetworkState
    {
        NotConnect,
        Lobby,
        Room,
    }

    public NetworkState currentState = NetworkState.NotConnect;




  //マッチメイキング編
    [SerializeField]
    string Nickname;//プレイヤーネーム
    [SerializeField]
    public string RoomPlayerList { get; private set; }//ルームの名前欄
    public bool IsHost { get; private set; }//ホストかどうか
    public bool IsReady { get; private set; }//ルーム待機中で準備完了かどうか

    public event System.Action  OnUpdateMember;//Stateで数値の変化を受け取るためのAction

    private ExitGames.Client.Photon.Hashtable hashtable;//カスタムプロパティ用

    //ゲーム編
    public int delayTime { get; private set; }
    public int limitTime { get; private set; }


    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += Loaded;
    }
    /// <summary>
    /// サーバー接続関係
    /// </summary>
    public void ConnectSever()
    {
        PhotonNetwork.ConnectUsingSettings();//Photonサーバーに接続するための関数
    }
    public override void OnConnectedToMaster()//サーバーに接続できた時に呼ばれる関数
    {
        PhotonNetwork.SendRate = 20; // 1秒間にメッセージ送信を行う回数
        PhotonNetwork.SerializationRate = 10; // 1秒間にオブジェクト同期を行う回数
        PhotonNetwork.LocalPlayer.NickName = Nickname;
        currentState = NetworkState.Lobby;
    }

    /// <summary>
    /// ロビー関係
    /// </summary>
    public void ConnectLobby()//Lobbyに参加する時に呼ぶ
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinLobby();
        }
    }
    /// <summary>
    /// 引数に入れたニックネームに変える
    /// </summary>
    /// <param name="str"></param>
    public void ChangeNickName(string str) => Nickname = str;

    public void LeaveLobby() => PhotonNetwork.Disconnect();

    /// ここからルーム関係
    public void CreateRoom1()
    {
        var room1Options = new RoomOptions();
        room1Options.MaxPlayers = 3;
        try
        {
            PhotonNetwork.JoinOrCreateRoom("room4", room1Options, TypedLobby.Default);//roomが無かったら新しくルームを作成して参加する
        }
        catch //部屋を作成出来なかったときの処理
        {
            Debug.Log("部屋の作成に失敗しました");
        }
    }
    public void LeaveRoom()=>PhotonNetwork.LeaveRoom();



    void RefleshList()//部屋のメンバーリストを再取得
    {
        RoomPlayerList = "";//文字列の初期化
        foreach (var player in PhotonNetwork.PlayerList)//PhotonNetworkでプレイヤーリストは取得出来ているのでそこから持ってくる
        {
            RoomPlayerList += player.NickName + "\n";
        }
       
    }

    public void CheckHost() //Hostかそうでないかを確認
    {
        if (PhotonNetwork.IsMasterClient)//ホストだったら
        {
            IsHost = true;
        }
        else//ホストじゃなかったら
        {
            IsHost = false;
        }

    }

    public override void OnJoinedRoom()//ルームに参加できた時に呼ばれる
    {
        currentState = NetworkState.Room;
        roomSetUp();

    }

    /// <summary>
    /// supportLoggerをオーバライドしてプレイヤーの入室を検知かつメンバーリストの更新をできるようにした
    /// </summary>
    public override void OnPlayerEnteredRoom(Player player)
    {
        Debug.Log(player.NickName + "が部屋に入ってきたよ");
        RefleshList();
        OnUpdateMember?.Invoke();
    }

    /// <summary>
    /// supportLoggerをオーバライドしてプレイヤーの退出を検知かつメンバーリストの更新をできるようにした
    /// </summary>
    public override void OnPlayerLeftRoom(Player player)
    {
        Debug.Log(player.NickName + "が退出していったよ");
        RefleshList();
        OnUpdateMember?.Invoke();
    }

    public override void OnJoinRoomFailed(short returnCode, string message)//ルームへの参加に失敗した時
    {
        Debug.Log($"ルームへの参加に失敗したよ: {message}");
    }
    
    public void GameStart()
    {
        if (!PhotonNetwork.IsMasterClient) { return; }//マスタークライアントじゃないならreturnする
        photonView.RPC(nameof(SceneChange), RpcTarget.All);//同じルームのリモートプレイヤーをシーン遷移させる
    }

    //送信者以外もシーン移動させるためにRPCが必要
    [PunRPC]
    private void SceneChange()
    {
        PhotonNetwork.IsMessageQueueRunning = false;//シーンを跨ぐため一度サーバーに情報を送るのを停止する。これを使った後は早めに解除しないといけない
        SceneManager.LoadScene("GameScene 1");
    }
    
    public void PressedReadyButton() //Readyボタンが押されたときの処理
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
    public void PressedStartButton() //ゲームスタートボタンが押されたときの処理
    {
        hashtable["Ready"] = true;
        PhotonNetwork.LocalPlayer.SetCustomProperties(hashtable);
        if (CheckReady())GameStart();
    }
    
    public bool CheckReady()//ホスト用チェック
    {
        bool check = false;
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

        if (allReady)
        {
            check = true;
            Debug.Log("ゲームの準備ができました!");
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
        RefleshList();
        CheckHost();
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        base.OnMasterClientSwitched(newMasterClient);
        if (PhotonNetwork.IsMasterClient)
        {
            hashtable["Ready"] = true;//Readyキーをtrueに設定
        }
        else
        {
            hashtable["Ready"] = false;//Readyキーをfalseに設定
        }
        PhotonNetwork.LocalPlayer.SetCustomProperties(hashtable);
        CheckHost();
    }

    private void Loaded(Scene nextScene, LoadSceneMode mode)
    {
        PhotonNetwork.IsMessageQueueRunning = true;//メッセージの送信を開始する
        if (nextScene.name == "GameScene 1")
        {
            GameSetUp();
        }

    }


    //ゲームシーン
    public enum GameState
    {
        None,
        Ready,
        Playing,
        Replay,
        Finish,
    }
    public GameState currentGameState = GameState.None;

    public void GameSetUp()//ゲームの初期化
    {
        currentGameState = GameState.Ready;
        delayTime = unchecked(PhotonNetwork.ServerTimestamp);//0秒後に流れるようにしてある
        limitTime = unchecked(PhotonNetwork.ServerTimestamp + 9000);//9秒後に終了
    }

     

     void Update()
    {if (currentState != NetworkState.Room) return;
        switch (currentGameState)
        {
            case GameState.Ready:
                if (unchecked(PhotonNetwork.ServerTimestamp - delayTime) > 0)//サーバー時間からdelayTime秒後に始める
                {
                    PhotonNetwork.Instantiate("PlayerMagnet", new Vector2(0f, 0f), Quaternion.identity);//magnetを生成する
                 
                    currentGameState = GameState.Playing;
                }
                break;
            case GameState.Playing:
                if (unchecked(PhotonNetwork.ServerTimestamp - limitTime) > 0)//終了時間を満たしていたら
                {
                    Debug.Log("リプレイ状態");
                    currentGameState = GameState.Replay;
                }
                    break;
            case GameState.Replay:
        
                if (unchecked(PhotonNetwork.ServerTimestamp - (limitTime+9000)) > 0)//リプレイ終了時間を満たしていたら
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
        PhotonNetwork.IsMessageQueueRunning = false;//メッセージの送信を停止する

    }
    

}