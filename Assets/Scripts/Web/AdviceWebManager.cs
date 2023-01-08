using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace Web
{
    public class AdviceWebManager : MonoBehaviour
    {
        [SerializeField] private string url;
        public event Action<Message> MessageReceivedEvent;

        private Coroutine coroutine;

        public void RequestMessage()
        {
            if (coroutine == null)
            {
                StartCoroutine(SendRequest());
            }
        }

        public void ForceRequestMessage()
        {
            StopCoroutine(coroutine);
            coroutine = null;
            StartCoroutine(SendRequest());
        }

        private IEnumerator SendRequest()
        {
            UnityWebRequest request = UnityWebRequest.Get(url);
            yield return request.SendWebRequest();

            var o = JsonUtility.FromJson<Message>(request.downloadHandler.text);
            MessageReceivedEvent?.Invoke(o);
        }
    }
}