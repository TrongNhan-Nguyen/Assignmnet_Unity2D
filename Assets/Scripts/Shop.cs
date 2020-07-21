using System;
using System.Collections;
using System.Collections.Generic;
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
            StartCoroutine(Main.Instance.Server.GetItem(itemID, getItemInfoCallback));

            yield return new WaitUntil(() => isDone == true);
            GameObject item = Instantiate(Resources.Load("Prefabs/Item") as GameObject);
            item.transform.SetParent(this.transform);
            item.transform.localScale = Vector3.one;
            item.transform.localPosition = Vector3.zero;

            item.transform.Find("Name").GetComponent<Text>().text = itemInfoJson["name"];
            item.transform.Find("Price").GetComponent<Text>().text = itemInfoJson["price"];
            item.transform.Find("Description").GetComponent<Text>().text = itemInfoJson["description"];


            Action<Sprite> getItemIconCallback = (dowloadedSprite) =>
            {
                item.transform.Find("Image").GetComponent<Image>().sprite = dowloadedSprite;
            };
            StartCoroutine(Main.Instance.Server.GetItemIcon(itemID, getItemIconCallback));
        }
        gameObject.SetActive(false);
    }
    public IEnumerator GetAllItems(System.Action<string> callback)
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
    private void clg(string s)
    {
        Debug.Log(s);
    }
}
