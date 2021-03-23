using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WorldCreator : MonoBehaviour
{
    public int halfWidth = 50;
    public int halfDepth = 50;
    public int halfHeight = 50;

    public GameObject sphere;
    public int sphereCount = 10000;

    public OctTree octTree;

    List<GameObject> gameObjects;

    void Start()
    {
        octTree = new OctTree(new Bounds(transform.position, new Vector3(halfWidth * 2, halfHeight * 2, halfDepth * 2)));

        gameObjects = new List<GameObject>();

        CreateSpheres(sphereCount);
        octTree.Rebuild(gameObjects);
    }

    private void Update()
    {        
        octTree.Rebuild(gameObjects);
        octTree.DrawBox();
    }

    void CreateSpheres(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject newSphere = Instantiate(sphere, RandomPosition(), Quaternion.identity, transform);
            newSphere.GetComponent<Bubble>().octTree = octTree;
            gameObjects.Add(newSphere);
        }
    }

    Vector3 RandomPosition()
    {
        return new Vector3(Random.Range(-halfWidth, halfWidth), Random.Range(-halfHeight, halfHeight), Random.Range(-halfDepth, halfDepth));
    }
}
