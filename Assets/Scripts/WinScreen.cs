using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;
using System.Collections;
using System.Collections.Generic;

public class WinScreen : MonoBehaviour
{
    [Header("Level Loader")]

    public string levelToLoad;

    private IEnumerator levelLoadDelay;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        levelLoadDelay = waitToGoNextLevel(5.0f);
        StartCoroutine(levelLoadDelay);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator waitToGoNextLevel(float delay)
    {
        yield return new WaitForSeconds(delay);
        PhotonNetwork.LoadLevel(levelToLoad);
    }
}
