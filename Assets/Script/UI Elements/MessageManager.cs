using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageManager : MonoBehaviour
{
    private static MessageManager instance; // Istanza del MessageManager

    private Queue<Coroutine> coroutineQueue;
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
        coroutineQueue = new Queue<Coroutine>();
    }

    public static void ShowMessage(GameObject messageObject, float delay)
    {
        Coroutine newCoroutine = instance.StartCoroutine(instance.ShowAndHideObjectAfterDelay(messageObject, delay));
        instance.coroutineQueue.Enqueue(newCoroutine);
    }

    private IEnumerator ShowAndHideObjectAfterDelay(GameObject obj, float delay)
    {
        obj.SetActive(true); // Mostra l'oggetto

        yield return new WaitForSeconds(delay);

        obj.SetActive(false); // Nascondi l'oggetto

        coroutineQueue.Dequeue(); // Rimuovi la coroutine dalla coda

        // Se ci sono altre coroutine in coda, avvia la successiva
        if (coroutineQueue.Count > 0)
        {
            Coroutine nextCoroutine = coroutineQueue.Peek();
            yield return nextCoroutine;
        }
    }
}
