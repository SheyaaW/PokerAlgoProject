using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    public float sens;
    public Transform orient;
    float xRotate = 0f;
    float yRotate = 0f;
    private bool cursorVisible = true;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float x = Input.GetAxis("Mouse X") * Time.deltaTime * sens;
        float y = Input.GetAxis("Mouse Y") * Time.deltaTime * sens;

        xRotate -= y;
        yRotate += x;
        xRotate = Mathf.Clamp(xRotate, -5f, 95f);
        yRotate = Mathf.Clamp(yRotate, -75f, 75f);

        transform.localRotation = Quaternion.Euler(xRotate, yRotate, 0f);
        orient.Rotate(Vector3.left * y);
        orient.Rotate(Vector3.up * x);

        if (Input.GetKeyDown(KeyCode.Y))
        {
            cursorVisible = !cursorVisible;
            Cursor.visible = cursorVisible;
            if (cursorVisible)
            {
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
            Cursor.visible = cursorVisible;
        }
    }
}
