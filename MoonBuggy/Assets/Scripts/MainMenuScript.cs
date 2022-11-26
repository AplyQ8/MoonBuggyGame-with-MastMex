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
    [SerializeField] private TMP_Text message;

    private void Awake()
    {
        message.enabled = false;
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
            message.enabled = true;
            return;
        }
        message.enabled = false;
        client.GetComponent<Client>().CreateConn(ipText.text, Convert.ToInt32(portText.text));
        SceneManager.LoadScene(2);
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
