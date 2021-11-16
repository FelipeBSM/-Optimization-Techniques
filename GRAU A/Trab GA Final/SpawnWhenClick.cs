using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class SpawnWhenClick : MonoBehaviour
{
    [Header("Spawn Configs")]
    public GameObject pointObject;
    public GameObject map;

    public bool doneCreating;

    private List<GameObject> pointsList =  new List<GameObject>();
    private Vector3 mousePos, objectPos;
    private Utilities utils = new Utilities();
    
    private int count;

    void Awake()
    {
        utils.SetView(17.5f);
        
        doneCreating = false;
        count = 0;
    }

    private void Update()
    {
        if(doneCreating == false)
        {
            if (Input.GetMouseButtonDown(0))
            {
                count++;
                mousePos = Input.mousePosition;
              
                objectPos = Camera.main.ScreenToWorldPoint(mousePos);
                objectPos.z = 0f;

                GameObject point = Instantiate(pointObject, objectPos, Quaternion.identity, 
                    map.transform);
                point.name = point.name + count;

                pointsList.Add(point);
            }
        }
    }

    public List<GameObject> GetPointList()
    {
        return this.pointsList;
    }
}
