using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpawnManager : MonoBehaviour
{
    private float spawnRange = 9f;
    public GameObject[] enemyPrefabs;
    public GameObject[] powerUpPrefabs;
    public GameObject bossPrefab;
    private int enemyCount;
    private int waveNumber=1;
    public TextMeshProUGUI warningText;
    public float textFlickerInterval;
    

    // Start is called before the first frame update
    void Start()
    {
        SpawnEnemyWave(waveNumber);
        SpawnPowerUp(powerUpPrefabs[Random.Range(0,powerUpPrefabs.Length)]);
    }

    // Update is called once per frame
    void Update()
    {
        //Use this function to find objects by scripts attached to them as example.
        enemyCount = FindObjectsOfType<EnemyController>().Length;
        if (enemyCount==0)
        {
            SpawnPowerUp(powerUpPrefabs[Random.Range(0,powerUpPrefabs.Length)]);
            waveNumber++;
            SpawnEnemyWave(waveNumber);
            if((waveNumber==5)||(waveNumber==10))
                {
                SpawnBoss();
                }
        }
    
    }

    private Vector3 GenerateSpawnPosition()
    {
        float positionX = Random.Range(-spawnRange, spawnRange);
        float positionZ = Random.Range(-spawnRange, spawnRange);

        Vector3 spawnPosition = new Vector3(positionX, 0, positionZ);
        return spawnPosition;
    }

    private void SpawnEnemyWave(int enemiesToSpawn)
    {
        for(int i=0; i<enemiesToSpawn; i++)
        {
            if((i!=0)&&(i%3==0))
            {
                Instantiate(enemyPrefabs[1], GenerateSpawnPosition(), enemyPrefabs[1].transform.rotation);
            }
            else
            {
                Instantiate(enemyPrefabs[0], GenerateSpawnPosition(), enemyPrefabs[0].transform.rotation);
            }
        }
    }

    private void SpawnPowerUp(GameObject objectToSpawn)
    {
        Instantiate(objectToSpawn, GenerateSpawnPosition(), objectToSpawn.transform.rotation);
    }
    private void SpawnBoss()
    {
        StartCoroutine(FlickerText(textFlickerInterval));
        Debug.Log("DA BOSS IS COMING");
        Instantiate(bossPrefab, GenerateSpawnPosition(), bossPrefab.transform.rotation);
    }

    private IEnumerator FlickerText(float interval)
    {
        //Don't disable the Text(TMP) object in the editor to hide a TextMeshPro, disable the TextMeshPro component!
        warningText.enabled=true;
        yield return new WaitForSeconds(interval);
        warningText.enabled=false;
        yield return new WaitForSeconds(interval);
        warningText.enabled=true;
        yield return new WaitForSeconds(interval);
        warningText.enabled=false;
        yield return new WaitForSeconds(interval);
        warningText.enabled=true;
        yield return new WaitForSeconds(interval);
        warningText.enabled=false;
        
    }
}
