using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Utilities : MonoBehaviour
{
    public List<GameObject> OrderBy_X(List<GameObject> _pointList)
    {
        return _pointList.OrderBy(_pointList => _pointList.transform.position.x).ToList();
    }
    public List<GameObject> OrderBy_Y(List<GameObject> _pointList)
    {
        return _pointList.OrderBy(_pointList => _pointList.transform.position.y).ToList();
    }

    public void SetView(float _value)
    {
        Camera.main.orthographicSize = _value;
    } 
}
