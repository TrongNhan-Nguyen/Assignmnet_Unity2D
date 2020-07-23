using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    Action<string> _createItemsCallback;
    void Start()
    {

    }

    public void CreateItems()
    {
        //StartCoroutine(Main.Instance.Server.GetUserItems(UserInfo.UserID,_createItemsCallback));
        StartCoroutine(GetUserItems("3", _createItemsCallback));

    }

    private void OnEnable()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
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
   
    IEnumerator CreateItemsRoutine (string jsonArrayString)
    {
        JSONArray jsonArray = JSON.Parse(jsonArrayString) as JSONArray ;
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
            GameObject item = Instantiate(Resources.Load("Prefabs/ItemInventory") as GameObject);
            item.transform.SetParent(this.transform);
            item.transform.localScale = Vector3.one;
            item.transform.localPosition = Vector3.zero;

            item.transform.Find("Name").GetComponent<Text>().text = itemInfoJson["name"];
            item.transform.Find("Description").GetComponent<Text>().text = itemInfoJson["description"];
            UserInfo.itemList.Add(itemInfoJson["name"]);


            Action<Sprite> getItemIconCallback = (dowloadedSprite) =>
            {
                item.transform.Find("Image").GetComponent<Image>().sprite = dowloadedSprite;
            };
            StartCoroutine(GetItemIcon(itemID, getItemIconCallback));
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
    public IEnumerator GetItemIcon(string itemID, System.Action<Sprite> callback)
    {
        WWWForm form = new WWWForm();
        form.AddField("itemID", itemID);
        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/assignment/getitem_icon.php", form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                toast(www.error);
                clg(www.error);
            }
            else
            {
                byte[] bytes = www.downloadHandler.data;
                Texture2D texture = new Texture2D(2, 2);
                texture.LoadImage(bytes);
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                callback(sprite);
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
