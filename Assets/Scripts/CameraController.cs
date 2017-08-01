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

        if (player != null && player.gameObject.active)
        {
            transform.position = player.transform.position + new Vector3(0,0,-3);
        } else
        {
            BuildingRevive b = UnityEngine.Object.FindObjectOfType<BuildingRevive>();

            if(b!= null && b.gameObject != null)
            {
                transform.position = new Vector3(b.gameObject.transform.position.x, b.gameObject.transform.position.y, -3);

            }
            else
            {
                transform.position = new Vector3(0, 0, -3);

            }

        }
    }
}