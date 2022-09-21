using Photon.Pun;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class GameRoomTimer : MonoBehaviour
{
    private TextMeshProUGUI timeLabel;
    public  NewNetworkManager networkManager;
    private void Start()
    {
        timeLabel = GetComponent<TextMeshProUGUI>();
        networkManager = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<NewNetworkManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // まだルームに参加していない時は更新しない
        if (networkManager.currentGameState==NewNetworkManager.GameState.Playing) {
            // ゲーム開始時刻からの経過時間を求めて、テキスト表示する
            float elapsedTime = networkManager.limitTime - networkManager.currentTime;
            timeLabel.text = (elapsedTime/1000).ToString("f1");

        }
  
    }
}
