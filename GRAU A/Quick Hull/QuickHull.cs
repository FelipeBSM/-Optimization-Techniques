using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class QuickHull : MonoBehaviour
{
    public SpawnRandomPoints spawn;

    [Header("Convex Render Settings")]
    public Material lineMaterial;
    public Color mainColor;
    [Range(0, 1)] public float lineTickness = 0.5f;


    private List<GameObject> pointsList;
    private List<GameObject> outputHullList = new List<GameObject>();
    static GameObject point0;

    private Utilities utils = new Utilities();
    void Awake()
    {
        
        pointsList = spawn.GetPointsList();
      
        pointsList = utils.OrderBy_X(pointsList);
       
        if (pointsList.Count >= 3)
        {
            GetQuickHull();
            DrawConvexHull();
        }
        else
        {
            Debug.LogWarning("Convex Hull cant have two points... Please insert tree or more points");
        }
       
    }

    
    private void GetQuickHull()
    {
        GameObject lessXPoint = pointsList[0]; // smallest x point
        GameObject biggestXPoint = pointsList[pointsList.Count-1]; // biggest x point

        Debug.Log(lessXPoint.name);
        Debug.Log(biggestXPoint.name);

        MakeQuickHull(pointsList, pointsList.Count, lessXPoint, biggestXPoint, 1);
        MakeQuickHull(pointsList, pointsList.Count, lessXPoint, biggestXPoint, -1);
    }
    private void MakeQuickHull(List<GameObject> points, int num_points, GameObject p1
        ,GameObject p2, int sideOfpoints)
    {
        int ind = -1;
        int max_Dist = 0;

        for(int i = 0; i < num_points; i++)
        {
            int temp = DistanceLine(p1, p2, points[i]);
            if(FindSide(p1,p2,points[i]) == sideOfpoints && temp > max_Dist)
            {
                ind = i;
                max_Dist = temp;
            }
        }

        if(ind == -1)
        {
            outputHullList.Add(p1);
            outputHullList.Add(p2);
            return;
        }
        MakeQuickHull(points, num_points, points[ind], p1, -FindSide(points[ind], p1, p2));
        MakeQuickHull(points, num_points, points[ind], p2, -FindSide(points[ind], p2, p1));

    }
    private void DrawConvexHull()
    {
    
        if (outputHullList != null)
        {
            OriantationQuick comp = new OriantationQuick();
            outputHullList = utils.OrderBy_Y(outputHullList);
            point0 = outputHullList[0];

            outputHullList.Sort(1, outputHullList.Count - 1, comp);
            for (int i = 0; i < outputHullList.Count; i++)
            {
                GameObject o = outputHullList[i];
                 
                o.AddComponent<LineRenderer>();

                LineRenderer ln = o.GetComponent<LineRenderer>();
                ln.material = lineMaterial;
                ln.SetColors(mainColor, mainColor);
                ln.startWidth = lineTickness;
                ln.sortingOrder = 500;

                if (i != outputHullList.Count-1)
                {
                    ln.SetPosition(0, outputHullList[i].transform.position);
                    ln.SetPosition(1, outputHullList[i + 1].transform.position);
                }
                else
                {
                    ln.SetPosition(0, outputHullList[i].transform.position);
                    ln.SetPosition(1, outputHullList[0].transform.position);
                }
            }
        }
        else
        {
            Debug.LogError("Hull points are null... Check Graham Scan method!");
        }
    }
    private int DistanceLine(GameObject _p1, GameObject _p2, GameObject _p)
    {
        Vector3 p1 = ConvertGameObjectToVec3(_p1);
        Vector3 p2 = ConvertGameObjectToVec3(_p2);
        Vector3 p = ConvertGameObjectToVec3(_p);

        return (int)Mathf.Abs((p.y - p1.y) * (p2.x - p1.x) - (p2.y - p1.y) * (p.x - p1.x));
    }
    private int FindSide(GameObject _p1, GameObject _p2, GameObject _p)
    {
        Vector3 p1 = ConvertGameObjectToVec3(_p1);
        Vector3 p2 = ConvertGameObjectToVec3(_p2);
        Vector3 p = ConvertGameObjectToVec3(_p);

        int val = (int)((p.y - p1.y) * (p2.x - p1.x) - (p2.y - p1.y) * (p.x - p1.x));
        if (val > 0)
            return 1;
        if (val < 0)
            return -1;
        return 0;
    }
    private Vector3 ConvertGameObjectToVec3(GameObject _obj)
    {
        return new Vector3(_obj.transform.position.x, _obj.transform.position.y, 0);
    }
    public static int GameObjectOrientation(Vector2 p, Vector2 q, Vector2 r)
    {
        float val = (q.y - p.y) * (r.x - q.x) -
                    (q.x - p.x) * (r.y - q.y);

        val = (int)val;

        if (val == 0) return 0;  // collinear
        return (val > 0) ? 1 : 2; // clock or counterclock wise
    }
    public static float Distance(Vector2 go, Vector2 go1)
    {
        Vector3 p1 = go;
        Vector3 p2 = go1;
        return (p1.x - p2.x) * (p1.x - p2.x) + (p1.y - p2.y) * (p1.y - p2.y);
    }
    public static Vector2 GetPoint0()
    {
        return point0.transform.position;
    }

}
