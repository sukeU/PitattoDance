using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenConfigScript : MonoBehaviour
{
    public GameObject Config;

    public void OpenConfig()
    {
        Config.SetActive(true);
    }
}
