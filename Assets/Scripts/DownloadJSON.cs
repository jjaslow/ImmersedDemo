using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public static class DownloadJSON
{

    public static IEnumerator DownloadJSONFiles()
    {
        string _baseURL = "http://www.emergereality.com/";

        UnityWebRequest _jsonRequest = UnityWebRequest.Get(_baseURL + "file.json");
        yield return _jsonRequest.SendWebRequest();

        if (_jsonRequest.isDone == false || _jsonRequest.error != null)
        {
            Debug.Log("Download Request ERROR: " + _jsonRequest.error);
        }
        else
        {
            string _pathToFile = Path.Combine(Application.persistentDataPath, "file.json");
            File.WriteAllText(_pathToFile, _jsonRequest.downloadHandler.text);
        }
    }


}
