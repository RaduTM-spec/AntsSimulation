using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Vector2 movDirection;
    [SerializeField] private float movementSpeed = .5f;
    float maxZoomIn = 7f, maxZoomOut = 30f;
    float unitsMovePerScrollMove = 1f;
    float smoothnessZoomSpeed = 1f;

    Camera camComponent;
    // Start is called before the first frame update
    void Start()
    {
        camComponent = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        GetDirection();
        MoveCamera();
        Zoom();
    }
    void GetDirection()
    {
        float xDir = Input.GetAxis("Horizontal");
        float yDir = Input.GetAxis("Vertical");
        movDirection = new Vector2(xDir, yDir).normalized;
    }
    void MoveCamera()
    {
        Vector2 movePosition = (Vector2)transform.position + movDirection;
        transform.position = Vector2.Lerp(transform.position, movePosition, movementSpeed * Time.deltaTime);
        transform.position += new Vector3(0f, 0f, -10f);
        //this.transform.Translate(movDirection * movementSpeed * Time.deltaTime);
    }
    void Zoom()
    {

        if (Input.mouseScrollDelta.y < 0f)//if scroll up, this means zoom in
        {
            if(camComponent.orthographicSize<maxZoomOut)
                 camComponent.orthographicSize+=unitsMovePerScrollMove;
        }
        else if (Input.mouseScrollDelta.y > 0f)
            if(camComponent.orthographicSize > maxZoomIn)
                this.GetComponent<Camera>().orthographicSize-=unitsMovePerScrollMove;
    }
}

