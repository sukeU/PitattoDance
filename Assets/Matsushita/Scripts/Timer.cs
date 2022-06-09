using System;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField]
    private int timeLimit; //制限時間
    [SerializeField]
    private TextMeshProUGUI SecondText; //制限時間表示のTextオブジェクト(秒)
    [SerializeField]
    private TextMeshProUGUI MinuteText; //制限時間表示のTextオブジェクト(分)
    public int soundLimit;
    DateTime startTime; //スタート時間の日時を取得
    int Etime; //経過時間(秒表示)
    bool isFinished;
    bool isSoundLimit;
    //DateTime endTime;
    void Start()
    {
        startTime = DateTime.Now; //現在の日時を取得
        isFinished = false;
        isSoundLimit = false;
    }

    // Update is called once per frame
    void Update()
    {
        var endTime = DateTime.Now; //現在の日時を取得
        Etime = (endTime.Minute - startTime.Minute) * 60 + (endTime.Second - startTime.Second);

        if (Etime < timeLimit)//制限時間内のとき、
        {
            if ((timeLimit - Etime) % 60 < 10)//「秒」の値が１桁の時に１０の位に０をつける
            {
                SecondText.text = "0" + ((timeLimit - Etime) % 60).ToString();
            }
            else
            {
                SecondText.text = ((timeLimit - Etime) % 60).ToString();
            }
            if ((timeLimit - Etime) / 60 < 10)//「分」の値が１桁の時に１０の位に０をつける
            {
                MinuteText.text = "0" + ((timeLimit - Etime) / 60).ToString();
            }
            else
            {
                MinuteText.text = ((timeLimit - Etime) / 60).ToString();
            }
            //time.text = (timeLimit - Etime).ToString();

            if (Etime < soundLimit)
            {
                isSoundLimit = true;
            }
        }
        else
        {
            Finish();
        }
    }

    private void Finish()
    {
        MinuteText.text = "00";
        SecondText.text = "00";

        isFinished = true;
    }

    // タイムアップしたか
    public bool IsFinished()
    {
        return isFinished;
    }

    // 残り時間が少なくなった？
    public bool IsSoundLimit()
    {
        return isSoundLimit;
    }
}
