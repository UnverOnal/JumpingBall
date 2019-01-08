using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchControlManager : Singleton<TouchControlManager> {

    TouchControl touchControl;

    public GameObject WholePlatform { get; set; }

    private bool iscontrolActive = true;
    public bool IsControlActive
    {
        get { return iscontrolActive; }
        set { iscontrolActive = value; }
    }

    private void Start()
    {
        touchControl = gameObject.AddComponent<TouchControl>();
    }

    private void Update()
    {
        if (IsControlActive)
            touchControl.TouchCtrl(WholePlatform);
    }
}
