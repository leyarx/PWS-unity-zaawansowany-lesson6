using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    public OctTree octTree;
    public float radius = 0.5f;
    Bounds boundary;
    Vector3 direction;

    [SerializeField]
    float speed = 1;

    void CheckCollision()
    {
        boundary.center = transform.position;

        foreach (GameObject go in octTree.Query(boundary, null))
        {
            if (go == gameObject)
            {
                continue;
            }

            float dist = Vector3.Distance(go.transform.position, transform.position);

            if (dist <= radius * 2)
            {
                GetComponent<Renderer>().material.color = Color.red;
                
                Vector3 between = go.transform.position - transform.position;
                Vector3 normal = between.normalized;
                direction = Vector3.Reflect(direction, normal);

                // move object outside of another object
                transform.position += (between.magnitude - radius * 2) * normal;
                
            }
            else
            {
                GetComponent<Renderer>().material.color = Color.white;
            }
        }
    }

    void Start()
    {
        boundary = new Bounds(transform.position, Vector3.one * radius * 4);
        direction = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }

    void Update()
    {
        // reflect bubble from world boundary
        if(!octTree.boundary.Contains(transform.position))
        {
            Vector3 normal = (octTree.boundary.ClosestPoint(transform.position) - transform.position).normalized;
            direction = Vector3.Reflect(direction, normal);

            // move object inside of bounding box
            transform.position = octTree.boundary.ClosestPoint(transform.position);
        }

        Debug.DrawRay(transform.position, direction, Color.green);

        CheckCollision();

        transform.position = transform.position + direction * speed * Time.deltaTime;
    }
}
