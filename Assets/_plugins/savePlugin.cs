using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using UnityEngine.InputSystem;

public class savePlugin : MonoBehaviour
{
    [DllImport("engineLabDLL")]
    private static extern int getID();

    [DllImport("engineLabDLL")]
    private static extern void setID(int i);

    [DllImport("engineLabDLL")]
    private static extern Vector3 getPosition();

    [DllImport("engineLabDLL")]
    private static extern void setPosition(float x, float y, float z);

    [DllImport("engineLabDLL")]
    private static extern void saveToFile(int id, float x, float y, float z);

    [DllImport("engineLabDLL")]
    private static extern void startWriting(string filename);

    [DllImport("engineLabDLL")]
    private static extern void endWriting();


     UserInput inputAction;

    string path;
    string fn;

    private void Start()
    {
        inputAction = InputController.controller.inputAction;

        inputAction.Player.Save.performed += cntxt => OnSave();

        path = Application.dataPath;
        fn = path + "/save.txt";
        Debug.Log(fn);
        Debug.Log(getPosition());
    }

    void OnSave()
    {
        startWriting(fn);
        foreach(GameObject o in GameObject.FindGameObjectsWithTag("Pachinko"))
        {
            Vector3 t = o.transform.position;
            setPosition(t.x, t.y, t.z);
            saveToFile(1, t.x, t.y, t.z);
        }
        endWriting();
    }
}
