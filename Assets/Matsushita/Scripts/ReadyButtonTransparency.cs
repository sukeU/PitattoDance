using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;

public class ReadyButtonTransparency : MonoBehaviour
{
    /*
    [SerializeField]
    Button GameButton; //ゲームスタートボタン
    private TextMeshProUGUI GameButtonText;
    public NetworkManager networkmanager;
    // Start is called before the first frame update
    void Start()
    {
        GameButtonText = this.GetComponentInChildren<TextMeshProUGUI>();
    }

    private void FixedUpdate()
    {
        //ホストのゲームスタートボタンの透過設定
        ColorBlock cbBtn = GameButton.colors;

        if (PhotonNetwork.IsMasterClient)
        {
            if (networkmanager._isReady)
            {
                if (networkmanager.CheckReady())
                {
                    cbBtn.normalColor = new Color(255, 255, 255, 255);
                    GameButtonText.color = new Color(0, 0, 0, 255);
                }
                else
                {
                    cbBtn.normalColor = new Color(255, 255, 255, 120);
                    GameButtonText.color = new Color(0, 0, 0, 120);
                }
                GameButton.colors = cbBtn;
            }
        }
    }
    */
}