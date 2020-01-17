using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MultiplayerLoad : MonoBehaviourPunCallbacks
{
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("2 players have connected. Loading level and respawning players in room ");
        if (PhotonNetwork.InRoom && PhotonNetwork.PlayerList.Length == 2)
        {
            // PhotonNetwork.LoadLevel("GameScene");
            SceneManager.LoadScene("GameScene");
            FindObjectOfType<NetworkController>().CreatePlayer();
        }
    }

    public void cancel()
    {
        PhotonNetwork.Disconnect();
        if (GameObject.Find("ColorToggle") != null)
            Destroy(GameObject.Find("ColorToggle"));
        Destroy(GameObject.Find("NetworkController"));
        SceneManager.LoadScene("MenuScene");
    }
}
