using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenHintScript : MonoBehaviour
{
    public GameObject Hint;
    public GameObject CloseButton;

    public void OpenHint()
    {
        Hint.SetActive(true);
        CloseButton.SetActive(true);
    }
}
