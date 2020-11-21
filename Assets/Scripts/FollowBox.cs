using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowBox : MonoBehaviour
{
    public Transform box;
    private Vector3 offset;

    private void Start()
    {
        offset = transform.position - box.position;
    }

    private void LateUpdate()
    {
        transform.position = box.position + offset;
    }
}
