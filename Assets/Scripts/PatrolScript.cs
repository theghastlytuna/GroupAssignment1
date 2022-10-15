using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolScript : MonoBehaviour
{
    public Transform P1T;
    public Transform P2T;
    
    Vector3 patrol1;
    Vector3 patrol2;
    bool targettingP1 = true;
    GameObject trackedPlayer;

    Vector3 target;

    public Rigidbody rb;
    //SphereCollider trigger;

    bool trackingPlayer = false;
    float speed;
    [Range(0.001f, 0.1f)] [SerializeField] float speedDelta = 0.1f;
    [SerializeField] float speedMax = 30.0f;
    [SerializeField] float playerTargettingCooldown = 10.0f;
    float currentCooldown = 0.0f;


    // Start is called before the first frame update
    void Start()
    {
        //patrol1 = transform.Find("Target 1").position;
        //patrol2 = transform.Find("Target 2").position;

        patrol1 = P1T.position;
        patrol2 = P2T.position;

        target = patrol1;

        //rb = GetComponent<Rigidbody>();
        
        //SphereCollider[] colliders = GetComponents<SphereCollider>();
        //if (colliders[0].isTrigger)
        //{
        //    trigger = colliders[0];
        //}
        //else
        //{
        //    trigger = colliders[1];
        //}
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Update Player Position
        if (trackingPlayer)
        {
            target = trackedPlayer.transform.position;
        }

        //Draw a vector to the target.
        //If we've reached out patrol point, then switch to next patrol point and redraw the target
        Vector3 thisToTarget = target - transform.position;
        if (!trackingPlayer && thisToTarget.magnitude < 0.4)
        {
            Debug.Log("Point Reached. Switching to next point");
            if (targettingP1)
            {
                target = patrol2;
                targettingP1 = false;
            }
            else
            {
                target = patrol1;
                targettingP1 = true;
            }
            thisToTarget = target - transform.position;
        }

        thisToTarget.y = 0;
        if (speed < speedMax)
        {
            speed = speed + speedDelta;
        }
        rb.velocity = thisToTarget.normalized * speed;
    }

    private void Update()
    {
        currentCooldown -= Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        //Start Tracking Player
        if(other.tag == "Player" && currentCooldown < 0)
        {
            Debug.Log("White Woman Detected");
            trackingPlayer = true;
            trackedPlayer = other.gameObject;
            currentCooldown = playerTargettingCooldown / 2;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        rb.velocity = Vector3.zero;
        //If you hit the player, stop tracking it
        if(collision.gameObject.tag == "Player")
        {
            Debug.Log("Hit Player");
            trackingPlayer = false;
            trackedPlayer = null;
            currentCooldown = playerTargettingCooldown;
        }
    }
}
