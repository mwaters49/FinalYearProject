/* 
 * Matthew Waters
 * Player Movement script
 */


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.UIElements.GraphView;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class movement : MonoBehaviour
{
    public CharacterController characterController;
    public Transform drawOnGroundSphere;
    public Transform playerRotate;
    public LayerMask groundTag;
    public GameObject taggedPlayer;

    public Text Score;

    public float speed;
    public float sensitivity = 0.2f;

    float gravity = -19f;
    float jumpHeight = 1.5f;
    float onGroundSphereRadius = 0.4f;

    Vector3 velocity;

    bool isOnGround;
    int scoreCount = 0;

    private void Start()
    {
        Debug.Log("Hello");
    }
    

    void Update()
    { 
        float leftRight = Input.GetAxis("Horizontal"); //A and D keys
        float forwardBack = Input.GetAxis("Vertical");   // W and S keys
        float mouseXPos = Input.GetAxis("Mouse X");

        isOnGround = Physics.CheckSphere(drawOnGroundSphere.position, onGroundSphereRadius, groundTag);
        
        if(isOnGround)
        {
            velocity.y = 0f;
            speed = 6f;
        }
        else
        {
            speed = 3f;
        }

        Vector3 movementDirection = transform.forward * forwardBack + transform.right * leftRight;
        characterController.Move(movementDirection * speed * Time.deltaTime);
        
        float rotate = (mouseXPos * sensitivity) * Mathf.Rad2Deg;
        playerRotate.Rotate(Vector3.up * rotate);

        if (isOnGround && Input.GetButtonDown("Jump"))
        {
            velocity.y = Mathf.Sqrt(-2f * gravity * jumpHeight);
        }

        velocity.y += gravity * Time.deltaTime;

        characterController.Move(velocity * Time.deltaTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "tag")
        {

            Debug.Log("Tagged");

            taggedPlayer.transform.position = new Vector3(Random.Range(-7f, 7f), 1f, Random.Range(-9.3f, 5.3f));

            scoreCount++;
            string text =  "Score: " + scoreCount.ToString();

            Score.text = text;
        }
    }
}
