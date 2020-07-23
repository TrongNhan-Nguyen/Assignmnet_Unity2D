using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class Server : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(GetDate());
        //StartCoroutine(GetUser());
        //StartCoroutine(Login("testuser2", "qwerty"));
        //StartCoroutine(Register("testuser3","123456"));
        //StartCoroutine(GetAllItems());
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
    // public IEnumerator GetUser(string userID, System.Action<string> callback)
    //{
    //    WWWForm form = new WWWForm();
    //    form.AddField("userID", userID);
    //    using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/assignment/getuser.php",form))
    //    {
    //        yield return www.SendWebRequest();

    //        if (www.isNetworkError || www.isHttpError)
    //        {
    //            toast(www.error);
    //            clg(www.error);
    //        }
    //        else
    //        {
    //            //clg(www.downloadHandler.text);
    //            string jsonObject = www.downloadHandler.text;
    //            callback(jsonObject);

    //        }
    //    }
    //}
    public IEnumerator GetUserItems(string userID,System.Action<string> callback)
    {
        WWWForm form = new WWWForm();
        form.AddField("userID", userID);
        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/assignment/getuser_items.php",form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                toast(www.error);
                clg(www.error);
            }
            else
            {
                string jsonArray = www.downloadHandler.text;
                callback(jsonArray);
               
            }
        }
    }
    public IEnumerator GetItem(string itemID, System.Action<string> callback)
    {
        WWWForm form = new WWWForm();
        form.AddField("itemID", itemID);
        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/assignment/getitem.php", form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                toast(www.error);
                clg(www.error);
            }
            else
            {
                string jsonArray = www.downloadHandler.text;
                callback(jsonArray);

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
