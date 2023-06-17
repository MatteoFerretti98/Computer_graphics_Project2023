using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void SceneChange(string name)
    {
        //AudioManager.instance.Play("Menu Selection");
        SceneManager.LoadScene(name);
        if (name == "Menu") AudioManager.instance.StopMusic();
        else if (name == "BossArena")
        {
            AudioManager.instance.StopMusic();
            AudioManager.instance.PlayMusic("InGameBossMusic");
        }
        Time.timeScale = 1;
    }

    private void OnEnable()
    {
        SceneManager.sceneUnloaded += DisablePreviousSceneObjects;
    }

    private void OnDisable()
    {
        SceneManager.sceneUnloaded -= DisablePreviousSceneObjects;
    }

    private void DisablePreviousSceneObjects(Scene scene)
    {
        // Disable or destroy game objects from the previous scene
        GameObject[] sceneObjects = scene.GetRootGameObjects();
        foreach (GameObject obj in sceneObjects)
        {
            obj.SetActive(false); // Or use Destroy(obj) if you want to destroy the objects
        }
    }

    public void AudioSelector(string type)
    {
        switch (type)
        {
            case "Start":
                AudioManager.instance.PlaySFX("ClickButtonStart");
                break;
            case "Continue":
                AudioManager.instance.PlaySFX("ClickButtonMenu");
                break;
            case "Back":
                AudioManager.instance.PlaySFX("Back");
                break;
            default:
                break;
        }
    }
}
