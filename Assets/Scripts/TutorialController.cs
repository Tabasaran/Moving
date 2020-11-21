using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    public GameObject tutorialPanel;

    private float time = 0f;
    private bool isAFK = true;

    public bool showInStart;
    private float showInSecondsIfAFK = 5f;

    private void Start()
    {
        StartCoroutine(ShowTutorial());
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            time = 0f;
            isAFK = false;
            tutorialPanel.SetActive(false);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isAFK = true;
        }
        else if (isAFK)
        {
            time += Time.deltaTime;
            if (time > showInSecondsIfAFK)
            {
                tutorialPanel.SetActive(true);
            }
        }
    }

    private IEnumerator ShowTutorial()
    {
        yield return new WaitForSeconds(2.55f);
        if (showInStart)
        {
            tutorialPanel.SetActive(true);
        }
        else time = 0f;

    }
}
