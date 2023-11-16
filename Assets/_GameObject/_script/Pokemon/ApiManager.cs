using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ApiManager : MonoBehaviour
{
    public static Action<string, Action<string>> GetAPIData;

    private void OnEnable()
    {
        GetAPIData += OnFetchPokemons;
    }

    private void OnDisable()
    {
        GetAPIData -= OnFetchPokemons;
    }

    private void OnFetchPokemons(string url, Action<string> callback)
    {
        StartCoroutine(FetchData(url, callback));
    }

    public IEnumerator FetchData(string url, Action<string> callback)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(request.error);
            }
            else
            {
                Debug.Log($"request.downloadHandler.text {request.downloadHandler.text}");
                callback?.Invoke(request.downloadHandler.text);
            }
        }
    }
}
