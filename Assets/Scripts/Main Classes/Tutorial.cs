using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Tutorial : MonoBehaviour
{
    public bool isActiveButtonTutDone;

    public Button btnGatherSticks;
    public GameObject panelTutGatherSticks, panelTutResourceSticks, panelSkipTutorial, panelTutActiveButtons, panelTextSticksResource;
    public TMP_Text textSticksResource, textWelcomeTutorial;
    private bool isTutActiveButtons;

    public void Start()
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

        if (TimeManager.hasPlayedBefore)
        {
            if (isTutActiveButtons)
            {
                isTutActiveButtons = true;
                panelTutGatherSticks.SetActive(true);
                panelSkipTutorial.SetActive(true);
                textWelcomeTutorial.text = "";               
                StopAllCoroutines();
                StartCoroutine(EffectTypewriter("Welcome Founder. To get started, you should try and gather some sticks. Try tapping here.", textWelcomeTutorial));
            }
            else
            {
                panelTutGatherSticks.SetActive(false);
                panelSkipTutorial.SetActive(false);
            }
        } 
        else
        {
            isTutActiveButtons = true;
            panelTutGatherSticks.SetActive(true);
            panelSkipTutorial.SetActive(true);
            textWelcomeTutorial.text = "";
            StopAllCoroutines();
            StartCoroutine(EffectTypewriter("Welcome Founder. To get started, you should try and gather some sticks. Try tapping here.", textWelcomeTutorial));
        }
    }
    public void OnGatherSticksTutClick()
    {
        btnGatherSticks.onClick.Invoke();
        panelTutGatherSticks.SetActive(false);
        panelTutResourceSticks.SetActive(true);
        panelSkipTutorial.SetActive(false);
        textSticksResource.text = "";
        StopAllCoroutines();
        StartCoroutine(EffectTypewriter("Notice that your 'Sticks' Resource went up!\n(Tap anywhere to continue).", textSticksResource));
    }
    public void OnPressAnywhereToContinue()
    {
        textSticksResource.text = "";
        StopAllCoroutines();
        StartCoroutine(EffectTypewriter("Collect 30 Sticks to get started.\n(Tap anywhere to continue)", textSticksResource));
    }
    public void OnPressAnywhereToContinue2()
    {
        panelTutActiveButtons.SetActive(false);
        panelSkipTutorial.SetActive(false);
        isTutActiveButtons = false;
    }
    // PlayerPrefs.SetInt("Mute_FX", mute ? 1 : 0);
    // PlayerPrefs.GetInt("Mute_FX") == 1 ? true : false;
    // Need to go do this on all other playerprefs. It's just cleaner.
    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("isTutActiveButtons", isTutActiveButtons ? 1 : 0);
    }

    public void OnSkipTutorial()
    {
        if (isTutActiveButtons)
        {
            isTutActiveButtons = false;
            panelTutActiveButtons.SetActive(false);
            panelSkipTutorial.SetActive(false);
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
}
