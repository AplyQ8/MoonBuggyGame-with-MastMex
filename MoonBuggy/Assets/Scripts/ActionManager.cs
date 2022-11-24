using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ActionManager : MonoBehaviour
{
    [SerializeField] private GameObject tcpClient;
    [SerializeField] private Client client;
    [SerializeField] private Sprite checkMark;
    [SerializeField] private GameObject pointer;
    [SerializeField] private GameObject readyBtn;
    [SerializeField] private TMP_Text waitingForOthers;
    [SerializeField] private GameObject player;
    [SerializeField] private TMP_Text lostMessage;
    [SerializeField] private int? lobbyID;
    
    
    private void Awake()
    {
        tcpClient = GameObject.Find("Client");
        client = tcpClient.GetComponent<Client>();
        client.SetActionManager(gameObject);
        lobbyID = client.currentLobbyID;
        client.StartListening();
        lostMessage.enabled = false;
    }
//-------------Send Requests-------------------------
    public void SendRequest_for_Leaving_Lobby()
    {
        client.Leave_Lobby(lobbyID);
    }

    public void Send_Ready()
    {
        client.Send_Ready();
    }

    public void Send_Jump(Int32 unixTime)
    {
        client.Send_Jump(unixTime);
    }
//---------------------------------------------------

//-------------Get Requests--------------------------
    public void Accept_Request_Leave_Lobby()
    {
        SceneManager.LoadScene(2);
    }

    public void Accept_Readiness()
    {
        pointer.GetComponent<Image>().sprite = checkMark;
        waitingForOthers.enabled = true;
    }

    public void StartGame()
    {
        player.GetComponent<BuggyScript>().enabled = true;
        readyBtn.SetActive(false);
        waitingForOthers.enabled = false;
    }

    public void LostTheGame()
    {
        player.GetComponent<BuggyScript>().enabled = false;
        lostMessage.enabled = true;
    }
//---------------------------------------------------    
}
