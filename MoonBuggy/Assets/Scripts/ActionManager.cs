using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = System.Random;

public class ActionManager : MonoBehaviour
{
    [SerializeField] private GameObject wall;
    [SerializeField] private GameObject startBtn;
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
    [SerializeField] private GameObject backCount;
    [SerializeField] private GameObject backGround;
    [SerializeField] private Spawner spawner;
    [SerializeField] private GameObject playerSpawnPos;
    [SerializeField] private GameObject startGameBTN;
    [SerializeField] private int? lobbyID;
    private List<GameObject> enemies = new List<GameObject>();
    private TMP_Text backCountText;
    private float _currentSpeed = 5f;
    private Vector3 _playerPos;
    
    
    private void Awake()
    {
        tcpClient = GameObject.Find("Client");
        client = tcpClient.GetComponent<Client>();
        lobbyID = client.currentLobbyID;
        client.SetManager(gameObject);
        List_Players();
        RequestForReadyPlayers();
        lostMessage.enabled = false;
        backCount.SetActive(false);
        backCountText = backCount.GetComponent<TMP_Text>();
        backGround.GetComponent<MoveBackground>().enabled = false;
        player.GetComponent<BuggyScript>().enabled = false;
        _playerPos = player.transform.position;
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
    private void RequestForReadyPlayers()
    {
        client.ListReadyPlayers(lobbyID);
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

    public void StartGame(double unixTime)
    {
        startBtn.SetActive(false);
        readyBtn.SetActive(false);
        backCount.SetActive(true);
        StartCoroutine(BackCount(SecondsLeft(unixTime)));
        //BackCountWithThread(SecondsLeft(unixTime));
        
    }

    public void SendRequestForStartingGame()
    {
        client.SendRequestForStartingGame();
    }
    IEnumerator BackCount(int seconds)
    {
        while (seconds > 0)
        {
            backCountText.text = seconds.ToString();
            seconds--;
            yield return new WaitForSeconds(1f);
        }
        player.GetComponent<BuggyScript>().enabled = true;
        backCount.SetActive(false);
        backGround.GetComponent<MoveBackground>().enabled = true;
        startGameBTN.SetActive(false);
        
    }
    public void LostTheGame()
    {
        player.GetComponent<BuggyScript>().enabled = false;
        lostMessage.enabled = true;
    }

    public void ReceivereadyPlayers(string[] param)
    {
        for (int i = 2; i < param.Length; i++)
        {
            foreach (var enemy in enemies)
            {
                if(enemy.GetComponent<EnemyScript>().CheckID(param[i]))
                    enemy.GetComponent<EnemyScript>().ChangeStatus();
            }
        }
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
        enemies.Add(enemy);
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
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i].GetComponent<EnemyScript>().CheckID(id))
            {
                Destroy(enemies[i]);
                enemies.Remove(enemies[i]);
                break;

            }
        }
    }
    public void IncreaseSpeed(float acceleration)
    {
        _currentSpeed += acceleration;
        backGround.GetComponent<MoveBackground>().SetSpeed(_currentSpeed);
    }

    public void ReceivePlayerSpawnEvent(string[] arguments)
    {
        for (int i = 2; i < arguments.Length-1; i += 2)
        {
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds( Convert.ToDouble(arguments[i + 1]) ).ToLocalTime();
            Debug.Log($"Received message to spawn {arguments[i]} at {dateTime}");
            var seconds = SecondsLeft(Convert.ToDouble(arguments[i + 1]));
            var spawnPos = _currentSpeed * seconds;
            spawner.SpawnBarrier(
                new Vector3(_playerPos.x + spawnPos, _playerPos.y - 0.4f, 0),
                wall.transform.position,
                arguments[i],
                _currentSpeed);
            foreach (var enemy in enemies)
            {
                var enemyScript = enemy.GetComponent<EnemyScript>();
                spawner.SpawnBarrier(
                    new Vector3(enemyScript.ReturnEnemyXPos()+spawnPos, enemyScript.ReturnEnemyYPos() - 0.4f, 0),
                    enemy.GetComponent<EnemyScript>().ReturnWallPos(),
                    arguments[i],
                    _currentSpeed);
            }
            backGround.GetComponent<MoveBackground>().SetSpeed(_currentSpeed);
        }
    }
    
    
//---------------------------------------------------  
    private int SecondsLeft(double unixTime)
    {
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds( unixTime ).ToLocalTime();
        
        DateTime now = DateTime.Now;
        int secondsLeft = Convert.ToInt32((dateTime - now).TotalSeconds);
        return secondsLeft;
    }

    private IEnumerator WaitForCall(int seconds, Vector3 spawnPos,Vector3 target, string type, float speed)
    {
        while (seconds > 0)
        {
            seconds--;
            yield return new WaitForSeconds(1f);
        }
        spawner.SpawnBarrier(spawnPos,target, type, speed);
        foreach (var enemy in enemies)
        {
            spawner.SpawnBarrier(
                enemy.GetComponent<EnemyScript>().ReturnSpawnPos(),
                enemy.GetComponent<EnemyScript>().ReturnWallPos(),
                type, speed);
        }
        
    }
}
