using UnityEngine;
using UnityEngine.SceneManagement;

public class ManagerSceneAutoLoader : MonoBehaviour
{

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void LoadManagerScene()
    {
        string managerSceneName = "ManagerScene";

        //ManagerScene���L���łȂ��Ƃ��ɒǉ����[�h
        if (!SceneManager.GetSceneByName(managerSceneName).IsValid())
        {

            SceneManager.LoadScene(managerSceneName, LoadSceneMode.Additive);

        }
    }
}