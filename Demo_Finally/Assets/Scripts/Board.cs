using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Board : MonoBehaviour
{
    public List<GameObject> pooledObjects;
    public GameObject[] objectsToPool;
    public int amountToPool;
    public GameObject tempSuperBall;
    // Start is called before the first frame update
    void Start()
    {
        pooledObjects = new List<GameObject>();
        StartCoroutine(GenerateBalls());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator GenerateBalls()
    {
        for (int i = 0; i < amountToPool; i++)
        {
            yield return new WaitForSeconds(0.005f);
            int matIndex = Random.Range(0, objectsToPool.Length);
            Vector3 pointStart = new Vector3(transform.position.x + Random.Range(-2.0f, 2.0f), transform.position.y, transform.position.z);
            GameObject obj = (GameObject)Instantiate(objectsToPool[matIndex], pointStart, Quaternion.identity);
            obj.SetActive(true);
            pooledObjects.Add(obj);
        }
    }
}
