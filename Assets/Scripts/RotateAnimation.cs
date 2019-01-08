using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAnimation : MonoBehaviour {

    private float yAxesOfCurrentRotation;
    private float yAxesOfInitialRotation;
    private float spinSpeed=40f;
    private float spinSpace = 90f;
    private bool canSpinTowardRight =true;


    private void Start()
    {
        spinSpace = Random.Range(spinSpace,180f);
        yAxesOfInitialRotation = RealRotationValueOnY();
    }

    void Update ()
    {
        RotateMovement();
        SpecifyCanTheObjectSpinTowardRight();
    }

    private void RotateMovement()
    {
        yAxesOfCurrentRotation = RealRotationValueOnY();

        if (yAxesOfCurrentRotation < yAxesOfInitialRotation + spinSpace && canSpinTowardRight)
        {
            transform.Rotate(Vector3.up * Time.deltaTime * spinSpeed);
        }
        else if (yAxesOfCurrentRotation > yAxesOfInitialRotation && !canSpinTowardRight)
        {
            transform.Rotate(Vector3.down * Time.deltaTime * spinSpeed);
        }
    }

    private void SpecifyCanTheObjectSpinTowardRight()
    {
        if (yAxesOfCurrentRotation > yAxesOfInitialRotation + spinSpace)
        {
            canSpinTowardRight = false;
        }
        else if (yAxesOfCurrentRotation < yAxesOfInitialRotation)
        {
            canSpinTowardRight = true;
        }
    }

    //calculates and returns real rotation value on y axes only
    private float RealRotationValueOnY()
    {
        float yAxesValue;

        if (transform.eulerAngles.y > 180)
        {
            yAxesValue = transform.eulerAngles.y - 360f;
            return yAxesValue;
        }
        else
        {
            yAxesValue = transform.eulerAngles.y;
            return yAxesValue;
        }
    }
}
