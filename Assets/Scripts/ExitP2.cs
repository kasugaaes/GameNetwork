using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class ExitP2 : MonoBehaviour
{
    public GameManager gameManager;

    void Start()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Debug.Log("Right Player Finished");
            gameManager.player2Finish = true;
        }
    }
}
