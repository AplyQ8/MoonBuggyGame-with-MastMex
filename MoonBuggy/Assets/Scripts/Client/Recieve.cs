using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class Recieve : MonoBehaviour
{
    private Socket socket;
    [SerializeField] private Client client;
    [SerializeField] private static Threadmanager _threadManager;

    private void Awake()
    {
        _threadManager = GetComponent<Threadmanager>();
    }

    public void StartListening()
    {
        
    }
    public void SetSocket(Socket _socket)
    {
        socket = _socket;
    }

    private void ThreadAction()
    {
        while (true)
        {
            byte[] bytes = new byte[1024];
            int bytesRec = socket.Receive(bytes);
            String res = Encoding.UTF8.GetString(bytes, 0, bytesRec);
            String[] commands = res.Split(@"\r\n\r\n");
            for (int i = 0; i < commands.Length - 1; i++)
            {
                String[] param = commands[i].Split("_");
                SwitchFunc(param);
            }
        }
    }

    private void SwitchFunc(string[] arguments)
    {
        switch (arguments[0])
        {
            case "/start_game":
                _threadManager.ExecuteOnMainThread(() => { StartGame(); });
                break;
            case "/get_map":
                break;
        }
    }
    
    enum Status
    {
        OK,
        ERROR,
        DEATH
    }
    

    public void Recieve_Lobby()
    {
        byte[] bytes = new byte[1024];
        int bytesRec = socket.Receive(bytes);
        String res = Encoding.UTF8.GetString(bytes, 0, bytesRec);
        String[] commands = res.Split(@"\r\n\r\n");
        String[] param = commands[0].Split(" ");
        switch ((Status)Enum.Parse(typeof(Status), param[0]))
        {
            case Status.OK:
                client.Lobby_Creation(Convert.ToInt32(param[1]));
                break;
            case Status.ERROR:
                Debug.Log($"{param[0]} {param[1]}");
                break;
        }
    }

    public void Recieve_Request_For_Joining()
    {
        byte[] bytes = new byte[1024];
        int bytesRec = socket.Receive(bytes);
        String res = Encoding.UTF8.GetString(bytes, 0, bytesRec);
        String[] commands = res.Split(@"\r\n\r\n");
        String[] param = commands[0].Split(" ");
        switch ((Status)Enum.Parse(typeof(Status), param[0]))
        {
            case Status.OK:
                client.Joining_Lobby();
                break;
            case Status.ERROR:
                client.currentLobbyID = null;
                Debug.Log($"{param[0]} {param[1]}");
                break;
        }
    }

    public void Recieve_Lobby_List()
    {
        byte[] bytes = new byte[1024];
        int bytesRec = socket.Receive(bytes);
        String res = Encoding.UTF8.GetString(bytes, 0, bytesRec);
        String[] commands = res.Split(@"\r\n\r\n");
        String[] param = commands[0].Split(" ");
        switch ((Status)Enum.Parse(typeof(Status), param[0]))
        {
            case Status.OK:
                client.Make_Lobby_List(param.Length, param);
                break;
            case Status.ERROR:
                Debug.Log($"{param[0]}: can not get a list of lobby...");
                break;
        }
    }

    public void Recieve_Lobby_Leaving()
    {
        byte[] bytes = new byte[1024];
        int bytesRec = socket.Receive(bytes);
        String res = Encoding.UTF8.GetString(bytes, 0, bytesRec);
        String[] commands = res.Split(@"\r\n\r\n");
        String[] param = commands[0].Split(" ");
        switch ((Status)Enum.Parse(typeof(Status), param[0]))
        {
            case Status.OK:
                client.Accept_Leave_Lobby();
                break;
            case Status.ERROR:
                Debug.Log($"{param[0]}: {param[1]}");
                break;
        }
    }

    public void Receive_Response_For_Readiness()
    {
        byte[] bytes = new byte[1024];
        int bytesRec = socket.Receive(bytes);
        String res = Encoding.UTF8.GetString(bytes, 0, bytesRec);
        String[] commands = res.Split(@"\r\n\r\n");
        String[] param = commands[0].Split(" ");
        switch ((Status)Enum.Parse(typeof(Status), param[0]))
        {
            case Status.OK:
                client.Accept_Readiness();
                break;
            case Status.ERROR:
                Debug.Log($"{param[0]}: {param[1]}");
                break;
        }
    }

    public void Accept_Jump()
    {
        byte[] bytes = new byte[1024];
        int bytesRec = socket.Receive(bytes);
        String res = Encoding.UTF8.GetString(bytes, 0, bytesRec);
        String[] commands = res.Split(@"\r\n\r\n");
        String[] param = commands[0].Split(" ");
        switch ((Status)Enum.Parse(typeof(Status), param[0]))
        {
            case Status.OK:
                client.Accept_Jump();
                break;
            case Status.DEATH:
                client.LostTheGame();
                break;
        }
    }

    private void StartGame()
    {
        client.StartGame();
    }
}
