using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{
    void Update()
    {
        transform.LookAt(CameraFollow.Instance.Camera.transform);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, 135, transform.eulerAngles.z);
    }
}
