﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Server : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(GetDate());
        //StartCoroutine(GetUser());
        //StartCoroutine(Login("testuser2", "qwerty"));
        //StartCoroutine(Register("testuser3","123456"));
    }

    public IEnumerator GetDate()
    {
        using (UnityWebRequest www = UnityWebRequest.Get("http://localhost/assignment/getdate.php"))
        {
            yield return www.SendWebRequest();

            if(www.isNetworkError || www.isHttpError)
            {
                toast(www.error);
                clg(www.error);
            }
            else
            {
                toast(www.downloadHandler.text);
                clg(www.downloadHandler.text);

                byte[] results = www.downloadHandler.data;
            }
        }
    }
     public IEnumerator GetUser()
    {
        using (UnityWebRequest www = UnityWebRequest.Get("http://localhost/assignment/getuser.php"))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                toast(www.error);
                clg(www.error);
            }
            else
            {
                toast(www.downloadHandler.text);
                clg(www.downloadHandler.text);

                byte[] results = www.downloadHandler.data;
            }
        }
    }
    public IEnumerator Login(string username, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("loginUser", username);
        form.AddField("loginPass", password);
        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/assignment/login.php",form))
        {
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                toast(www.error);
                clg(www.error);
            }
            else
            {
                toast(www.downloadHandler.text);
                clg(www.downloadHandler.text);
            }
        }

    }
    public IEnumerator Register(string username, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("registerUser", username);
        form.AddField("registerPass", password);
        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/assignment/register.php", form))
        {
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                toast(www.error);
                clg(www.error);
            }
            else
            {
                toast(www.downloadHandler.text);
                clg(www.downloadHandler.text);
            }
        }

    }
    private void toast(string s)
    {
        SSTools.ShowMessage(s, SSTools.Position.bottom, SSTools.Time.twoSecond);
    }
    private void clg(string s)
    {
        Debug.Log(s);
    }
}
