using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Client : MonoBehaviour
{
    [SerializeField] private Recieve reciever;
    [SerializeField] private Send sender;
    [SerializeField] private GameObject activeManager;
    [SerializeField] private GameObject serverMessage;
    private Socket socket;
    public int? currentLobbyID;
    private string _id;
    
    private void Awake()
    {
        serverMessage.SetActive(false);
        DontDestroyOnLoad(gameObject);
    }

    public void CreateConn(string ip, int port)
    {
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        try
        {
            socket.Connect(ip, port);
            serverMessage.SetActive(false);
            Debug.Log("Connected to server\n");
            //Debug.Flush();
            reciever.SetSocket(socket);
            StartListening();
            sender.SetSocket(socket);
            RequestPlayerID();
            SceneManager.LoadScene(2);
        }
        catch (SocketException ex)
        {
            Debug.Log(ex.StackTrace);
            serverMessage.SetActive(true);
        }
    }

    public void SetManager(GameObject manager)
    {
        activeManager = manager;
    }

    private void StartListening()
    {
        reciever.StartListening();
    }

    public string GetID() => _id;

    //-------------Send Request---------------------
    public void Create_Lobby()
    {
        sender.Create_Lobby();
    }

    public void Join_Lobby(int id)
    {
        sender.Join_Lobby(id);
    }

    public void Get_Lobby_List()
    {
        sender.Lobby_List();
    }

    public void Leave_Lobby(int? id)
    {
        sender.Leave_Lobby(id);
    }

    public void Send_Ready()
    {
        sender.Send_Ready();
    }

    public void ListReadyPlayers(int? id)
    {
        sender.RequestForReadyPlayers(id);
    }
    public void Send_Jump(Int32 unixTime)
    {
        sender.Send_Jump(unixTime);
    }

    public void Request_For_Player_List(int? id)
    {
        sender.Request_For_Players_List(id);
    }

    private void RequestPlayerID()
    {
        sender.Request_Player_ID();
    }
    public void SendRequestForStartingGame()
    {
        sender.StartGame();
    }
    //----------------------------------------------
    
    
    //-------------Get Request----------------------
    public void Lobby_Creation(int id)
    {
        activeManager.GetComponent<GameManagerScript>().Get_Request_Create_Lobby(id);
        currentLobbyID = id;
    }

    public void Joining_Lobby()
    {
        activeManager.GetComponent<GameManagerScript>().Get_Request_Join_Lobby();
    }

    public void Make_Lobby_List(int lengthOfList, string[] ids)
    {
        activeManager.GetComponent<GameManagerScript>().Make_Lobby_List(lengthOfList, ids);
    }

    public void Accept_Leave_Lobby()
    {
        activeManager.GetComponent<ActionManager>().Accept_Request_Leave_Lobby();
    }

    public void Accept_Readiness()
    {
        activeManager.GetComponent<ActionManager>().Accept_Readiness();
    }

    public void StartGame(int secondsLeft)
    {
        activeManager.GetComponent<ActionManager>().StartGame(secondsLeft);
    }
    
    public void Accept_Players(string[] param)
    {
        activeManager.GetComponent<ActionManager>().Accept_player_List(param, _id);
    }
    public void LostTheGame()
    {
        activeManager.GetComponent<ActionManager>().LostTheGame();
    }

    public void SetID(string id)
    {
        _id = id;
    }

    public void Player_Add_Event(string id)
    {
        activeManager.GetComponent<ActionManager>().Player_Add_Event(id);
    }

    public void Player_Ready_Event(string id)
    {
        activeManager.GetComponent<ActionManager>().Player_Ready_Event(id);
    }

    public void Player_Delete_event(string id)
    {
        activeManager.GetComponent<ActionManager>().Player_Delete_Event(id);
    }

    public void ReceiveErrorMessage(string message)
    {
        //activeManager.GetComponent<GameManagerScript>().AddMessageToErrorLog(message);
    }

    public void ReceiveReadyPlayers(string[] arguments)
    {
        activeManager.GetComponent<ActionManager>().ReceivereadyPlayers(arguments);
    }
    public void ReceivePlayerSpawnEvent(string[] arguments)
    {
        activeManager.GetComponent<ActionManager>().ReceivePlayerSpawnEvent(arguments);
    }

    public void EventSpeedUp(float acceleration)
    {
        activeManager.GetComponent<ActionManager>().IncreaseSpeed(acceleration);
    }

    public void EventJumpPlayer(string id)
    {
        activeManager.GetComponent<ActionManager>().PlayerEventsOnGame("Jump", id);
    }

    public void EventDeathPlayer(string id)
    {
        activeManager.GetComponent<ActionManager>().PlayerEventsOnGame("Death", id);
    }
    //----------------------------------------------
}
