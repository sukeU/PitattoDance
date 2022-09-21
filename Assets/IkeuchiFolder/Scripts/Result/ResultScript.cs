using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ResultScript : MonoBehaviour
{
    [SerializeField]
    private int score; // スコアを受け取るための変数
    private int combo; //コンボ数を受け取るための変数
    public bool resultDisplay = false;   // リザルトの表示の切り替えに必要 trueで表示 falseで非表示
    private float countTime;    //経過時間を図る
    [SerializeField]
    private GameObject scoreUI; //"スコア："という文字のUI
    [SerializeField]
    private GameObject scoreNumUI; //スコア（点数）UI
    TextMeshProUGUI scoreNumText;
    [SerializeField]
    private GameObject commentUI;     //コメントのUI
    //コメントに使うスプライト
    [SerializeField]
    private Sprite badImg;
    [SerializeField]
    private Sprite okImg;
    [SerializeField]
    private Sprite goodImg;
    [SerializeField]
    private Sprite excellentImg;
    [SerializeField]
    private Image ResultPhoto;
    [SerializeField]
    private GameObject comboUI; //"コンボ："という文字のUI
    [SerializeField]
    private GameObject comboNumUI;     //コンボ数のUI
    TextMeshProUGUI comboNumText;
    [SerializeField]
    private GameObject ReturnToRoomButoonUI;    //ルームに戻るボタンのUI
    GamePlayManager gamePlayManager; //スコアの取得に必要
    NewNetworkManager networkManager; //部屋にもどるときに必要

    // Start is called before the first frame update
    void Start()
    {
        countTime = 0;
        scoreNumText = scoreNumUI.GetComponent<TextMeshProUGUI>();
        comboNumText = comboNumUI.GetComponent<TextMeshProUGUI>();
        ResultPhoto = commentUI.GetComponent<Image>();
        gamePlayManager = GameObject.FindGameObjectWithTag("GamePlayManager").GetComponent<GamePlayManager>();
        networkManager = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<NewNetworkManager>();

        //初期化
        scoreNumUI.SetActive(false);
        scoreUI.SetActive(false);
        comboNumUI.SetActive(false);
        comboUI.SetActive(false);
        commentUI.SetActive(false);
        ReturnToRoomButoonUI.SetActive(false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        score = networkManager.score;//ゲームシーンからスコアを取得
        scoreNumText.text = score.ToString();
        combo = networkManager.MaxCombo; //ゲームシーンから最大コンボ数を取得
        comboNumText.text = combo.ToString();

        
        // リプレイが終わったら、resultDisplayがtrueになるようにする
        if (gamePlayManager.result== true)
        {
            //得点からスコア(評価)を決める
            if (score <= 25)
                ResultPhoto.sprite = badImg;
            else if (score <= 50)
                ResultPhoto.sprite = okImg;
            else if (score <= 80)
                ResultPhoto.sprite = goodImg;
            else
                ResultPhoto.sprite = excellentImg;

            countTime += Time.deltaTime;

            if (countTime >= 1.0)
                commentUI.SetActive(true);

            if (countTime >= 2.0)
            {
                scoreNumUI.SetActive(true);
                scoreUI.SetActive(true);
            }

            if (countTime >= 3.0)
            {
                comboNumUI.SetActive(true);
                comboUI.SetActive(true);
            }

            if (countTime >= 4.0)
                ReturnToRoomButoonUI.SetActive(true);
        }
    }

    public void Return()
    {
        networkManager.StartCoroutine("ReturnCall");
    }
}
