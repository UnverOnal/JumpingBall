using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : Singleton<UiManager> {

    public int PercentageOfRingCountThatPassed { get; set; }
    public int Point { get; set; }

    //images of the progress bar
    private Image firstCircle, secondCircle, backgroundPart, fillPart;

    //first and final color variables
    private Color currentColor, targetColor;

    public bool IsPointShown { get; set; }
    public float TouchTime { get; set; }

    public GameObject pointTextPrefab;

    private Ui ui;
    private Canvas mainCanvas;
    private GameDatas gameDatas;

    private void Start()
    {
        //gets instances
        ui = gameObject.AddComponent<Ui>() as Ui;
        mainCanvas = FindObjectOfType(typeof(Canvas)) as Canvas;

        //get datas saved
        SaveLoad.Load();
        gameDatas = SaveLoad.gameDatas;

        InitialProgressBarSettings();
        SpecifyFirstAndLastColorsOfProgressBar();
        PrintBestScore();
    }

    private void Update()
    {
        PrintPercentageOfCompletedParts();

        //settings that related progress bar
        UpdateProgressBar(fillPart);    
        PrintLevelCountToCirclesOfBar();

        //color transition for progress bar
        firstCircle.color = Color.Lerp(currentColor, targetColor,PercentageOfRingCountThatPassed/100f);

        //score settings
        PrintPoint();

        PrintLevelIsPassed();

        //if the ball passes through the control point
        //pointText object is instantiated animation is played
        if (IsPointShown)
        {
            GameObject pointTextObject = Instantiate(pointTextPrefab);
            pointTextObject.transform.SetParent(mainCanvas.transform);
            pointTextObject.transform.localPosition = new Vector3(0f,400f,0f);
            IsPointShown = false;
        }
    }


    //it works when the screen game object scene is activated
    private void PrintPercentageOfCompletedParts()
    {
        if (mainCanvas.transform.Find("GameOverScene").gameObject.activeSelf == true)
        {
            string percentageText = PercentageOfRingCountThatPassed + "% " + "COMPLETED";
            ui.PrintToScreen("accomplishmentText", percentageText);
        }
    }

    //prints level counts on circles of the progress bar
    private void PrintLevelCountToCirclesOfBar()
    {
        ui.PrintToScreen("firstCircleText", gameDatas.LevelCount.ToString());
        ui.PrintToScreen("secondCircleText", (gameDatas.LevelCount + 1).ToString());
    }

    //prints current point
    private void PrintPoint()
    {
        ui.PrintToScreen("scoreText", Point.ToString());
    }

    //prints best score
    private void PrintBestScore()
    {
        ui.PrintToScreen("bestScore", "best:  " + gameDatas.BestScore.ToString());
    }

    //prints that is level is passed
    private void PrintLevelIsPassed()
    {
        if (mainCanvas.transform.Find("levelPassed").gameObject.activeSelf == true)
        {
            ui.PrintToScreen("levelPassedText", ("Level " + gameDatas.LevelCount + " Passed"));
        }
    }

    //sets initial colors for progress bar
    private void InitialProgressBarSettings()
    {
        Image[] barImages=ui.GetImages("progressBar",mainCanvas);

        //assign image components to image variables
        backgroundPart = barImages[0];
        fillPart = barImages[1];
        secondCircle = barImages[2];
        firstCircle = barImages[3];

        AssignColorUsingImageComponent();
    }

    private void AssignColorUsingImageComponent()
    {
        Vector3 firstCircleValues, secondCircleValues, backgroundPartValues, fillPartValues;

        firstCircleValues = ui.ConvertRgbToZeroToOneScale(new Vector3(255f, 255f, 111f));
        ui.ChangeImageColor(firstCircle, firstCircleValues, 1f);

        secondCircleValues = ui.ConvertRgbToZeroToOneScale(new Vector3(140f, 140f, 140f));
        ui.ChangeImageColor(secondCircle, secondCircleValues, 1f);

        backgroundPartValues = ui.ConvertRgbToZeroToOneScale(new Vector3(0f, 0f, 0f));
        ui.ChangeImageColor(backgroundPart, backgroundPartValues, 1f);

        fillPartValues = ui.ConvertRgbToZeroToOneScale(new Vector3(255f, 255f, 111f));
        ui.ChangeImageColor(fillPart, fillPartValues, 1f);
    }

    //updates fill amount of fill part
    //also updates color of fill part related to first circle's color
    private void UpdateProgressBar(Image fillPart)
    {
        fillPart.fillAmount = PercentageOfRingCountThatPassed / 100f;
        fillPart.color = firstCircle.color;

        if (fillPart.fillAmount == 1)
        {
            secondCircle.color = firstCircle.color;
        }
    }

    private void SpecifyFirstAndLastColorsOfProgressBar()
    {     
        Vector3 targetColorValues;

        currentColor = firstCircle.color;
        targetColorValues = ui.ConvertRgbToZeroToOneScale(new Vector3(220f,0f,0f));//it is red
        targetColor = new Color(targetColorValues.x,targetColorValues.y,targetColorValues.z);
    }
 
    public int ExtraPointAmount(int successiveRingCount)
    {
        if (successiveRingCount <= 1)
        {
            return gameDatas.LevelCount;
        }
        else
        {
            return successiveRingCount * gameDatas.LevelCount;
        }
    }
}
