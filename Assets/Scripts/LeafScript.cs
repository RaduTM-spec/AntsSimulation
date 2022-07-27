using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafScript : MonoBehaviour
{
    [SerializeField] int health = 100;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        VerifyCollisionWithAnt();

    }
    void VerifyCollisionWithAnt()
    {

    }

    public void Damage()
    {
        health--;
        if (health <= 0)
            Destroy(this.gameObject);

        //Shrink the leaf
        this.transform.localScale -= new Vector3(0.01f, 0.01f, 0.01f);
    }

}
