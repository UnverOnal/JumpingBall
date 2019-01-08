using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallManager : Singleton<BallManager>
{
    private int ringCountThatPassed = 0;
    public int RingCountThatPassed
    {
        get { return ringCountThatPassed; }
        set { ringCountThatPassed = value; }
    }

    private int platformCount = 20;
    public int PlatformCount
    {
        get { return platformCount; }
        set { platformCount = value; }
    }

    public int LevelCount { get; set; }
    public int PointAmount { get; set; }
    public int Point { get; set; }
    public int BestScore { get; set; }
    public int SuccessiveRingCount { get; set; }

    public GameObject targetForCamera;
    private Vector3 targetOffset;

    private Canvas mainCanvas;
    public bool canCollide = true;

    private GameObject[] pointTextObjects;
    private GameObject bestScoreObject;

    private GameManager gameManager;
    private GameEnvironmentSetUp gameEnvironmentSetUp;
    private UiManager uiManager;
    private TouchControlManager touchControlManager;
    private LevelSettings levelSettings;
    GameDatas gameDatas;

    private void Start()
    {
        //gets instances
        uiManager = UiManager.Instance;
        gameManager = GameManager.Instance;
        gameEnvironmentSetUp = gameObject.AddComponent<GameEnvironmentSetUp>() as GameEnvironmentSetUp;
        touchControlManager = TouchControlManager.Instance;
        levelSettings = gameObject.AddComponent<LevelSettings>();
        gameDatas = new GameDatas();

        //get datas that is saved
        SaveLoad.Load();
        BestScore = SaveLoad.gameDatas.BestScore;
        LevelCount = SaveLoad.gameDatas.LevelCount;
        platformCount = SaveLoad.gameDatas.PlatformCount;

        targetForCamera = GameObject.FindWithTag("targetForCamera");
        mainCanvas = FindObjectOfType(typeof(Canvas)) as Canvas;

        //calculates initial distance between ball and targetForCamera object
        targetOffset = targetForCamera.transform.position - transform.position;

        bestScoreObject = mainCanvas.transform.Find("bestScore").gameObject;
    }

    private void LateUpdate()
    {
        //makes target object follows the ball
        if(targetForCamera.transform.position.y-transform.position.y>targetOffset.y)
        {
            targetForCamera.transform.position = transform.position + targetOffset;
        }     
        

        if (!canCollide &&  GetComponent<Rigidbody>().velocity.y < 0)
        {
            canCollide = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("controlCollider"))
        {
            //calculates extra point amount 
            SuccessiveRingCount += 1;
            PointAmount = uiManager.ExtraPointAmount(SuccessiveRingCount);

            //calculates current point and send it to UiManager 
            Point += PointAmount;
            uiManager.Point = Point;

            //best score settings
            if (Point > BestScore)
            {
                gameDatas.BestScore = Point;
                gameDatas.LevelCount = LevelCount;
                gameDatas.PlatformCount = PlatformCount;
                SaveLoad.gameDatas = gameDatas;
                SaveLoad.Save();
            }
            if (bestScoreObject.activeSelf == true)
            {
                bestScoreObject.SetActive(false);
            }

            //destroyes the control point that is passed
            Destroy(other.transform.gameObject);

            //this is the same with successive ring count
            //but this one is used for percentage calculations
            ringCountThatPassed += 1;

            //calculates current percentage and send it to uiManager
            int ringCount = PlatformCount - 1;
            uiManager.PercentageOfRingCountThatPassed = (ringCountThatPassed * 100) / ringCount;

            //shows and animates point
            uiManager.IsPointShown = true;

            //fragmentation sets
            gameManager.IsBallPassesTheRing = true;
            GameObject passedRing = other.transform.parent.gameObject;
            gameManager.PassedRing = passedRing;
        }

        if (other.gameObject.CompareTag("baseSlice"))
        {
            SuccessiveRingCount = 0;
        }

        //when the ball touches forbidden slices...
        if (other.gameObject.CompareTag("forbiddenSlice") && canCollide)
        {
            Time.timeScale = 0f;
            mainCanvas.transform.Find("GameOverScene").gameObject.SetActive(true);
            touchControlManager.IsControlActive = false;

            canCollide = false;

            //if there are pointTextObjects delete
            gameEnvironmentSetUp.DestroyGameObjectsInAnArray("pointText");
        }

        //when the level is completed
        if (other.gameObject.CompareTag("lastRingSlice") && canCollide)
        {
            //if there are pointTextObjects delete
            gameEnvironmentSetUp.DestroyGameObjectsInAnArray("pointText");

            levelSettings.ContinueToNextLevel(this,gameDatas);
        }
    }
}
