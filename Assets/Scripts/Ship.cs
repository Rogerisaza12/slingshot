using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Ship : MonoBehaviour 
{

    public float maxStretch = 7f;
    private float maxStretchSqr;
    public LineRenderer planeta1;
    private Transform Planet;
    private SpringJoint2D joint;
    private Ray rayToMouse;
    private Ray leftPlanet;
    private bool clickedOn;
    private Vector2 speed; 
    private float circleRadiius;


    void Awake () 
	{
		
        joint = GetComponent<SpringJoint2D>();
        Planet = joint.connectedBody.transform;
	}
	
    void Start()
    {
        LineRendererSetup();
        rayToMouse = new Ray(Planet.position, Vector3.zero);
        leftPlanet = new Ray(planeta1.transform.position, Vector3.zero);
        maxStretchSqr = maxStretch * maxStretch;
        CircleCollider2D circle = GetComponent<Collider2D>() as CircleCollider2D;
        circleRadiius = circle.radius;
    }

    void LineRendererSetup()
    {
        planeta1.SetPosition(0, planeta1.transform.position);
        planeta1.sortingLayerName = "Foreground";
        planeta1.sortingOrder = 3;

    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnMouseDown();
        }
        if (Input.GetMouseButtonUp(0))
        {
            OnMouseUp();
        }
        if (clickedOn)
            Dragging();

        if (joint != null)
        {
            if (!GetComponent<Rigidbody2D>().isKinematic && speed.sqrMagnitude > GetComponent<Rigidbody2D>().velocity.sqrMagnitude)
            {
                Destroy(joint);
                GetComponent<Rigidbody2D>().velocity = speed;
            }
            if (!clickedOn)

                speed = GetComponent<Rigidbody2D>().velocity;

                LineRendererUpdate();
        }
        else
        {
            planeta1.enabled = false;
            
        }
    }

    private void LookTowards (Vector2 direction)
	{
		
		transform.localRotation = Quaternion.LookRotation (Vector3.forward, direction);
	}

    void Dragging()
    {
        Vector3 mouseWorldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 catapultToMouse = mouseWorldPoint - Planet.position;
        if (catapultToMouse.sqrMagnitude > maxStretchSqr)
        {
            rayToMouse.direction = catapultToMouse;
            mouseWorldPoint = rayToMouse.GetPoint(maxStretch);
        }
        mouseWorldPoint.z = 0f;
        transform.position = mouseWorldPoint;
    }
    void OnMouseDown()
    {
        joint.enabled = false;
        clickedOn = true;
    }
    void OnMouseUp()
    {
        joint.enabled = true;
        GetComponent<Rigidbody2D>().isKinematic = false;
        clickedOn = false;
    }
    void LineRendererUpdate()
    {
        Vector2 catapultToProjectile = transform.position - planeta1.transform.position;
        leftPlanet.direction = catapultToProjectile;
        Vector3 holdPoint = leftPlanet.GetPoint(catapultToProjectile.magnitude + circleRadiius);
        planeta1.SetPosition(1, holdPoint);
        

    }


}
