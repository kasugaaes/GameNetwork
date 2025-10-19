using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;

public class GameManager : MonoBehaviourPunCallbacks
{
    [Header("Checked if Finish")]
    public bool player1Finish = false;
    public bool player2Finish = false;

    public string currentLevel;
    public string levelToLoad;
    private int playerId;

    private IEnumerator levelLoadDelay;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Awake()
    {
        playerId = PhotonNetwork.LocalPlayer.ActorNumber;
        // Make sure this applies before joining any room
        PhotonNetwork.AutomaticallySyncScene = true;
    }


    void Start()
    {
        //set up coroutine
        levelLoadDelay = waitToGoNextLevel(2.0f);
    }

    // Update is called once per frame
    void Update()
    {
        CheckCompletion();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        //PhotonNetwork.LoadLevel(currentLevel);
    }

    public void CheckCompletion()
    {
        if (player1Finish==true && player2Finish == true && playerId == 1)
        {
            StartCoroutine(levelLoadDelay);
        }
    }

    private IEnumerator waitToGoNextLevel(float delay)
    {
        yield return new WaitForSeconds(delay);
        PhotonNetwork.LoadLevel(levelToLoad);
    }


}
