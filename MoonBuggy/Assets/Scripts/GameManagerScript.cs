using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
    [SerializeField] private GameObject tcpClient;
    [SerializeField] private Client client;
    [SerializeField] private GameObject content;
    [SerializeField] private GameObject lobbyPrefab;
    

    private void Awake()
    {
        tcpClient = GameObject.Find("Client");
        client = tcpClient.GetComponent<Client>();
        client.SetGamamanager(gameObject);
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
    }

    public void Get_Request_Join_Lobby()
    {
        SceneManager.LoadScene(3);
    }

    public void Make_Lobby_List(int length, string[] ids)
    {
        // foreach (var child in content.GetComponentsInChildren<Transform>())
        // {
        //     Destroy(child.gameObject);
        // }
        for (int i = 2; i < length; i++)
        {
            if(ids[i] != "")
                Get_Request_Create_Lobby(Convert.ToInt32(ids[i]));
        }
    }
    //----------------------------------------------
}
