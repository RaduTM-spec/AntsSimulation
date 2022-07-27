using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailScript : MonoBehaviour
{
    [SerializeField] string parentAntName;

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeParentTo(string parent)
    {
        parentAntName = parent;
    }
    public string GetParentName()
    {
         return parentAntName; 
    }
    
}
