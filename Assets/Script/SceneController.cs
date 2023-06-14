using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void SceneChange(string name)
    {
        SceneManager.LoadScene(name);
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
}
