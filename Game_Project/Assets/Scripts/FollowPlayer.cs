using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] GameObject Follow;

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = Follow.transform.position + new Vector3(0, 0, -10);
    }
}