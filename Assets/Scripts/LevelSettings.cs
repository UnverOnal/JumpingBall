using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSettings : MonoBehaviour {

    private Canvas mainCanvas;
    private GameObject levelPassedScreen;
    private int maxPlatformCount = 100;

    private void Start()
    {
        mainCanvas= FindObjectOfType(typeof(Canvas)) as Canvas;
        levelPassedScreen = mainCanvas.transform.Find("levelPassed").gameObject;
    }

    //makes next level configurations
    public void ContinueToNextLevel(BallManager ballManager,GameDatas gameDatas)
    {
        //makes enable level passed element
        levelPassedScreen.SetActive(true);

        //doesn't allow the collider of the last ring's slices collide
        ballManager.canCollide = false;

        //pauses the game
        Time.timeScale = 0f;

        //save level count
        gameDatas.LevelCount += 1;
        SaveLoad.gameDatas = gameDatas;
        SaveLoad.Save();

        //save platform count
        if (gameDatas.PlatformCount<maxPlatformCount)
        {
            gameDatas.PlatformCount += 1;
            SaveLoad.gameDatas = gameDatas;
            SaveLoad.Save();
        }

        StartCoroutine(DissapearLevelPassedScreen(ballManager));
    }

    IEnumerator DissapearLevelPassedScreen(BallManager ballManager)
    {
        yield return new WaitForSecondsRealtime(2f);

        SceneManager.LoadScene(0);

        //makes disable level passed element
        levelPassedScreen.SetActive(false);

        //starts the game
        Time.timeScale = 1f;
    }
}
