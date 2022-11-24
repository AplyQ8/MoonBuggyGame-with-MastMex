using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField] private GameObject client;
    public void SoloGaming()
    {
        SceneManager.LoadScene(1);
    }

    public void Multiplayer()
    {
        client.GetComponent<Client>().CreateConn();
        SceneManager.LoadScene(2);
    }
    public void Quit()
    {
        Application.Quit();
    }
}
