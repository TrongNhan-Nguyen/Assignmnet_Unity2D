using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class UserInfo : MonoBehaviour
{
    public static string UserID;
    public static string UserName;
    public static string Level;
    public static string Coins;
    public static List<string> itemList = new List<string>();
    public Text nameUser;
    public Text leveUser;
    public Text coinsUser;
    Action<string> _createUserCallback;
    Action<string> _createItemsCallback;
    public GameObject ItemsInventory;
    public GameObject Inventory;
    private void Start()
    {
        Inventory.SetActive(false);

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
            if (ItemsInventory.activeSelf == false)
            {
                ItemsInventory.SetActive(true);
                Inventory.SetActive(true);
                ItemsInventory.GetComponent<Inventory>().enabled = true;

            }
            else
            {
                Inventory.SetActive(false);
                ItemsInventory.SetActive(false);
                ItemsInventory.GetComponent<Inventory>().enabled = false;
            }
        }
    }
    public void CreateUser()
    {
        //StartCoroutine(Main.Instance.Server.GetUser(UserID, _createUserCallback));
        StartCoroutine(GetUser("1", _createUserCallback));
    }
    public void CreateItems()
    {
        //StartCoroutine(Main.Instance.Server.GetUserItems(UserInfo.UserID,_createItemsCallback));
        StartCoroutine(GetUserItems("3", _createItemsCallback));

    }

    private void OnEnable()
    {
        _createItemsCallback = (jsonArrayString) =>
        {
            StartCoroutine(CreateItemsRoutine(jsonArrayString));
        };
        CreateItems();

    }
    public IEnumerator GetUserItems(string userID, Action<string> callback)
    {
        WWWForm form = new WWWForm();
        form.AddField("userID", userID);
        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/assignment/getuser_items.php", form))
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

    IEnumerator CreateItemsRoutine(string jsonArrayString)
    {
        JSONArray jsonArray = JSON.Parse(jsonArrayString) as JSONArray;
        UserInfo.itemList.Clear();
        for (int i = 0; i < jsonArray.Count; i++)
        {
            bool isDone = false;
            string itemID = jsonArray[i].AsObject["itemID"];
            JSONObject itemInfoJson = new JSONObject();
            Action<string> getItemInfoCallback = (itemInfo) =>
            {
                isDone = true;
                JSONArray tempArray = JSON.Parse(itemInfo) as JSONArray;
                itemInfoJson = tempArray[0].AsObject;
            };
            StartCoroutine(GetItem(itemID, getItemInfoCallback));

            yield return new WaitUntil(() => isDone == true);
            itemList.Add(itemInfoJson["name"]);
        }
        jsonArray = new JSONArray();
    }
    public IEnumerator GetItem(string itemID, Action<string> callback)
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
    IEnumerator CreateUserRoutine(string jsonObjectString)
    {
        JSONObject jsonObject = JSON.Parse(jsonObjectString) as JSONObject;
        bool isDone = false;
        Action<string> getUserInfoCallback = (userInfo) =>
        {
            isDone = true;
            jsonObject = JSON.Parse(userInfo) as JSONObject;
        };
        StartCoroutine(GetUser(UserID, getUserInfoCallback));
        yield return new WaitUntil(() => isDone == true);

        nameUser.text = "Name: " + jsonObject["username"];
        leveUser.text = "Level: " + jsonObject["level"];
        coinsUser.text = "Coins: " + jsonObject["coins"];
    }
    public IEnumerator GetUser(string userID, Action<string> callback)
    {
        WWWForm form = new WWWForm();
        form.AddField("userID", userID);
        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/assignment/getuser.php", form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                toast(www.error);
                clg(www.error);
            }
            else
            {
                //clg(www.downloadHandler.text);
                string jsonObject = www.downloadHandler.text;
                callback(jsonObject);

            }
        }
    }
    private void toast(string s)
    {
        SSTools.ShowMessage(s, SSTools.Position.bottom, SSTools.Time.twoSecond);
    }
    private void clg (string s)
    {
        Debug.Log(s);
    }
}
