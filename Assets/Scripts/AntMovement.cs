using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntMovement : MonoBehaviour
{
    [Header("Total Ants")]
    public static int antsNumber = 0;

    [SerializeField] bool infiniteLife = true;
    [SerializeField] float antLifeTime = 60f;
    [Tooltip("The ant life time will be random, between [antLifeTime-offset%, antLifeTime+offset%]")]
    [SerializeField] float antLifeTimeOffsetPercentage = 20f;

    [Header("Ant Movement")]
    public float antSpeed = 5f;
    public float rotationSpeed = 5f;

    [Header("Random Moving Direction Change Time Range")]
    public float rndMoveDirChangeTimeRangeMin = 1f;
    public float rndMoveDirChangeTimeRangeMax = 3f;

    [Header("Time Left until next Direction Change")]
    [SerializeField]float rndMoveDirChangeTimeLeft = 0f;
    float randomYdir = 0f, randomXdir = 0f;

    [Header("Max angle when moving direction (calculated between 0f and 1f")]
    [Tooltip("When the movement is randomly changed, it must not be that different from the previous directions" +
             "When 1f, the ant may randomly change the direction anywhere")]
    [Range(0f, 1f)]
    public float rndDirChangeMaxOffset = .5f;

    [Tooltip("Give the ant a direction a bit different from home position")]
    [SerializeField] float homeOffset = 0.25f;

    public GameObject home;
    public AntTrails antTrailsScript;
    public bool hasFoodTrail = false;
    public bool hasHomeTrail = false;
    public bool isGoingHome = false;
    public bool isMovingRandom = true;
    [SerializeField]public bool hasFood = false;

    public bool hasBorrowedTrails = false;
    public string parentOfBorrowedTrail;

    SpriteRenderer mouthFoodSprRend;

    [SerializeField]int foodTrailAntPos = 0;

    public GameObject foodTrWOParent;//unassigned

    [Header("RayCasting")]
    [SerializeField] float rayOffset = .5f;
    [SerializeField] float antViewLength = 1f;
    private void Start()
    {
        mouthFoodSprRend = this.transform.GetChild(0).GetComponent<SpriteRenderer>();
        antLifeTime += Random.Range(-antLifeTime*antLifeTimeOffsetPercentage/100f, antLifeTime*antLifeTimeOffsetPercentage/100f);
    }

    private void Update()
    {
        if(!infiniteLife)
            UpdateLife();


        if (!hasFood)
            isGoingHome = false;
        else
            isGoingHome = true;

        SearchForFood();
        GoHome();
       
    }
    void UpdateLife()
    {
        antLifeTime -= Time.deltaTime;
        if (antLifeTime <= 0f)
        {
            if(antTrailsScript.foodTrails.Count>1)
                for (int i = 1; i < antTrailsScript.foodTrails.Count; i++)
                {
                    //just for no bugs
                    Destroy(antTrailsScript.foodTrails[i]);
                    continue;
                    //just for no bugs


                    //Transfer Trail to FoodTrailsWOParent
                    if (foodTrWOParent == null)
                         Destroy(antTrailsScript.foodTrails[i]);
                }
                     
            foreach (GameObject trail in antTrailsScript.homeTrails)
                Destroy(trail);
            Destroy(this.gameObject); 
        }
    }
    private void OnDestroy()
    {
        antsNumber--;
        Debug.Log(this.name + " died.");
    }
    void SearchForFood()
    {
        RecheckTheFoodTrail();
        if(!isGoingHome && !hasFood && !hasFoodTrail)//in search for food randomly
        {
            antTrailsScript.needHomeTrail = true;
            isMovingRandom = true;
            RandomMovement();
        }
        
        if(!hasFood && !isGoingHome && hasFoodTrail)//go for food in the same place
        {
            isMovingRandom = false;

            if (antTrailsScript.foodTrails.Count>2 && antTrailsScript.foodTrails[2] != null)//the trails didn't disappeared
                GoForFoodOnFoodTrail();
            else
            {
                antTrailsScript.foodTrails.Clear();
                hasBorrowedTrails = false;
                parentOfBorrowedTrail = null;
                hasFoodTrail = false;
                isMovingRandom = true;
            }
            
        }
    }
    void RecheckTheFoodTrail()
    {
        if (antTrailsScript.foodTrails.Count == 0)
            this.hasFoodTrail = false;
    }
    void GoForFoodOnFoodTrail()
    {
        
        if (!hasFoodTrail)
            return;
        isMovingRandom = false;
        UpdateTrailIfStolen();
        if(antTrailsScript.foodTrails.Count > 0)
        {
            Vector2 targetDir = Vector2.zero;
            try
            {
                targetDir = antTrailsScript.foodTrails[foodTrailAntPos].transform.position - transform.position;

            }
            catch
            {
                ;//Because the leaf will be null, you cannot access transform.position, so you try to, if doesn t work => the and will go randomly 
            }
          
            MoveTo(targetDir);
            if (foodTrailAntPos > 0 && foodTrailAntPos < antTrailsScript.foodTrails.Count && antTrailsScript.foodTrails[foodTrailAntPos].GetComponent<CircleCollider2D>().bounds.Contains(transform.position))//if it has to encounter a circle trail
                foodTrailAntPos--;
            else if(foodTrailAntPos==0)//if it has to encounter the leaf
            {
                //Check if the leaf is no more
                
                if (antTrailsScript.foodTrails[0] == null)
                {
                    
                    antTrailsScript.DestroyFoodTrails();
                    hasBorrowedTrails = false;
                    parentOfBorrowedTrail = null;
                    foodTrailAntPos = 0;
                    hasFoodTrail = false;

                    return;
                }
                

            }
        }
    }
    void UpdateTrailIfStolen()
    {
        if (hasBorrowedTrails == false)
            return;
        //Check If The Trail Still Exists
        GameObject theOtherAnt = GameObject.Find(parentOfBorrowedTrail);
        List<GameObject> theOtherAntFoodTrails = theOtherAnt.GetComponent<AntTrails>().foodTrails;
        if (theOtherAntFoodTrails.Count == 0)//if no more 
        {
            antTrailsScript.foodTrails.Clear();
            hasBorrowedTrails = false;
            return;
        }
        if(theOtherAntFoodTrails.Count != antTrailsScript.foodTrails.Count)
        {
            for (int i = antTrailsScript.foodTrails.Count; i < theOtherAntFoodTrails.Count-1; i++)
            {
                antTrailsScript.foodTrails.Add(theOtherAntFoodTrails[i]);
            }
        }
    }
    void GoHome()
    {
        if (!hasFood)
            return;
        isMovingRandom = false;
        isGoingHome = true;
  
        if(hasHomeTrail && antTrailsScript.homeTrails.Count > 0)
        {
           Vector2 targetDir = antTrailsScript.homeTrails[antTrailsScript.homeTrails.Count - 1].transform.position - transform.position;
           MoveTo(targetDir);
           if (antTrailsScript.homeTrails[antTrailsScript.homeTrails.Count - 1].GetComponent<CircleCollider2D>().bounds.Contains(transform.position))
           {
               Destroy(antTrailsScript.homeTrails[antTrailsScript.homeTrails.Count - 1]);
               antTrailsScript.homeTrails.RemoveAt(antTrailsScript.homeTrails.Count - 1);
           }
           
        }
      
    }
  
    void RandomMovement()
    {
        
        //Change Direction
        if (rndMoveDirChangeTimeLeft <= 0f)
        {
            SetRandomMovementValue();
            rndMoveDirChangeTimeLeft = Random.Range(rndMoveDirChangeTimeRangeMin,rndMoveDirChangeTimeRangeMax);
        }
        else rndMoveDirChangeTimeLeft -= Time.deltaTime;

        //Get small offset from home
        //Vector2 homeDirection = ((Vector2)transform.position - (Vector2)home.transform.position).normalized;
       


        //Move
        if(CheckIfNotWallInFront())
          transform.Translate(new Vector3(randomXdir, randomYdir, 0f).normalized * antSpeed * Time.deltaTime, Space.World);

        //Rotate towards direction
        Quaternion toRotation = Quaternion.LookRotation(Vector3.forward, new Vector3(randomXdir, randomYdir, 0f));
        transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);

        


        hasHomeTrail = true;
    }
    bool CheckIfNotWallInFront()
    {
        RaycastHit2D antFrontView = Physics2D.Raycast((Vector2)transform.localPosition + new Vector2(randomXdir, randomYdir).normalized * rayOffset, new Vector2(randomXdir, randomYdir).normalized, antViewLength);
        Debug.DrawRay((Vector2)transform.localPosition + new Vector2(randomXdir,randomYdir).normalized * rayOffset, new Vector2(randomXdir,randomYdir).normalized * antViewLength,Color.red);
        if (antFrontView.collider != null)
            if (antFrontView.collider.tag == "Wall")
            {
                //Change randomXDir and randomYDir a bit more
                //Get offset from wall
                randomXdir = -randomXdir + Random.Range(-.5f,.5f);
                randomYdir = -randomYdir + Random.Range(-.5f, .5f);
                //return false; 

                //MODIFICATION WAS MADE FOR OPTIMIZATION
                /*otherwise the ants will think much longer when hitting a wall*/
            }
        return true;
    }
   
    void SetRandomMovementValue()
    {

        randomYdir = Random.Range(randomYdir-rndDirChangeMaxOffset,randomYdir+rndDirChangeMaxOffset);
        randomXdir = Random.Range(randomXdir-rndDirChangeMaxOffset,randomXdir+rndDirChangeMaxOffset);


        //If randomDir is not in [-1f,1f]
        if (randomXdir > 1f)
            randomXdir = 1f;
        else if (randomXdir < -1f)
            randomXdir = -1f;
        if (randomYdir > 1f)
            randomYdir = 1f;
        else if(randomYdir < -1f)
            randomYdir= -1f;
    }

    void MoveTo(Vector2 targetDir)
    {
        this.transform.Translate(targetDir.normalized * antSpeed * Time.deltaTime, Space.World);
        Quaternion toRotation = Quaternion.LookRotation(Vector3.forward, new Vector3(targetDir.x,targetDir.y, 0f));
        transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Food")
        {
            //Add the food in the first position of the foodList
            if (antTrailsScript.foodTrails.Count == 0)
            { antTrailsScript.foodTrails.Add(collision.gameObject);}

            isGoingHome = true;
            hasFood = true;
            if (!hasFoodTrail)
                antTrailsScript.needFoodTrail = true;
            collision.GetComponent<LeafScript>().Damage();
            if(mouthFoodSprRend.color.a != 255)
                 mouthFoodSprRend.color += new Color32(0, 0, 0, 255);
            foodTrailAntPos = antTrailsScript.foodTrails.Count;
            return;
        }
        if(collision.name == "Home")
        {
            if (hasFood)
            {
                isGoingHome = false;
                

                hasFood = false;
                collision.GetComponent<HomeScript>().IncreaseFood();


                if (antTrailsScript.foodTrails.Count > 2)
                {  
                    hasFoodTrail = true;
                    antTrailsScript.needFoodTrail = false;
                }

                foodTrailAntPos = antTrailsScript.foodTrails.Count - 1;

                if (mouthFoodSprRend.color.a != 0)
                    mouthFoodSprRend.color -= new Color32(0, 0, 0, 255);
     
                return;
            }
        }
        if(collision.name == "FoodTrail(Clone)" && isMovingRandom)
        {
            
            
            //*If the Ant is moving Random and it finds otherAnt FoodTrail, this ant will take that ant food trails
            //*
            isMovingRandom = false;
            antTrailsScript.DestroyFoodTrails();
            
            GameObject theOtherAnt = GameObject.Find(collision.GetComponent<TrailScript>().GetParentName());

            if (theOtherAnt != null)
            {
                Debug.Log(this.name + " stole trail from " + theOtherAnt.name);

                List<GameObject> theOtherAntTrails = theOtherAnt.GetComponent<AntTrails>().foodTrails;
                //Take it's food trails
                foreach (GameObject trail in theOtherAntTrails)
                    antTrailsScript.foodTrails.Add(trail);


                hasBorrowedTrails = true;
                parentOfBorrowedTrail = theOtherAnt.name;
                hasFoodTrail = true;

                //Get it's Position on The Trail
                foodTrailAntPos = theOtherAnt.GetComponent<AntTrails>().foodTrails.IndexOf(collision.gameObject);
            }
            
            return;

        }
    }

    
}
//Mechanics behind movement
/* At start, it creates home trail
 * When found the first food (it doesn t have foodTrail) it creates a foodTrail
 * It move back to home on the home trail
 * If it has green trail, searches for food on the green trail
 * if it has green trail and reaches the end, and it doesn t have food, the green trail is removed ///CASE WHEN FINNISHED
 * 
 * 
 * 
 * 
 */