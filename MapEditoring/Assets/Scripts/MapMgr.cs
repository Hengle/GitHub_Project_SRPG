using UnityEngine;
using System.Collections;

public class MapMgr
{
    // map 3차원배열
    public HexInfo[][][] Map;
    private GameObject Go_Hex;
    private GameObject MapRoot;
    public float HexW;
    public float HexH;

    private static MapMgr inst = null;
    public static MapMgr GetInst()
    {
        if(inst == null)
        {
            inst = new MapMgr();
            inst.Go_Hex = (GameObject)Resources.Load("Prefabs/Hex");
            inst.HexW = inst.Go_Hex.transform.renderer.bounds.size.x;
            inst.HexH = inst.Go_Hex.transform.renderer.bounds.size.z;
        }
        return inst;
    }

    public Vector3 GetWorldPos(int x, int y, int z)
    {
        float X, Z;

        X = x * HexW + (z * HexW * 0.5f);
        Z = (-z) * HexH * 0.75f;
        
        return new Vector3(X, 0.5f, Z);
    }

    public void CreateMap(int sizeX, int sizeY, int sizeZ)
    {
        Debug.Log("Create Map(x,y,z) : " + sizeX + ", " + sizeY + ", " + sizeZ);
        
        MapRoot = new GameObject("Map");

        if(sizeX > 0 && sizeY > 0 && sizeZ > 0)
        {
            Map = new HexInfo[sizeX * 2 + 1][][];
            for(int x = -sizeX; x <= sizeX; x++)
            {
                Map[x + sizeX] = new HexInfo[sizeY * 2 + 1][];
                for(int y = -sizeY; y <= sizeY; y++)
                {
                    Map[x + sizeX][y + sizeY] = new HexInfo[sizeZ * 2 + 1];
                    for(int z = -sizeZ; z <= sizeZ; z++)
                    {
                        if(x + y + z == 0)
                        {
                            GameObject hex = (GameObject)GameObject.Instantiate(Go_Hex);
                            hex.transform.parent = MapRoot.transform;
                            HexInfo hexInfo = hex.GetComponent<HexInfo>();
                            hexInfo.Passable = true;
                            hexInfo.transform.position = GetWorldPos(x, y, z);
                            hexInfo.SetMapPos(x, y, z);
                            Map[x + sizeX][y + sizeY][z + sizeZ] = hexInfo;
                        }
                    }
                }
            }
        }
        else
        {
            return;
        }
    }

    public void RemoveMap()
    {
        GameObject.Destroy(MapRoot);
    }
}
