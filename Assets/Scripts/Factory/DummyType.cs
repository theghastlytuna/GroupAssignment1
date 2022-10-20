using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DummyType : MonoBehaviour
{
    public abstract string Name { get; }

    public abstract GameObject Create(GameObject prefab);
}

public class DummyShort : DummyType
{
    public override string Name => "DummyShort";

    public override GameObject Create(GameObject prefab)
    {
        GameObject newObject = Instantiate(prefab);
        //Debug.Log
        return newObject;
    }
}

public class DummyTall : DummyType
{
    public override string Name => "DummyTall";

    public override GameObject Create(GameObject prefab)
    {
        GameObject newObject = Instantiate(prefab);
        //Debug.Log
        return newObject;
    }
}