using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ResultScript : MonoBehaviour
{
    [SerializeField]
    private int score; // �X�R�A���󂯎�邽�߂̕ϐ�
    private int combo; //�R���{�����󂯎�邽�߂̕ϐ�
    public bool resultDisplay = false;   // ���U���g�̕\���̐؂�ւ��ɕK�v true�ŕ\�� false�Ŕ�\��
    private float countTime;    //�o�ߎ��Ԃ�}��
    [SerializeField]
    private GameObject scoreUI; //"�X�R�A�F"�Ƃ���������UI
    [SerializeField]
    private GameObject scoreNumUI; //�X�R�A�i�_���jUI
    TextMeshProUGUI scoreNumText;
    [SerializeField]
    private GameObject commentUI;     //�R�����g��UI
    //�R�����g�Ɏg���X�v���C�g
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
    private GameObject comboUI; //"�R���{�F"�Ƃ���������UI
    [SerializeField]
    private GameObject comboNumUI;     //�R���{����UI
    TextMeshProUGUI comboNumText;
    [SerializeField]
    private GameObject ReturnToRoomButoonUI;    //���[���ɖ߂�{�^����UI
    GamePlayManager gamePlayManager; //�X�R�A�̎擾�ɕK�v
    NewNetworkManager networkManager; //�����ɂ��ǂ�Ƃ��ɕK�v

    // Start is called before the first frame update
    void Start()
    {
        countTime = 0;
        scoreNumText = scoreNumUI.GetComponent<TextMeshProUGUI>();
        comboNumText = comboNumUI.GetComponent<TextMeshProUGUI>();
        ResultPhoto = commentUI.GetComponent<Image>();
        gamePlayManager = GameObject.FindGameObjectWithTag("GamePlayManager").GetComponent<GamePlayManager>();
        networkManager = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<NewNetworkManager>();

        //������
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
        score = networkManager.score;//�Q�[���V�[������X�R�A���擾
        scoreNumText.text = score.ToString();
        combo = networkManager.MaxCombo; //�Q�[���V�[������ő�R���{�����擾
        comboNumText.text = combo.ToString();

        
        // ���v���C���I�������AresultDisplay��true�ɂȂ�悤�ɂ���
        if (gamePlayManager.result== true)
        {
            //���_����X�R�A(�]��)�����߂�
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
