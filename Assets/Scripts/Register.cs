using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.WSA;

public class Register : MonoBehaviour
{
    public InputField userName;
    public InputField userPassword;
    public InputField confirmPassword;
    public Button btnSubmit;
    public Button btnLogin;
    void Start()
    {
        btnSubmit.onClick.AddListener(() =>
        {
            string name = userName.text;
            string pass = userPassword.text;
            string confirm = confirmPassword.text;

            if(name.Equals("") || pass.Equals(""))
            {
                toast("Please, fill up this form");
            }
            else if(!pass.Equals(confirm))
            {
                toast("Confirm password does not mach");
            }
            else
            {
                StartCoroutine(register(name, pass));
            }
        });

        btnLogin.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("Login");
        });

    }
    public IEnumerator register(string username, string password)
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
                if (www.downloadHandler.text == "Already exists")
                {
                    toast("User name already exists");
                }
                else
                {
                    toast("Register succeessfully");
                }
              
            }
        }

    }

    private void clg (string s)
    {
        Debug.Log(s);
    }
    private void toast (string s)
    {
        SSTools.ShowMessage(s, SSTools.Position.bottom, SSTools.Time.twoSecond);
    }
}
