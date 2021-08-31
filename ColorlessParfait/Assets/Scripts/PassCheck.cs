using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassCheck : MonoBehaviour
{
    public BoxCollider2D ground;
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("GroundObject"))
        {
            other.isTrigger = true;
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("GroundObject"))
        {
            other.isTrigger = false;
        }
    }
}