using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloseHintScript : MonoBehaviour
{
    public GameObject Hint;

    public void CloseHint()
    {
        Hint.SetActive(false);
        this.gameObject.SetActive(false);
    }
}
