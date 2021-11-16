using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject FindClosestOBJ;
    public SpawnRandomPoints SpawnOBJ;
    void Start()
    {
        FindClosestOBJ.SetActive(false);
    }

    public void EnableScript()
    {
        FindClosestOBJ.SetActive(true);
       
    }
    public void PredefinedButton()
    {
        FindClosestOBJ.SetActive(false);
        SpawnOBJ.DeletePrev();
        SpawnOBJ.SpawnPreDefined();
    }
    public void RandomButton()
    {
        
        SpawnOBJ.DeletePrev();
        SpawnOBJ.SpawnPoint();
    }

}
