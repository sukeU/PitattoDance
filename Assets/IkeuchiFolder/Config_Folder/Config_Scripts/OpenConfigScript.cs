using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenConfigScript : MonoBehaviour
{
    public GameObject configUI;

    public void OpenConfig()
    {
        configUI.SetActive(true);
    }
}
