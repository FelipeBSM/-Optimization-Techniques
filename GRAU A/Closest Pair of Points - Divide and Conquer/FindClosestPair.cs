using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public struct ClosestPoint // strcut that contains the definition of closest point 
{
    public GameObject point1; // final point
    public GameObject point2; // final point2
    public float minDistance; // final distance
}


public class FindClosestPair : MonoBehaviour
{

    [SerializeField]private SpawnRandomPoints spManager;

    [SerializeField] private LineRenderer lr; // line render reference

    private List<GameObject> pointList = new List<GameObject>(); // points list

    private int numberOfPoints;
    public bool useBruteForce = false;

    ClosestPoint finalData;
    void Awake()
    {
        lr.positionCount = 2;
        if (spManager != null)
        {
            pointList = spManager.GetPointsList();
            numberOfPoints = spManager.GetTotalPoints();
            pointList = OrderBy_X(pointList);
         
            if (useBruteForce == true)
                finalData = BruteForce(pointList,numberOfPoints);
            else
                finalData = ClosestPoint(pointList, numberOfPoints);

            DrawLine(finalData.point1, finalData.point2);

            Debug.Log(finalData.point1.name +" " +  finalData.point2.name);
            Debug.Log("Menor distância: " + finalData.minDistance);
            //this.gameObject.SetActive(false);
         
           
        }
           

       
        
    }


    private ClosestPoint ClosestPoint(List<GameObject> _points, int numberPoints)
    {
        _points = OrderBy_X(_points);
        if (_points.Count < 3)
            return BruteForce(_points, numberPoints);

        int middlePoint = numberPoints / 2;
        List<GameObject> p1 = _points.GetRange(0, middlePoint);
        List<GameObject> p2 = _points.GetRange(middlePoint, numberPoints - middlePoint);

        foreach (GameObject obj in p1)
            Debug.Log("ESQUERDA: " + obj.name);
        foreach (GameObject obj in p2)
            Debug.Log("DIREITA: " + obj.name);

        Debug.Log("Tamanho Listas: " + p1.Count + "/" + p2.Count);

        ClosestPoint dl = ClosestPoint(p1, p1.Count);
        ClosestPoint dr = ClosestPoint(p2, p2.Count);

        ClosestPoint df = min(dl, dr);
        

        Debug.Log("Distâncias: dl: " + dl.minDistance + " /dr: " + dr.minDistance + " /df: " +df.minDistance);

        GameObject midObj = _points[_points.Count / 2].gameObject;

        List<GameObject> strip = new List<GameObject>();

        foreach (GameObject obj in _points)
        {
          
            if (obj.transform.position.x - midObj.transform.position.x < df.minDistance)
            {
                strip.Add(obj);
                
            }
            
        }
        //teste
        foreach (GameObject obj in strip)
        {
            Debug.Log("PONTO DENTRO DE STRIP:" + obj.name);

        }

        ClosestPoint dStrip = ClosestPointStrip(strip, df);
        Debug.Log("Distância da faixa:" + dStrip.minDistance.ToString());
        if (dStrip.minDistance < df.minDistance)
        {
            
            Debug.Log("DTRIP: " + dStrip + " foi escolhido!");
            return dStrip;
        }
        else
        {
            
            Debug.Log("D: " + df.minDistance + " foi escolhido!");
            return df;
        }

    }
    private ClosestPoint ClosestPointStrip(List<GameObject> _strip, ClosestPoint _dif)
    {
        ClosestPoint closestStrip = new ClosestPoint();
        closestStrip = _dif;
        _strip = OrderBy_Y(_strip);
        
        for (int i = 0; i < _strip.Count; i++)
        {
           
            for (int j = i + 1; j < _strip.Count && (_strip[j].transform.position.y - _strip[i].transform.position.y) < closestStrip.minDistance; j++)
            {

                float distance = DistanceP(_strip[i], _strip[j]);

                if (distance < closestStrip.minDistance)
                {
                    Debug.Log(_strip[i].name + " / " +  _strip[j].name);
                    closestStrip.minDistance = distance;

                    closestStrip.point1 = _strip[i];
                    closestStrip.point2 = _strip[j];
                }

            }
        }
        
        return closestStrip;
    }
    ClosestPoint BruteForce(List<GameObject> points, int numberPoints)
    {
        ClosestPoint closestPointBrute =  new ClosestPoint();
        closestPointBrute.minDistance = float.MaxValue;
        for (int i = 0; i < numberPoints; i++)
        {
            for (int j = i + 1; j < numberPoints; j++)
            {
                float d = DistanceP(points[i], points[j]);
                if (d < closestPointBrute.minDistance)
                {
                    closestPointBrute.minDistance = d;
                    //drawLine
                    Debug.Log("PONTOS: " + points[i].name + " & " + points[j].name);

                  
                  closestPointBrute.point1 = points[i];
                  closestPointBrute.point2 = points[j];



                }

            }
        }
       
        return closestPointBrute;
    }
    private List<GameObject> OrderBy_X(List<GameObject> _pointList)
    {
        return _pointList.OrderBy(_pointList => _pointList.transform.position.x).ToList();
    }
    private List<GameObject> OrderBy_Y(List<GameObject> _pointList)
    {
        return _pointList.OrderBy(_pointList => _pointList.transform.position.y).ToList();
    }
    private float DistanceP(GameObject go, GameObject go1)
    {
        Vector3 p1 = go.transform.position;
        Vector3 p2 = go1.transform.position;
        return Mathf.Sqrt(((p1.x - p2.x) * (p1.x - p2.x)) + ((p1.y - p2.y) * (p1.y - p2.y)));
    }
    private ClosestPoint min(ClosestPoint left, ClosestPoint right)
    {
        if (left.minDistance < right.minDistance)
            return left;
        else
            return right;
    }
   
    
    private void DrawLine(GameObject obj1, GameObject obj2)
    {
        lr.SetPosition(0, obj1.transform.position);
        lr.SetPosition(1, obj2.transform.position);
    }


}
