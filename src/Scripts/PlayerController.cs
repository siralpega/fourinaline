using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class PlayerController : MonoBehaviour
{
    public GameObject chipPrefab, arrowPrefab, arrow;
    public BoardManager bm;
    public int player;
    public Color color;
    private int column = 4;
    private readonly float width = 1.17f;
    private PhotonView photonView;
    private Text textObject;

    void Start()
    {
        bm = FindObjectOfType<BoardManager>();
        photonView = GetComponent<PhotonView>();
        DontDestroyOnLoad(this);
    }

    void Update()
    {
        if (this.gameObject.tag != "Player")
            return;
        if (bm == null || textObject == null || photonView == null)
        {
            if (bm == null)
                bm = FindObjectOfType<BoardManager>();
            if (textObject == null)
            {
                if (GameObject.FindGameObjectWithTag("TurnText") == null) //doesn't load in sync/fast enough
                    return;
                else
                    textObject = GameObject.FindGameObjectWithTag("TurnText").GetComponent<Text>();
            }
            if (photonView == null)
                photonView = GetComponent<PhotonView>();
            return;
        }
        if (!bm.isTurn(player))
            return;
        else if (arrow == null)
        {
            float x = 0;
            if (column < 4)
                x = -width * (4 - column);
            else if (column > 4)
                x = width * (column - 4);
            arrow = Instantiate(arrowPrefab, new Vector2(x, 4), Quaternion.identity, this.gameObject.transform);
            textObject.text = "Your Turn";
        }

        //Input
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (column >= 7)
                return;
            column++;
            arrow.transform.Translate(width, 0, 0);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            if (column <= 1)
                return;
            column--;
            arrow.transform.Translate(-width, 0, 0);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            textObject.text = "Opponent's Turn";
            photonView.RPC("PlaceChipRPC", RpcTarget.All, player, column, arrow.transform.position);
            Destroy(arrow);
            //  column = 4;
        }
    }

    [PunRPC]
    void PlaceChipRPC(int p, int col, Vector3 pos)
    {
        GameObject c = chipPrefab;
        c.GetComponent<Chip>().owner = p;
        c.GetComponent<Chip>().column = col - 1;
        c.GetComponent<Chip>().color = GameObject.Find("Player " + p).GetComponent<PlayerController>().color;
        c = Instantiate(c, pos, Quaternion.identity);
        bm.addChip(c.GetComponent<Chip>());
        /*    if (photonView.IsMine)
            {

            } */
    }

    [PunRPC]
    public void AssignPlayersRPC(int[] rgb, int id)
    {
        GameObject p = GameObject.Find("Player(Clone)");
        p.name = "Player " + id;
        p.GetComponent<PlayerController>().player = id;
        p.GetComponent<PlayerController>().color = new Color32((byte)rgb[0], (byte)rgb[1], (byte)rgb[2], 255);
    }
}
