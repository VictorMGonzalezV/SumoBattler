using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    private  bool hasPowerup = false;
    private bool canSlam = false;
    private bool isSlamming = false;
    public float slamRadius;
    public float slamPower;
    public float powerUpStrength = 1f;
    public float powerUpDuration = 15f;
    public GameObject powerUpIndicator;
    private Rigidbody playerRB;
    private GameObject focalPoint;
    public GameObject[] emitters;
    public GameObject particlePrefab;
    //Create an IEnumerator variable (the return type of any coroutine) so you can cache it later on
    private IEnumerator emittingRadiation=null;
    public float jumpForce;
    private float startingYPos;

  

    // Start is called before the first frame update
    void Start()
    {
        playerRB = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("FocalPoint");
        startingYPos = transform.position.y;
        Debug.Log("Starting position is: " + startingYPos);
        
    }

    // Update is called once per frame
    void Update()
    {
        //startingYPos = transform.position.y;
        //Debug.Log("Starting position is: " + startingYPos);
        float forwardInput = Input.GetAxis("Vertical");
        playerRB.AddForce(focalPoint.transform.forward * forwardInput * speed);
        powerUpIndicator.transform.position = transform.position + new Vector3(0, -0.5f, 0);

        if (Input.GetKeyDown(KeyCode.Space)&&(canSlam)&&(!isSlamming))
        {
           
            Jump();
            //isSlamming = true;
          
        }
        if((transform.position.y<=0.0920f)&&(isSlamming))
        {
            Debug.Log("Gonna slam!");
            GroundSlam();
            //isSlamming = false;
        }
       
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Powerup")&&(!hasPowerup))
        {
            //Switching the state of booleans for effects over time should be done here in the main thread, 
            //not inside the coroutine that controls the duration off the effect
            hasPowerup = true;
            powerUpIndicator.gameObject.SetActive(true);
            ActivatePowerUp(other.gameObject.GetComponent<PowerUp>().GetPowerUpType());
            Destroy(other.gameObject);
            StartCoroutine(PowerUpCoroutine(powerUpDuration));
        }
    }

    //OnCollisionEnter should be used instead of colliders+triggers when physics are involved
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            Rigidbody enemyRB = collision.gameObject.GetComponent<Rigidbody>();
            //Substract the player's position from that of the colliding object to get the vector pointing away from the player.
            Vector3 awayFromPlayer = collision.gameObject.transform.position - transform.position;
            enemyRB.AddForce(awayFromPlayer * powerUpStrength, ForceMode.Impulse);
        }
    }

    private IEnumerator PowerUpCoroutine(float duration)
    {
        //It's possible to write instructions after the yield return new WaitForSeconds statement, the coroutine will execute that code
        //after the time is over and then return control to the Update thread.
        yield return new WaitForSeconds(duration);
        hasPowerup = false;
        powerUpIndicator.gameObject.SetActive(false);
        //Use this to force execution of the default case and reset all powerup features
        ActivatePowerUp(-1);
       
    }
   
    private IEnumerator EmitRadiation(float emissionRate)
    {
       while (true)
        {
          for (int i = 0; i < emitters.Length; i++)
          {
            Instantiate(particlePrefab, emitters[i].transform.position, emitters[i].transform.rotation);
          }
          //This is how you write a coroutine that keeps doing something every x time, an infinite loop with the yield INSIDE of it
          //putting the yield inside the for loop makes it wait after each individual iteration, so it's perfect for sequential attacks
          yield return new WaitForSeconds(emissionRate);
        }
    }
    

    

    private void ActivatePowerUp(int type )
    {
        switch (type)
        {
            case 0:
                {
                    powerUpStrength = 15f;
                    Debug.Log("Force Boost");
                }
                break;

            case 1:
                {
                    //This is what caching a coroutine is, instead of calling it directly,you store the call in the variable you created before
                    //then you call StartCoroutine on that variable
                    emittingRadiation = EmitRadiation(1.5f);
                    StartCoroutine(emittingRadiation);
                    Debug.Log("Radioactive Particles");
                }
                break;

            case 2:
                {
                    Debug.Log("Press space for a ground slam");
                    canSlam = true;
                }
                break;

            default:
                //This is why you cache coroutines, trying to call StopCoroutine(name(params)) will try to stop a NEW instance so it won't work
                //it's smarter to simply cache a coroutine that loops and needs to be stopped manually.
                //StopCoroutine(emittingRadiation);
                StopAllCoroutines();
                powerUpStrength = 1f;
                hasPowerup = false;
                canSlam = false;
                isSlamming = false;
                Debug.Log("Cannot into power");
                break;
        }
    }

    private void Jump()
    {
        {
            playerRB.AddForce(Vector3.up * jumpForce,ForceMode.Impulse);
            isSlamming = true;   
        }
     
    }

    private void GroundSlam()
    {
        Debug.Log("SLAM!");
        Vector3 explosionPos = transform.position;
        //This is how you do the equivalent of the UE sphere trace in C#
        Collider[] colliders = Physics.OverlapSphere(explosionPos, slamRadius);
        foreach (Collider hit in colliders)
        {
            //OverlapSphere will detect the Player calling it, make sure you implement a way to ignore it when applying the effects
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if((rb!=null)&&(!rb.gameObject.CompareTag("Player")))
            {
                Debug.Log("is of hit");
                rb.AddExplosionForce(slamPower, explosionPos, 0f,5f,ForceMode.Impulse);
            }
            
        }


        isSlamming = false;
        canSlam = true;
    }
    

   
    


}
