using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using TMPro;
using UnityEngine.SceneManagement;

/// <summary>
/// �Q�[���V�[���p�̃l�b�g���[�N�I�u�W�F�N�g
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
    int TagCount = 0;//�̈�ɓ����Ă���^�O�̐�
    int NumOfMenber = 3;//�Q���҂̐l��
    public bool timing = false;//�^�C�~���O�������Ă��邩�ǂ���
    float aTime = 0.0f;
    public bool isReplay { get; private set; } = false;
    public bool result { get; private set; } = false;


    void Start()
    {
        PhotonNetwork.IsMessageQueueRunning = true;//���b�Z�[�W�̑��M���J�n����

        PhotonNetwork.Instantiate("PlayerMagnet", new Vector2(0f, 0f), Quaternion.identity);//magnet�𐶐�����

        if (PhotonNetwork.IsMasterClient)
        {
            PoseInst(NumOfMenber);
        }
        GameStart();
    }

    private void FixedUpdate()
    {
        CheckTags();
        if (unchecked(PhotonNetwork.ServerTimestamp - delayTime) > 0 && _IsBgmPlaying is false)//BGM������Ă��Ȃ����A�T�[�o�[���Ԃ���5�b��Ɏn�߂�
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

        if (unchecked(PhotonNetwork.ServerTimestamp - limitTime) > 0 && _IsBgmPlaying is true)//BGM������Ă��邩�I�����Ԃ𖞂����Ă�����
        {
            SoundManager.Instance.StopBgm();
            _IsBgmPlaying = false;
            if (!isReplay)
            {
                isReplay = true;
            }
         
        }
        if (unchecked(PhotonNetwork.ServerTimestamp - (limitTime+24000)) > 0 )//BGM������Ă��邩�I�����Ԃ𖞂����Ă�����
        {
            result = true;
        }
     }
    public void GameStart()//�^�C�����X�^�[�g����
    {
        delayTime = unchecked(PhotonNetwork.ServerTimestamp + 3000);//3�b��ɗ����悤�ɂ��Ă���
        limitTime = unchecked(PhotonNetwork.ServerTimestamp + 63000);//63�b��ɏI��
    }
    public void ScoreUpdate(int value)//�X�R�A���X�V����
    {
        score += value;
    }

    public void TagUpdate(int value)
    {
        TagCount = Mathf.Clamp(TagCount + value, 0, 5);
    }

    public void CheckTags()//�S�Ĕ���̒��ɓ����Ă��邩�m�F����
    {
        switch (NumOfMenber)//�Q���҂̐�
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

    //�l�b�g���[�N�I�u�W�F�N�g�ɂ���K�v���Ȃ��̂�expo��ɏ���������
    public void PoseObjectInst(string TargetTag, Vector2 pos)//�����Ƀ^�[�Q�b�g�^�O�Əꏊ��^���Đ�������֐�
    {
        var PoseInstObj = PhotonNetwork.Instantiate("PoseObj", pos, Quaternion.identity).GetComponent<CollisionDetection>();//�������Ɏ擾
        PoseInstObj.SetTargetTag(TargetTag);//�^�[�Q�b�g�^�O���Z�b�g����
    }

    public void PoseInst(int NumOfPlayer)
    {//���̒��Ƀ|�[�Y�W�̃A���S���Y����g�ݍ���
        var v = new Vector2(Random.Range(-2.5f, 10.0f), Random.Range(-4.0f, 2.5f));//�����Əc���������_���ɑI��
        PoseObjectInst("Body", v);
        var t = v + new Vector2(Random.Range(1.0f, 2.5f), Random.Range(-1.0f, 1.0f));// �l�`���猩�ĉE��p
        PoseObjectInst("Larm", t);
        t = v + new Vector2(Random.Range(-2.5f, -1.0f), Random.Range(-1.0f, 1.0f));//�l�`���猩�č���p
        PoseObjectInst("Rarm", t);
        if (NumOfPlayer == 4)
        {
            t = v + new Vector2(Random.Range(-2.5f, 2.5f), Random.Range(-1.0f, 1.0f));//x��-2.5~2.5�܂Ł@y-1.0~1.0 ��p
            PoseObjectInst("Lleg", t);
        }
        if (NumOfPlayer == 5)
        {
            t = v + new Vector2(Random.Range(-2.5f, 2.5f), Random.Range(-1.0f, 1.0f));//x��-2.5~2.5�܂Ł@y-1.0~1.0 ��p
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
        PhotonNetwork.IsMessageQueueRunning = false;//���b�Z�[�W�̑��M���~����
    }
}
