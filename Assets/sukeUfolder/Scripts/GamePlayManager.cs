using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GamePlayManager : MonoBehaviour
{
    NewNetworkManager networkManager;
    public bool isReplay { get; private set; } = false;//リプレイ用の変数
    public bool result { get; private set; } = false; //リザルト切替用
    //public bool play { get; private set; } = false; //プレイ画面切り替え用
    public int score { get; private set; }

    [SerializeField]
    GameObject playObj;
    [SerializeField]
    GameObject resultObj;
    public int combo { get; private set; } //ゲーム中のコンボ数
    public int MaxCombo { get; private set; } //最大コンボ数
    private bool IsRunning = false;
    [SerializeField]
    GameObject[] PoseObjs = new GameObject[3];//Poseの数に応じて増やす必要がある
                                              //CollisionManager[] PoseManager = new CollisionManager[3];
    [SerializeField]
    TextMeshProUGUI scoreText;
    [SerializeField]
    TextMeshProUGUI comboText;
    void Start()
    {
        SoundManager.Instance.PlaySeByName("Start");
        networkManager = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<NewNetworkManager>();
        networkManager.OnUpdateScore += GetScore;//ネットワークマネージャーでスコアの変化があると呼ばれる様になる
         //初期化
        score = 0;
        combo = 0;
        MaxCombo = 0;
    }

    // Update is called once per frame
    void Update()
    {
        switch (networkManager.currentGameState)
        {
            case NewNetworkManager.GameState.Ready:
                playObj.SetActive(false);
                resultObj.SetActive(false);
                break;
            case NewNetworkManager.GameState.Playing:
                StartCoroutine("Pose");
                playObj.SetActive(true); //制限時間の表記をオン
                //play = true;
                break;
            case NewNetworkManager.GameState.Replay:
                playObj.SetActive(false); //制限時間の表記をオフ
                foreach (var obj in PoseObjs)
                {
                    obj.SetActive(false);
                }
                isReplay = true;
                break;
            case NewNetworkManager.GameState.Finish:
                result = true;
                resultObj.SetActive(true);
                break;
        }
    }

    void PoseDisplay()
    {
        var pose = (int)networkManager.currentPose;
        foreach (var obj in PoseObjs)
        {
            obj.SetActive(false);
        }

        PoseObjs[pose].SetActive(true);

    }

    public void MatchPose()//ポーズが合っているのを動かす関数 
    {
        networkManager.MatchPose();
    }

    public void GetScore()//スコアを受け取る
    {
        if (networkManager.currentGameState != NewNetworkManager.GameState.Playing) return;//ゲームプレイ中じゃなかったらリターン
        score = networkManager.score;
        combo = networkManager.combo;
        MaxCombo = networkManager.MaxCombo;
        Debug.Log("スコア:" + score);
        scoreText.text = score.ToString();
        comboText.text = combo.ToString();
    }
   
    IEnumerator Pose()
    {
        if (IsRunning)yield break;
        IsRunning = true;
        yield return new WaitForSeconds(0.5f);
        PoseDisplay();
        IsRunning = false;
    }

}

