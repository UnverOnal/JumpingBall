using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchControl : MonoBehaviour {

    private float dragAmount = 0f;

    public void TouchCtrl(GameObject wholePlatform)
    {
        //gets dragging value
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved)
            {
                dragAmount -= touch.deltaPosition.x;   
            }

            //set dragging value to whole platforms y axis
            wholePlatform.transform.eulerAngles = new Vector3(0f, dragAmount * 0.5f, 0f);
        }
    }
}
