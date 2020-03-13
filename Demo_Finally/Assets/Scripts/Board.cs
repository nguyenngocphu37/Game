using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Board : MonoBehaviour
{
    public List<GameObject> pooledObjects;
    public List<GameObject> superBalls;
    public GameObject[] objectsToPool;
    public int amountToPool;
    GameObject obj;
    // Start is called before the first frame update
    void Start()
    {
        pooledObjects = new List<GameObject>();
        superBalls = new List<GameObject>();
        StartCoroutine(GenerateBalls(amountToPool));
    }

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator GenerateBalls(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            int matIndex = Random.Range(0, objectsToPool.Length);
            Vector3 pointStart = new Vector3(transform.position.x + Random.Range(-2.0f, 2.0f), transform.position.y, transform.position.z);
            GameObject obj = Instantiate(objectsToPool[matIndex], pointStart, Quaternion.identity) as GameObject;

            obj.SetActive(true);
            pooledObjects.Add(obj);
        }

        // Rơi xuống
        foreach (var item in pooledObjects)
        {
            yield return new WaitForSeconds(0.005f);
            item.SetActive(true);
            item.AddComponent<Rigidbody2D>(); // Add the rigidbody.
        }
    }

    IEnumerator GenerateNewBalls()
    {
        GameObject[] temps = pooledObjects.Where(x => !x.activeInHierarchy)?.ToArray();
        if (temps != null && temps.Length > 0)
        {
            foreach (var item in temps)
            {
                yield return new WaitForSeconds(0.005f);
                int matIndex = Random.Range(0, objectsToPool.Length);
                Vector3 pointStart = new Vector3(transform.position.x + Random.Range(-2.0f, 2.0f), transform.position.y, transform.position.z);
                GameObject obj = Instantiate(objectsToPool[matIndex], pointStart, Quaternion.identity) as GameObject;
                obj.AddComponent<Rigidbody2D>();
                pooledObjects.Remove(item);
                pooledObjects.Add(obj);
                Destroy(item);
            }
        }
        // Debug.Log("Tổng: " + pooledObjects.Count());
    }

    public void ActiveBall()
    {
        StartCoroutine(GenerateNewBalls());
    }
}