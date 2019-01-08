using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveUp : MonoBehaviour {

    private Ui ui;
    private BallManager ballManager;
    private Canvas canvas;

    private Text pointText;

    //this is used as amount of point
    public int PointAmount { get; set; }

    private int normalTextSize; 
    private float topConstrainForPoint;
    private float speed = 150f;
    private float amountOfAlphaDecreasement = 0.01f;
    private int amountOfFontSizeIncreasement = 10;

    private void Start()
    {
        //gets instances
        ballManager = BallManager.Instance;
        ui = gameObject.AddComponent<Ui>() as Ui;
        canvas = FindObjectOfType(typeof(Canvas)) as Canvas;

        PointAmount = ballManager.PointAmount;
        pointText = GetComponent<Text>();
        pointText.text = "+" + PointAmount;

        normalTextSize = canvas.transform.Find("scoreText").GetComponent<Text>().fontSize;
        topConstrainForPoint = canvas.transform.Find("scoreText").transform.position.y;
    }

    private void Update()
    {
        MoveUpEffect();
    }

    private void MoveUpEffect()
    {
        ui.IncreaseFontSize(pointText, normalTextSize, amountOfFontSizeIncreasement);

        if (pointText.fontSize >= normalTextSize)
        {
            ui.IncreaseYAxesValue(gameObject, topConstrainForPoint, speed);
            ui.DecreaseAlpha(pointText, amountOfAlphaDecreasement);
        }
        if (gameObject.transform.localPosition.y >= topConstrainForPoint-1f)
        {
            Destroy(gameObject);
        }
    }
}
