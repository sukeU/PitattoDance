using UnityEngine;
using UnityEngine.SceneManagement;

public class ManagerSceneAutoLoader : MonoBehaviour
{

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void LoadManagerScene()
    {
        string managerSceneName = "ManagerScene";

        //ManagerScene‚ª—LŒø‚Å‚È‚¢‚Æ‚«‚É’Ç‰Áƒ[ƒh
        if (!SceneManager.GetSceneByName(managerSceneName).IsValid())
        {

            SceneManager.LoadScene(managerSceneName, LoadSceneMode.Additive);

        }
    }
}