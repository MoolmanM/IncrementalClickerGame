using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Tutorial : MonoBehaviour
{
    public Swipe swipe;

    public GameObject objTapGatherSticksOnce, objNoticeLumber, objGathered10Sticks, objCraftWoodenAxe, objBuilding, objSkipTutorial, objTextDescription;
    public GameObject objNoticeLumberTA, objGather10SticksTA, objGathered10SticksTA, objBuildingTA, objSwipingRight, objMainTutorial;
    public bool hasSwipedToCrafting, hasReceivedGatheringObjective, hasSwipedToBuildings, hasCompletedTutorials, hasBuiltLumberMill, hasTappedGatherSticks, hasNoticedLumber ,hasGathered10Sticks, hasCraftedWoodenAxe, hasNoticedNewBuilding, hasSkippedTutorial;
    public Button btnGatherSticks, btnCraftWoodenAxe, btnBuildLumberMill;   
    public TMP_Text textDescription;

    
    void Start()
    {
        hasCompletedTutorials = PlayerPrefs.GetInt("hasCompletedTutorials") == 1 ? true : false;
        hasBuiltLumberMill = PlayerPrefs.GetInt("hasBuiltLumberMill") == 1 ? true : false;
        hasSwipedToBuildings = PlayerPrefs.GetInt("hasSwipedToBuildings") == 1 ? true : false;
        hasTappedGatherSticks = PlayerPrefs.GetInt("hasTappedGatherSticks") == 1 ? true : false;
        hasNoticedLumber = PlayerPrefs.GetInt("hasNoticedLumber") == 1 ? true : false;
        hasGathered10Sticks = PlayerPrefs.GetInt("hasGathered10Sticks") == 1 ? true : false;
        hasSwipedToCrafting = PlayerPrefs.GetInt("hasSwipedToCrafting") == 1 ? true : false;
        hasCraftedWoodenAxe = PlayerPrefs.GetInt("hasCraftedWoodenAxe") == 1 ? true : false;
        hasNoticedNewBuilding = PlayerPrefs.GetInt("hasNoticedNewBuilding") == 1 ? true : false;
        hasSwipedToBuildings = PlayerPrefs.GetInt("hasSwipedToBuildings") == 1 ? true : false;

        if (hasCompletedTutorials)
        {
            objMainTutorial.SetActive(false);
        }
        else
        {
            if (!hasTappedGatherSticks)
            {
                StartTapGatherSticksTutorial();
            }
            else if (!hasNoticedLumber)
            {
                StartNoticeLumberTutorial();
            }
            else if (!hasGathered10Sticks)
            {
                StartGather10SticksTutorial();
            }
            else if (!hasSwipedToCrafting)
            {
                StartSwipeToCraftingTutorial();
            }
            else if (!hasCraftedWoodenAxe)
            {
                StartCraftWoodenAxeTutorial();
            }
            else if (!hasSwipedToBuildings)
            {
                StartSwipeToBuildingTutorial();
            }
            else if (!hasNoticedNewBuilding)
            {
                StartNoticeNewBuildingTutorial();
            }
            else if (!hasBuiltLumberMill)
            {
                StartBuildLumberMillTutorial();
            }      
        }      
    }
    private void StartBuildLumberMillTutorial()
    {      
        // Disable swiping left
        
        hasCompletedTutorials = true;
        objSwipingRight.SetActive(false);
        objBuilding.SetActive(true);
        objCraftWoodenAxe.SetActive(false);
        Resource.Resources[ResourceType.Lumber].amount += 20;
        textDescription.text = "";
        StopAllCoroutines();
        StartCoroutine(EffectTypewriter("Now tap here to build a Lumber Mill", textDescription));
    }
    private void StartTapGatherSticksTutorial()
    {
        objTapGatherSticksOnce.SetActive(true);
        objTextDescription.SetActive(true);
        textDescription.text = "";
        StopAllCoroutines();
        StartCoroutine(EffectTypewriter("Welcome Founder. To get started, tap here.", textDescription));
    }
    private void StartNoticeLumberTutorial()
    {
        if (objTextDescription.activeSelf == false)
        {
            objTextDescription.SetActive(true);
        }
        objNoticeLumber.SetActive(true);
        textDescription.text = "";
        objNoticeLumberTA.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(EffectTypewriter("Notice that your Lumber has increased!\n(Tap anywhere to continue).", textDescription));
    }
    private void StartGather10SticksTutorial()
    {
        if (!objNoticeLumber.activeSelf)
        {
            objNoticeLumber.SetActive(true);
        }
        hasNoticedLumber = true;
        objGather10SticksTA.SetActive(true);
        objNoticeLumberTA.SetActive(false);
        textDescription.text = "";
        StopAllCoroutines();
        StartCoroutine(EffectTypewriter("Now gather until you have 10 Lumber.\n(Tap anywhere to continue).", textDescription));
    }
    private void StartSwipeToCraftingTutorial()
    {
        hasGathered10Sticks = true;
        objGathered10Sticks.SetActive(true);
        objTextDescription.SetActive(true);
        objGathered10SticksTA.SetActive(true);
        textDescription.text = "";
        StopAllCoroutines();
        StartCoroutine(EffectTypewriter("Well done! You have now unlocked a new crafting option, swipe to go check it out.\n(Swipe left!).", textDescription));
    }
    private void StartCraftWoodenAxeTutorial()
    {
        hasSwipedToBuildings = true;
        objGathered10Sticks.SetActive(false);
        objCraftWoodenAxe.SetActive(true);
        textDescription.text = "";
        StopAllCoroutines();
        StartCoroutine(EffectTypewriter("Now it's time we automate gathering sticks, craft a 'Wooden Axe'", textDescription));
    }
    private void StartNoticeNewBuildingTutorial()
    {

    }
    private void StartSwipeToBuildingTutorial()
    {
        hasCraftedWoodenAxe = true;
        objSwipingRight.SetActive(true);
        btnCraftWoodenAxe.onClick.Invoke();
        objCraftWoodenAxe.SetActive(false);
        textDescription.text = "";
        StopAllCoroutines();
        StartCoroutine(EffectTypewriter("Great! now swipe right to start building some 'Lumber Mill's", textDescription));
    }
    public void OnTappedGatherSticks()
    {
        hasTappedGatherSticks = true;
        btnGatherSticks.onClick.Invoke();
        objTapGatherSticksOnce.SetActive(false);
        StartNoticeLumberTutorial();
    }
    public void NoticeLumberOnTA()
    {
        StartGather10SticksTutorial();
    }
    public void Gather10SticksOnTA()
    {
        objGather10SticksTA.SetActive(false);
        objNoticeLumber.SetActive(false);
        objTextDescription.SetActive(false);
    }
    public void Gathered10SticksOnTA()
    {
        objGathered10SticksTA.SetActive(false);
        objGathered10Sticks.SetActive(true);
    }
    public void OnCraftWoodenAxe()
    {
        StartSwipeToBuildingTutorial();
    }
    public void OnBuildLumberMill()
    {
        hasBuiltLumberMill = true;
        btnBuildLumberMill.onClick.Invoke();
        objBuildingTA.SetActive(true);
        textDescription.text = "";
        StopAllCoroutines();
        StartCoroutine(EffectTypewriter("Excellent, now you are automatically generating Lumber, do this for Food also and you are well on your way Founder. (Tap anywhere to continue)", textDescription));
    }
    public void BuiltLumberMillOnTA()
    {
        objBuilding.SetActive(false);
        objTextDescription.SetActive(false);
        objMainTutorial.SetActive(false);
    }
    public void OnSkipTutorial()
    {
        hasSkippedTutorial = true;
        hasCompletedTutorials = true;
        objMainTutorial.GetComponent<Canvas>().enabled = false;
    }

    private IEnumerator EffectTypewriter(string text, TMP_Text uiText)
    {
        foreach (char character in text.ToCharArray())
        {
            uiText.text += character;
            yield return new WaitForSeconds(0.08f);
        }
    }
    private void Update()
    {
        if (!hasGathered10Sticks)
        {
            if (Resource.Resources[ResourceType.Lumber].amount == 10)
            {
                StartSwipeToCraftingTutorial();
            }
        }
        if (hasGathered10Sticks && !hasSwipedToBuildings)
        {
            if (swipe.SwipeLeft)
            {
                StartCraftWoodenAxeTutorial();
            }
        }
        if (!hasCompletedTutorials && hasCraftedWoodenAxe)
        {
            if (swipe.SwipeRight)
            {
                StartBuildLumberMillTutorial();
            }
        }
    }
    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("hasCompletedTutorials", hasCompletedTutorials ? 1 : 0);
        PlayerPrefs.SetInt("hasBuiltLumberMill", hasBuiltLumberMill ? 1 : 0);
        //PlayerPrefs.SetInt("hasSwipedToBuildings", hasSwipedToBuildings ? 1 : 0);
        PlayerPrefs.SetInt("hasNoticedLumber", hasNoticedLumber ? 1 : 0);
        //PlayerPrefs.SetInt("hasReceivedGatheringObjective", hasReceivedGatheringObjective ? 1 : 0);
        PlayerPrefs.SetInt("hasGathered10Sticks", hasGathered10Sticks ? 1 : 0);
        PlayerPrefs.SetInt("hasSwipedToCrafting", hasSwipedToCrafting ? 1 : 0);
        PlayerPrefs.SetInt("hasCraftedWoodenAxe", hasCraftedWoodenAxe ? 1 : 0);
        PlayerPrefs.SetInt("hasSwipedToCrafting", hasSwipedToCrafting ? 1 : 0);
        PlayerPrefs.SetInt("hasNoticedNewBuilding", hasNoticedNewBuilding ? 1 : 0);
        PlayerPrefs.SetInt("hasSkippedTutorial", hasSkippedTutorial ? 1 : 0);
        PlayerPrefs.SetInt("hasTappedGatherSticks", hasTappedGatherSticks ? 1 : 0);
    }
}
