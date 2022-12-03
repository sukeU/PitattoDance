using Photon.Pun;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class GameRoomTimer : MonoBehaviour
{
    private TextMeshProUGUI timeLabel;
    public  NetworkManager networkManager;
    private void Start()
    {
        timeLabel = GetComponent<TextMeshProUGUI>();
        networkManager = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<NetworkManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // �܂����[���ɎQ�����Ă��Ȃ����͍X�V���Ȃ�
        if (networkManager.currentGameState==NetworkManager.GameState.Playing) {
            // �Q�[���J�n��������̌o�ߎ��Ԃ����߂āA�e�L�X�g�\������
            float elapsedTime = networkManager.limitTime - networkManager.currentTime;
            timeLabel.text = (elapsedTime/1000).ToString("f1");

        }
  
    }
}
