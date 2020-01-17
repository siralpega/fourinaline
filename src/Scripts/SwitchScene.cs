using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class SwitchScene : MonoBehaviour
{
    public void s()
    {
        if (SceneManager.GetActiveScene().name == "MenuScene")
            SceneManager.LoadScene("GameScene");
        else
        {
            if (GameObject.Find("NetworkController") != null)
                Destroy(GameObject.Find("NetworkController"));
            if (GameObject.Find("Player 1") != null)
                Destroy(GameObject.Find("Player 1"));
            if (GameObject.Find("Player 2") != null)
                Destroy(GameObject.Find("Player 2"));
            if (GameObject.Find("ColorToggle") != null)
                Destroy(GameObject.Find("ColorToggle"));
            if (PhotonNetwork.InRoom)
                PhotonNetwork.LeaveRoom();
            SceneManager.LoadScene("MenuScene");
        }
    }

    public void s(string name)
    {
        SceneManager.LoadScene(name);
    }
}
