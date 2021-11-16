using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Comparer : IComparer<GameObject>
{
    // Start is called before the first frame update
    public int Compare(GameObject p1, GameObject p2)
    {
        int angleOrtientation = (int)GrahamScan.GameObjectOrientation(GrahamScan.GetPoint0(), 
            p1.transform.position, p2.transform.position);

        if (angleOrtientation == 0)
        {
            return (GrahamScan.Distance(GrahamScan.GetPoint0(),
                p2.transform.position) >= GrahamScan.Distance(GrahamScan.GetPoint0(), 
                p1.transform.position)) ? -1 : 1;
        }
        return (angleOrtientation == 2) ? -1 : 1;
    }
}
