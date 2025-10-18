using System.Runtime.CompilerServices;
using UnityEngine;

public class PressurePlateToDisable : MonoBehaviour
{

    

    [SerializeField]
    [Header("Object References")]
    public GameObject toDisableObject;
    public bool isTriggerByPlayer;
    public bool isTriggerByBox;


    //If player or box is stepping on the pad
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player" && isTriggerByPlayer == true)
        {
            toDisableObject.SetActive(false);
        }

        if (collision.gameObject.tag == "Box" && isTriggerByBox == true)
        {
            toDisableObject.SetActive(false);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && isTriggerByPlayer == true)
        {
            toDisableObject.SetActive(true);
        }

        if (collision.gameObject.tag == "Box" && isTriggerByBox == true)
        {
            toDisableObject.SetActive(true);
        }
    }
}
