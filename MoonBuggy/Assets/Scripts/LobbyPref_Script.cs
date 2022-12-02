using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyPref_Script : MonoBehaviour
{
    [SerializeField] private Button join_btn;
    [SerializeField] private TMP_Text id;
    [SerializeField] private GameObject gameManager;
    private int lobby_id;

    private void Awake()
    {
        gameManager = GameObject.Find("GameManager");
        gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(604.47f, 138.38f);
    }

    public void SetInformation(int _id)
    {
        lobby_id = _id;
        id.text = _id.ToString();
    }

    public void Join_Lobby()
    {
        gameManager.GetComponent<GameManagerScript>().Send_Request_Join_Lobby(lobby_id);
    }
}
