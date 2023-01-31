using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed = 3f;
    private Rigidbody enemyRB;
    private GameObject player;
    // Start is called before the first frame update
    protected void Start()
    {
        enemyRB = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
        
    }

    // Update is called once per frame
    protected void Update()
    {
        //Substracting the enemy's position from the player's will calculate the vector the enemy needs to move along to reach the player
        //Remember to normalize the vector so the applied force does NOT increase with distance
        Vector3 lookDirection = (player.transform.position - transform.position).normalized;
        enemyRB.AddForce(lookDirection*speed);

        if(transform.position.y<-10)
        {
            Destroy(gameObject);
        }
    }
}
