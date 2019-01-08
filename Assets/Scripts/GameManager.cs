using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager> {

    public GameObject column, slice;
    public GameObject[] forbiddenObjects;

    private float platformPositionOnY;
    private float nextColumnPosition = 10f;

    public int PlatformCount { get; set; }

    private int firstGetHarderPoint, secondGetHarderPoint, thirdGetHarderPoint;

    private GameEnvironmentSetUp gameEnvironmentSetUp;
    private TouchControlManager touchControlManager;
    private GameDatas gameDatas;

    public GameObject WholePlatform{ get; set; }
    private Transform desiredRing;

    public GameObject PassedRing { get; set; }
    public bool IsBallPassesTheRing { get; set; }

    int[] minMaxForbiddenCount = { 0, 2 };

    public bool IsLevelPassedActive{ get; set; }

    private void Start()
    {
        //gets instances
        gameEnvironmentSetUp = gameObject.AddComponent<GameEnvironmentSetUp>() as GameEnvironmentSetUp;
        touchControlManager = TouchControlManager.Instance;

        //gets datas saved
        SaveLoad.Load();
        gameDatas = SaveLoad.gameDatas;

        WholePlatform = new GameObject("wholePlatform");

        PlatformCount = gameDatas.PlatformCount;

        //get harder point settings
        firstGetHarderPoint = (PlatformCount * 25) / 100;
        secondGetHarderPoint = (PlatformCount * 50) / 100;
        thirdGetHarderPoint = (PlatformCount * 75) / 100;

        PlatformSetUp();
        MakeControlPointForARing();
    }

    private void FixedUpdate()
    {
        if (IsBallPassesTheRing)
        {
            gameEnvironmentSetUp.RingFragmentation(PassedRing,500f);
            IsBallPassesTheRing = false;
        }    
    }


    //each platform part contains a column and slices
    private void MakePlatformParts(float platformPositionOnY,bool isForbiddenSlicesAdded,int activeForbiddenSliceCount,bool isRingCompleted)
    {
        GameObject ringParent = new GameObject("ringParent");
        GameObject platform = new GameObject("platform");

        gameEnvironmentSetUp.MakeColumn(column, -11, platform);
        gameEnvironmentSetUp.MakeARing(slice,ringParent,isRingCompleted);
        if(isForbiddenSlicesAdded)
            gameEnvironmentSetUp.AddForbiddenSlices(forbiddenObjects, ringParent, activeForbiddenSliceCount,minMaxForbiddenCount);

        ringParent.transform.parent = platform.transform;
        platform.transform.position = new Vector3(0f,-platformPositionOnY,0f);
        platform.transform.parent = WholePlatform.transform;

        //send wholePlatform to use for Touch
        touchControlManager.WholePlatform = WholePlatform;
    }

    //set up for game environment
    private void PlatformSetUp()
    {
        for (int i = 0; i < PlatformCount; i++)
        {
            if (i == 0)
            {
                MakePlatformParts(platformPositionOnY,false,2,true);
                desiredRing = gameEnvironmentSetUp.GetSpecificRing(WholePlatform, 0);
                Destroy(desiredRing.transform.GetChild(1).gameObject);
                Destroy(desiredRing.transform.GetChild(2).gameObject);
            }
            else if (i == PlatformCount-1)
            {
                MakePlatformParts(platformPositionOnY, false,2,true);
                gameEnvironmentSetUp.SetTagToAArray(WholePlatform, PlatformCount - 1, "lastRingSlice");
            }
            else if (i>firstGetHarderPoint && i<secondGetHarderPoint)
            {
                minMaxForbiddenCount[0] = 1;
                minMaxForbiddenCount[1] = 3;
                MakePlatformParts(platformPositionOnY, true, 2, false);
            }
            else if (i>secondGetHarderPoint && i<thirdGetHarderPoint)
            {
                minMaxForbiddenCount[0] = 2;
                minMaxForbiddenCount[0] = 4;
                MakePlatformParts(platformPositionOnY, true, 2, false);
            }
            else if(i> thirdGetHarderPoint && i<PlatformCount-1)
            {
                minMaxForbiddenCount[0] = 3;
                minMaxForbiddenCount[0] = 5;
                MakePlatformParts(platformPositionOnY, true, 3,false);
            }
            else
            {
                MakePlatformParts(platformPositionOnY, true, 2,false);
            }
            platformPositionOnY += nextColumnPosition;
        }
        MakeAForbiddenSliceMoving();
    }

    //generates collider for make control points
    //also makes the collider is child of the given ring
    private void MakeControlPointForARing()
    {
        for (int i=0;i<WholePlatform.transform.childCount;i++)
        {
            GameObject desiredRing = gameEnvironmentSetUp.GetSpecificRing(WholePlatform, i).gameObject;
            Vector3 positionForControlCollider = desiredRing.transform.position - new Vector3(0f,3.2f,0f);
            BoxCollider controlCollider = gameEnvironmentSetUp.MakeControlCollider(positionForControlCollider);

            controlCollider.tag = "controlCollider";

            controlCollider.isTrigger = true;

            controlCollider.transform.parent = desiredRing.transform;
        }
    }

    private void MakeAForbiddenSliceMoving()
    {
        int randomMovingSliceCount = Random.Range(2, thirdGetHarderPoint - secondGetHarderPoint);
        for (int i=0;i<randomMovingSliceCount;i++)
        {
            int randomIndex = Random.Range(secondGetHarderPoint + 1, thirdGetHarderPoint - 1);
            Transform desiredRing = gameEnvironmentSetUp.GetSpecificRing(WholePlatform, randomIndex);
            GameObject forbiddenSlice = gameEnvironmentSetUp.GetAForbiddenSliceFromARing(desiredRing.gameObject);
            if (forbiddenSlice != null && forbiddenSlice.GetComponent<RotateAnimation>()==null)
            {
                Destroy(forbiddenSlice.GetComponent<AvoidOverlapping>());
                forbiddenSlice.AddComponent<RotateAnimation>();
            }
        }
    }
}
