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
    private bool openInventory  = false;
    private void Start()
    {
        if (UserID != null)
        {
            _createUserCallback = (jsonObjectString) =>
            {
                StartCoroutine(CreateUserRoutine(jsonObjectString));
            };
            CreateUser();
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (Inventory.activeSelf == false)
            {
                Inventory.SetActive(true);
            }
            else
            {
                Inventory.SetActive(false);

            }

        }


    }
    private void FixedUpdate()
    {
       
       
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
