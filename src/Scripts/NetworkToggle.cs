using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NetworkToggle : MonoBehaviour
{
    static float HEIGHT_DIFF = 30, WIDTH_DIFF = 60, START_X = -30, START_Y = 50;

    public static void toggle(List<RoomInfo> list)
    {
        GameObject canvas = GameObject.Find("Canvas");
        for (int i = 0; i < list.Count; i++)
        {
            Vector3 pos = new Vector3(START_X, START_Y + (HEIGHT_DIFF * i), 0);
            GameObject textObj = new GameObject("GameInfo" + i);
            textObj.transform.SetParent(canvas.transform);
            textObj.transform.SetPositionAndRotation(canvas.transform.position + pos, Quaternion.identity);
            textObj.AddComponent<Text>().text = "Game " + i;
            textObj.GetComponent<Text>().font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            textObj.GetComponent<Text>().fontSize = 16;

            pos.Set(START_X + WIDTH_DIFF, pos.y, pos.z);
            textObj = new GameObject("GameInfoPlayers" + i);
            textObj.transform.SetParent(canvas.transform);
            textObj.transform.SetPositionAndRotation(canvas.transform.position + pos, Quaternion.identity);
            textObj.AddComponent<Text>().text = "(" + list[i].PlayerCount + "/" + list[i].MaxPlayers + ")";
            textObj.GetComponent<Text>().font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            textObj.GetComponent<Text>().fontSize = 16;

            pos.Set(0 + 20, pos.y - 7, pos.z);
            GameObject buttonObj = new GameObject("JoinGameButton" + i);
            buttonObj.transform.SetParent(canvas.transform);
            buttonObj.transform.SetPositionAndRotation(textObj.transform.position + pos, Quaternion.identity);
            buttonObj.AddComponent<Button>().onClick.AddListener(delegate { JoinGame(i); });
            buttonObj.AddComponent<RectTransform>().sizeDelta = new Vector2(60, 25);
            buttonObj.AddComponent<Image>();
            buttonObj.GetComponent<Button>().targetGraphic = buttonObj.GetComponent<Image>();
            textObj = new GameObject("Text");
            textObj.transform.SetParent(buttonObj.transform);
            pos.Set(35, -42, pos.z);
            textObj.transform.SetPositionAndRotation(buttonObj.transform.position + pos, Quaternion.identity);
            textObj.AddComponent<Text>().text = "Join";
            textObj.GetComponent<Text>().font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            textObj.GetComponent<Text>().color = Color.black;
        }
    }

    static void JoinGame(int i)
    {
        GameObject.FindObjectOfType<NetworkController>().GetComponent<NetworkController>().joinGame(i - 1);
    }


}
