using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
                toast("User name or password can't be empty");
            }
            else if(!pass.Equals(confirm))
            {
                toast("Confirm password does not mach");
            }
            else
            {
                StartCoroutine(Main.Instance.Server.Register(name, pass));
                toast("Regsiter");

            }
        });

        btnLogin.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("Login");
        });

    }

    private void toast (string s)
    {
        SSTools.ShowMessage(s, SSTools.Position.bottom, SSTools.Time.twoSecond);
    }
}
