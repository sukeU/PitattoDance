using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InputFieldManager : MonoBehaviour
{
    //InputField���i�[����
    [SerializeField]
    private GameObject inputFieldUI;
    private TMP_InputField inputField;
    //�u����{�^���v�̃I�u�W�F�N�g
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

    //���͂��ꂽ���O����ǂݎ��֐�
    public void GetInputName()
    {
        string name = inputField.text;

        if (name == "")
            GoRoomButton.SetActive(false);
        else
            GoRoomButton.SetActive(true);
    }
}
