using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloseConfigScript : MonoBehaviour
{
    public GameObject Config;

    public void CloseConfig()
    {
        Config.SetActive(false);
    }
}
