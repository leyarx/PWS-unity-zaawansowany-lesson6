using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class OctTree
{
    public Bounds boundary;
    int capicity;
    bool isDivided = false;

    List<GameObject> gameObjects;
    public List<OctTree> octTrees;


    public OctTree(Bounds boundary, int capicity = 8)
    {
        this.boundary = boundary;
        this.capicity = capicity;

        gameObjects = new List<GameObject>();
    }

    public bool Insert(GameObject gameObject)
    {
        if (!boundary.Contains(gameObject.transform.position))
            return false;

        if (gameObjects.Count < capicity)
        {
            gameObjects.Add(gameObject);
            return true;
        }

        if (!isDivided)
        {
            Subdivide();
        }

        // insert point to each subdivided       
        foreach (OctTree tree in octTrees)
        {
            if (tree.Insert(gameObject))
            {
                break;
            }
        }

        return true;
    }

    public void Rebuild(List<GameObject> gameObjects)
    {
        if(this.octTrees != null)
            this.octTrees.Clear();
        if (this.gameObjects != null)
            this.gameObjects.Clear();

        this.isDivided = false;

        foreach (GameObject gameObject in gameObjects)
        {
            Insert(gameObject);
        }
    }

    public void Subdivide()
    {
        octTrees = new List<OctTree>();

        for(int y = -1; y < 2; y+=2)
        {
            for(int x = -1; x < 2; x+=2)
            {
                for(int z = -1; z < 2; z+=2)
                {
                    Vector3 center = boundary.center + new Vector3(boundary.extents.x / 2 * x, boundary.extents.y / 2 * y, boundary.extents.z / 2 * z);

                    Bounds bounds = new Bounds(center, boundary.extents);
                    octTrees.Add(new OctTree(bounds, capicity));
                }
            }
        }

        isDivided = true;
    }

    public List<GameObject> Query(Bounds boundary, List<GameObject> found)
    {
        if(found == null)
        {
            found = new List<GameObject>();
        }

        if(!this.boundary.Intersects(boundary))
        {
            return found;
        }

        if (gameObjects.Count > 0)
        {
            foreach (GameObject gameObject in gameObjects)
            {
                if (boundary.Contains(gameObject.transform.position))
                {
                    found.Add(gameObject);
                }          
            }
        }

        if(this.isDivided)
        {
            foreach (OctTree tree in octTrees)
            {
                tree.Query(boundary, found);
            }
        }

        return found;
    }

    public void DrawBox()
    {
        Color color = Color.yellow;
        float time = 0f;

        Debug.DrawLine(
            boundary.center + new Vector3(boundary.extents.x * -1, boundary.extents.y * -1, boundary.extents.z * -1),
            boundary.center + new Vector3(boundary.extents.x * 1, boundary.extents.y * -1, boundary.extents.z * -1),
            color, time);
        Debug.DrawLine(
            boundary.center + new Vector3(boundary.extents.x * -1, boundary.extents.y * -1, boundary.extents.z * -1),
            boundary.center + new Vector3(boundary.extents.x * -1, boundary.extents.y * 1, boundary.extents.z * -1),
            color, time);
        Debug.DrawLine(
            boundary.center + new Vector3(boundary.extents.x * -1, boundary.extents.y * -1, boundary.extents.z * -1),
            boundary.center + new Vector3(boundary.extents.x * -1, boundary.extents.y * -1, boundary.extents.z * 1),
            color, time);

        Debug.DrawLine(
            boundary.center + new Vector3(boundary.extents.x * 1, boundary.extents.y * -1, boundary.extents.z * 1),
            boundary.center + new Vector3(boundary.extents.x * 1, boundary.extents.y * -1, boundary.extents.z * -1),
            color, time);
        Debug.DrawLine(
            boundary.center + new Vector3(boundary.extents.x * 1, boundary.extents.y * -1, boundary.extents.z * 1),
            boundary.center + new Vector3(boundary.extents.x * -1, boundary.extents.y * -1, boundary.extents.z * 1),
            color, time);
        Debug.DrawLine(
            boundary.center + new Vector3(boundary.extents.x * 1, boundary.extents.y * -1, boundary.extents.z * 1),
            boundary.center + new Vector3(boundary.extents.x * 1, boundary.extents.y * 1, boundary.extents.z * 1),
            color, time);

        Debug.DrawLine(
            boundary.center + new Vector3(boundary.extents.x * -1, boundary.extents.y * 1, boundary.extents.z * 1),
            boundary.center + new Vector3(boundary.extents.x * -1, boundary.extents.y * -1, boundary.extents.z * 1),
            color, time);
        Debug.DrawLine(
            boundary.center + new Vector3(boundary.extents.x * -1, boundary.extents.y * 1, boundary.extents.z * 1),
            boundary.center + new Vector3(boundary.extents.x * -1, boundary.extents.y * 1, boundary.extents.z * -1),
            color, time);
        Debug.DrawLine(
            boundary.center + new Vector3(boundary.extents.x * -1, boundary.extents.y * 1, boundary.extents.z * 1),
            boundary.center + new Vector3(boundary.extents.x * 1, boundary.extents.y * 1, boundary.extents.z * 1),
            color, time);

        Debug.DrawLine(
            boundary.center + new Vector3(boundary.extents.x * 1, boundary.extents.y * 1, boundary.extents.z * -1),
            boundary.center + new Vector3(boundary.extents.x * 1, boundary.extents.y * 1, boundary.extents.z * 1),
            color, time);
        Debug.DrawLine(
            boundary.center + new Vector3(boundary.extents.x * 1, boundary.extents.y * 1, boundary.extents.z * -1),
            boundary.center + new Vector3(boundary.extents.x * -1, boundary.extents.y * 1, boundary.extents.z * -1),
            color, time);
        Debug.DrawLine(
            boundary.center + new Vector3(boundary.extents.x * 1, boundary.extents.y * 1, boundary.extents.z * -1),
            boundary.center + new Vector3(boundary.extents.x * 1, boundary.extents.y * -1, boundary.extents.z * -1),
            color, time);

        if (this.isDivided)
        {
            foreach (OctTree tree in octTrees)
            {
                tree.DrawBox();
            }
        }
    }
}