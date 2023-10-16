using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMove : MonoBehaviour
{
    public Transform camera_position;
    
    void Update()
    {
        transform.position = camera_position.position;
    }
}
