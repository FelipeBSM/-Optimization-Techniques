using System.Collections;
using System.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetector : MonoBehaviour
{

    public LayerMask portalLayers = 2;
    [Header("Raycasting")]
    public string portalTags;
    public string ignoreTag;
    [Range(2,180)]
    public float raysPerLeaf =10f;
    [Range(5, 180)]
    public float totalVisionAngle = 120f;
    [Range(1, 5)]
    public float numberOfLeafs = 2;
    [Range(0.02f, 0.15f)]
    public float distanceBetweenLeafs = 0.1f;
    public float visionDistance =10f;

    public List<GameObject> portalsList = new List<GameObject>();
    public List<GameObject> currentDetectedPortals = new List<GameObject>();

    LayerMask notPortalLayer;

    private Collider col;
    void Start()
    {
        notPortalLayer = ~portalLayers;
    }

    void Update()
    {
        CreateRays();
    }

    private void CreateRays()
    {
        float leafLimit = numberOfLeafs * 0.5f;
        foreach (GameObject o in currentDetectedPortals)
        {
            o.GetComponent<MeshRenderer>().enabled = false;

        }
        currentDetectedPortals.Clear();
        for (int x =0;x<= raysPerLeaf; x++) //horizontal rays
        {
            for(float y = -leafLimit + 0.5f; y <= leafLimit; y++) //vertical rays
            {
                //distance between each ray per leaf
                float angleToRay = x * (totalVisionAngle / raysPerLeaf) + ((180.0f - totalVisionAngle) * 0.5f); //rotation
                Vector3 directionMultiplayer = (-transform.right) + (transform.up * y * distanceBetweenLeafs); // orientation per leaf
                Vector3 rayDirection = Quaternion.AngleAxis(angleToRay, transform.up) * directionMultiplayer; // get each ray direction per leaf

               
                RaycastHit[] hitRay;
               
                hitRay = Physics.RaycastAll(transform.position, rayDirection, visionDistance); // raycast

                Array.Sort(hitRay, (x, y) => x.distance.CompareTo(y.distance)); //order by hit distance
                for (int i = 0; i < hitRay.Length; i++)
                {
                    
                    if (hitRay[i].collider.CompareTag(portalTags))
                    {
                        Debug.DrawRay(this.transform.position, rayDirection * visionDistance, Color.green);
                        currentDetectedPortals.Add(hitRay[i].collider.gameObject); //add detected OBJ
                        hitRay[i].collider.GetComponent<MeshRenderer>().enabled = true;
                    }
                    else
                    {
                        Debug.DrawRay(this.transform.position, rayDirection * visionDistance, Color.red);
                        break; // break this for loop -> stop detecting
                        //hitRay.collider.GetComponent<MeshRenderer>().enabled = false;
                    }
                }
                   
                       
                
               





            }
        }
    }
}
