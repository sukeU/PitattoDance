using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloseConfigScript : MonoBehaviour
{
    public GameObject configUI;

    public void CloseConfig()
    {
        configUI.SetActive(false);
    }
}
