using UnityEngine;
using System.Collections;

public class Butter : MonoBehaviour
{

    public float maxStretch = 3.0f; //how far we can stretch the thing
    private LineRenderer catapultLineFront; //references to the two lines we are effecting
    private LineRenderer catapultLineBack; //these will be dragged in from the editor

    private Rigidbody2D rb2d;
    private SpringJoint2D spring;
    private Rigidbody2D catapultRB;
    private GameObject catapult;
    private Ray rayToMouse;
    private Ray leftCatapultToProjectile;
    private float maxStretchSqr;
    private float circleRadius;
    private bool clickedOn;
    private Vector2 prevVelocity;
    private Vector3 startingPosition;
    private GameObject prefabToInstantiate;



    //[SerializeField]
    //private Butter player;
    //private CameraController cameraController;
    Camera cameraMain;

    Animator anim;

    void Awake()
    {

    }

    // Use this for initialization
    void Start()
    {
        spring = GetComponent<SpringJoint2D>(); //get the component for spring
        catapult = GameObject.Find("Catapult");
        catapultLineFront = catapult.transform.GetChild(0).GetComponent<LineRenderer>();
        catapultLineBack = catapult.GetComponent<LineRenderer>();
        spring.enabled = true;
        catapultLineBack.enabled = true;
        catapultLineFront.enabled = true;
		LineRendererSetup(); //sets up the line renderer, we does this in a separte function so it doesn't clutter
        catapultRB = catapult.GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        startingPosition = transform.position;
        prefabToInstantiate = GameObject.FindGameObjectWithTag("Player");
        //print("Awake is finished");
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.isKinematic = true;
        cameraMain = Camera.main;
        spring.connectedBody = catapultRB;
        rayToMouse = new Ray(catapult.transform.position, Vector3.zero); //this ray will start at our catapult position
                                                                         //but it has no direction (vector3.zero), we will set this in our Dragging() function
                                                                         //just like above, we make a new ray set to begin at the left arm and have no direction yet
        leftCatapultToProjectile = new Ray(catapultLineFront.transform.position, Vector3.zero);
        maxStretchSqr = maxStretch * maxStretch;
        CircleCollider2D circle = GetComponent<Collider2D>() as CircleCollider2D; //circle collider2d is our collider2d as a circle collider
        circleRadius = circle.radius + .2f;
        GameManager.instance.setOnSlingshot();
    }

    // Update is called once per frame
    void Update()
    {
        if (clickedOn)
        { //if we are clicked on the object
            Dragging(); //we are dragging it
        }

        //for our flinging physics, the spring joint gives the force for us depending how car we pull back
        //but when we fling and we get past the point where the spring will start to slow it down
        //then we need to destroy the spring joint so that we continue to move forward without springing back
        if (!GameManager.instance.PlayerReleased)
        { //if we still have a spring, we haven't launched it yet
          //if the rigidbody is not kinematic (if we have let go of the rock)
          //AND our previous velocity is greater than this current frame's velocity. (we passed the halfway point on the spring)
            if (!rb2d.isKinematic && prevVelocity.sqrMagnitude > rb2d.velocity.sqrMagnitude)
            {
                //Destroy (spring); //destroy the spring
                spring.enabled = false;
                rb2d.velocity = prevVelocity; //assign last frame's velocity to be the current velocity so it doesn't lose speed
                                              //we disable our lines so that the band isn't there anymore
                catapultLineFront.enabled = false;
                catapultLineBack.enabled = false;
				GameManager.instance.weeee();
            }

            if (!clickedOn)
            { //if we have released the button but the rock hasn't crossed the point yet
                prevVelocity = rb2d.velocity; //we set the prevVelocity to be the current velocity for the future
            }

            //we need to update the line renderer so the band will move
            LineRendererUpdate(); //called every frame while the spring is still there

        }
		
    }

    void LineRendererSetup()
    {
        //first we want to set the starting position of the line renderer, the 0th position
        catapultLineFront.SetPosition(0, catapultLineFront.transform.position);
        catapultLineBack.SetPosition(0, catapultLineBack.transform.position);

        catapultLineFront.sortingLayerName = "Foreground";
        catapultLineBack.sortingLayerName = "Foreground";

        catapultLineFront.sortingOrder = 3;
        catapultLineBack.sortingOrder = 1;
    }

    void OnMouseDown()
    {
        spring.enabled = false; //disable our spring joint so we can move it
        clickedOn = true; //we are dragging
    }

    void OnMouseUp()
    {
        spring.enabled = true; //enable the spring joint to fling the rock
        rb2d.isKinematic = false; //the rb becomes a dynamic rb to use physics
        clickedOn = false; //we are no longer dragging
        anim.SetTrigger("released");
		
    }

    void Dragging()
    { //is called each frame we are dragging the rock
        Vector3 mouseWorldPoint = cameraMain.ScreenToWorldPoint(Input.mousePosition); //gets the position of the mouse
                                                                                      //length between the catapult to mouse is the mouse position - catapult position
        Vector2 catapultToMouse = mouseWorldPoint - catapult.transform.position;

        //We don't want to be able to drag back further than our max stretch
        //also we want to keep the rock in line with where our mouse is when we drag farther back
        //we can use a ray to keep track of this when we drag more than our max stretch
        //when we compare values and magnitudes its quicker and more efficient to compare the square magnitude rather than the magnitude itself
        //if the distance to the catapult from the mouse is greater than the max stretch (remember both squared)...
        if (catapultToMouse.sqrMagnitude > maxStretchSqr)
        {
            rayToMouse.direction = catapultToMouse; //...we set the rayToMouse direction (vector 2) to keep it the same direction
            mouseWorldPoint = rayToMouse.GetPoint(maxStretch); //then we find a point along that ray that is max stretch
        }

        mouseWorldPoint.z = 0f; //we are working in 2D so z doesn't matter and is 0
        transform.position = mouseWorldPoint; //make the rock drag with the mouse cursor
    }

    //will be called every frame the 
    void LineRendererUpdate()
    {
        //first thing is we want to find a vector for the line renderer
        //this is the distance from the rock to the catapult
        Vector2 catapultToProjectile = transform.position - catapultLineFront.transform.position;
        //set the direction of the leftCatapultToProjectile ray with the vector 2 we made above
        leftCatapultToProjectile.direction = catapultToProjectile;
        //where the band will hold to, its the length of the catapult to projectile + radius so it goes to the edge of the rock
        Vector3 holdPoint = leftCatapultToProjectile.GetPoint(catapultToProjectile.magnitude + circleRadius);

        //applies our calculations to the line renderer
        catapultLineFront.SetPosition(1, holdPoint); //remember we set the 1 position, because our 0 position is the catapult
        catapultLineBack.SetPosition(1, holdPoint);
		print("line render update called.");
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        anim.SetTrigger("splat");
        Invoke("Splat", 2);
    }

    //called when the player dies
    void Splat()
    {
        gameObject.tag = "DeadButter";
		this.gameObject.GetComponent<Rigidbody2D>().isKinematic = false;
		this.gameObject.layer = 10;
        Respawn();
        GameManager.instance.PlayerSplat();
    }

    void Respawn()
    {
        GameObject butter = Instantiate(prefabToInstantiate, startingPosition, transform.rotation);
		butter.name = "Butter";
        butter.tag = "Player";
        print("Butter spawned");
        Destroy(this);
        //gameObject.SetActive(false);
    }
}
