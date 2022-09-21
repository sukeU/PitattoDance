using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

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

    public event System.Action OnUpdateMember;//Stateで数値の変化を受け取るためのAction
    public event System.Action OnJoinLobby;//Stateでロビーに参加できたかどうかを受け取るためのAction
    public event System.Action OnJoinRoom;//Stateで部屋に参加できたかどうかを受け取るためのAction
    public event System.Action OnLeftRoomA;//Stateで部屋から退出できたかどうかを受け取るためのAction


    private ExitGames.Client.Photon.Hashtable hashtable;//カスタムプロパティ用
                                                        //ゲームシーン編

    const string gameScene = "GameScene";//すぐ書き換えられるようにした

    public bool IsReplay { get; private set; }//ゲームがリプレイ中かどうか

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
        if (currentGameState == GameState.Finish)
        {
            currentState = NetworkState.Room;
        }
        else
        {
            currentState = NetworkState.Lobby;
            ConnectLobby();
        }
        
      
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

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        if(currentState == NetworkState.Lobby)
        {
            OnJoinLobby.Invoke();
        }
        else
        {
            OnLeftRoomA.Invoke();
        }

    }
    /// <summary>
    /// 引数に入れたニックネームに変える
    /// </summary>
    /// <param name="str"></param>
    public void ChangeNickName(string str) => Nickname = str;

    public void LeaveLobby()
    {
        PhotonNetwork.Disconnect();
    }

    /// ここからルーム関係
    public void CreateRoom1()
    {
        var room1Options = new RoomOptions();
        room1Options.MaxPlayers = 3;
        try
        {
            PhotonNetwork.JoinOrCreateRoom("room1", room1Options, TypedLobby.Default);//roomが無かったら新しくルームを作成して参加する
        }
        catch //部屋を作成出来なかったときの処理
        {
            Debug.Log("部屋の作成に失敗しました");
        }
    }
    public void CreateRoom2()
    {
        var room2Options = new RoomOptions();
        room2Options.MaxPlayers = 3;
        try
        {
            PhotonNetwork.JoinOrCreateRoom("room2", room2Options, TypedLobby.Default);//roomが無かったら新しくルームを作成して参加する
        }
        catch //部屋を作成出来なかったときの処理
        {
            Debug.Log("部屋の作成に失敗しました");
        }
    }

    public void CreateRoom3()
    {
        var room2Options = new RoomOptions();
        room2Options.MaxPlayers = 3;
        try
        {
            PhotonNetwork.JoinOrCreateRoom("room3", room2Options, TypedLobby.Default);//roomが無かったら新しくルームを作成して参加する
        }
        catch //部屋を作成出来なかったときの処理
        {
            Debug.Log("部屋の作成に失敗しました");
        }
    }
    public void LeaveRoom() {
        PhotonNetwork.LeaveRoom();
    }

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
        OnJoinRoom.Invoke();
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
        SceneManager.LoadScene(gameScene);
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
        if (CheckReady()) GameStart();
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
        if (nextScene.name == gameScene)
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

    public enum PoseType  //現れるポーズタイプ
    {
        None,
        Banzai,
        GrandPa_L,
        GrandPa_R,
        HandWaving_L,
        HandWaving_R,
        High_hat,
        InaBauer_L,
        InaBauer_R,
        Muscle,
        SlantingT_L,
        SlantingT_R,
        Sye_L,
        Sye_R,
        T,
    }
    public PoseType currentPose = PoseType.None;//ホスト側で設定する
    public int score { get; private set; } = 0;
    public int combo { get; private set; } //ゲーム中のコンボ数
    public int MaxCombo { get; private set; } //最大コンボ数
    public event System.Action OnUpdateScore;//GamePlayManagerでスコアの変化を受け取るためのAction
    public bool IsRunning=false;
    public bool SyncIsRunning = false;
    public int currentTime { get; private set; }
    public int delayTime { get; private set; }
    public int limitTime { get; private set; }
    public int poseTime { get; private set; }//ポーズが表示されている時間
    private PoseType previousPose;
    public bool matchPose = false;//ポーズを取れたかどうか
    private bool IsCooldown = false;//ポーズをセットするまでのクールダウン
    private bool IsReturning = false;
    public void GameSetUp()//ゲームの初期化
    {
        currentGameState = GameState.Ready;
        delayTime = unchecked(PhotonNetwork.ServerTimestamp + 5000);//5秒後に流れるようにしてある
        limitTime = unchecked(PhotonNetwork.ServerTimestamp + 65000);//30秒+5秒後に終了
        score = 0;
        combo = 0;
        MaxCombo = 0;
        IsCooldown = false;
        if (PhotonNetwork.IsMasterClient)
        {
            hashtable["Match"] = false;
            hashtable["Score"] = score;
            hashtable["Combo"] = combo;
            hashtable["MCombo"] = MaxCombo;
            PhotonNetwork.CurrentRoom.SetCustomProperties(hashtable);
        }
        SoundManager.Instance.StopBgm();
        SoundManager.Instance.PlaySeByName("DecisionD");
        SoundManager.Instance.PlaySeByName("Start");
    }

    void SetPropertiesPose()//ホスト側でポーズを指定する
    {
        currentPose = (PoseType)Random.Range(0, 15);
        hashtable["Pose"] = currentPose;
        PhotonNetwork.CurrentRoom.SetCustomProperties(hashtable);
    }

    PoseType GetPropertiesPose()//ホスト以外がポーズを取得する
    {
        if (PhotonNetwork.CurrentRoom.CustomProperties["Pose"] == null)
        {
            return PoseType.None;
        }
        else
        {
            return (PoseType)PhotonNetwork.CurrentRoom.CustomProperties["Pose"];
        }
    }

    public void UpdateScore(int value)
    {
        if (PhotonNetwork.IsMasterClient)//ホストがカスタムプロパティを利用してスコアを更新する
        {
            score = value;
            hashtable["Score"] = score;
            hashtable["Combo"] = combo;
            if (MaxCombo < combo)MaxCombo = combo;
            hashtable["MCombo"] = MaxCombo;
            PhotonNetwork.CurrentRoom.SetCustomProperties(hashtable);
        }
        else//それ以外はスコアを取得する　ここは遅延によって値がおかしくなるかもしれない
        {
            score = (int)PhotonNetwork.CurrentRoom.CustomProperties["Score"];
            combo = (int)PhotonNetwork.CurrentRoom.CustomProperties["Combo"];
            MaxCombo= (int)PhotonNetwork.CurrentRoom.CustomProperties["MCombo"];
        }
    }

    public void MatchPose()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            hashtable["Match"] = true;
            PhotonNetwork.CurrentRoom.SetCustomProperties(hashtable);
        }
        OnUpdateScore.Invoke();//スコアのActionを受け取る
    }
    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        if (currentGameState== GameState.Playing)
        {
             OnUpdateScore.Invoke();//スコアのActionを受け取る
            if (matchPose != (bool)PhotonNetwork.CurrentRoom.CustomProperties["Match"])
            {
                matchPose=(bool)PhotonNetwork.CurrentRoom.CustomProperties["Match"];
            }
        }
    }
    void Update()
    {
        if (currentState != NetworkState.Room) return;
        switch (currentGameState)
        {
            case GameState.Ready:
                IsReplay = false;
                if (unchecked(PhotonNetwork.ServerTimestamp - delayTime) > 0)//サーバー時間からdelayTime秒後に始める
                {
                    PhotonNetwork.Instantiate("PlayerMagnet", new Vector2(0f, 0f), Quaternion.identity);//magnetを生成する
                    currentGameState = GameState.Playing;
                    SoundManager.Instance.PlayBgmByName("BGM");
                }
                break;
            case GameState.Playing:
                currentTime = unchecked(PhotonNetwork.ServerTimestamp);
                if (currentPose == PoseType.None)
                {
                    if (!IsCooldown)//クールダウン中じゃなかったら
                    {
                        IsCooldown = true;//クールダウンにする
                        StartCoroutine("SetPose");
                    }
                    poseTime = unchecked(PhotonNetwork.ServerTimestamp + 10000);//10秒後に終了時間をセット
                }
                else
                {
                        StartCoroutine("SyncScore");
                    if (!IsCooldown&&!PhotonNetwork.IsMasterClient)//クールダウン中じゃなかったら
                    {
                        IsCooldown = true;//クールダウンにする
                        StartCoroutine("SetPose");
                    }
                    if (matchPose)//ポーズを決めたかどうか
                    {
                        if (PhotonNetwork.IsMasterClient)
                        {
                            StartCoroutine("SetScore");
                            hashtable["Match"] = false;
                            PhotonNetwork.CurrentRoom.SetCustomProperties(hashtable);
                        }
                        matchPose = false;
                        currentPose = PoseType.None;
                        SoundManager.Instance.PlaySeByName("FitPose");
                        Debug.Log("ネットワークマッチポーズ");
                      
                    }
                    if (unchecked(PhotonNetwork.ServerTimestamp - poseTime) > 0)//終了時間になったらポーズ形をやめる
                    {
                        currentPose = PoseType.None;
                        Debug.Log("時間経過");
                        combo = 0;//コンボが途切れる
                        UpdateScore(score);
                    }
                }
                if (unchecked(PhotonNetwork.ServerTimestamp - limitTime) > 0)//終了時間を満たしていたら
                {
                    Debug.Log("リプレイ状態");
                    currentGameState = GameState.Replay;
                    IsReplay = true;
                    SoundManager.Instance.PlaySeByName("Replay");
                    SoundManager.Instance.StopBgm();
                    UpdateScore(score);
                }
                break;
            case GameState.Replay:
                if (unchecked(PhotonNetwork.ServerTimestamp - (limitTime + 22000)) > 0)//リプレイ終了時間を満たしていたら
                {
                    currentGameState = GameState.Finish;
                    SoundManager.Instance.PlaySeByName("Result");
                }
                break;
            case GameState.Finish:
                break;
        }
    }

    IEnumerator SetPose()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            SetPropertiesPose();
        }
        else
        {
            yield return new WaitForSeconds(1f);
            currentPose = GetPropertiesPose();
        }
        yield return new WaitForSeconds(1f);
        IsCooldown = false;
    }

    IEnumerator ReturnCall()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (IsReturning) yield break;
            IsReturning = true;
            photonView.RPC(nameof(ReturnRoom), RpcTarget.All);
            PhotonNetwork.DestroyAll();
            IsReturning = false;
        }
    }

    [PunRPC]
    public void ReturnRoom()
    {
        currentGameState = GameState.None;
        currentState = NetworkState.Room;
        SceneManager.LoadScene("MatchMaking");
        PhotonNetwork.IsMessageQueueRunning = false;//メッセージの送信を停止する
    }
    IEnumerator SetScore()//スコアを設定する 
    {
        if (IsRunning) yield break;
        IsRunning = true;
        score += ComboCalculate(combo);
        combo += 1;//ここ仮にコンボ数を増やしている
        UpdateScore(score);//ネットワークマネージャーのスコアをアップデートする
        yield return new WaitForSeconds(0.5f);
        IsRunning = false;
    }
    public int ComboCalculate(int combo)//コンボ数をもとに増分を求める
    {
        var value = 10.0; //増分スコア
        var magnification = 1.0; //倍率

        //倍率の変更
        magnification += combo / 10.0;
        //増分の計算
        value = value * magnification;

        return (int)value;
    }
    IEnumerator SyncScore()
    {
        if (SyncIsRunning) yield break;
        SyncIsRunning = true;
        yield return new WaitForSeconds(0.5f);
        UpdateScore(score);
        SyncIsRunning = false;
    }
}