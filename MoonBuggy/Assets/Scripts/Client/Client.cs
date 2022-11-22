using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Client : MonoBehaviour
{
    [SerializeField] private Recieve reciever;
    [SerializeField] private Send sender;
    [SerializeField] private GameObject gameManager;
    
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void SetGamamanager(GameObject manager)
    {
        gameManager = manager;
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
    //----------------------------------------------
}
