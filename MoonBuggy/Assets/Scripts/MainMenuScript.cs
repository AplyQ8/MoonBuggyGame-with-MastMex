using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField] private GameObject client;
    [SerializeField] private GameObject connectMenu;
    [SerializeField] private TMP_InputField portText;
    [SerializeField] private TMP_InputField ipText;
    [SerializeField] private GameObject message;
    [SerializeField] private GameObject serverMessage;

    private void Awake()
    {
        message.SetActive(false);
        connectMenu.SetActive(false);
    }
    public void SoloGaming()
    {
        SceneManager.LoadScene(1);
    }

    public void Multiplayer()
    {
        if (portText.text == "" || ipText.text == "")
        {
            serverMessage.SetActive(false);
            message.SetActive(true);
            return;
        }
        message.SetActive(false);
        client.GetComponent<Client>().CreateConn(ipText.text, Convert.ToInt32(portText.text));
    }

    public void OpenMenu()
    {
        connectMenu.SetActive(true);
    }

    public void CloseMenu()
    {
        connectMenu.SetActive(false);
    }
    public void Quit()
    {
        Application.Quit();
    }
}
