using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HomeScript : MonoBehaviour
{
    public List<GameObject> antsList = new List<GameObject> ();//First Ant is Ant0
    public GameObject ant;
    public AntMovement antScript;
    

    [Min(1)]
    public int startingAntsNumber = 1;

    [Header("Food")]
    [SerializeField] private int foodProgress = 0;
    [SerializeField] private int foodNeccesaryForNewAnt = 10;

    [Header("Text")]
    public TMP_Text antsNumber;
    public TMP_Text foodStored;
    // Start is called before the first frame update
    void Start()
    {
        InstantiateStartingAnts();
        
    }
    void InstantiateStartingAnts()
    {
        for (int i = 0; i < startingAntsNumber; i++)
        {
            GameObject newAnt = Instantiate(ant,transform.position,Quaternion.identity);
            newAnt.name = "Ant" + AntMovement.antsNumber.ToString();
            antsList.Add(newAnt);
            AntMovement.antsNumber++;
        }

    }
    void InstantiateNewAnts()
    {
        if(foodProgress >= foodNeccesaryForNewAnt)
        {
            GameObject newAnt = Instantiate(ant, transform.position, Quaternion.identity);
            newAnt.name = "Ant" + AntMovement.antsNumber.ToString();
            antsList.Add(newAnt);
            AntMovement.antsNumber++;

            foodProgress = 0;
        }
    }
    // Update is called once per frame
    void Update()
    {
        UpdateText();
        InstantiateNewAnts();
    }
    void UpdateText()
    {
        if (antsNumber != null && foodStored != null)
        {
            antsNumber.text = AntMovement.antsNumber.ToString();
            foodStored.text = foodProgress.ToString();
        }
    }
    public void IncreaseFood()
    {
        foodProgress++;
    }

    public void NewAnt()
    {
        foodProgress = foodNeccesaryForNewAnt;
    }
}
