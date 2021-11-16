using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineIntersection : MonoBehaviour
{
    [SerializeField] private SpawnLine sp;
    [SerializeField] private GameObject pointPreFab;
    [SerializeField] private GameObject map;

    int numberOfLines;
    private List<Lines> lines =  new List<Lines>();
    private List<Lines> totalIntersections = new List<Lines>();

    void Awake()
    {
        Camera.main.orthographicSize = 5f;
        lines = sp.GetList();
        numberOfLines = sp.GetList().Count;
        totalIntersections = GetIntersection(lines);
        DrawPoints(totalIntersections);
    }

    private void DrawPoints(List<Lines> _lines)
    {
       for(int i=0; i < _lines.Count; i++)
       {
            Vector3 posToSpawn = new Vector3(_lines[i].x, _lines[i].y, 0);
            GameObject point = Instantiate(pointPreFab, posToSpawn, Quaternion.identity, map.transform);
            point.name = "P_I" + i.ToString();
       }
    }

    private List<Lines> GetIntersection(List<Lines> lines)
    {
        List<Lines> allIntersections = new List<Lines>();
        for(int i=0; i< lines.Count; i++)
        {
            for (int j = i+1; j < lines.Count; j++)
            {
                Lines intersection = IntersectionPoint(lines[i], lines[j]);
                Debug.Log(intersection.x.ToString() + " / " + intersection.y.ToString());
                if (intersection.hasIntersection)
                {
                    Debug.Log(intersection.message);
                    allIntersections.Add(intersection);
                }
                else
                {
                    Debug.Log(intersection.message);
                }
                    
            }
        }
        return allIntersections;
    }

    private Lines IntersectionPoint(Lines L1, Lines L2)
    {
        Lines _line = new Lines();
        float det = Det(L1, L2);
        if(det != 0)
        {
            float s = LineL(L1, L2) / det;
            if ((s > 1) && (s < 0))
            {
                _line.hasIntersection = false;
                _line.message = "Não existe intersecção!";
                
            }
            else
            {
                _line.message = "Existe intersecção!";
                _line.hasIntersection = true;
                _line.x = L1.point1.transform.position.x +
                (L1.point2.transform.position.x - L1.point1.transform.position.x) * s;
                _line.y = L1.point1.transform.position.y +
                    (L1.point2.transform.position.y - L1.point1.transform.position.y) * s;
            }
            
        }
        else
        {
            _line.hasIntersection = false;
            _line.message = "Não existe intersecção! ";
        }
        return _line;
    }
    private float LineL(Lines l1, Lines l2)
    {
        return (l2.point2.transform.position.x - l2.point1.transform.position.x) *
            (l2.point1.transform.position.y - l1.point1.transform.position.y) -
            (l2.point2.transform.position.y - l2.point1.transform.position.y) *
            (l2.point1.transform.position.x - l1.point1.transform.position.x);
    }
    private float Det(Lines l1, Lines l2)
    {
        return (l2.point2.transform.position.x - l2.point1.transform.position.x) *
            (l1.point2.transform.position.y - l1.point1.transform.position.y) -
            (l2.point2.transform.position.y - l2.point1.transform.position.y) *
            (l1.point2.transform.position.x - l1.point1.transform.position.x);

    }
   
}
