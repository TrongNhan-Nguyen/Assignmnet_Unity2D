using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
            StartCoroutine(Main.Instance.Server.Login(userName.text, userPassword.text));
        });

        btnRegister.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("Register");
        });
    }
}
