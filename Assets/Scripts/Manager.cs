// ===============================
// AUTHOR     : Rafael Maio (rafael.maio@ua.pt)
// PURPOSE     : Main script - Handles the configuration logic.
// SPECIAL NOTES: Communicates with the Usability Test script for the user tests.
// ===============================

using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Utilities.Solvers;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Manager : MonoBehaviour
{
    /// <summary>
    /// Model game object.
    /// </summary>
    public GameObject lineBoschParent;

    /// <summary>
    /// Modes enumerate for the setup, adaptation, qrcode placement and object placement.
    /// </summary>
    public enum Modes { SurfacePlacement, FixSurfacePlacement, Tranining, QRCodePlacement, CubePlacement };

    /// <summary>
    /// Current mode.
    /// </summary>
    private Modes modes = Modes.SurfacePlacement;

    /// <summary>
    /// Communication with the Usability Test script.
    /// </summary>
    public UsabilityTest usabilityTest;

    /// <summary>
    /// Objects palced.
    /// </summary>
    private List<GameObject> goS = new List<GameObject>();

    /// <summary>
    /// Blue cube prefab.
    /// </summary>
    public GameObject cubePrefab;

    /// <summary>
    /// QR code prefab.
    /// </summary>
    public GameObject qrcodePrefab;

    /// <summary>
    /// QRCode object.
    /// </summary>
    private GameObject qrcode;

    /// <summary>
    /// Information balloon object - For the component association.
    /// </summary>
    private GameObject balloon;

    /// <summary>
    /// Information balloon prefab.
    /// </summary>
    public GameObject balloonPrefab;

    /// <summary>
    /// Component information.
    /// </summary>
    private List<string> boxPieceInformation = new List<string>();

    /// <summary>
    /// Green cube prefab.
    /// </summary>
    public GameObject cubeDonePrefab;

    /// <summary>
    /// Component associated - Flag for confirmation.
    /// </summary>
    private bool confirmedFlag = false;

    /// <summary>
    /// Training cube object.
    /// </summary>
    private GameObject trainingCube;

    /// <summary>
    /// Place the next cube object.
    /// </summary>
    public void next()
    {
        switch (modes)
        {
            // Place the model on top of a surface.
            case Modes.SurfacePlacement:
                lineBoschParent.GetComponent<TapToPlace>().enabled = false;
                lineBoschParent.GetComponent<ObjectManipulator>().enabled = true;
                lineBoschParent.GetComponent<NearInteractionGrabbable>().enabled = true;
                lineBoschParent.GetComponent<RotationAxisConstraint>().enabled = true;
                lineBoschParent.GetComponent<MoveAxisConstraint>().enabled = true;

                modes = Modes.FixSurfacePlacement;
                break;
            // Translate, rotate and scale the model in the surface.
            case Modes.FixSurfacePlacement:
                lineBoschParent.GetComponent<ObjectManipulator>().enabled = false;
                lineBoschParent.GetComponent<NearInteractionGrabbable>().enabled = false;
                lineBoschParent.GetComponent<RotationAxisConstraint>().enabled = false;
                lineBoschParent.GetComponent<MoveAxisConstraint>().enabled = false;

                trainingCube = Instantiate(cubePrefab, Vector3.zero, Quaternion.identity);
                trainingCube.transform.SetParent(lineBoschParent.transform, false);
                trainingCube.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);

                modes = Modes.Tranining;
                break;
            // Adapation period previous to the user test.
            case Modes.Tranining:
                Destroy(trainingCube);

                usabilityTest.placeQRCode();

                qrcode = Instantiate(qrcodePrefab, Vector3.zero, Quaternion.identity);
                qrcode.transform.SetParent(lineBoschParent.transform, false);

                usabilityTest.setTime();

                modes = Modes.QRCodePlacement;
                break;
            // QRCode placement for computing objects coordinations in relation to its pose.
            case Modes.QRCodePlacement:

                usabilityTest.stopTime(qrcode, "aux_go");

                qrcode.transform.GetChild(1).gameObject.SetActive(false);

                usabilityTest.placeNextObject(goS.Count);
                GameObject firstCube = Instantiate(cubePrefab, Vector3.zero, Quaternion.identity);
                goS.Add(firstCube);
                firstCube.transform.SetParent(lineBoschParent.transform, false);

                firstCube.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);

                firstCube.GetComponent<RotationAxisConstraint>().enabled = true;
                firstCube.GetComponent<MinMaxScaleConstraint>().enabled = true;

                modes = Modes.CubePlacement;
                break;
            // Cube objects placement.
            case Modes.CubePlacement:
                if (confirmedFlag)
                {
                    usabilityTest.placeNextObject(goS.Count);
                    GameObject cube = Instantiate(cubePrefab, Vector3.zero, Quaternion.identity);
                    cube.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
                    
                    // Usability new Tests:
                    if (goS.Count < 2)
                    {
                        cube.GetComponent<RotationAxisConstraint>().enabled = true;
                        cube.GetComponent<MinMaxScaleConstraint>().enabled = true;
                    }
                    else if (goS.Count < 4)
                    {
                        cube.GetComponent<MoveAxisConstraint>().enabled = true;
                        cube.GetComponent<MinMaxScaleConstraint>().enabled = true;
                        if (goS.Count == 2)
                        {
                            cube.transform.position = new Vector3(-1.8f, 0.4f, -0.1f);
                        }
                        else
                        {
                            cube.transform.position = new Vector3(4.95f, 0.14f, -1.3f);
                        }
                    }
                    else if (goS.Count < 6)
                    {
                        cube.GetComponent<RotationAxisConstraint>().enabled = true;
                        cube.GetComponent<MoveAxisConstraint>().enabled = true;
                        if (goS.Count == 4)
                        {
                            cube.transform.position = new Vector3(-1.6f, -0.43f, 2.91f);
                        }
                        else
                        {
                            cube.transform.position = new Vector3(-1.60f, -0.17f, 1.81f);
                        }
                    }
                    goS.Add(cube);
                    cube.transform.SetParent(lineBoschParent.transform, false);
                    confirmedFlag = false;
                }
                break;
        }
    }

    /// <summary>
    /// Return to the previous mode.
    /// </summary>
    public void back()
    {
        switch (modes)
        {
            case Modes.FixSurfacePlacement:
                lineBoschParent.GetComponent<TapToPlace>().enabled = true;
                lineBoschParent.GetComponent<ObjectManipulator>().enabled = false;
                lineBoschParent.GetComponent<NearInteractionGrabbable>().enabled = false;
                lineBoschParent.GetComponent<RotationAxisConstraint>().enabled = false;
                lineBoschParent.GetComponent<MoveAxisConstraint>().enabled = false;

                modes = Modes.SurfacePlacement;
                break;
            case Modes.QRCodePlacement:
                lineBoschParent.GetComponent<ObjectManipulator>().enabled = true;
                lineBoschParent.GetComponent<NearInteractionGrabbable>().enabled = true;
                lineBoschParent.GetComponent<RotationAxisConstraint>().enabled = true;
                lineBoschParent.GetComponent<MoveAxisConstraint>().enabled = true;

                modes = Modes.FixSurfacePlacement;
                break;
            case Modes.CubePlacement:
                if (getGoSCount())
                {
                    qrcode.transform.GetChild(1).gameObject.SetActive(true);
                    modes = Modes.QRCodePlacement;
                }
                GameObject toRemove = goS[goS.Count - 1];
                goS.RemoveAt(goS.Count - 1);
                Destroy(toRemove);
                break;
        }
    }

    /// <summary>
    /// Get the current mode.
    /// </summary>
    /// <returns>The current mode.</returns>
    public Modes getCurrentMode()
    {
        return modes;
    }

    /// <summary>
    /// Change from object placement to the component association and vice-versa.
    /// </summary>
    /// <param name="associationOn">Is the association on?</param>
    public void changeGoToAssociation(bool associationOn)
    {
        //Change to association.
        if (associationOn)
        {
            balloon = Instantiate(balloonPrefab, lineBoschParent.transform);
            balloon.transform.position = goS[goS.Count - 1].transform.position;
            balloon.transform.rotation = goS[goS.Count - 1].transform.rotation;
            balloon.transform.localScale = goS[goS.Count - 1].transform.localScale * 6;
            if (boxPieceInformation.Count > 0)
            {
                balloon.transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>().text = boxPieceInformation[0];
                balloon.transform.GetChild(1).GetChild(1).GetComponent<TMP_Text>().text = boxPieceInformation[1];
                balloon.transform.GetChild(1).GetChild(2).GetComponent<TMP_Text>().text = boxPieceInformation[2];
            }
            GameObject toRemove = goS[goS.Count - 1];
            goS.RemoveAt(goS.Count - 1);
            Destroy(toRemove);
        }
        // Change to object placement.
        else
        {
            if (balloon != null)
            {
                if (!confirmedFlag)
                {
                    GameObject newGo = Instantiate(cubePrefab, lineBoschParent.transform);
                    newGo.transform.position = balloon.transform.position;
                    newGo.transform.rotation = Quaternion.Euler(balloon.GetComponent<Orientate>().getInitialRotation());
                    newGo.transform.localScale = balloon.transform.localScale / 6;
                    //newGo.transform.SetParent(lineBoschParent.transform, false);
                    Destroy(balloon);
                    goS.Add(newGo);
                }
            }
        }
    }

    /// <summary>
    /// Component associated.
    /// </summary>
    /// <param name="clickedButton">Button from the component list pressed.</param>
    public void componentClicked(GameObject clickedButton)
    {
        string[] info = clickedButton.GetComponent<ButtonConfigHelper>().MainLabelText.Split('\n');
        boxPieceInformation = new List<string> { info[0], info[1], info[2] };
        balloon.transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>().text = boxPieceInformation[0];
        balloon.transform.GetChild(1).GetChild(1).GetComponent<TMP_Text>().text = boxPieceInformation[1];
        balloon.transform.GetChild(1).GetChild(2).GetComponent<TMP_Text>().text = boxPieceInformation[2];
    }

    /// <summary>
    /// Confirm the component association.
    /// </summary>
    public void confirmAssociation()
    {
        if (balloon != null)
        {
            GameObject newGo = Instantiate(cubeDonePrefab, lineBoschParent.transform);
            newGo.transform.position = balloon.transform.position;
            newGo.transform.rotation = Quaternion.Euler(balloon.GetComponent<Orientate>().getInitialRotation());
            newGo.transform.localScale = balloon.transform.localScale / 6;
            //newGo.transform.SetParent(lineBoschParent.transform, false);
            Destroy(balloon);
            goS.Add(newGo);

            usabilityTest.stopTime(goS[goS.Count - 1], boxPieceInformation[1]);

            boxPieceInformation.Clear();
            confirmedFlag = true;
        }
    }

    /// <summary>
    /// Get if the box has any component associated.
    /// </summary>
    /// <returns>0 if no component associated and more than 0 otherwise.</returns>
    public int getBoxPieceInformation()
    {
        return boxPieceInformation.Count;
    }

    /// <summary>
    /// Get the number of objects in the scene.
    /// </summary>
    /// <returns>The number of objects in the scene.</returns>
    public bool getGoSCount()
    {
        return goS.Count == 1;
    }
}