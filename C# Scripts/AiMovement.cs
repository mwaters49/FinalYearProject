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
using Unity.MLAgents;
using Random = UnityEngine.Random;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class AiMovement : Agent
{
    [SerializeField] private Transform tagPosition;
    

    public CharacterController characterController;
    public Transform drawOnGroundSphere;
    public Transform playerRotate;
    public LayerMask groundTag;
    public GameObject taggedPlayer;
    public Rigidbody playerRigid;

    public Material successMaterial;
    public Material failureMaterial;
    public Material defaultMaterial;
    public Renderer matRenderer;

    public Text Score;

    public float speed;
    public float sensitivity = 0.2f;


    float gravity = -19f;
    float jumpHeight = 1.5f;
    float onGroundSphereRadius = 0.4f;

    Vector3 velocity;

    bool isOnGround;
    int scoreCount = 0;

    void Update()
    {
        //Debug.Log("update()");

        isOnGround = Physics.CheckSphere(drawOnGroundSphere.position, onGroundSphereRadius, groundTag);
        if (isOnGround)
        {
            velocity.y = 0f;
        }

        
        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }

    void Move(ActionSegment<int> act)
    {
        //Debug.Log("move()");

        /*float leftRight = Input.GetAxis("Horizontal"); //A and D keys
        float forwardBack = Input.GetAxis("Vertical");   // W and S keys
        float mouseXPos = Input.GetAxis("Mouse X");
        if (isOnGround)
        {
            velocity.y = 0f;
            speed = 6f;
        }
        else
            speed = 3f;


        Vector3 movementDirection = transform.forward * forwardBack + transform.right * leftRight;
        characterController.Move(movementDirection * speed * Time.deltaTime);

        float rotate = (mouseXPos * sensitivity) * Mathf.Rad2Deg;
        playerRotate.Rotate(Vector3.up * rotate);

        characterController.Move(velocity * Time.deltaTime);*/

        //isOnGround = Physics.CheckSphere(drawOnGroundSphere.localPosition, onGroundSphereRadius, groundTag);
        //if (isOnGround)
        //    velocity.y = 0f;

        var dirToGo = Vector3.zero;
        var rotateDir = Vector3.zero;

        var action = act[0];
        var action1 = act[1];

        switch(action)
        {
            case 1:
                dirToGo = transform.forward * 1f; //forward
                break;
            case 2:
                dirToGo = transform.forward * -1f; //back
                break;
            case 3:
                dirToGo = transform.right * 1f; //right
                break;
            case 4:
                dirToGo = transform.right * -1f; //left
                break;
        }

        switch(action1)
        {
            case 1:
                rotateDir = transform.up * 1f;
                break;
            case 2:
                rotateDir = transform.up * -1f;
                break;
        }

        //velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
        characterController.Move(dirToGo * speed * Time.deltaTime);
        transform.Rotate(rotateDir, Time.deltaTime * 200f);
    }

    /*void Jump()
    {
        if (isOnGround)
            velocity.y = Mathf.Sqrt(-2f * gravity * jumpHeight);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "tag")
        {

            Debug.Log("Tagged");

            taggedPlayer.transform.position = new Vector3(Random.Range(-7f, 7f), 1f, Random.Range(-9.3f, 5.3f));

            scoreCount++;
            string text = "Score: " + scoreCount.ToString();

            Score.text = text;
        }
    }*/

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Player: onCollisionEnter()");
        if (collision.gameObject.tag == "tag")
        {
            TaggedAPlayer();
            scoreCount++;
            string text = "Tags: " + scoreCount.ToString();
            Score.text = text;
            EndEpisode();
        }


    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Player: onTriggerEnter()");
        if (other.gameObject.tag == "wall")
        {
            Debug.Log("Player wall hit");
            AddReward(-0.5f);
            StartCoroutine(ChangeMaterial(failureMaterial, 2));
            EndEpisode();
        }
    }
    public void TaggedAPlayer()
    {
        Debug.Log("TaggedAPlayer()");
        AddReward(5f);
        Debug.Log("Reward: " + GetCumulativeReward());
        StartCoroutine(ChangeMaterial(successMaterial, 2));
        EndEpisode();
    }

    public override void Initialize()
    {
        //things to set at beginning, same as Start() function
    }
    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        //Debug.Log("OnActionReceived()");
        //what ai does with various actions
        Move(actionBuffers.DiscreteActions);

        AddReward(-1f / MaxStep);
        if(GetCumulativeReward() == -1f)
            StartCoroutine(ChangeMaterial(failureMaterial, 2));

        Debug.Log("Reward: " + GetCumulativeReward() + " Max Step: " + MaxStep);

    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(tagPosition.localPosition);
    }

    public override void OnEpisodeBegin()
    {
        //reset
        Debug.Log("OnEpisodeBegin()");
        ResetPlayerPos();
        taggedPlayer.transform.localPosition = new Vector3(Random.Range(-7f, 7f), 1f, Random.Range(-9.3f, 5.3f));
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        //player override for test
        //Debug.Log("Heuristic()");
        var discreteActionsOut = actionsOut.DiscreteActions;
        discreteActionsOut[0] = 0;
        discreteActionsOut[1] = 0;

        if(Input.GetKey(KeyCode.Y))
        {
            discreteActionsOut[0] = 1;
        }
        else if (Input.GetKey(KeyCode.G))
        {
            discreteActionsOut[0] = 4;
        }
        else if (Input.GetKey(KeyCode.H))
        {
            discreteActionsOut[0] = 2;
        }
        else if (Input.GetKey(KeyCode.J))
        {
            discreteActionsOut[0] = 3;
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            discreteActionsOut[1] = 2;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            discreteActionsOut[1] = 1;
        }
    }

    public void ResetPlayerPos()
    {
        Vector3 resetPos = new Vector3(0.3f, 1.42f, -2.22f);
        characterController.enabled = false;
        characterController.transform.localPosition = resetPos;
        characterController.enabled = true;
    }

    IEnumerator ChangeMaterial (Material material, float time)
    {
        matRenderer.material = material;
        yield return new WaitForSeconds(time);
        matRenderer.material = defaultMaterial;
    }
}

