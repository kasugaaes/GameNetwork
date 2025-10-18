using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    [Header("Checked if Finish")]
    public bool player1Finish = false;
    public bool player2Finish = false;

    public string levelToLoad;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckCompletion();
    }

    public void CheckCompletion()
    {
        if (player1Finish==true && player2Finish == true)
        {
            PhotonNetwork.LoadLevel(levelToLoad);
        }
    }
}
