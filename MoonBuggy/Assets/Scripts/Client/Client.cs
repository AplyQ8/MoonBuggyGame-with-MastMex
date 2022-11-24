using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Client : MonoBehaviour
{
    [SerializeField] private Recieve reciever;
    [SerializeField] private Send sender;
    [SerializeField] private GameObject gameManager;
    [SerializeField] private GameObject actionManager;
    private Socket socket;
    public int? currentLobbyID;
    
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void CreateConn()
    {
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        try
        {
            socket.Connect("26.149.21.51", 1457);
            reciever.SetSocket(socket);
            sender.SetSocket(socket);
        }
        catch (SocketException ex)
        {
            Debug.Log(ex.StackTrace);
        }
    }

    public void SetGamamanager(GameObject manager)
    {
        gameManager = manager;
    }

    public void SetActionManager(GameObject manager)
    {
        actionManager = manager;
    }

    public void StartListening()
    {
        reciever.StartListening();
    }

    //-------------Send Request---------------------
    public void Create_Lobby()
    {
        sender.Create_Lobby();
        reciever.Recieve_Lobby();
    }

    public void Join_Lobby(int id)
    {
        sender.Join_Lobby(id);
        reciever.Recieve_Request_For_Joining();
    }

    public void Get_Lobby_List()
    {
        sender.Lobby_List();
        reciever.Recieve_Lobby_List();
    }

    public void Leave_Lobby(int? id)
    {
        sender.Leave_Lobby(id);
        reciever.Recieve_Lobby_Leaving();
    }

    public void Send_Ready()
    {
        sender.Send_Ready();
        reciever.Receive_Response_For_Readiness();
    }

    public void Send_Jump(Int32 unixTime)
    {
        sender.Send_Jump(unixTime);
        reciever.Accept_Jump();
    }
    //----------------------------------------------
    
    
    //-------------Get Request----------------------
    public void Lobby_Creation(int id)
    {
        gameManager.GetComponent<GameManagerScript>().Get_Request_Create_Lobby(id);
    }

    public void Joining_Lobby()
    {
        gameManager.GetComponent<GameManagerScript>().Get_Request_Join_Lobby();
    }

    public void Make_Lobby_List(int lengthOfList, string[] ids)
    {
        gameManager.GetComponent<GameManagerScript>().Make_Lobby_List(lengthOfList, ids);
    }

    public void Accept_Leave_Lobby()
    {
        actionManager.GetComponent<ActionManager>().Accept_Request_Leave_Lobby();
    }

    public void Accept_Readiness()
    {
        actionManager.GetComponent<ActionManager>().Accept_Readiness();
    }

    public void StartGame()
    {
        actionManager.GetComponent<ActionManager>().StartGame();
    }

    public void Accept_Jump()
    {
        
    }

    public void LostTheGame()
    {
        actionManager.GetComponent<ActionManager>().LostTheGame();
    }
    //----------------------------------------------
}
