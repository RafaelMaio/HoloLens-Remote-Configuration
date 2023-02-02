// ===============================
// AUTHOR     : Rafael Maio (rafael.maio@ua.pt)
// PURPOSE     : Menu navigation script.
// SPECIAL NOTES: X
// ===============================

using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Microsoft.MixedReality.Toolkit.Utilities;

public class MenuScript : MonoBehaviour
{
    /// <summary>
    /// Next stage button.
    /// </summary>
    public GameObject nextButton;

    /// <summary>
    /// Previous stage button.
    /// </summary>
    public GameObject backButton;

    /// <summary>
    /// Component association button.
    /// </summary>
    public GameObject associationButton;

    /// <summary>
    /// Menu with two buttons.
    /// </summary>
    public GameObject PanelTwoButtons;

    /// <summary>
    /// Menu with one button.
    /// </summary>
    public GameObject PanelOneButton;

    /// <summary>
    /// Menu with three buttons.
    /// </summary>
    public GameObject PanelThreeButtons;

    /// <summary>
    /// Communication with the Manager scrit (main script).
    /// </summary>
    public Manager manager;

    /// <summary>
    /// User test progress indicator.
    /// </summary>
    public GameObject progressIndicator;

    /// <summary>
    /// Collection of buttons.
    /// </summary>
    public GameObject buttonsCollection;

    /// <summary>
    /// Collection of components.
    /// </summary>
    public GameObject componentsCollection;

    /// <summary>
    /// File containing every component.
    /// </summary>
    public TextAsset componentsListFile;

    /// <summary>
    /// Prefab for the button of each component.
    /// </summary>
    public GameObject componentButtonPrefab;

    /// <summary>
    /// List of component buttons.
    /// </summary>
    private List<GameObject> componentsButtonList = new List<GameObject>();

    /// <summary>
    /// Unity Start function.
    /// </summary>
    private void Start()
    {
        LoadComponentsLists();
    }

    /// <summary>
    /// Next button pressed.
    /// </summary>
    public void next()
    {
        switch (manager.getCurrentMode())
        {
            case Manager.Modes.SurfacePlacement:
                PanelTwoButtons.SetActive(true);
                backButton.SetActive(true);
                PanelOneButton.SetActive(false);
                break;
            case Manager.Modes.FixSurfacePlacement:
                break;
            case Manager.Modes.Tranining:
                break;
            case Manager.Modes.QRCodePlacement:
                changeNextButtonName();
                progressIndicator.SetActive(true);
                PanelThreeButtons.SetActive(true);
                associationButton.SetActive(true);
                PanelTwoButtons.SetActive(false);
                break;
            case Manager.Modes.CubePlacement:
                break;
        }
        manager.next();
    }

    /// <summary>
    /// Back button pressed.
    /// </summary>
    public void back()
    {
        switch (manager.getCurrentMode())
        {
            case Manager.Modes.FixSurfacePlacement:
                PanelTwoButtons.SetActive(false);
                backButton.SetActive(false);
                PanelOneButton.SetActive(true);
                break;
            case Manager.Modes.QRCodePlacement:
                break;
            case Manager.Modes.Tranining:
                break;
            case Manager.Modes.CubePlacement:
                if (manager.getGoSCount())
                {
                    changeNextButtonName();
                    progressIndicator.SetActive(false);
                    PanelThreeButtons.SetActive(false);
                    associationButton.SetActive(false);
                    PanelTwoButtons.SetActive(true);
                }
                break;
        }
        manager.back();
    }

    /// <summary>
    /// Change the number of objects already configured.
    /// </summary>
    /// <param name="numObjs">Number of objects.</param>
    public void changeNumObjectsText(int numObjs)
    {
        Transform bar = StaticFunctions.FindChildByRecursion(progressIndicator.transform, "Bar");
        StaticFunctions.FindChildByRecursion(progressIndicator.transform, "ProgressText").GetComponent<TMP_Text>().text = numObjs + "/8";
        switch (numObjs)
        {
            case 0:
                bar.localScale = new Vector3(0.0f, bar.localScale.y, bar.localScale.z);
                StaticFunctions.FindChildByRecursion(progressIndicator.transform, "MessageText").GetComponent<TMP_Text>().text = "8-738-726-533 - Saco 325x230";
                break;
            case 1:
                bar.localScale = new Vector3(0.125f, bar.localScale.y, bar.localScale.z);
                StaticFunctions.FindChildByRecursion(progressIndicator.transform, "MessageText").GetComponent<TMP_Text>().text = "8-738-710-491 - Mangueira Flexivel";
                break;
            case 2:
                bar.localScale = new Vector3(0.25f, bar.localScale.y, bar.localScale.z);
                StaticFunctions.FindChildByRecursion(progressIndicator.transform, "MessageText").GetComponent<TMP_Text>().text = "8-709-918-680 - Pilha alcalina 1,5V LR20";
                break;
            case 3:
                bar.localScale = new Vector3(0.375f, bar.localScale.y, bar.localScale.z);
                StaticFunctions.FindChildByRecursion(progressIndicator.transform, "MessageText").GetComponent<TMP_Text>().text = "8-738-726-577 - Etiqueta";
                break;
            case 4:
                bar.localScale = new Vector3(0.5f, bar.localScale.y, bar.localScale.z);
                StaticFunctions.FindChildByRecursion(progressIndicator.transform, "MessageText").GetComponent<TMP_Text>().text = "8-710-103-045 - Anilha de vedação";
                break;
            case 5:
                bar.localScale = new Vector3(0.625f, bar.localScale.y, bar.localScale.z);
                StaticFunctions.FindChildByRecursion(progressIndicator.transform, "MessageText").GetComponent<TMP_Text>().text = "8-709-918-850 - Acessório de instalação";
                break;
            case 6:
                bar.localScale = new Vector3(0.75f, bar.localScale.y, bar.localScale.z);
                StaticFunctions.FindChildByRecursion(progressIndicator.transform, "MessageText").GetComponent<TMP_Text>().text = "7-709-003-556 - Acessório Nr.1083";
                break;
            case 7:
                bar.localScale = new Vector3(0.875f, bar.localScale.y, bar.localScale.z);
                StaticFunctions.FindChildByRecursion(progressIndicator.transform, "MessageText").GetComponent<TMP_Text>().text = "8-731-500-264 - Manípulo";
                break;
            case 8:
                bar.localScale = new Vector3(1f, bar.localScale.y, bar.localScale.z);
                StaticFunctions.FindChildByRecursion(progressIndicator.transform, "MessageText").GetComponent<TMP_Text>().text = "Configuration Done!";
                break;
        }
    }

    /// <summary>
    /// Change the button name depending on the menu.
    /// </summary>
    public void changeNextButtonName()
    {
        if(nextButton.GetComponent<ButtonConfigHelper>().MainLabelText.Equals("Next Step")){
            nextButton.GetComponent<ButtonConfigHelper>().MainLabelText = "Add Object";
            nextButton.GetComponent<ButtonConfigHelper>().SetQuadIconByName("IconAdd");
            backButton.GetComponent<ButtonConfigHelper>().MainLabelText = "Remove Object";
            backButton.GetComponent<ButtonConfigHelper>().SetQuadIconByName("IconClose");
        }
        else
        {
            nextButton.GetComponent<ButtonConfigHelper>().MainLabelText = "Next Step";
            nextButton.GetComponent<ButtonConfigHelper>().SetQuadIconByName("IconHide");
            backButton.GetComponent<ButtonConfigHelper>().MainLabelText = "Back";
            backButton.GetComponent<ButtonConfigHelper>().SetQuadIconByName("IconHide");
        }
    }

    /// <summary>
    /// Open/close the association menu when the button is pressed.
    /// </summary>
    /// <param name="open">Open/close flag.</param>
    public void openAssociationMenu(bool open)
    {
        componentsCollection.SetActive(open);
        buttonsCollection.SetActive(!open);
        manager.changeGoToAssociation(open);
    }

    /// <summary>
    /// Load the list of existing components from the file.
    /// </summary>
    public void LoadComponentsLists()
    {
        string[] lines = componentsListFile.text.Split('\n');
        foreach (string line in lines)
        {
            string[] fields = line.Split(';');

            GameObject spawnedButton = Instantiate(componentButtonPrefab, componentsCollection.transform);
            spawnedButton.GetComponent<ButtonConfigHelper>().MainLabelText = fields[0] + "\n" + fields[1] + "\n" + fields[2];
            spawnedButton.SetActive(true);
            componentsButtonList.Add(spawnedButton);
        }
        componentsCollection.gameObject.GetComponent<GridObjectCollection>().UpdateCollection();
    }

    /// <summary>
    /// Association confirmation button pressed.
    /// </summary>
    public void confirmAssociation()
    {
        if (manager.getBoxPieceInformation() != 0)
        {
            manager.confirmAssociation();
            openAssociationMenu(false);
            for (int i = 2; i < componentsCollection.transform.childCount; i++)
            {
                StaticFunctions.FindChildByRecursion(componentsCollection.transform.GetChild(i), "Quad").gameObject.SetActive(false);
                StaticFunctions.FindChildByRecursion(componentsCollection.transform.GetChild(i).transform, "QuadClicked").gameObject.SetActive(false);
            }
        }
        next();
    }

    /// <summary>
    /// Component button pressed.
    /// </summary>
    /// <param name="clickedButton">Component button that was pressed.</param>
    public void componentClicked(GameObject clickedButton)
    {
        for (int i = 2; i < componentsCollection.transform.childCount; i++)
        {
            if (!componentsCollection.transform.GetChild(i).GetComponent<ButtonConfigHelper>().MainLabelText.Equals(
                clickedButton.GetComponent<ButtonConfigHelper>().MainLabelText))
            {
                StaticFunctions.FindChildByRecursion(componentsCollection.transform.GetChild(i), "Quad").gameObject.SetActive(true);
                StaticFunctions.FindChildByRecursion(componentsCollection.transform.GetChild(i), "QuadClicked").gameObject.SetActive(false);
            }
        }
        StaticFunctions.FindChildByRecursion(clickedButton.transform, "Quad").gameObject.SetActive(false);
        StaticFunctions.FindChildByRecursion(clickedButton.transform, "QuadClicked").gameObject.SetActive(true);
        manager.componentClicked(clickedButton);
    }
}