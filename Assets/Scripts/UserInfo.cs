using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using UnityEngine.UI;

public class UserInfo : MonoBehaviour
{
    public static string UserID;
    public static string UserName;
    public static string Level;
    public static string Coins;
    public Text nameUser;
    public Text leveUser;
    public Text coinsUser;
    Action<string> _createUserCallback;
    public GameObject Inventory;
    private void Start()
    {
        if (UserID != null)
        {
            _createUserCallback = (jsonObjectString) =>
            {
                StartCoroutine(CreateUserRoutine(jsonObjectString));
            };
            CreateUser();
            //StartCoroutine(Main.Instance.Server.GetUserItems(UserID));
        }
    }

    private void FixedUpdate()
    {
        //if (Input.GetKey(KeyCode.E))
        //{
        //    Inventory.SetActive(true);
        //}
        //else if (Input.GetKey(KeyCode.F))
        //{
        //    Inventory.SetActive(false);
        //}
    }
    public void CreateUser()
    {
        //StartCoroutine(Main.Instance.Server.GetUser(UserID, _createUserCallback));
        StartCoroutine(Main.Instance.Server.GetUser("1", _createUserCallback));
    }
    IEnumerator CreateUserRoutine(string jsonObjectString)
    {
        JSONObject jsonObject = JSON.Parse(jsonObjectString) as JSONObject;
        bool isDone = false;
        Action<string> getUserInfoCallback = (userInfo) =>
        {
            isDone = true;
            jsonObject = JSON.Parse(userInfo) as JSONObject;
        };
        StartCoroutine(Main.Instance.Server.GetUser(UserID, getUserInfoCallback));
        yield return new WaitUntil(() => isDone == true);

        nameUser.text = "Name: " + jsonObject["username"];
        leveUser.text = "Level: " + jsonObject["level"];
        coinsUser.text = "Coins: " + jsonObject["coins"];
    }
    private void clg (string s)
    {
        Debug.Log(s);
    }
}
