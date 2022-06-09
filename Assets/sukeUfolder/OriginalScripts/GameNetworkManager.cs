using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using TMPro;
using UnityEngine.SceneManagement;

/// <summary>
/// ゲームシーン用のネットワークオブジェクト
/// </summary>
public class GameNetworkManager : MonoBehaviourPunCallbacks//, IPunObservable
{
    [SerializeField]
    GameObject Prefab;
    [SerializeField]
    public int score { get; private set; }
    public int limitTime { get; private set; }
    int delayTime;
    bool _IsBgmPlaying = false;
    [SerializeField]
    int TagCount = 0;//領域に入っているタグの数
    int NumOfMenber = 3;//参加者の人数
    public bool timing = false;//タイミングがあっているかどうか
    float aTime = 0.0f;
    public bool isReplay { get; private set; } = false;
    public bool result { get; private set; } = false;


    void Start()
    {
        PhotonNetwork.IsMessageQueueRunning = true;//メッセージの送信を開始する

        PhotonNetwork.Instantiate("PlayerMagnet", new Vector2(0f, 0f), Quaternion.identity);//magnetを生成する

        if (PhotonNetwork.IsMasterClient)
        {
            PoseInst(NumOfMenber);
        }
        GameStart();
    }

    private void FixedUpdate()
    {
        CheckTags();
        if (unchecked(PhotonNetwork.ServerTimestamp - delayTime) > 0 && _IsBgmPlaying is false)//BGMが流れていないかつ、サーバー時間から5秒後に始める
        {
            SoundManager.Instance.PlayBgmByName("gamesound");
            _IsBgmPlaying = true;
        }
        if (timing)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                PoseInst(NumOfMenber);
            }
            aTime += Time.deltaTime;
            if (aTime > 1.0f)
            {
                aTime = 0.0f;
                timing = false;
            }
        }

        if (unchecked(PhotonNetwork.ServerTimestamp - limitTime) > 0 && _IsBgmPlaying is true)//BGMが流れているかつ終了時間を満たしていたら
        {
            SoundManager.Instance.StopBgm();
            _IsBgmPlaying = false;
            if (!isReplay)
            {
                isReplay = true;
            }
         
        }
        if (unchecked(PhotonNetwork.ServerTimestamp - (limitTime+24000)) > 0 )//BGMが流れているかつ終了時間を満たしていたら
        {
            result = true;
        }
     }
    public void GameStart()//タイムをスタートする
    {
        delayTime = unchecked(PhotonNetwork.ServerTimestamp + 3000);//3秒後に流れるようにしてある
        limitTime = unchecked(PhotonNetwork.ServerTimestamp + 63000);//63秒後に終了
    }
    public void ScoreUpdate(int value)//スコアを更新する
    {
        score += value;
    }

    public void TagUpdate(int value)
    {
        TagCount = Mathf.Clamp(TagCount + value, 0, 5);
    }

    public void CheckTags()//全て判定の中に入っているか確認する
    {
        switch (NumOfMenber)//参加者の数
        {
            case 1: case 2: case 3:
                if (TagCount >= 3)
                {
                    TagCount = 0;
                    timing = true;
                }
                else
                {
                    timing = false;
                }
                    break;
            case 4: break;
            case 5: break;
        }
    }

    //ネットワークオブジェクトにする必要がないのでexpo後に書き換える
    public void PoseObjectInst(string TargetTag, Vector2 pos)//引数にターゲットタグと場所を与えて生成する関数
    {
        var PoseInstObj = PhotonNetwork.Instantiate("PoseObj", pos, Quaternion.identity).GetComponent<CollisionDetection>();//生成時に取得
        PoseInstObj.SetTargetTag(TargetTag);//ターゲットタグをセットする
    }

    public void PoseInst(int NumOfPlayer)
    {//この中にポーズ集のアルゴリズムを組み込む
        var v = new Vector2(Random.Range(-2.5f, 10.0f), Random.Range(-4.0f, 2.5f));//横幅と縦幅をランダムに選ぶ
        PoseObjectInst("Body", v);
        var t = v + new Vector2(Random.Range(1.0f, 2.5f), Random.Range(-1.0f, 1.0f));// 人形から見て右手用
        PoseObjectInst("Larm", t);
        t = v + new Vector2(Random.Range(-2.5f, -1.0f), Random.Range(-1.0f, 1.0f));//人形から見て左手用
        PoseObjectInst("Rarm", t);
        if (NumOfPlayer == 4)
        {
            t = v + new Vector2(Random.Range(-2.5f, 2.5f), Random.Range(-1.0f, 1.0f));//xは-2.5~2.5まで　y-1.0~1.0 手用
            PoseObjectInst("Lleg", t);
        }
        if (NumOfPlayer == 5)
        {
            t = v + new Vector2(Random.Range(-2.5f, 2.5f), Random.Range(-1.0f, 1.0f));//xは-2.5~2.5まで　y-1.0~1.0 手用
            PoseObjectInst("Rleg", t);
        }
    }

    public void ReturnCall()
    {
        if (unchecked(PhotonNetwork.ServerTimestamp - limitTime) > 0){
            photonView.RPC(nameof(ReturnRoom), RpcTarget.All);
        }
    }

    [PunRPC]
    public void ReturnRoom()
    {
        SceneManager.LoadScene("MatchMaking");
        PhotonNetwork.IsMessageQueueRunning = false;//メッセージの送信を停止する
    }
}
