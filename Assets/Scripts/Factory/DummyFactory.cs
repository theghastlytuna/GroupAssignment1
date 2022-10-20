using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Reflection;
using System;
using TMPro;

public class DummyFactory : MonoBehaviour
{
    public GameObject dummyPrefab1;
    public GameObject dummyPrefab2;

    public GameObject buttonPanel;
    public GameObject buttonPrefab;

    List<DummyType> dummies;

    // Start is called before the first frame update
    void Start()
    {
        var dummyTypes = Assembly.GetAssembly(typeof(DummyType)).GetTypes().Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(DummyType)));

        dummies = new List<DummyType>();

        foreach (var type in dummyTypes)
        {
            var tempType = Activator.CreateInstance(type) as DummyType;
            dummies.Add(tempType);
        }

        ButtonPanel();

    }

    public DummyType GetDummy(string dummyType)
    {
        foreach (DummyType dummy in dummies)
        {
            if (dummy.Name == dummyType)
            {
                Debug.Log(dummy.Name);
                var target = Activator.CreateInstance(dummy.GetType()) as DummyType;

                return target;
            }
        }

        return null;
    }

    void ButtonPanel()
    {
        foreach (DummyType dummy in dummies)
        {
            var button = Instantiate(buttonPrefab);
            button.transform.SetParent(buttonPanel.transform);
            button.gameObject.name = dummy.Name + " Button";
            button.GetComponentInChildren<TextMeshProUGUI>().text = dummy.Name;
        }
    }
}
