using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class ResultTestScript : MonoBehaviour
{
    [SerializeField]
    private int score; // スコアを受け取るための変数
    //public bool resultDisplay = false;   // リザルトの表示の切り替えに必要 trueで表示 falseで非表示
    private float countTime;    //経過時間を図る
    [SerializeField]
    private GameObject resultUI;    //"Result"という文字のUI
    [SerializeField]
    private GameObject ComboUI;
    [SerializeField]
    private GameObject scoreUI;     //スコア(評価)のUI
    TextMeshProUGUI scoreText;
    [SerializeField]
    private GameObject ReturnToRoomButoonUI;    //ルームに戻るボタンのUI
    //public GameNetworkManager GameNetworkManager;//ゲームシーンからスコアを取得するため、必要


    // Start is called before the first frame update
    void Start()
    {
        countTime = 0;
        scoreText = scoreUI.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //score = GameNetworkManager.score;//ゲームシーンからスコアを取得
        // リプレイが終わったら、resultDisplayがtrueになるようにする
        //if (GameNetworkManager.result == true)
        //{
            //得点からスコア(評価)を決める
            if (score <= 3)
                scoreText.text = "残念";
            else if (score <= 6)
                scoreText.text = "まずまず";
            else if (score <= 9)
                scoreText.text = "いいね";
            else if (score <= 12)
                scoreText.text = "すごい";
            else
                scoreText.text = "素晴らしい";


            countTime += Time.deltaTime;

            if (countTime >= 1.0)
                resultUI.SetActive(true);

        if (countTime >= 3.0) {
            ComboUI.SetActive(true);
            scoreUI.SetActive(true);
        }

            if (countTime >= 5.0)
                ReturnToRoomButoonUI.SetActive(true);
        //}
    }

    public void Return()
    {
        //GameNetworkManager.ReturnCall();
    }
}