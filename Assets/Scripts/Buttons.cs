using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : Singleton<Buttons> {

    private Canvas mainCanvas;
    private UiManager uiManager;
    private BallManager ballManager;
    private TouchControlManager touchControlManager;

    private void Start()
    {
        mainCanvas = FindObjectOfType(typeof(Canvas)) as Canvas;
        uiManager = UiManager.Instance;
        ballManager= BallManager.Instance;
        touchControlManager = TouchControlManager.Instance;
    }
    public void Restart()
    {
        mainCanvas.transform.Find("GameOverScene").gameObject.SetActive(false);

        SceneManager.LoadScene(0);

        uiManager.PercentageOfRingCountThatPassed = 0;
        uiManager.Point = 0;

        ballManager.RingCountThatPassed = 0;
        ballManager.Point = 0;

        touchControlManager.IsControlActive = true;

        Time.timeScale = 1f;
    }
}
