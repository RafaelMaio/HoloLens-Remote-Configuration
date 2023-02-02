// ===============================
// AUTHOR     : Rafael Maio (rafael.maio@ua.pt)
// PURPOSE     : Contains the static functions that can be accessed by any script.
// SPECIAL NOTES: X
// ===============================

// ===============================
// AUTHOR     : Rafael Maio (rafael.maio@ua.pt)
// PURPOSE     : Script for handling the user study.
// SPECIAL NOTES: X
// ===============================

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class UsabilityTest : MonoBehaviour
{
    /// <summary>
    /// Orange QRCode prefab.
    /// </summary>
    public GameObject qrCodeTransparentPrefab;

    /// <summary>
    /// Orange cube prefab.
    /// </summary>
    public GameObject cubeTransparentPrefab;

    /// <summary>
    /// Array of orange cubes to be automatically placed.
    /// </summary>
    private GameObject[] cubes = new GameObject[9];

    /// <summary>
    /// Line model game object.
    /// </summary>
    public GameObject lineBoschModel;

    /// <summary>
    /// Number of oranges cubes placed.
    /// </summary>
    private int counter = 0;

    /// <summary>
    /// Communication with the Menu Script script.
    /// </summary>
    public MenuScript menuScript;

    /// <summary>
    /// Usability test date.
    /// </summary>
    private DateTime date;

    /// <summary>
    /// File path for saving the usability tests results.
    /// </summary>
    private string filePathName;

    /// <summary>
    /// Participant identifier.
    /// </summary>
    private int participantId = 0;

    /// <summary>
    /// Unity Start function.
    /// </summary>
    void Start()
    {
        date = DateTime.Now;
        filePathName = Application.persistentDataPath + "/testConfDesktop.txt";
        if (File.Exists(filePathName))
        {
            if (File.ReadAllLines(filePathName).Length == 0)
            {
                participantId = 1;
            }
            else
            {
                participantId = Int32.Parse(File.ReadAllLines(filePathName)[File.ReadAllLines(filePathName).Length - 1].Split(',')[0]) + 1;
            }
        }
        else
        {
            File.Create(filePathName);
            participantId = 1;
        }
    }

    /// <summary>
    /// Place the QRCode.
    /// </summary>
    public void placeQRCode()
    {
        cubes[0] = Instantiate(qrCodeTransparentPrefab, new Vector3(1f, -0.3f, 1f), Quaternion.Euler(new Vector3(0, 0, 0)));
        cubes[0].transform.SetParent(lineBoschModel.transform, false);
    }

    /// <summary>
    /// Enable the next object placement.
    /// </summary>
    /// <param name="numObject">Number of the last placed object.</param>
    public void placeNextObject(int numObject)
    {
        counter += 1;
        menuScript.changeNumObjectsText(numObject);
        //Translação
        if (numObject == 0)
        {
            cubes[counter] = Instantiate(cubeTransparentPrefab, new Vector3(2.48f, -0.2f, -1.5f), Quaternion.Euler(new Vector3(0, 0, 0)));
            cubes[counter].transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
        }
        else if (numObject == 1)
        {
            cubes[counter] = Instantiate(cubeTransparentPrefab, new Vector3(3.1f, -0.42f, -1.6f), Quaternion.Euler(new Vector3(0, 0, 0)));
            cubes[counter].transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
        }
        //Rotação
        else if (numObject == 2)
        {
            cubes[counter] = Instantiate(cubeTransparentPrefab, new Vector3(-1.8f, 0.4f, -0.1f), Quaternion.Euler(new Vector3(0, 45, 0)));
            cubes[counter].transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
        }
        else if (numObject == 3)
        {
            cubes[counter] = Instantiate(cubeTransparentPrefab, new Vector3(4.95f, 0.14f, -1.3f), Quaternion.Euler(new Vector3(10, 70, 35)));
            cubes[counter].transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
        }
        //Escala
        else if (numObject == 4)
        {
            cubes[counter] = Instantiate(cubeTransparentPrefab, new Vector3(-1.60f, -0.43f, 2.91f), Quaternion.Euler(new Vector3(0, 0, 0)));
            cubes[counter].transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
        }
        else if (numObject == 5)
        {
            cubes[counter] = Instantiate(cubeTransparentPrefab, new Vector3(-1.60f, -0.17f, 1.81f), Quaternion.Euler(new Vector3(0, 0, 0)));
            cubes[counter].transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        }
        //TUDO
        else if (numObject == 6)
        {
            cubes[counter] = Instantiate(cubeTransparentPrefab, new Vector3(0.64f, 0.22f, -2.08f), Quaternion.Euler(new Vector3(0, 20, 50)));
            cubes[counter].transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }
        else if (numObject == 7)
        {
            cubes[counter] = Instantiate(cubeTransparentPrefab, new Vector3(1.125f, -0.43f, 0.57f), Quaternion.Euler(new Vector3(80, 0, 0)));
            cubes[counter].transform.localScale = new Vector3(0.35f, 0.35f, 0.35f);
        }
        if (numObject < 8)
        {
            cubes[counter].transform.SetParent(lineBoschModel.transform, false);
            changeSortOrder(cubes[counter].transform);
        }
    }

    /// <summary>
    /// Change the transparent object sorting order, so they appear behind the opaque ones.
    /// </summary>
    /// <param name="objectChild">Object piece transform component.</param>
    private void changeSortOrder(Transform objectChild)
    {
        for (int i = 0; i < objectChild.childCount; i++)
        {
            if (objectChild.GetChild(i).GetComponent<Renderer>() != null)
            {
                objectChild.GetChild(i).GetComponent<Renderer>().sortingOrder = 10;
            }
            changeSortOrder(objectChild.GetChild(i));
        }
    }

    /// <summary>
    /// Stop the object placement timer.
    /// </summary>
    public void stopTime(GameObject goAux, string piece)
    {
        Transform go = goAux.transform;
        TimeSpan time = DateTime.Now - date;
        Vector3 position = go.transform.position - cubes[counter].transform.position;
        Vector3 rotation = go.transform.rotation.eulerAngles - cubes[counter].transform.rotation.eulerAngles;
        if (goAux.GetComponent<Orientate>() != null)
        {
            rotation = goAux.GetComponent<Orientate>().getInitialRotation() - cubes[counter].transform.rotation.eulerAngles;
        }
        Vector3 scale = go.transform.localScale - cubes[counter].transform.localScale;
        using (StreamWriter sw = File.AppendText(filePathName))
        {
            sw.WriteLine(participantId.ToString() + "," + counter.ToString() + "," + time.ToString() + "," +
                position.x.ToString() + "," + position.y.ToString() + "," + position.z.ToString() + "," +
                rotation.x.ToString() + "," + rotation.y.ToString() + "," + rotation.z.ToString() + "," +
                scale.x.ToString() + "," + piece);
        }
        date = DateTime.Now;
        cubes[counter].SetActive(false);
    }

    /// <summary>
    /// Set the initial time.
    /// </summary>
    public void setTime()
    {
        date = DateTime.Now;
    }
}