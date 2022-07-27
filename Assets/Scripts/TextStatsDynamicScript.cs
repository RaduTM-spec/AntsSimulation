using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class TextStatsDynamicScript : MonoBehaviour
{
    public HomeScript hs;
   
    public TMP_Text text;
    int wanderingAnts = 0, antsGoingHome = 0, antsGoingForFoodOnTrail = 0, foodTrails = 0;
    int counter1,counter2,counter3, counter4;//relative for those up
    // Start is called before the first frame update
    void Start()
    {
       
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateStats();
    }
    void UpdateStats()
    {
        wanderingAnts = 0;
        antsGoingForFoodOnTrail= 0;
        antsGoingHome = 0;
        foodTrails = 0;
        foreach(GameObject ant in hs.antsList)
        {
            try
            {
                AntMovement antDetails = ant.GetComponent<AntMovement>();
                if (antDetails.isMovingRandom)
                    wanderingAnts++;
                if (antDetails.isGoingHome)
                    antsGoingHome++;
                if (antDetails.hasFoodTrail)
                    antsGoingForFoodOnTrail++;
                if (antDetails.hasFoodTrail && !antDetails.hasBorrowedTrails)
                    foodTrails++;
            }
            catch { }
            
        }
        text.text = "Wandering Ants: " + wanderingAnts.ToString() + "\n"
                  + "Ants Going Home: " + antsGoingHome.ToString() + "\n"
                  + "Ants Going for Food on Green Trail: " + antsGoingForFoodOnTrail.ToString() + "\n"
                  + "Total Food Trails: " + foodTrails.ToString();
    }
}
