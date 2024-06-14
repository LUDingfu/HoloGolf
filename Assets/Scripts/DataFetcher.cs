using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class InitialRequest : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(SendRequestToMatlab());
    }

    IEnumerator SendRequestToMatlab()
    {
        string url = "http://localhost:3000/";

        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            if (request.downloadHandler.text == "true")
            {
                Debug.Log("Signal received to start the project.");
                // Call the function to start the project
                StartProject();
            }
        }
        else
        {
            Debug.Log("Error in communication: " + request.error);
        }
    }

    void StartProject()
    {
        // Code to start your project
        Debug.Log("Project has started.");
    }
}