using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DummyButton : MonoBehaviour
{
    private DummyFactory factory;

    TextMeshProUGUI btnText;

    // Start is called before the first frame update
    void Start()
    {
        factory = GameObject.Find("GameManager").GetComponent<DummyFactory>();

        btnText = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void OnClickSpawn()
    {
        switch (btnText.text)
        {
            case "DummyShort":
                factory.GetDummy("DummyShort").Create(factory.dummyPrefab1).transform.position =
                    GameObject.FindGameObjectsWithTag("Player")[0].transform.position + GameObject.FindGameObjectsWithTag("Player")[0].transform.forward * 5;
                break;

            case "DummyTall":
                factory.GetDummy("DummyTall").Create(factory.dummyPrefab2).transform.position =
                    GameObject.FindGameObjectsWithTag("Player")[0].transform.position + GameObject.FindGameObjectsWithTag("Player")[0].transform.forward * 5;
                break;

            default:
                break;
        }
    }
}
