using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounce : MonoBehaviour {

    private Rigidbody rigidBodyOfTheBall;

    private void OnTriggerEnter(Collider other)
    {
        GameObject parentObject = other.transform.parent.gameObject;
        BallManager ballManager = gameObject.GetComponent<BallManager>();

        if (parentObject.gameObject.CompareTag("baseSlice") && ballManager.canCollide)
        {
            ballManager.canCollide = false;

            rigidBodyOfTheBall = gameObject.GetComponent<Rigidbody>();
            rigidBodyOfTheBall.velocity = new Vector3(0f, 25f, 0f); 
        }
    }
}
