using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class tagDetection : MonoBehaviour
{

    public Text Score;
    public GameObject taggedPlayer;
    int scoreCount = 0;
    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "tag")
        {

            Debug.Log("Tagged");

            taggedPlayer.transform.position = new Vector3(Random.Range(-7f, 7f), 4f, Random.Range(-9.3f, 5.3f));

            scoreCount++;
            string text = scoreCount.ToString();

            Score.text = text;
        }
    }
}
