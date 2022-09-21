using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InputFieldManager : MonoBehaviour
{
    //InputFieldを格納する
    [SerializeField]
    private GameObject inputFieldUI;
    private TMP_InputField inputField;
    //「入場ボタン」のオブジェクト
    [SerializeField]
    private GameObject GoRoomButton;
    // Start is called before the first frame update
    void Start()
    {
        inputField = inputFieldUI.GetComponent<TMP_InputField>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //入力された名前情報を読み取る関数
    public void GetInputName()
    {
        string name = inputField.text;

        if (name == "")
            GoRoomButton.SetActive(false);
        else
            GoRoomButton.SetActive(true);
    }
}
