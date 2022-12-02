using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Recieve : MonoBehaviour
{
    private Socket socket;
    [SerializeField] private Client client;
    [SerializeField] private static Threadmanager _threadManager;
    private static Thread testThread = null;
    private bool _isListen = true;

    private void Awake()
    {
        _threadManager = GetComponent<Threadmanager>();
    }

    public void StartListening()
    {
        testThread = new Thread(new ThreadStart(ThreadAction));
        testThread.Start();
    }
    public void SetSocket(Socket _socket)
    {
        socket = _socket;
    }

    private void ThreadAction()
    {
        while (_isListen)
        {
            byte[] bytes = new byte[1024];
            int bytesRec;
            try
            {
                 bytesRec = socket.Receive(bytes);
                 String res = Encoding.UTF8.GetString(bytes, 0, bytesRec);
                 String[] commands = res.Split("\r\n\r\n");
                 for (int i = 0; i < commands.Length - 1; i++)
                 {
                     String[] param = commands[i].Split(" ");
                     switch ((Status)Enum.Parse(typeof(Status), param[0]))
                     {
                         case Status.OK:
                             SwitchFunc(param);
                             break;
                         case Status.ERROR:
                             _threadManager.ExecuteOnMainThread(()=> {ReceiveErrorMessage(param);});
                             break;
                     }
               
                 }
            }
            catch (SocketException sEx)
            {
                _isListen = false;
                _threadManager.ExecuteOnMainThread(()=> LostConnection(testThread, socket));
            }
        }
    }
    enum Status
    {
        OK,
        ERROR,
        DEATH
    }
    private void SwitchFunc(string[] arguments)
    {
        switch (arguments[1])
        {
            case "/create_lobby":
                _threadManager.ExecuteOnMainThread(() => { Recieve_Lobby(Convert.ToInt32(arguments[2])); });
                break;
            case "/list_lobby":
                _threadManager.ExecuteOnMainThread(() => { Recieve_Lobby_List(arguments); });
                break;
            case "/join_lobby":
                _threadManager.ExecuteOnMainThread(() => { Recieve_Request_For_Joining(); });
                break;
            case "/leave_lobby":
                _threadManager.ExecuteOnMainThread(() => { Recieve_Lobby_Leaving(); });
                break;
            case "/list_players":
                _threadManager.ExecuteOnMainThread(() => { Receive_Player_List(arguments); });
                break;
            case "/ready_to_play":
                _threadManager.ExecuteOnMainThread(() => { Receive_Response_For_Readiness(); });
                break;
            case "/get_player_id":
                _threadManager.ExecuteOnMainThread(()=> { Receive_Player_ID(arguments[2]);});
                break;
            case "/player_add_event":
                _threadManager.ExecuteOnMainThread(()=>{Player_Add_Event(arguments[2]);});
                break;
            case "/player_ready_event":
                _threadManager.ExecuteOnMainThread(()=>{Player_Ready_Event(arguments[2]);});
                break;
            case "/player_delete_event":
                _threadManager.ExecuteOnMainThread(()=>{Player_Delete_Event(arguments[2]);});
                break;
            case "/list_ready_players":
                _threadManager.ExecuteOnMainThread(()=>{ReceiveReadyPlayers(arguments);});
                break;
            case "/start_game_event":
                _threadManager.ExecuteOnMainThread(() => { StartGame(Convert.ToDouble(arguments[2])); });
                break;
            case "/map_event":
                _threadManager.ExecuteOnMainThread(() => { ReceivePlayerSpawnEvent(arguments);});
                break;
            case "/speed_up_event":
                _threadManager.ExecuteOnMainThread(() => {EventSpeedUp(Convert.ToSingle(arguments[2]));});
                break;
            case "/player_jump_event":
                _threadManager.ExecuteOnMainThread(()=> EventJumpPlayer(arguments[2]));
                break;
            case "/player_death_event":
                _threadManager.ExecuteOnMainThread(()=> EventDeathPlayer(arguments[2]));
                break;
            case "/Death":
                _threadManager.ExecuteOnMainThread(ReceiveDeath);
                break;
        }
    }

    private void LostConnection(Thread thread, Socket _socket)
    {
        thread.Abort();
        _socket.Close();
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }

    private void Player_Add_Event(string id)
    {
        client.Player_Add_Event(id);
    }

    private void Player_Ready_Event(string id)
    {
        client.Player_Ready_Event(id);
    }

    private void Player_Delete_Event(string id)
    {
        client.Player_Delete_event(id);
    }
    
    private void Recieve_Lobby(int id)
    {
        client.Lobby_Creation(id);
    }

    private void Recieve_Request_For_Joining()
    {
        client.Joining_Lobby();
    }

    private void Recieve_Lobby_List(string[] param)
    {
        client.Make_Lobby_List(param.Length, param);
    }

    private void Recieve_Lobby_Leaving()
    {
        client.Accept_Leave_Lobby();
    }

    private void Receive_Response_For_Readiness()
    {
        client.Accept_Readiness();
    }

    private void Receive_Player_List(string[] param)
    {
        client.Accept_Players(param);
    }

    private void ReceiveErrorMessage(string[] message)
    {
        string errorMessage = "";
        for (int i = 0; i < message.Length; i++)
        {
            errorMessage += message[i];
        }
        client.ReceiveErrorMessage(errorMessage);
    }
    
    private void Receive_Player_ID(string id)
    {
        client.SetID(id);
    }

    private void StartGame(double unixTime)
    {
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds( unixTime ).ToLocalTime();
        DateTime now = DateTime.Now;
        int secondsLeft = Convert.ToInt32((dateTime - now).TotalSeconds);
        //Debug.Log($"Received message at {now} to start game at {dateTime} => Left: {secondsLeft}");
        client.StartGame(unixTime);
    }

    private void ReceiveReadyPlayers(string[] param)
    {
        client.ReceiveReadyPlayers(param);
    }

    private void ReceivePlayerSpawnEvent(string[] arguments)
    {
        // string mes = "";
        // for (int i = 0; i < arguments.Length; i++)
        // {
        //     mes += arguments[i] + " ";
        // }
        // Debug.Log(mes);
        client.ReceivePlayerSpawnEvent(arguments);
    }

    private void EventSpeedUp(float speed)
    {
        client.EventSpeedUp(speed);
    }

    private void EventJumpPlayer(string id)
    {
        client.EventJumpPlayer(id);
    }

    private void EventDeathPlayer(string id)
    {
        client.EventDeathPlayer(id);
    }

    private void ReceiveDeath()
    {
        client.LostTheGame();
    }
}
