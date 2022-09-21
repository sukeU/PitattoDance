using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GamePlayManager : MonoBehaviour
{
    NewNetworkManager networkManager;
    public bool isReplay { get; private set; } = false;//���v���C�p�̕ϐ�
    public bool result { get; private set; } = false; //���U���g�ؑ֗p
    //public bool play { get; private set; } = false; //�v���C��ʐ؂�ւ��p
    public int score { get; private set; }

    [SerializeField]
    GameObject playObj;
    [SerializeField]
    GameObject resultObj;
    public int combo { get; private set; } //�Q�[�����̃R���{��
    public int MaxCombo { get; private set; } //�ő�R���{��
    private bool IsRunning = false;
    [SerializeField]
    GameObject[] PoseObjs = new GameObject[3];//Pose�̐��ɉ����đ��₷�K�v������
                                              //CollisionManager[] PoseManager = new CollisionManager[3];
    [SerializeField]
    TextMeshProUGUI scoreText;
    [SerializeField]
    TextMeshProUGUI comboText;
    void Start()
    {
        SoundManager.Instance.PlaySeByName("Start");
        networkManager = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<NewNetworkManager>();
        networkManager.OnUpdateScore += GetScore;//�l�b�g���[�N�}�l�[�W���[�ŃX�R�A�̕ω�������ƌĂ΂��l�ɂȂ�
         //������
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
                playObj.SetActive(true); //�������Ԃ̕\�L���I��
                //play = true;
                break;
            case NewNetworkManager.GameState.Replay:
                playObj.SetActive(false); //�������Ԃ̕\�L���I�t
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

    public void MatchPose()//�|�[�Y�������Ă���̂𓮂����֐� 
    {
        networkManager.MatchPose();
    }

    public void GetScore()//�X�R�A���󂯎��
    {
        if (networkManager.currentGameState != NewNetworkManager.GameState.Playing) return;//�Q�[���v���C������Ȃ������烊�^�[��
        score = networkManager.score;
        combo = networkManager.combo;
        MaxCombo = networkManager.MaxCombo;
        Debug.Log("�X�R�A:" + score);
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

