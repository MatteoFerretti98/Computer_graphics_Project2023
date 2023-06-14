using System.Collections;
using UnityEngine;

public class MessageManager : MonoBehaviour
{
    private static MessageManager instance; // Istanza del MessageManager

    private Coroutine currentCoroutine; // Coroutine corrente in esecuzione

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void ShowMessage(GameObject messageObject, float delay)
    {
        if (instance.currentCoroutine != null)
        {
            instance.StopCoroutine(instance.currentCoroutine);
        }

        instance.currentCoroutine = instance.StartCoroutine(instance.ShowAndHideObjectAfterDelay(messageObject, delay));
    }

    private IEnumerator ShowAndHideObjectAfterDelay(GameObject obj, float delay)
    {
        obj.SetActive(true); // Mostra l'oggetto

        yield return new WaitForSeconds(delay);

        obj.SetActive(false); // Nascondi l'oggetto
    }
}
