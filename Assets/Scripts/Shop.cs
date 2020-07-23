using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SimpleJSON;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    Action<string> _createItemsCallback;
    void Start()
    {   
        _createItemsCallback = (jsonArrayString) =>
        {
            StartCoroutine(CreateItemsRoutine(jsonArrayString));
        };
        CreateItems();
    }
    
    public void CreateItems()
    {
       StartCoroutine(GetAllItems(_createItemsCallback));
    }

    IEnumerator CreateItemsRoutine (string jsonArrayString)
    {
        // Parsing  json array strings ass array
        JSONArray jsonArray = JSON.Parse(jsonArrayString) as JSONArray ;
        for(int i = 0; i < jsonArray.Count; i++)
        {
            bool isDone = false;
            string itemID = jsonArray[i].AsObject["id"];
            JSONObject itemInfoJson = new JSONObject();
            Action<string> getItemInfoCallback = (itemInfo) =>
            {
              isDone = true;
                JSONArray tempArray = JSON.Parse(itemInfo) as JSONArray;
                itemInfoJson = tempArray[0].AsObject;
            };
            StartCoroutine(GetItem(itemID, getItemInfoCallback));

            yield return new WaitUntil(() => isDone == true);
            GameObject item = Instantiate(Resources.Load("Prefabs/ItemShop") as GameObject);
            item.transform.SetParent(this.transform);
            item.transform.localScale = Vector3.one;
            item.transform.localPosition = Vector3.zero;

            item.transform.Find("Name").GetComponent<Text>().text = itemInfoJson["name"];
            item.transform.Find("Price").GetComponent<Text>().text = "Price: " + itemInfoJson["price"];
            item.transform.Find("Description").GetComponent<Text>().text = itemInfoJson["description"];
            Button buy = item.transform.Find("Buy").GetComponent<Button>();
            buy.onClick.AddListener(() =>
            {
                StartCoroutine(buyItem(itemID, "3", itemInfoJson["name"]));
            });

            Action<Sprite> getItemIconCallback = (dowloadedSprite) =>
            {
                item.transform.Find("Image").GetComponent<Image>().sprite = dowloadedSprite;
            };
            StartCoroutine(GetItemIcon(itemID, getItemIconCallback));
        }
        //gameObject.SetActive(false);
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
    IEnumerator buyItem(string itemID, string userID,String itemName)
    {
        WWWForm form = new WWWForm();
        form.AddField("itemID", itemID);
        form.AddField("userID", userID);
        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/assignment/buyitem.php",form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                clg(www.error);
            }
            else
            {
                string result = www.downloadHandler.text;
                if(result== "You already has this item")
                {
                    toast(result);

                }
                else
                {
                    toast(result);
                    UserInfo.itemList.Add(itemName);
                }
            }
        }
    }
    public IEnumerator GetItemIcon(string itemID, Action<Sprite> callback)
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
    public IEnumerator GetAllItems(Action<string> callback)
    {
        using (UnityWebRequest www = UnityWebRequest.Get("http://localhost/assignment/get_all_items.php"))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
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
