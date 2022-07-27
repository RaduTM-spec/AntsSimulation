using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodTrailsWOParent : MonoBehaviour { 
    public List<List<GameObject>> foodTrailsListsThatRemainedUndeleted = new List<List<GameObject>>();
    public int number = 0;

    void Update()
    {
       if(number!=0)
        foreach (List<GameObject> list in foodTrailsListsThatRemainedUndeleted)
            if (list[1] == null)
            {
                foreach (GameObject trail in list)
                {
                    try
                    {
                        
                    }
                    catch { };
                }
                
                number--;
            }

    }
}
