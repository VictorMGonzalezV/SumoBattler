using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyParticle : MonoBehaviour
{
    private float lifetime=2f;
    public float speed;
    public float impactForce;
    private Rigidbody particleRB;
    // Start is called before the first frame update
    void Start()
    {
        particleRB = GetComponent<Rigidbody>();
        Destroy(gameObject,lifetime);
    }

    // Update is called once per frame
    void Update()
    {
        particleRB.AddForce(Vector3.forward * speed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            Rigidbody enemyRB = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromParticle = collision.gameObject.transform.position - transform.position;
            enemyRB.AddForce(awayFromParticle * impactForce, ForceMode.Impulse);
            Destroy(gameObject);
        }
        
       
    }
}
