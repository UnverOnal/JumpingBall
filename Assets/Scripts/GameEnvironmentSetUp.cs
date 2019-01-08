using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEnvironmentSetUp : MonoBehaviour {

    private float sliceSize = 36f;
    GameObject rightSide;
    GameObject leftSide;
    float realValueOfTheRotationYAxes;

    //makes a complete ring by using slices
    public void MakeARing(GameObject slice, GameObject parentObject,bool isRingCompleted)
    {    
        //specifying sliceCount variable
        int sliceCount;
        if (isRingCompleted)
            sliceCount = 10;
        else
            sliceCount = Random.Range(5, 8);

        List<Vector3> availableRotations = RandomSliceRotations(sliceCount); 
        for (int i =0;i<sliceCount;i++)
        {
            GameObject sliceCopy = Instantiate(slice);
            sliceCopy.transform.rotation = Quaternion.Euler(availableRotations[i]);
            sliceCopy.transform.parent = parentObject.transform;
        }
    }

    //instantiates a column
    public void MakeColumn(GameObject column, float columnHeight, GameObject parentObject)
    {
        Vector3 newColumnPosition = new Vector3(0f, 0f, 0f);
        GameObject columnCopy = Instantiate(column, column.transform.position, column.transform.rotation);

        columnCopy.transform.position = newColumnPosition;
        newColumnPosition += new Vector3(0f, columnHeight, 0f);
        columnCopy.transform.parent = parentObject.transform;
    }

    //settings about forbidden areas... Adds forbidden slices onto regular ones
    public void AddForbiddenSlices(GameObject[] forbiddenSlices, GameObject sliceParent, int activeForbiddenSliceCount,int[] minMaxForbidden)
    {
        for (int i = 0; i < Random.Range(minMaxForbidden[0], minMaxForbidden[1]); i++)
        {
            GameObject forbiddenSliceCopy = Instantiate(forbiddenSlices[Random.Range(0, activeForbiddenSliceCount)]);

            Transform[] sliceTransforms = sliceParent.GetComponentsInChildren<Transform>();
            Transform randomSliceTransform = sliceTransforms[Random.Range(0, sliceTransforms.Length)];

            //add AvoidOverlapping script to forbidden slices and set legal transforms 
            AvoidOverlapping avoidOverlapping = forbiddenSliceCopy.AddComponent<AvoidOverlapping>() as AvoidOverlapping;
            avoidOverlapping.SliceTransforms = sliceTransforms;

            forbiddenSliceCopy.transform.rotation = randomSliceTransform.rotation;
            forbiddenSliceCopy.transform.parent = sliceParent.transform;
        }
        ConstraintLongForbiddenSliceCount(sliceParent);
    }

    //gets children of a parent GameObject and makes an Transform array consist of them
    public Transform[] ParentGameObjectToTransformArray(GameObject parentGameObject)
    {
        Transform[] childs = new Transform[parentGameObject.transform.childCount];

        for (int i = 0; i < childs.Length; i++)
        {
            childs[i] = parentGameObject.transform.GetChild(i);
        }

        return childs;
    }

    //generates box collider with its gameObject
    public BoxCollider MakeControlCollider(Vector3 position)
    {
        //produces gameObject that the collider will be attached.
        //assings collider size
        GameObject controlObject = new GameObject("controlObject");
        BoxCollider controlCollider = controlObject.AddComponent<BoxCollider>();
        controlCollider.size = new Vector3(12f, 1f, 12f);

        controlCollider.gameObject.transform.position = position;
        return controlCollider;
    }

    //returns specific ring by ringIndex as Transform format
    public Transform GetSpecificRing(GameObject parentGameObject, int ringIndex)
    {
        Transform[] platforms = ParentGameObjectToTransformArray(parentGameObject);
        Transform[] rings = new Transform[platforms.Length];

        for (int i = 0; i < platforms.Length; i++)
        {
            rings[i] = platforms[i].Find("ringParent");
        }

        return rings[ringIndex];
    }

    //sets tag all members of an transforms array 
    public void SetTagToAArray(GameObject parentObject, int ringIndex, string tag)
    {
        Transform desiredRing = GetSpecificRing(parentObject, ringIndex);
        Transform[] sliceTransformsOfDesiredRing = desiredRing.GetComponentsInChildren<Transform>();

        foreach (Transform transform in sliceTransformsOfDesiredRing)
        {
            transform.gameObject.tag = tag;
        }
    }

    //takes ring gameobject as a parameter and returns one forbidden slices from its children
    public GameObject GetAForbiddenSliceFromARing(GameObject ring)
    {
        Transform[] allSlices;
        allSlices = ParentGameObjectToTransformArray(ring);

        List<Transform> forbiddenObjects = new List<Transform>();
        foreach (Transform transform in allSlices)
        {
            if (transform.tag == "forbiddenSlice" && transform.name != "longForbiddenSlice(Clone)")
            {
                forbiddenObjects.Add(transform);
            }
        }

        return forbiddenObjects[0].gameObject;
    }

    /// <summary>
    ///destroys game objects in an array 
    /// </summary>
    /// <param name="tag">tag is needed for the find needed gameobjects </param>
    public void DestroyGameObjectsInAnArray(string tag)
    {
        GameObject[] pointTextObjects = GameObject.FindGameObjectsWithTag(tag);

        if (pointTextObjects.Length > 0)
        {
            for (int i = 0; i < pointTextObjects.Length; i++)
            {
                Destroy(pointTextObjects[i]);
            }
        }
    }

    //fragmentates the ring that is passed by the ball
    public void RingFragmentation(GameObject ringObject,float speed)
    {
        Transform[] slices = ParentGameObjectToTransformArray(ringObject);

        foreach (Transform sliceTransform in slices)
        {
            sliceTransform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            sliceTransform.GetComponent<Rigidbody>().useGravity = true;

            if (sliceTransform.rotation.eulerAngles.y > 180)
            {
                realValueOfTheRotationYAxes = sliceTransform.rotation.eulerAngles.y - 360;
            }
            else
            {
                realValueOfTheRotationYAxes = sliceTransform.rotation.eulerAngles.y;
            }

            if (realValueOfTheRotationYAxes<0)
            {
                sliceTransform.GetComponent<Rigidbody>().AddForce(Vector3.back * speed);
            }
            else
            {
                sliceTransform.GetComponent<Rigidbody>().AddForce(Vector3.forward * speed);
            }
        }
        StartCoroutine(DeleteSliceGroups(ringObject));
    }

    //deletes slice groups after for a while slice groups are instantiated
    IEnumerator DeleteSliceGroups(GameObject ringObject)
    {
        yield return new WaitForSeconds(0.75f);

        Destroy(ringObject);
    }

    //returns random rotation list for positioning to instantiated slices
    private List<Vector3> RandomSliceRotations(int sliceRotationCount)
    {
        List<Vector3> allTheRotations = new List<Vector3>();
        allTheRotations = RotationListForSliceInstantiate();

        for(int i = 0; i < 10 - sliceRotationCount; i++)
        {
            int removeIndex = Random.Range(0,allTheRotations.Count-1);

            allTheRotations.RemoveAt(removeIndex);
        }
        return allTheRotations;
    }

    //returns complete rotation list for positioning to instantiated slices
    private List<Vector3> RotationListForSliceInstantiate()
    {
        Vector3 newSliceRotation = new Vector3(0f, 0f, 0f);
        List<Vector3> rotationList = new List<Vector3>();

        for (int i=0;i<10;i++)
        {
            rotationList.Add(newSliceRotation);
            newSliceRotation += new Vector3(0f,sliceSize,0f);
        }

        return rotationList;
    }

    //doesn't allow the long forbidden slice count is bigger than one
    private void ConstraintLongForbiddenSliceCount(GameObject sliceParent)
    {
        Transform[] sliceTransforms = ParentGameObjectToTransformArray(sliceParent);
        List<Transform> forbiddenLongSlices= new List<Transform>();

        for (int i =0;i<sliceTransforms.Length;i++)
        {
            if (sliceTransforms[i].CompareTag("forbiddenSlice") && sliceTransforms[i].name== "longForbiddenSlice(Clone)")
                forbiddenLongSlices.Add(sliceTransforms[i]);
        }

        if (forbiddenLongSlices.Count > 1)
            DeleteExtraForbiddenLongSlice(forbiddenLongSlices);
    }

    //helps to constraint long forbidden slice count by deleting extra ones
    private void DeleteExtraForbiddenLongSlice(List<Transform> forbiddenLongSlices)
    {
        for (int i=0;i<forbiddenLongSlices.Count;i++)
        {
            if (i == 0)
                continue;

            Destroy(forbiddenLongSlices[i].gameObject);
            forbiddenLongSlices.RemoveAt(i);
        }
    }
}
