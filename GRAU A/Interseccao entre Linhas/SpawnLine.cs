using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Lines
{
    //linePoints
    public GameObject point1;
    public GameObject point2;

    //intersection
    public bool hasIntersection;

    //intersection points
    public float x;
    public float y;
    public string message;
}
public class SpawnLine : MonoBehaviour
{
    [SerializeField] private GameObject pointPreFab;
    [SerializeField] private GameObject map;
    [SerializeField] private Material lineMaterial;

    [SerializeField]private List<Lines> _lines =  new List<Lines>();
    [SerializeField]private int ammountToSpawn = 2; // 5 lines - 10 points

    private Utilities utils = new Utilities();

    bool usePreDefined = false;

    public Vector3[] positions;
    private Vector3 sizeMap;
    private Vector3 centerMap;

  
    void Awake()
    {
        utils.SetView(5f);
        sizeMap = map.transform.localScale;
        centerMap = new Vector3(0, 0, 0);
    
        SpawnLines();
    }

    
    private void SpawnLines()
    {
        for (int i = 0; i < ammountToSpawn; i++)
        {
            Lines points = new Lines();
            Vector3 posToSpawn = centerMap + new Vector3(Random.Range(-sizeMap.x / 2, sizeMap.x / 2),
                Random.Range(-sizeMap.y / 2, sizeMap.y / 2), 0);

            points.point1 = Instantiate(pointPreFab, posToSpawn, Quaternion.identity, map.transform);
            points.point1.name = "P" +"0"+i.ToString();
            points.point1.AddComponent<LineRenderer>().startWidth=0.1f;
            points.point1.GetComponent<LineRenderer>().SetColors(Color.green,Color.green);
            points.point1.GetComponent<LineRenderer>().sortingOrder = 500;
            points.point1.GetComponent<LineRenderer>().material = lineMaterial;


            Vector3 posToSpawn2 = centerMap + new Vector3(Random.Range(-sizeMap.x / 2, sizeMap.x / 2),
              Random.Range(-sizeMap.y / 2, sizeMap.y / 2), 0);

            points.point2 = Instantiate(pointPreFab, posToSpawn2, Quaternion.identity, map.transform);
            points.point2.name = "P" + i.ToString();

            points.point1.GetComponent<LineRenderer>().SetPosition(0, points.point1.transform.position);
            points.point1.GetComponent<LineRenderer>().SetPosition(1, points.point2.transform.position);
            _lines.Add(points);

        }
       
    }
    public List<Lines> GetList()
    {
        return _lines;
    }
   
}
