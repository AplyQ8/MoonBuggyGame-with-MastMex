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
    [SerializeField] private GameObject enemyField;
    [SerializeField] private GameObject enemyPref;
    [SerializeField] private int? lobbyID;
    private List<GameObject> enemies = new List<GameObject>();
    
    
    private void Awake()
    {
        tcpClient = GameObject.Find("Client");
        client = tcpClient.GetComponent<Client>();
        lobbyID = client.currentLobbyID;
        client.SetActionManager(gameObject);
        List_Players();
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

    private void List_Players()
    {
        client.Request_For_Player_List(lobbyID);
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

    public void Accept_player_List(string[] param, string playerID)
    {
        for (int i = 3; i < param.Length - 1; i++)
        {
            if (param[i] != playerID)
            {
                GameObject enemy = Instantiate(enemyPref, new Vector3(0, 0, 0), Quaternion.identity);
                enemy.transform.SetParent(enemyField.transform);
                enemy.transform.localScale = new Vector3(1, 1, 1);
                enemy.GetComponent<EnemyScript>().SetID(param[i]);
                enemies.Add(enemy);
            }
        }
    }

    public void Player_Add_Event(string id)
    {
        GameObject enemy = Instantiate(enemyPref, new Vector3(0, 0, 0), Quaternion.identity);
        enemy.transform.SetParent(enemyField.transform);
        enemy.transform.localScale = new Vector3(1, 1, 1);
        enemy.GetComponent<EnemyScript>().SetID(id);
    }

    public void Player_Ready_Event(string id)
    {
        foreach (var enemy in enemies)
        {
            if (enemy.GetComponent<EnemyScript>().CheckID(id))
                enemy.GetComponent<EnemyScript>().ChangeStatus();
        }
    }

    public void Player_Delete_Event(string id)
    {
        foreach (var enemy in enemies)
        {
            if (enemy.GetComponent<EnemyScript>().CheckID(id))
            {
                enemies.Remove(enemy);
                Destroy(enemy);
            }
        }
    }
//---------------------------------------------------    
}
