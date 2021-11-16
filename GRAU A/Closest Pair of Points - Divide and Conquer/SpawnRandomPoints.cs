using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRandomPoints : MonoBehaviour
{
    [SerializeField]private GameObject pointPreFab;
    [SerializeField]private GameObject map;

    [SerializeField]private int pointsToSpawn;


    private List<GameObject> pointsList = new List<GameObject>();

    private Vector3 sizeMap;
    private Vector3 centerMap;

    private Utilities utils = new Utilities();

    public Vector3[] positions;
    

    public bool randomPoints = true;
    

    // Start is called before the first frame update
    void Start()
    {
        utils.SetView(5f);
        sizeMap = map.transform.localScale;
        centerMap = new Vector3(0, 0, 0);
        if (randomPoints == true)
            SpawnPoint();
        else
            SpawnPreDefined();
    }
    
    public void DeletePrev()
    {
        for(int i=0;i< pointsList.Count; i++)
        {
            Destroy(pointsList[i].gameObject);
        }
        pointsList.Clear();

    }
    public void SpawnPreDefined()
    {
        float minX = -sizeMap.x/2;
        float maxX = sizeMap.x/2;
        float minY = -sizeMap.y/2;
        float maxY = sizeMap.y/2;
        for(int i = 0; i < positions.Length; i++)
        {
            float dx = (positions[i].x - minX) / (maxX- minX);
            float dy = (positions[i].y - minY) / (maxY - minY);

            //float xc = dx * 1.8f;
            //float yc = (1 - dy) * 1.5f;
            float xc = dx * (sizeMap.x / 2) * 0.5f;
            float yc = (1 - dy) * (sizeMap.y / 2) * 0.45f;
            Vector3 posToSpawn = centerMap + new Vector3(xc, yc, 0);

            GameObject point = Instantiate(pointPreFab, posToSpawn, Quaternion.identity, map.transform);
            point.name = "P" + i.ToString();
            pointsList.Add(point);
        }
        
       

    
    }
    public void SpawnPoint()
    {
        for(int i=0;i< pointsToSpawn; i++)
        {
            Vector3 posToSpawn = centerMap + new Vector3(Random.Range(-sizeMap.x / 2, sizeMap.x / 2),
                Random.Range(-sizeMap.y / 2, sizeMap.y / 2), 0);

            GameObject point = Instantiate(pointPreFab,posToSpawn, Quaternion.identity,map.transform);
            point.name = "P" + i.ToString();

            pointsList.Add(point);
        }
       
    }
    public List<GameObject> GetPointsList()
    {
        return this.pointsList;
    }

    public int GetTotalPoints()
    {
        return pointsList.Count; 
    }
}
