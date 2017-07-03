using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralBuilding : MonoBehaviour
{
    public bool generate;

    [Header("Doors")]
    public Transform doorsRoot;
    public GameObject doorPrefab;
    [Range(1, 4)]
    public int doorsNumber;
    [Range(0f, 1f)]
    public float doorsX;
    [Range(0f, 1f)]
    public float doorsY;
    private GameObject[] doors;

    private BoxCollider boxCollider;

    #region Sizes
    private float width;
    private float height;
    private float depth;

    private float halfWidth;
    private float halfHeight;
    private float halfDepth;

    private Vector3 center;

    private Vector3 topCenter;
    private Vector3 bottomCenter;

    private Vector3 rightCenter;
    private Vector3 leftCenter;

    private Vector3 backwardCenter;
    private Vector3 forwardCenter;

    private Vector3 right;
    private Vector3 top;
    private Vector3 forward;
    #endregion Sizes

    protected void OnValidate()
    {
        if (generate)
        {
            boxCollider = GetComponent<BoxCollider>();

            RandomScale();
            DefineLenghts();
            DefineCenters();

            SetDoors();

            generate = false;
        }
    }

    private void RandomScale()
    {
        float x = 1f + Random.Range(0f, 2f);
        float z = x;
        float y = 2f + Random.Range(0f, 2f);

        //transform.localScale = ExtensionMethods.RandomVector3(x, y, z);
        transform.localScale = new Vector3(2f, 5f, 2f);
    }

    private void SetDoors()
    {
        doors = new GameObject[doorsNumber];

        for (int i = 0; i < doorsRoot.childCount; i++)
        {
            
        }

        GameObject[] doorsInstances = new GameObject[doorsNumber];
        for (int i = 0; i < doorsNumber; i++)
        {
            doorsInstances[i] = Instantiate(doorPrefab, Vector3.zero, Quaternion.identity);
            doorsInstances[i].transform.SetParent(doorsRoot);
            doorsInstances[i].transform.localScale = new Vector3(transform.localScale.x * doorsX, transform.localScale.y * doorsY, transform.localScale.z * doorsX);
        }

        for (int i = 0; i < doorsInstances.Length; i++)
        {
            switch(i)
            {
                case 0:
                    doorsInstances[i].transform.position = bottomCenter + forward;
                    break;
                case 1:
                    doorsInstances[i].transform.position = bottomCenter + right;
                    break;
                case 2:
                    doorsInstances[i].transform.position = bottomCenter - forward;
                    break;
                case 4:
                    doorsInstances[i].transform.position = bottomCenter - right;
                    break;
            }
        }
    }

    private void DefineLenghts()
    {
        width = boxCollider.size.x;
        halfWidth = boxCollider.size.x * 0.5f;

        height = boxCollider.size.y;
        halfHeight = boxCollider.size.y * 0.5f;

        depth = boxCollider.size.z;
        halfDepth = boxCollider.size.z * 0.5f;
    }

    private void DefineCenters()
    {
        center = boxCollider.center;

        top = new Vector3(0f, halfHeight, 0f);
        topCenter = center + top;
        bottomCenter = center - top;

        right = new Vector3(halfWidth, 0f, 0f);
        rightCenter = center + right;
        leftCenter = center - right;

        forward = new Vector3(0f, 0f, halfDepth);
        forwardCenter = center + forward;
        backwardCenter = center - forward;
    }

    public static T SafeDestroy<T>(T obj) where T : Object
    {
        if (Application.isEditor)
            Object.DestroyImmediate(obj);
        else
            Object.Destroy(obj);

        return null;
    }
    public static T SafeDestroyGameObject<T>(T component) where T : Component
    {
        if (component != null)
            SafeDestroy(component.gameObject);
        return null;
    }
}