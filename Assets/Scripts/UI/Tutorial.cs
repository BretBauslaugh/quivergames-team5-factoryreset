using System;
using UnityEngine;

namespace UI //Rider standard
{
    public class Tutorial : MonoBehaviour
    {
        public int tutorialIndex; //key to let other scripts know which part in the tutorial the player is in
        [SerializeField] private GameObject[] tutorialObjects; //list of tutorial objects
        [SerializeField] private bool skipTutorial; //tutorial won't show up if its enabled
        [SerializeField] private bool autoStart; //starts first tutorialObject at Start()

        public void UpdateTutorial(int tutorialNum)
        {
            if (skipTutorial) return;
            foreach (GameObject obj in tutorialObjects)
            {
                obj.SetActive(false); //hides every tutorial object
            }
            tutorialObjects[tutorialNum].SetActive(true); //shows selected object
            tutorialIndex = tutorialNum;
        }

        public void SetSkipTutorialMode(bool skip) //so buttons can call Tutorial.cs to either skip the tutorial or re-enable it.
        {
            skipTutorial = skip;
        }

        private void Start()
        {
            UpdateTutorial(0);
        }
    }
}
