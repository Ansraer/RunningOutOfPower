using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraController : MonoBehaviour
{

    public GameObject player;

    private Vector3 offset;

    void Start()
    {
        offset = transform.position - player.transform.position;
    }

    void LateUpdate()
    {

        if (player != null)
        {
            transform.position = player.transform.position + new Vector3(0,0,-3);
        } else
        {
            transform.position = new Vector3(0, 0, -3);
        }
    }
}