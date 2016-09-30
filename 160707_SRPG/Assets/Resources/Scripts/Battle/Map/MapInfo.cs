using UnityEngine;
using System.Collections;
// List사용.
using System.Collections.Generic;

// 3-05:22분30초
public class MapInfo
{
    public string BackgroundDir;
    public int MapSizeX;
    public int MapSizeY;
    public int MapSizeZ;

    public List<HexInfo> HexInfos = new List<HexInfo>();
}

public class HexInfo
{
    public int MapPosX;
    public int MapPosY;
    public int MapPosZ;

    public bool Passable;
    //public int TextureIdx;
}