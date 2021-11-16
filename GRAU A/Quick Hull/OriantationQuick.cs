using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OriantationQuick : IComparer<GameObject>
{
    
    public int Compare(GameObject p1, GameObject p2)
    {
        int angleOrtientation = (int)QuickHull.GameObjectOrientation(QuickHull.GetPoint0(),
            p1.transform.position, p2.transform.position);

        if (angleOrtientation == 0)
        {
            return (QuickHull.Distance(QuickHull.GetPoint0(),
                p2.transform.position) >= QuickHull.Distance(QuickHull.GetPoint0(),
                p1.transform.position)) ? -1 : 1;
        }
        return (angleOrtientation == 2) ? -1 : 1;
    }
}
