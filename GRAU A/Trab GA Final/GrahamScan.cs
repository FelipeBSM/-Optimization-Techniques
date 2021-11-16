

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using csDelaunay; // voronoi library made for unity


public class GrahamScan : MonoBehaviour
{

    private List<GameObject> pointsList;
    private List<GameObject> pointsListOutput =  new List<GameObject>();
    private Vector3 sizeMap;
    private Vector3 centerMap = new Vector3(0, 0, 0);

    private Utilities utils = new Utilities();

    static GameObject p0;

    public SpawnWhenClick spawn;
    public GameObject voronoiMap;

    [Header("Convex Render Settings")]
    public Material lineMaterial;
    public Color mainColor;
    [Range(0, 1)] public float lineTickness = 0.5f;

    [Header("Voronoi Settings")]

    public int imageSize;
    public GameObject lineOBJ;
    //voronoi
    private List<Edge> voronoiEdges;
    private Voronoi voronoi;
    private Dictionary<Vector2f, Site> sites;
    private List<Vector3> updatedPoints = new List<Vector3>();


  
    void Awake()
    {
        sizeMap = voronoiMap.transform.localScale;
        pointsList = spawn.GetPointList();
        if(pointsList.Count >= 3)
        {
            ConvexHull(utils.OrderBy_Y(pointsList)); // pass the list already organized by y position
            DrawHull(pointsListOutput);


            UpdatePointToSprite(pointsList);
            GenerateVoronoi(pointsList);
        }
        else
        {
            Debug.LogWarning("Convex Hull cant have two points... Please insert tree or more points");
        }
        
    }


    private void ConvexHull(List<GameObject> points)
    {
       
        p0 = points[0]; // get the less y point
        Comparer comp = new Comparer();
        points.Sort(1, points.Count - 1,comp);//sort it by angle

        int m = 1;
        for(int i = 1; i < points.Count; i++)
        {
            while(i<points.Count-1 && GameObjectOrientation(p0.transform.position,
                points[i].transform.position,points[i+1].transform.position) == 0)
            {
                i++; // if oriantation is collinear, skip the point
            }
            points[m] = points[i]; // grab the non collinear points
            m++; // increase the size of modified list
        }

        if (m < 3)
            return;

        //init the stack, with the firts three points
        Stack<GameObject> hullPoints = new Stack<GameObject>();
        hullPoints.Push(points[0]);
        hullPoints.Push(points[1]);
        hullPoints.Push(points[2]);

        for(int i = 3; i < m; i++)
        {
            while(hullPoints.Count>1 && GameObjectOrientation(NextPosTop(hullPoints).transform.position,
                hullPoints.Peek().transform.position, points[i].transform.position) != 2)
            {
                hullPoints.Pop(); // remove if oriantation is not couterclock wise
            }
            hullPoints.Push(points[i]); // add point to the stack
        }
        pointsListOutput = hullPoints.ToList(); // convert to list
       

    }
  
    public static float Distance(Vector2 go, Vector2 go1)
    {
        //distance between 2 points
        Vector3 p1 = go;
        Vector3 p2 = go1;
        return (p1.x - p2.x) * (p1.x - p2.x) + (p1.y - p2.y) * (p1.y - p2.y);
    }
    public static Vector2 GetPoint0()
    {
        return p0.transform.position;
    }
    public static int GameObjectOrientation(Vector2 p, Vector2 q, Vector2 r)
    {
        float val = (q.y - p.y) * (r.x - q.x) -
                    (q.x - p.x) * (r.y - q.y);

        val = (int)val;

        if (val == 0) return 0;  // collinear
        return (val > 0) ? 1 : 2; // clock or counterclock wise
    }
    public static GameObject NextPosTop(Stack<GameObject> stack)
    {
        GameObject p = stack.Peek(); // peek the first element in the stack
        stack.Pop(); // remove it
        GameObject res = stack.Peek(); // store the new firts in the stack
        stack.Push(p);// place it back
        return res; // return the 'new' object in the top position
    }

    private void DrawHull(List<GameObject> points)
    {
        if (points != null)
        {
            for (int i = 0; i < points.Count; i++)
            {
                GameObject o = points[i];
                o.AddComponent<LineRenderer>();
                LineRenderer ln = o.GetComponent<LineRenderer>();
                ln.material = lineMaterial;
                ln.SetColors(mainColor,mainColor);
                ln.startWidth = lineTickness;
                if (i!= points.Count-1)
                {
                    ln.SetPosition(0, points[i].transform.position + new Vector3(0,0,-5f));
                    ln.SetPosition(1, points[i + 1].transform.position + new Vector3(0, 0, -5f));
                }
                else
                {
                    ln.SetPosition(0, points[i].transform.position + new Vector3(0, 0, -5f));
                    ln.SetPosition(1, points[0].transform.position + new Vector3(0, 0, -5f));
                }
            }
        }
        else
        {
            Debug.LogError("Hull points are null... Check Graham Scan method!");
        }
    }
    private void GenerateVoronoi(List<GameObject> points)
    {
        Rectf bounds = new Rectf(0,0,imageSize,imageSize);
        List<Vector2f> pIN = new List<Vector2f>();
       
        for (int i=0;i<points.Count;i++)
        {
            Vector2f pointToAdd;
            pointToAdd.x =updatedPoints[i].x;
            pointToAdd.y = updatedPoints[i].y;
            pIN.Add(pointToAdd);
        }
        Voronoi voronoi = new Voronoi(pIN, bounds);

        this.voronoi = voronoi;
        this.voronoiEdges = voronoi.Edges;
        for (int i = 0; i < voronoiEdges.Count; i++)
        {
            //Debug.LogError(voronoiEdges[i].a +" / "+ voronoiEdges[i].b + " / " + voronoiEdges[i].c);
        }
        sites = voronoi.SitesIndexedByLocation;
        DrawLine();
    }
    private void DrawLine()
    {
        Texture2D tx = new Texture2D(imageSize, imageSize);
      
        foreach (KeyValuePair<Vector2f, Site> kv in sites)
        {
            tx.SetPixel((int)kv.Key.x, (int)kv.Key.y, Color.red);
            //Debug.Log(kv.Key.x + " / " + kv.Key.y);
            Vector3 pos = new Vector3(kv.Key.x, kv.Key.y, 0);
            //coco = Instantiate(lineOBJ, pos, Quaternion.identity);
        }
        foreach (Edge edge in voronoiEdges)
        {
            if (edge.ClippedEnds == null) continue;
            Debug.Log(edge.LeftSite.Coord + " LEFT ");
            Debug.Log(edge.RightSite.Coord + " RIGHT ");
           
            //Vector3 posL = new Vector3(edge.LeftSite.x,edge.LeftSite.y, 0);
            //Vector3 posR = new Vector3(edge.RightSite.x,edge.RightSite.y, 0);

            DrawVornoiLineVector(edge.ClippedEnds[LR.LEFT], edge.ClippedEnds[LR.RIGHT], tx, Color.black);
        }
        tx.Apply();

        voronoiMap.GetComponent<MeshRenderer>().material.mainTexture = tx;
        //this.renderer.material.mainTexture = tx;
    }
   
    void DrawVornoiLineVector(Vector2f p0, Vector2f p1, Texture2D tx, Color c, int offset = 0)
    {
        int x0 = (int)p0.x;
        int y0 = (int)p0.y;
        int x1 = (int)p1.x;
        int y1 = (int)p1.y;

        int dx = Mathf.Abs(x1 - x0);
        int dy = Mathf.Abs(y1 - y0);
        int sx = x0 < x1 ? 1 : -1;
        int sy = y0 < y1 ? 1 : -1;
        int err = dx - dy;

        while (true)
        {
            tx.SetPixel(x0 + offset, y0 + offset, c);
            

            if (x0 == x1 && y0 == y1) break;
            int e2 = 2 * err;
            if (e2 > -dy)
            {
                err -= dy;
                x0 += sx;
            }
            if (e2 < dx)
            {
                err += dx;
                y0 += sy;
            }
         
        }
    }

    private void UpdatePointToSprite(List<GameObject>points)
    {
        for (int i = 0; i < points.Count; i++)
        {
            float minX = -sizeMap.x / 2;
            float maxX = sizeMap.x / 2;
            float minY = -sizeMap.y / 2;
            float maxY = sizeMap.y / 2;
            float dx = (points[i].transform.position.x - minX) / (maxX - minX);
            float dy = (points[i].transform.position.y - minY) / (maxY - minY);

            //float xc = dx * 1.8f;
            //float yc = (1 - dy) * 1.5f;
            float xc = dx * ((sizeMap.x)*5);
            float yc = (1 - dy) * ((sizeMap.y)*5);
            Vector3 posToSpawn = centerMap + new Vector3(xc, yc, 0);
            updatedPoints.Add(posToSpawn);
        }
    }
}

