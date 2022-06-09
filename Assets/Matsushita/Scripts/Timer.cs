using System;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField]
    private int timeLimit; //��������
    [SerializeField]
    private TextMeshProUGUI SecondText; //�������ԕ\����Text�I�u�W�F�N�g(�b)
    [SerializeField]
    private TextMeshProUGUI MinuteText; //�������ԕ\����Text�I�u�W�F�N�g(��)
    public int soundLimit;
    DateTime startTime; //�X�^�[�g���Ԃ̓������擾
    int Etime; //�o�ߎ���(�b�\��)
    bool isFinished;
    bool isSoundLimit;
    //DateTime endTime;
    void Start()
    {
        startTime = DateTime.Now; //���݂̓������擾
        isFinished = false;
        isSoundLimit = false;
    }

    // Update is called once per frame
    void Update()
    {
        var endTime = DateTime.Now; //���݂̓������擾
        Etime = (endTime.Minute - startTime.Minute) * 60 + (endTime.Second - startTime.Second);

        if (Etime < timeLimit)//�������ԓ��̂Ƃ��A
        {
            if ((timeLimit - Etime) % 60 < 10)//�u�b�v�̒l���P���̎��ɂP�O�̈ʂɂO������
            {
                SecondText.text = "0" + ((timeLimit - Etime) % 60).ToString();
            }
            else
            {
                SecondText.text = ((timeLimit - Etime) % 60).ToString();
            }
            if ((timeLimit - Etime) / 60 < 10)//�u���v�̒l���P���̎��ɂP�O�̈ʂɂO������
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

    // �^�C���A�b�v������
    public bool IsFinished()
    {
        return isFinished;
    }

    // �c�莞�Ԃ����Ȃ��Ȃ����H
    public bool IsSoundLimit()
    {
        return isSoundLimit;
    }
}
