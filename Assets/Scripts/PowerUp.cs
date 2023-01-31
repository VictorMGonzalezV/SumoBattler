using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
   
    public enum Powerups { Boost,Radiation,GroundSlam}
    public Powerups type;
    private int powerUpType;

    // Start is called before the first frame update
    void Start()
    {
        powerUpType = (int)type;
    }

    public int GetPowerUpType()
    {
        return powerUpType;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
