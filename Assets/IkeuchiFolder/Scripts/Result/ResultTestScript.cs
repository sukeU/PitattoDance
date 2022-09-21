using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class ResultTestScript : MonoBehaviour
{
    [SerializeField]
    private int score; // �X�R�A���󂯎�邽�߂̕ϐ�
    //public bool resultDisplay = false;   // ���U���g�̕\���̐؂�ւ��ɕK�v true�ŕ\�� false�Ŕ�\��
    private float countTime;    //�o�ߎ��Ԃ�}��
    [SerializeField]
    private GameObject resultUI;    //"Result"�Ƃ���������UI
    [SerializeField]
    private GameObject ComboUI;
    [SerializeField]
    private GameObject scoreUI;     //�X�R�A(�]��)��UI
    TextMeshProUGUI scoreText;
    [SerializeField]
    private GameObject ReturnToRoomButoonUI;    //���[���ɖ߂�{�^����UI
    //public GameNetworkManager GameNetworkManager;//�Q�[���V�[������X�R�A���擾���邽�߁A�K�v


    // Start is called before the first frame update
    void Start()
    {
        countTime = 0;
        scoreText = scoreUI.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //score = GameNetworkManager.score;//�Q�[���V�[������X�R�A���擾
        // ���v���C���I�������AresultDisplay��true�ɂȂ�悤�ɂ���
        //if (GameNetworkManager.result == true)
        //{
            //���_����X�R�A(�]��)�����߂�
            if (score <= 3)
                scoreText.text = "�c�O";
            else if (score <= 6)
                scoreText.text = "�܂��܂�";
            else if (score <= 9)
                scoreText.text = "������";
            else if (score <= 12)
                scoreText.text = "������";
            else
                scoreText.text = "�f���炵��";


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