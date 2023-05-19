using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabWeapon : MonoBehaviour
{
    // 무기를 장착할 위치
    public Transform weaponAnchor;

    void OnTriggerEnter(Collider coll)
    {
        if (coll.CompareTag("WEAPON"))
        {
            coll.GetComponent<FollowHand>().targetHand = weaponAnchor;
        }
    }

    void OnTriggerExit(Collider coll)
    {
        if (coll.CompareTag("WEAPON"))
        {
            coll.GetComponent<FollowHand>().targetHand = null;
        }
    }

    void OnTriggerStay(Collider coll)
    {
        if (coll.CompareTag("WEAPON"))
        {
            if (Input.GetKey(KeyCode.Space))
                coll.GetComponent<FollowHand>().isGrabbed = true;
            else
                coll.GetComponent<FollowHand>().isGrabbed = false;

        }
    }



}
