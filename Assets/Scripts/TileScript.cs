using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Helpers;

public class TileScript : MonoBehaviour
{
    public TileAppearance appearance;
    //[EnumFlag]
    [BitMask(typeof(TileProperty))]
    public TileProperty properties;

    public int ProximityToMine
    {
        get { return proximityToMine; }
        set
        {
            proximityToMine = value;
            UpdateHeatmap();
        }
    }


    private GameObject heatmap;
    private GameObject model;
    private Timer heatmapUpdateTimer;
    private int proximityToMine = 0;

    void Start()
    {
        //Invoke("UpdateHeatmap", 
        heatmapUpdateTimer = new Timer(GameObject.FindGameObjectWithTag("Game Manager").GetComponent<TilePrefabHelpScript>().heatmapUpdateDelay, () => UpdateHeatmap());
        GetComponent<Renderer>().enabled = false;
        ChangeModel();
    }

    void Update()
    {
        heatmapUpdateTimer.Update(Time.deltaTime);
    }

    public bool QueryPassability()
    {
        return (properties & TileProperty.Traversable) == TileProperty.Traversable;
        /*switch (properties)
        {
            case TileProperty.Traversable:
                return true;
            default:
                return false;
        }*/
    }

    public bool Detonate()
    {
        if ((properties & TileProperty.Landmine) == TileProperty.Landmine)
        {
            appearance = TileAppearance.Crater;
            properties = properties ^ TileProperty.Landmine;
            properties = properties | TileProperty.Crater;
            ChangeModel();
            Camera.main.GetComponent<ShakeScript>().Shake();
            GameObject.Instantiate(GameObject.FindGameObjectWithTag("Game Manager").GetComponent<TilePrefabHelpScript>().explosionPrefab, transform.position, Quaternion.LookRotation(Vector3.up));
            heatmapUpdateTimer.Restart();
            return true;
        }
        return false;
    }

    public bool Dig()
    {
            if ((properties & TileProperty.Hole) == TileProperty.Hole ||
                (properties & TileProperty.Crater) == TileProperty.Crater)
            {
                return false;
            }
            appearance = TileAppearance.Hole;
            properties = properties | TileProperty.Hole;
            ChangeModel();
            heatmapUpdateTimer.Restart();
            return true;
    }

    private void ChangeModel()
    {
        if (model != null)
            GameObject.Destroy(model);
        model = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<TilePrefabHelpScript>().ChangeState(appearance);
        if (model != null)
        {
            model = GameObject.Instantiate(model) as GameObject;
            model.transform.parent = transform;
            model.transform.localPosition = new Vector3(0, 0, 0);
        }
    }

    private void UpdateHeatmap()
    {
        if (heatmap != null)
            GameObject.Destroy(heatmap);
        if ((properties & TileProperty.Hole) == TileProperty.Hole ||
            (properties & TileProperty.Crater) == TileProperty.Crater)
        {
            GameObject heat = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<TilePrefabHelpScript>().GetHeatmap(proximityToMine) as GameObject;
            if (heat != null)
            {
                heatmap = GameObject.Instantiate(heat) as GameObject;
                heatmap.transform.parent = transform;
                heatmap.transform.localPosition = new Vector3(0, heatmap.transform.position.y, 0);
            }
        }
    }

}


/*[System.Flags]
public enum TileProperty
{
    Traversable = 0x1,
    Landmine = 0x2,
    Hole = 0x4,
    Crater = 0x8,
    Start = 0x10,
    Finish = 0x20,
    Golden = 0x40
}*/

public enum TileAppearance
{
    Blank,
    Normal,
    Obstacle,
    Hole,
    Crater
}
