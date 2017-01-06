using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMouse : MonoBehaviour
{
    private float yaw = 0.0f;
    private float pitch = 0.0f;

    void Update()
    {
        yaw += 2.0f * Input.GetAxis("Mouse X");
        pitch -= 2.0f * Input.GetAxis("Mouse Y");

        transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
    }
}