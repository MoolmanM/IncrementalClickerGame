using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Tutorial : MonoBehaviour
{
    public Swipe _Swipe;

    public bool isActiveButtonTutDone;
    public Button btnGatherSticks, btnCraftWoodenHoe;
    public GameObject objActiveButtons, objGathering, objBuilding, objWorker,  objPanelGather, objPanelResource, objPanelGathered30, objPanelCraft, objPanelBuilding, objSkipTutorial, objPanelFirstWorker, objWorkerHighlight, objWorkerPoint;
    public TMP_Text textDescriptionResource, textWelcomeTutorial, textGathered30Sticks, textNewBuilding;
    private bool isTutActiveButtons, isTutGathering30Sticks, isTutSwipingToCrafting, isTutCraftedWoodenHoe, isTutSwipingToWorkers, isTutSwipingToResearch, isTutFirstWorker;

    private float _timer = 0.1f;

    void Start()
    {
        isTutActiveButtons = PlayerPrefs.GetInt("isTutActiveButtons") == 1 ? true : false;

        // Here we get playerprefs for the bools.
        // Do I need to check here through every bool assigned to a tutorial? I do believe so.
        // If I do, do that. I think it'll be best to check for the latest tutorial first and go down the list. 
        // So if you have completed tutorial 3, I don't have to check if you've done tutorial 1 because obviously you have then.
        // Also include a small button somewhere in the corner that asks if they want to skip the tutorial.
        // Skip tutorial should skip the entire category. So for food and potatoes, it should skip both since they are connected.
        // Actually have the first tutorial revolve around sticks so they can start crafting and building buildings. 
        // Also as soon as they get the first worker. The workers need to start deducting from food per second.
        // Conveniently should be able to use totalworkers for that problem.
        // Have a checkbox on the top right 'Disable tutorials?', and if you click that, you get a prompt that asks:
        // Are you sure? (You'll be able to renable this later from the options menu./
        // Have a typing animation with sound everytime there is text in the tutorials. Depending on difficulty of course.
        // Need a way to figure out which panel is to the right of the current panel and which is to the left.
        // Then lets say you craft something while in the crafting panel, a one shows towards the building panel if you unlocked a building.
        // And a one shows towards the right side if a new worker job is available or if there is new research to be done.
        // But also, If I craft something I need to check if there is a 1 already, + 1 it. So it becomes two.
        // So basically + 1 for every single thing that you unlocked there.
        // Then when the panel becomes active where the thing is that was unlocked, make sure to set these numbers to zero.
        // BUT Lets say you go the worker tab because there was a 2 pointing towards it.
        // And you open that panel, a new number needs to display again to the right if you also have new research options that you haven't seen it.
        // For everything unlocked set the 'isSeen' to false.
        // Then in UiManager, everytime you swipe panels, check to see if any 'isSeen' is false. And if it is false, turn it to true. 
        // Also use this, if any isSeen, increase the number pointer 
        // I could also use a number pointer for each panel as in gameobject. 
        // BUt for now I'll try just use one for left and one for right, instead of one each for each panel there is.
        // Need to add total amount of notifications that is to each side.

        // Have a tutorial for when the first worker arrives. Show them how to assign workers.
        // Then have a tutorial for when 10 workers arrive. 'Your latest worker learnt how to research stuff' or whatever.
        // Then show them how to research the first thing. 
        // Force them to swipe all the way to the right until they reach the wanted panel.

        // slider from fantasyiu

        // Different notification color, depending on if the event was a positive or a negative.
        if (TimeManager.hasPlayedBefore)
        {
            if (isTutActiveButtons)
            {
                isTutActiveButtons = true;
                objActiveButtons.SetActive(true);
                objPanelGather.SetActive(true);
                objSkipTutorial.SetActive(true);
                textWelcomeTutorial.text = "";               
                StopAllCoroutines();
                StartCoroutine(EffectTypewriter("Welcome Founder. To get started, you should try and gather some sticks. Try tapping here.", textWelcomeTutorial));
            }
            else
            {
                objActiveButtons.SetActive(false);
                objPanelGather.SetActive(false);
                objSkipTutorial.SetActive(false);
            }
        } 
        else
        {
            isTutActiveButtons = true;
            objActiveButtons.SetActive(true);
            objPanelGather.SetActive(true);
            objSkipTutorial.SetActive(true);
            textWelcomeTutorial.text = "";
            StopAllCoroutines();
            StartCoroutine(EffectTypewriter("Welcome Founder. To get started, you should try and gather some sticks. Try tapping here.", textWelcomeTutorial));
        }
    }
    public void OnGatherSticksTutClick()
    {
        btnGatherSticks.onClick.Invoke();
        objPanelGather.SetActive(false);
        objPanelResource.SetActive(true);
        objSkipTutorial.SetActive(false);
        textDescriptionResource.text = "";
        StopAllCoroutines();
        StartCoroutine(EffectTypewriter("Notice that your 'Sticks' Resource went up!\n(Tap anywhere to continue).", textDescriptionResource));
    }
    public void OnCraftWoodenHoeTutClick()
    {
        btnCraftWoodenHoe.onClick.Invoke();
        objPanelCraft.SetActive(false);
        objSkipTutorial.SetActive(false);
        isTutCraftedWoodenHoe = true;
    }
    public void OnPressAnywhereToContinue()
    {
        textDescriptionResource.text = "";
        StopAllCoroutines();
        StartCoroutine(EffectTypewriter("Collect 30 Sticks to get started.\n(Tap anywhere to continue)", textDescriptionResource));
    }
    public void OnPressAnywhereToContinue2()
    {
        objActiveButtons.SetActive(false);
        objSkipTutorial.SetActive(false);
        isTutActiveButtons = false;
        isTutGathering30Sticks = true;
    }
    // PlayerPrefs.SetInt("Mute_FX", mute ? 1 : 0);
    // PlayerPrefs.GetInt("Mute_FX") == 1 ? true : false;
    void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("isTutActiveButtons", isTutActiveButtons ? 1 : 0);
        // Need to save the rest of the tutorial bools, to make sure when the player accidentally leaves the game, that they will then again be greeted with the same tutorial.
        // Make sure to set all the associated bool to false when skip tutorial is pressed though.

    }
    private void CheckFor30Sticks()
    {
        if ((_timer -= Time.deltaTime) <= 0)
        {
            _timer = 1f;

            if (isTutGathering30Sticks)
            {
                if (Resource.Resources[ResourceType.Sticks].amount >= 30)
                {
                    objSkipTutorial.SetActive(true);
                    objGathering.SetActive(true);
                    objPanelGathered30.SetActive(true);
                    textGathered30Sticks.text = "";
                    StopAllCoroutines();
                    StartCoroutine(EffectTypewriter("Well done! Now you should be able to craft something with those sticks you gathered.\n(<b>Try swiping to the right</b>)", textGathered30Sticks));
                    isTutGathering30Sticks = false;
                    isTutSwipingToCrafting = true;
                }
            }
        }      
    }
    private void CheckForCrafted()
    {
        if (UIManager.isBuildingVisible && isTutCraftedWoodenHoe)
        {
            textNewBuilding.text = "";
            StopAllCoroutines();
            StartCoroutine(EffectTypewriter("Notice you have unlocked a new building! Now tap here to learn more about this new building", textNewBuilding));

            // Notice you have unlocked a new building, press anywhere to continue.
            // Then show them how to expand the extra information, show them how much it costs, and to get to that point to then build the building.
            // Then just let them know that this will be an automatic production. certainamount/sec.
            // Also somewhere show them to click and hold on the season icon. And tell them to be wary of the seasons.
            // When doing a pointer, use like a hand graphic pointing at something, and then blue out the rest.
            // Terrestrial Sculpting - Climate Restoration - Ecological Adaptation (All of them) (Don't forget the "WorldShaper" and "Master over Nature" Ascension Perk)
            // Apperently stellaris terms.
            // Pausing tutorial system for now, just want to get further into the game and then decide everything I need to do a tutorial for.

        }
    }
    private void CheckForCrafting()
    {
        if (UIManager.isCraftingVisible && isTutSwipingToCrafting)
        {
            objPanelGathered30.SetActive(false);
            objPanelCraft.SetActive(true);
            isTutSwipingToCrafting = false;
        }
    }
    public void OnSkipTutorial()
    {
        if (isTutActiveButtons)
        {
            isTutActiveButtons = false;
            objActiveButtons.SetActive(false);
            objGathering.SetActive(false);
            objSkipTutorial.SetActive(false);
        }

        // Going to have to check for every tutorial here to know what to set to false.
    }
    private IEnumerator EffectTypewriter(string text, TMP_Text uiText)
    {
        foreach (char character in text.ToCharArray())
        {
            uiText.text += character;
            yield return new WaitForSeconds(0.05f);
        }
    }
    void Update()
    {
        CheckFor30Sticks();
        CheckForCrafting();
        CheckForCrafted();
    }
}
