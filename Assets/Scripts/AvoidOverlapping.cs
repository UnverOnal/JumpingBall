using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// this script is attached to forbidden slices,
/// and thus ensures the forbidden slices don't overlapping eachother
/// </summary>
public class AvoidOverlapping : MonoBehaviour
{
    public Transform[] SliceTransforms { get; set; }

    private void OnCollisionEnter(Collision collision)
    {
        int randomNumber = Random.Range(0,SliceTransforms.Length-1);
        if (collision.gameObject.CompareTag("forbiddenSlice") && SliceTransforms[randomNumber] != null)
        {
            gameObject.transform.rotation = SliceTransforms[randomNumber].rotation;
        }
        else
        {
            randomNumber = Random.Range(0, SliceTransforms.Length - 1);
        }
    }
}
