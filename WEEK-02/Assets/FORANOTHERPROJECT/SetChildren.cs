using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetChildren : MonoBehaviour
{
    protected Transform[] children;

    protected virtual void OnValidate()
    {
        InitialiseChildren();
    }

    protected void InitialiseChildren()
    {
        children = new Transform[transform.childCount];

        for (int i = 0; i < children.Length; i++)
        {
            children[i] = transform.GetChild(i);
        }
    }
}