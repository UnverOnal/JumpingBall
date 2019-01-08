using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ui : MonoBehaviour {
    private Text textComponent;

    public void PrintToScreen(string elementName,string textContent)
    {
        GetTextComponent(elementName).text = textContent;
    }

    //finds and returns single text component by element name
    private Text GetTextComponent(string elementName)
    {
        Canvas mainCanvas = FindObjectOfType(typeof(Canvas)) as Canvas;
        Text[] textsOfWholeCanvas = mainCanvas.GetComponentsInChildren<Text>() as Text[];

        for (int i=0;i<textsOfWholeCanvas.Length;i++)
        {
            if (textsOfWholeCanvas[i].gameObject.name == elementName)
            {
                textComponent = textsOfWholeCanvas[i];
                break;
            }
        }
        return textComponent;
    }

    //finds and returns desired image components by parent object name
    public Image[] GetImages(string nameOfParentObject,Canvas mainCanvas)
    {
        GameObject parentObject = mainCanvas.transform.Find(nameOfParentObject).gameObject;
        Image[] desiredImages = parentObject.GetComponentsInChildren<Image>();
        return desiredImages;
    }

    public void ChangeImageColor(Image givenImage,Vector3 rgb,float alpha)
    {
        Color imageColor = new Color();
        imageColor.r = rgb.x;
        imageColor.g = rgb.y;
        imageColor.b = rgb.z;
        imageColor.a = alpha;

        givenImage.color = imageColor;
    }

    public Vector3 ConvertRgbToZeroToOneScale(Vector3 rgb)
    {
        Vector3 newRgbValues = new Vector3();
        newRgbValues.x = rgb.x / 255f;
        newRgbValues.y = rgb.y / 255f;
        newRgbValues.z = rgb.z / 255f;

        return newRgbValues;
    }

    //increases y axes of a position of an object 
    public void IncreaseYAxesValue(GameObject uiElement,float topConstrain, float speed)
    {
        float yAxesOfTheElement = uiElement.transform.localPosition.y;

        if (yAxesOfTheElement < topConstrain )
        {
            uiElement.transform.Translate(Vector3.up * Time.deltaTime*speed);
        }
    }

    //increases font size
    public void IncreaseFontSize(Text elementText,int normalTextSize,int amountOfIncreasement)
    {
        int elementSize = elementText.fontSize;

        if (elementSize < normalTextSize)
        {
            elementText.fontSize += amountOfIncreasement;
        }
    }

    //discreases the alpha value
    public void DecreaseAlpha(Text elementText, float amountOfDecreasement)
    {
        Color colorOfTheText = elementText.color;

        if (colorOfTheText.a > 0)
        {
            colorOfTheText.a -= amountOfDecreasement;
            elementText.color = colorOfTheText;
        }
    }
}
