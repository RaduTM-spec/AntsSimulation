using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AntTrails : MonoBehaviour
{
    public AntMovement antMovement;
    public List<GameObject> homeTrails = new List<GameObject> ();
    public List<GameObject> foodTrails = new List<GameObject> ();

    public GameObject homeTrail;
    public GameObject foodTrail;

    public bool needHomeTrail = true;
    public bool needFoodTrail = false;

    [Tooltip("Higher the value, more frequent the spawn")]
    [Min(0f)]

    [Header("Trail Spawning")]
    [SerializeField] float trailSpawnRate = 3f;
    [SerializeField] bool optimizedSpawn = false;
    float nextTrailSpawn = 0f;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(optimizedSpawn)
            trailSpawnRate = 1.0f / Time.deltaTime /100;
        SpawnTrails();
       
    }
    void SpawnTrails()
    {

        if (nextTrailSpawn <= 0f)
        {
            //SpawnHomeTrails
             if (needHomeTrail && !antMovement.isGoingHome)
             {
                GameObject newTrail = Instantiate(homeTrail, transform.position, Quaternion.identity);
                newTrail.GetComponent<TrailScript>().ChangeParentTo(this.name);
                homeTrails.Add(newTrail);
                    
             }
             if (needFoodTrail && antMovement.isGoingHome)
             {
                GameObject newTrail = Instantiate(foodTrail, transform.position, Quaternion.identity);
                newTrail.GetComponent<TrailScript>().ChangeParentTo(this.name);
                foodTrails.Add(newTrail);
                  
             }


           nextTrailSpawn = 1 / trailSpawnRate;
        }
        else
            nextTrailSpawn -= Time.deltaTime;
        
    }

    public void DestroyHomeTrails()
    {
        foreach(GameObject hTrail in homeTrails)
        {
            Destroy(hTrail);
           
        }
        homeTrails.Clear();
        antMovement.hasHomeTrail = false;
    }
    public void DestroyFoodTrails()
    {
        if(!antMovement.hasBorrowedTrails)
        for (int i = 1; i < foodTrails.Count; i++)
        {
            Destroy(foodTrails[i]);
        }
        foodTrails.Clear();
        antMovement.hasFoodTrail = false;
    }
    public void DestroyTrails()
    {
        DestroyHomeTrails();
        DestroyFoodTrails();
    }

}
