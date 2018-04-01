using UnityEngine;
using System.Net;
using UnityEditor;
using System.IO;
using System;
using System.Collections;
using UnityEngine.Networking;
using Newtonsoft.Json;
using UnityEngine.UI;

public class DownloadManager : MonoBehaviour
{
    [SerializeField]
    public int speedLowerConst;
    private int speedlower = 0;
    int counter = 1;
    int len = 0;
    int length;
    bool enableToPlay = false;
    UnityWebRequest www;

    Button test;
    Button first;
    Button second;
    public Canvas can;
    public string[] lesson;

    // Use this for initialization
    void Start()
    {
        first = can.transform.Find("First").gameObject.GetComponent<Button>();
        second = can.transform.Find("Second").gameObject.GetComponent<Button>();
        test = can.transform.Find("Test").gameObject.GetComponent<Button>();
        first.onClick.AddListener(() => StartCoroutine(Download(lesson[0])));
        second.onClick.AddListener(() => StartCoroutine(Download(lesson[1])));
        test.onClick.AddListener(Open_tabs);
    }
    
    void Open_tabs()
    {
        first.gameObject.SetActive(true);
        second.gameObject.SetActive(true);
    }

    public IEnumerator Download(string lesson)
    {
        enableToPlay = false;

        if (transform.childCount != 0)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
        }

        www = UnityWebRequest.Get("http://vrteleportator.azurewebsites.net/api/File/getinfo");
        yield return www.SendWebRequest();


        if (!www.isNetworkError && !www.isHttpError)
        {
            print("connected");
            WebClient client = new WebClient();
            yield return StartCoroutine(GetLength());
            for (int i = 0; i <= len - 1; i++)
            {
                string path = Application.dataPath + @"\Resources\" + lesson + @"\kek" + i + ".ply";
                if (!Directory.Exists(Application.dataPath + @"\Resources\" + lesson))
                {
                    Directory.CreateDirectory(Application.dataPath + @"\Resources\" + lesson);
                }
                if (!File.Exists(path))
                {
                    client.DownloadFile("http://vrteleportator.azurewebsites.net/Extracts/" + lesson + "/kek" + i + ".ply", path);
                }
            }
            AssetDatabase.Refresh();

        }

        AssetDatabase.Refresh();
        var r = Resources.LoadAll("kek", typeof(GameObject));
        length = r.Length;
        foreach (var item in r)
        {
            GameObject obj = Instantiate(item) as GameObject;
            obj.transform.SetParent(transform);
            obj.transform.position = transform.position;
            obj.transform.eulerAngles = new Vector3(112.9f, -124.8f, 0);
            obj.SetActive(false);
        }


        enableToPlay = true;
        yield return null;
    }

    private IEnumerator GetLength()
    {
        len = Convert.ToInt32(JsonConvert.DeserializeObject(www.downloadHandler.text));
        yield return null;
    }

    void Update()
    {
        if (speedlower == 100) speedlower = 0;

        try
        {
            if (speedlower % speedLowerConst == 0)
            {
                if (enableToPlay)
                {
                    if (counter < length)
                    {
                        if (counter == 1)
                        {
                            transform.Find("kek" + (length - 1) + "(Clone)").gameObject.SetActive(false);
                            transform.Find("kek" + counter + "(Clone)").gameObject.SetActive(true);
                        }
                        else
                        {
                            transform.Find("kek" + (counter - 1) + "(Clone)").gameObject.SetActive(false);
                            transform.Find("kek" + counter + "(Clone)").gameObject.SetActive(true);
                        }
                        counter++;

                    }
                    else counter = 1;
                }
            }
            speedlower++;
        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
        }
    }
    
}
