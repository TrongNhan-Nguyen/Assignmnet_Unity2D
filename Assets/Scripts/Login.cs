using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Login : MonoBehaviour
{
    public InputField userName;
    public InputField userPassword;
    public Button btnSubmit;
    public Button btnRegister;
    void Start()
    {
        btnSubmit.onClick.AddListener(() =>
        {
            StartCoroutine(login(userName.text, userPassword.text));
        });

        btnRegister.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("Register");
        });
    }
    public IEnumerator login(string username, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("loginUser", username);
        form.AddField("loginPass", password);
        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/assignment/login.php", form))
        {
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                toast(www.error);
                clg(www.error);
            }
            else
            {
                UserInfo.UserName = username;
                UserInfo.UserID = www.downloadHandler.text;
                string result = www.downloadHandler.text;
                if(result == "password incorrect")
                {
                    toast(result);
                }else if( result == "Username doesn't exists")
                {
                    toast(result);

                }
                else
                {
                    toast("Login successfully");
                    SceneManager.LoadScene("Profile");

                }

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
