using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : EnemyController

{
    public float waveRadius;
    public float waveForce;
    public GameObject waveIndicator;
    private float waveDelay = 2f;
    private float waveInterval = 3f;
    //Use new if you intend to have a variable with the same name as one in the parent class
    //public new float speed=1f;

    // The same applies with methods, put new before the return type if you're creating a method with the same name as one in the parent class
    new void  Start()
    {
        base.Start();
        InvokeRepeating("EmitShockwave", waveDelay, waveInterval);
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
    }

    private void EmitShockwave()
    {
        Debug.Log("SHOCKWAVE!!");
        StopAllCoroutines();
        StartCoroutine(DisplayIndicator());
        //waveIndicator.GetComponent<MeshRenderer>().enabled=true;
        Vector3 waveOrigin = transform.position;
        //This is how you do the equivalent of the UE sphere trace in C#
        Collider[] colliders = Physics.OverlapSphere(waveOrigin, waveRadius);
        foreach (Collider hit in colliders)
        {
            //OverlapSphere will detect the Player calling it, make sure you implement a way to ignore it when applying the effects
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if ((rb != null) && (!rb.gameObject.CompareTag("Enemy")))
            {
          
                rb.AddExplosionForce(waveForce, waveOrigin, 0f, 5f, ForceMode.Impulse);
            }

        }
        //Disabling the renderer here WON'T create a flickering effect, the renderer renders according to the last instructions in the frame
        //so it will be disabled again by the end of the frame, despite enabling it before this effect is best done with a coroutine
        //waveIndicator.GetComponent<MeshRenderer>().enabled=false;
    }

    private IEnumerator DisplayIndicator()
    {
        //either activate/deactivate instruction works, but it's good practice to use the MeshRenderer, that way it'll be easier to add
        //addditional logic to the GameObject beyond the simple graphic effect
        waveIndicator.GetComponent<MeshRenderer>().enabled = true;
        //waveIndicator.SetActive(true);
        Debug.Log("Of displayings, kurwa!!");
        yield return new WaitForSeconds(1f);
        waveIndicator.GetComponent<MeshRenderer>().enabled = false;
        //waveIndicator.SetActive(false);


    }
}
