using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Result : MonoBehaviour
{
    [SerializeField]
    GameNetworkManager Manager;
    [SerializeField]
    TextMeshProUGUI reviewText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Manager.score > 12)
        {
            reviewText.text = "excellent";
        }
        else if (Manager.score > 9)
        {
            reviewText.text = "great";
        }
        else if (Manager.score > 6)
        {
            reviewText.text = "good";
        }
        else if (Manager.score < 3)
        {
            reviewText.text = "bad";
        }
    }


}
