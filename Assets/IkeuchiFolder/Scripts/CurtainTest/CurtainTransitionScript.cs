using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CurtainTransitionScript : MonoBehaviour
{
    public void TestLoadingCurtainScene()
    {
        SceneManager.LoadScene("Curtain_Animation_Test_Scene");
    }
}
