using UnityEngine;

public class QuitManager : MonoBehaviour
{
    public void Quit()
    {
    #if UNITY_STANDALONE
            Application.Quit();
    #endif
    #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
    #endif
    }
}
