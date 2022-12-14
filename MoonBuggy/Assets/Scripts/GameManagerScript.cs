using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
    [SerializeField] private GameObject tcpClient;
    [SerializeField] private Client client;
    [SerializeField] private GameObject content;
    [SerializeField] private GameObject lobbyPrefab;
    [SerializeField] private TMP_Text errorMessage;
    [SerializeField] private GameObject errorLog;
    [SerializeField] private List<GameObject> lobbyList = new List<GameObject>();

    private void Awake()
    {
        tcpClient = GameObject.Find("Client");
        client = tcpClient.GetComponent<Client>();
        client.SetManager(gameObject);
        Send_request_For_Lobby_List();
    }
    
    //-------------Send Request---------------------
    public void Send_Request_Create_Lobby()
    {
        client.Create_Lobby();
    }

    public void Send_Request_Join_Lobby(int id)
    {
        client.currentLobbyID = id;
        client.Join_Lobby(id);
    }

    public void Send_request_For_Lobby_List()
    {
        client.Get_Lobby_List();
    }
    //----------------------------------------------
    
    
    //-------------Get Request----------------------
    public void Get_Request_Create_Lobby(int id)
    {
        GameObject lobby = Instantiate(lobbyPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        lobby.transform.SetParent(content.transform);
        lobby.transform.localScale = new Vector3(1, 1, 1);
        lobby.GetComponent<LobbyPref_Script>().SetInformation(id);
        lobbyList.Add(lobby);
        SceneManager.LoadScene(3);
    }

    public void Get_Request_Join_Lobby()
    {
        SceneManager.LoadScene(3);
    }

    public void Make_Lobby_List(int length, string[] ids)
    {
        for (int i = 0; i < lobbyList.Count; i++)
        {
            Destroy(lobbyList[i]);
            lobbyList.Remove(lobbyList[i]);
        }
        for (int i = 2; i < length; i++)
        {
            if (ids[i] != "")
            {
                GameObject lobby = Instantiate(lobbyPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                lobby.transform.SetParent(content.transform);
                lobby.transform.localScale = new Vector3(1, 1, 1);
                lobby.GetComponent<LobbyPref_Script>().SetInformation(Convert.ToInt32(ids[i]));
                lobbyList.Add(lobby);
            }
                //Get_Request_Create_Lobby(Convert.ToInt32(ids[i]));
        }
    }

    public void AddMessageToErrorLog(string message)
    {
        TMP_Text errorMsg = Instantiate(errorMessage, new Vector3(0, 0, 0), Quaternion.identity);
        errorMsg.text = message;
        errorMsg.transform.SetParent(errorLog.transform);
        errorMsg.transform.localScale = new Vector3(1, 1, 1);
    }
    //----------------------------------------------
}
