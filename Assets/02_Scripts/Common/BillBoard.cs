using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UI가 카메라를 보도록 하는 기능
/// </summary>
public class BillBoard : MonoBehaviour
{
    private Transform camTransform;

    private void LateUpdate()
    {
        camTransform = Camera.main.transform;

        transform.LookAt(transform.position + (camTransform.rotation * Vector3.forward));
    }
}
