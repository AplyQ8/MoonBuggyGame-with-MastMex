using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class Send : MonoBehaviour
{
    private Socket socket;

    public void SetSocket(Socket _socket)
    {
        socket = _socket;
    }

    public void Create_Lobby()
    {
        string message = "/create_lobby\r\n\r\n";
        byte[] requestData = Encoding.UTF8.GetBytes(message);
        socket.Send(requestData);
    }

    public void Join_Lobby(int id)
    {
        string message = $"/join_lobby {id}\r\n\r\n";
        byte[] requestData = Encoding.UTF8.GetBytes(message);
        socket.Send(requestData);
    }

    public void Lobby_List()
    {
        string message = "/list_lobby\r\n\r\n";
        byte[] requestData = Encoding.UTF8.GetBytes(message);
        SocketAsyncEventArgs e = new SocketAsyncEventArgs();
        e.SetBuffer(requestData, 0, requestData.Length);
        // 
        socket.SendAsync(e);
    }

    public void Leave_Lobby(int? id)
    {
        string message = $"/leave_lobby {id}\r\n\r\n";
        byte[] requestData = Encoding.UTF8.GetBytes(message);
        socket.Send(requestData);
    }

    public void Send_Ready()
    {
        string message = "/ready_to_play\r\n\r\n";
        byte[] requestData = Encoding.UTF8.GetBytes(message);
        socket.Send(requestData);
    }

    public void Send_Jump(Int32 unixTime)
    {
        string message = $"/jump {unixTime}\r\n\r\n";
        byte[] requestData = Encoding.UTF8.GetBytes(message);
        socket.Send(requestData);
    }

    public void Request_For_Players_List(int? id)
    {
        string message = $"/list_players {id}\r\n\r\n";
        byte[] requestData = Encoding.UTF8.GetBytes(message);
        socket.Send(requestData);
    }
    public void Request_Player_ID()
    {
        string message = $"/get_player_id\r\n\r\n";
        byte[] requestData = Encoding.UTF8.GetBytes(message);
        socket.Send(requestData);
    }
}
