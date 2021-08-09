using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptablePoolTypeGeneric<T> : ScriptableObject, ISerializationCallbackReceiver
{
    [SerializeField]
    public T initialValue; //This is the value your designers should care about.


    [System.NonSerialized]
    public T value; //This is the value your game should care about at runtime.
    public void OnAfterDeserialize()
    {
        value = initialValue;
    }

    public void OnBeforeSerialize()
    {

    }
}
