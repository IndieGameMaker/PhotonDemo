using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowHand : MonoBehaviour
{
    // 따라갈 손 앵커
    public Transform targetHand;
    public bool isGrabbed;

    void Update()
    {
        if (isGrabbed)
        {
            transform.SetPositionAndRotation(targetHand.position, targetHand.rotation);
        }
    }
}
