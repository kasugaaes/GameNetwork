using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerScriptEarthAir : MonoBehaviourPun, IPunObservable
{


    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 7f;

    private Rigidbody2D rb;
    public bool isGrounded = false;
    public Transform spawnPoint;

    public bool isEarth = true;
    public bool isAir = false;

    public bool reachedExit = false;
    public string mainMenuScene;

    private SpriteRenderer spriteRenderer;

    [Header("Interaction Settings")]
    public Transform interactPoint; //this is referenced to an empty child in front of the player
    public float interactRange = 1f;
    private InteractableObject grabbedObject = null;

    [Header("Network Sync")]
    private Vector3 networkPosition;
    private Quaternion networkRotation;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject spawner = GameObject.Find("Point 1");
        spawnPoint = spawner.GetComponent<Transform>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        networkPosition = transform.position;
        networkRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            HandleMovement();
            HandleInteraction();
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, networkPosition, Time.deltaTime * 10f);
            transform.rotation = Quaternion.Lerp(transform.rotation, networkRotation, Time.deltaTime * 10f);
        }
    }

    void HandleMovement()
    {
        float move = Input.GetAxis("Horizontal");

        // Move left/right
        rb.linearVelocity = new Vector2(move * moveSpeed, rb.linearVelocity.y);

        // Flip character when moving left/right
        if (move > 0.1f)
            transform.rotation = Quaternion.Euler(0, 0, 0);
        else if (move < -0.1f)
            transform.rotation = Quaternion.Euler(0, 180, 0);

        // Jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isGrounded = false;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PhotonNetwork.LeaveRoom(gameObject);
            SceneManager.LoadScene(mainMenuScene);
        }
    }

    void HandleInteraction()
    {
        if (Input.GetKeyDown(KeyCode.E)) //this is the key for grab or release
        {
            if (grabbedObject == null)
            {
                Collider2D hit = Physics2D.OverlapCircle(interactPoint.position, interactRange);
                if (hit != null && hit.GetComponent<InteractableObject>())
                {
                    grabbedObject = hit.GetComponent<InteractableObject>();
                    grabbedObject.photonView.RPC("RPC_SetGrabbed", RpcTarget.AllBuffered, photonView.ViewID);
                }
            }
            else
            {
                grabbedObject.photonView.RPC("RPC_Release", RpcTarget.AllBuffered);
                grabbedObject = null;
            }
        }

        if (grabbedObject != null)
        {
            grabbedObject.transform.position = Vector3.Lerp(
                grabbedObject.transform.position,
                interactPoint.position,
                Time.deltaTime * 10f);
        }
    }



    //checks all possible collisions
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //simple ground check
        if (collision.contacts[0].normal.y > 0.5f)
        {
            isGrounded = true;
        }

        if (collision.gameObject.tag == "Exit")
        {
            reachedExit = true;
        }

        if (collision.gameObject.tag == "Transformer")
        {
            if (isAir == true)
            {
                isAir = false;
                isEarth = true;
                spriteRenderer.color = Color.green;
            }
            else if (isEarth == true)
            {
                isAir = true;
                isEarth = false;
                spriteRenderer.color = Color.yellow;
            }
        }

        if (collision.gameObject.tag == "Wind")
        {
            if (isAir == true)
            {
                rb.AddForce(Vector2.up * jumpForce * 3, ForceMode2D.Impulse);
                isGrounded = false;
            }

        }

        if (collision.gameObject.tag == "Lightning")
        {
            if (isAir == true)
            {
                this.transform.position = spawnPoint.position;
            }

        }

        if (collision.gameObject.tag == "Hazard")
        {
            this.transform.position = spawnPoint.position;
        }
    }

    // 🔹 Photon built-in sync method
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting) // Local player → send data
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else // Remote player → receive data
        {
            networkPosition = (Vector3)stream.ReceiveNext();
            networkRotation = (Quaternion)stream.ReceiveNext();
        }
    }
}
