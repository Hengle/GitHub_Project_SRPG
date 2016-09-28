using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Path
{
    public Path Parent;         // 서칭시작 Hex의 위치로 현재의 Path는 Hex의 이웃임
    public HexGrid CurHex;
    public int F;               // H + G
    public int G;               // 시작점부터 현재까지의 거리값
    public int H;               // 현재부터 도착점까지의 거리값

    public Path(Path parent, HexGrid hex, int g, int h)
    {
        Parent = parent;
        CurHex = hex;
        G = g;
        H = h;
        F = H + G;
    }
}

// TODO : 나중에 싱글톤으로 바꾸면서 MonoBehaviour는 상속안받도록 수정할 것. => 2-08에와서 지움
public class MapManager
{
    // TODO : 드래그&드롭으로 HexGrid프리팹 적용, 나중에는 코드에서 적용해야함.
    public GameObject GO_Hex;

    // Hex Size는 Awake에서 설정됨
    public float HexW;
    public float HexH;

    // MapSize는 inspector에서 설정(x:10,y:10,z:10) => 2-08 24분:hierarchy 오브젝트가 삭제되면서 코드로 값설정
    public int MapSizeX = 10;
    public int MapSizeY = 10;
    public int MapSizeZ = 10;

    public Point[] Dirs;

    HexGrid[][][] Map;

    // 싱글턴 패턴(1-03:14분)이라는데 나중에 검색해봐야됨. => 2-08오면서 Awake()지우고 코드 수정
    private static MapManager inst = null;
    public static MapManager GetInst()
    {
        if(inst == null)
        {
            inst = new MapManager();
            inst.Init();
        }
        return inst;
    }

    public void Init()
    {
        GO_Hex = (GameObject)Resources.Load("Prefabs/HexGrid");
        SetHexSize();
        iniDirs();
    }
    
    public void iniDirs()
    {
        Dirs = new Point[6];
        Dirs[0] = new Point(+1, -1, 0);     // Right
        Dirs[1] = new Point(1, 0, -1);      // Up Right
        Dirs[2] = new Point(0, 1, -1);      // Up Left
        Dirs[3] = new Point(-1, 1, 0);      // Left
        Dirs[4] = new Point(-1, 0, 1);      // Down Left
        Dirs[5] = new Point(0, -1, 1);      // Down Right

    }

    void SetHexSize()
    {
        HexW = GO_Hex.transform.renderer.bounds.size.x;
        HexH = GO_Hex.transform.renderer.bounds.size.z;
    }

    // 
    public Vector3 GetWorldPos(int x, int y, int z)
    {
        // ex) x=-3, y=0, z=-3;
        // X = -3 + (-3 * 0.5f);
        // Z = -3 * 0.75f;
        // X = 4.5 Z = 2.25;

        float X, Z;

        X = x * HexW + (z * HexW * 0.5f);
        Z = (-z) * HexH * 0.75f;
        return new Vector3(X, 0, Z);
    }

    // 맵을 만든다
    public void CreateMap()
    {
        // 1-02
        Map = new HexGrid[MapSizeX * 2 + 1][][];
        // 2-08:12분 코드 수정(Hierarchy에서 지저분한것들 하나로 묶기위한 오브젝트)
        GameObject map = new GameObject("맵"); // "맵"=hierarchy에 보여질 이름

        for (int x = -MapSizeX; x <= MapSizeX; x++)
        {
            Map[x + MapSizeX] = new HexGrid[MapSizeY * 2 + 1][];
            for (int y = -MapSizeY; y <= MapSizeY; y++)
            {
                Map[x + MapSizeX][y + MapSizeY] = new HexGrid[MapSizeZ * 2 + 1];
                for (int z = -MapSizeZ; z <= MapSizeZ; z++)
                {
                    if(x + y + z == 0)
                    {
                        // 2-08:12분 hex오브젝트에 map붙임
                        GameObject hex = (GameObject)GameObject.Instantiate(GO_Hex);
                        hex.transform.parent = map.transform;

                        // Map[+MapSize]해주는 이유는, 
                        // 배열의 인덱스는 -(마이너스)가 될 수 없기 때문에 +(플러스)해주는거.
                        // 2-08:12분18초 hex로 코드수정
                        Map[x + MapSizeX][y + MapSizeY][z + MapSizeZ] = hex.GetComponent<HexGrid>();
                        Vector3 pos = GetWorldPos(x, y, z);
                        Map[x + MapSizeX][y + MapSizeY][z + MapSizeZ].transform.position = pos;
                        Map[x + MapSizeX][y + MapSizeY][z + MapSizeZ].SetMapPos(x, y, z);
                    }
                }
            }
        }
    }

    public HexGrid GetPlayerHex(int x, int y, int z)
    {
        return Map[x + MapSizeX][y + MapSizeY][z + MapSizeZ];
    }

    // 현재 Player위치(start)를 기준으로 MoveRange만큼의 Hex가 highlight됨.
    public bool HighlightMoveRange(HexGrid start, int moveRange)
    {
        int highLightedCount = 0;

        for (int x = -MapSizeX; x <= MapSizeX; x++)
        {   
            for (int y = -MapSizeY; y <= MapSizeY; y++)
            {
                for (int z = -MapSizeZ; z <= MapSizeZ; z++)
                {
                    if (x + y + z == 0 && Map[x + MapSizeX][y + MapSizeY][z + MapSizeZ].Passable == true)
                    {
                        int distance = GetDistance(start, Map[x + MapSizeX][y + MapSizeY][z + MapSizeZ]);
                        // 1-07:16분 주석처리.
                        /*Point pos1 = start.MapPos;
                        Point pos2 = Map[x + MapSizeX][y + MapSizeY][z + MapSizeZ].MapPos;
                        int distance = (Mathf.Abs(pos1.X - pos2.X) + Mathf.Abs(pos1.Y - pos2.Y) + Mathf.Abs(pos1.Z - pos2.Z)) / 2;*/
                        if (distance <= moveRange && distance != 0)
                        {
                            if (IsReachAble(start, Map[x + MapSizeX][y + MapSizeY][z + MapSizeZ], moveRange))
                            {
                                Map[x + MapSizeX][y + MapSizeY][z + MapSizeZ].transform.renderer.material.color = Color.green;
                                highLightedCount++;
                            }
                        }
                    }
                }
            }
        }
        if(highLightedCount == 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    // 2-01:공격범위 HighLight
    public bool HighlightAtkRange(HexGrid start, int atkRange)
    {
        PlayerManager pm = PlayerManager.GetInst();
        int highLightedCount = 0;

        for (int x = -MapSizeX; x <= MapSizeX; x++)
        {
            for (int y = -MapSizeY; y <= MapSizeY; y++)
            {
                for (int z = -MapSizeZ; z <= MapSizeZ; z++)
                {
                    if (x + y + z == 0 && Map[x + MapSizeX][y + MapSizeY][z + MapSizeZ].Passable == true)
                    {
                        int distance = GetDistance(start, Map[x + MapSizeX][y + MapSizeY][z + MapSizeZ]);
                        // 1-07:16분 주석처리.
                        /*Point pos1 = start.MapPos;
                        Point pos2 = Map[x + MapSizeX][y + MapSizeY][z + MapSizeZ].MapPos;
                        int distance = (Mathf.Abs(pos1.X - pos2.X) + Mathf.Abs(pos1.Y - pos2.Y) + Mathf.Abs(pos1.Z - pos2.Z)) / 2;*/
                        if (distance <= atkRange && distance != 0)
                        {
                            // 최초엔 '적이 없다'로 초기화
                            bool isExist = false;
                            foreach (PlayerBase pb in pm.Players)
                            {
                                // 아군은 제외하고 적만 검사해야하니깐.
                                if (pb is Monster)
                                {
                                    // userPlayer 주위의 적 검사
                                    if (pb.CurHex.MapPos == Map[x + MapSizeX][y + MapSizeY][z + MapSizeZ].MapPos)
                                    {
                                        // 적이 있다로 바꾸고 for문 탈출
                                        isExist = true;
                                        break;
                                    }
                                }
                            }

                            if (isExist == true)
                            {
                                if (IsReachAble(start, Map[x + MapSizeX][y + MapSizeY][z + MapSizeZ], atkRange))
                                {
                                    Map[x + MapSizeX][y + MapSizeY][z + MapSizeZ].transform.renderer.material.color = Color.red;
                                    highLightedCount++;
                                }
                            }
                        }
                    }
                }
            }
        } // x for문 끝

        if (highLightedCount == 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    // highlight준거 다시 원래색으로
    public void ResetMapColor()
    {
        for (int x = -MapSizeX; x <= MapSizeX; x++)
        {
            for (int y = -MapSizeY; y <= MapSizeY; y++)
            {
                for (int z = -MapSizeZ; z <= MapSizeZ; z++)
                {
                    if (x + y + z == 0)
                    {
                        Map[x + MapSizeX][y + MapSizeY][z + MapSizeZ].transform.renderer.material.color = Map[x + MapSizeX][y + MapSizeY][z + MapSizeZ].OriColor;
                    }
                }
            }
        }
    }

    // 2-03:11분
    // monster턴일때 hex color를 원래색으로 복귀(안해줘도될거같은데..)
    public void ResetMapColor(Point pos)
    {
        Map[pos.X + MapSizeX][pos.Y + MapSizeY][pos.Z + MapSizeZ].transform.renderer.material.color = Map[pos.X + MapSizeX][pos.Y + MapSizeY][pos.Z + MapSizeZ].OriColor;
    }

    // 두 Hex간의 거리구하는 메소드
    public int GetDistance(HexGrid h1, HexGrid h2)
    {
        // pos1 = Player위치
        Point pos1 = h1.MapPos;
        // pos2 = Map 좌표
        Point pos2 = h2.MapPos;
        // 거리 반환
        return (Mathf.Abs(pos1.X - pos2.X) + Mathf.Abs(pos1.Y - pos2.Y) + Mathf.Abs(pos1.Z - pos2.Z)) / 2;
    }

    public bool IsReachAble(HexGrid start, HexGrid dest, int moveRange)
    {
        List<HexGrid> path = GetPath(start, dest);
        if(path.Count == 0 || path.Count > moveRange)
        {
            return false;
        }
        return true;
    }

    // 1-08
    List<Path> OpenList;
    List<Path> ClosedList;
    public List<HexGrid> GetPath(HexGrid start, HexGrid dest)
    {
        OpenList = new List<Path>();
        ClosedList = new List<Path>();

        List<HexGrid> rtnVal = new List<HexGrid>();

        int H = GetDistance(start, dest);
        // start는 시작시 없기때문에 null
        Path p = new Path(null, start, 0, H);

        ClosedList.Add(p);

        Path result = Recursive_FindPath(p, dest);

        // result가 null이면 갈수있는 길이 없는것.
        if (result == null)
        {
            return rtnVal;
        }

        while (result.Parent != null)
        {
            rtnVal.Insert(0, result.CurHex);
            result = result.Parent;
        }

        return rtnVal;
    }

    public Path Recursive_FindPath(Path parent, HexGrid dest)
    {
        // 목적지를 찾은 경우
        // parent의 위치(경로)가 목적지의 위치와 같다면 원하는곳에 도착했다는 뜻
        if (parent.CurHex.MapPos == dest.MapPos)
        {
            return parent;
        }

        List<HexGrid> neighbors = GetNeighbors(parent.CurHex);

        foreach (HexGrid h in neighbors)
        {
            Path newP = new Path(parent, h, parent.G-1, GetDistance(h, dest));
            AddToOpenList(newP);
        }

        Path bestP;

        // 목적지까지 가는길이 없는 경우
        if(OpenList.Count == 0)
        {
            return null;
        }

        bestP = OpenList[0];
        foreach (Path p in OpenList)
        {
            if(p.F < bestP.F)
            {
                bestP = p;
            }
        }

        OpenList.Remove(bestP);
        ClosedList.Add(bestP);

        return Recursive_FindPath(bestP, dest);   
    }

    public void AddToOpenList(Path p)
    {
        foreach(Path inP2 in ClosedList)
        {
            if(p.CurHex.MapPos == inP2.CurHex.MapPos)
            {
                return;
            }
        }

        foreach(Path inP in OpenList)
        {
            if(p.CurHex.MapPos == inP.CurHex.MapPos)
            {
                if(p.F < inP.F)
                {
                    OpenList.Remove(inP);
                    OpenList.Add(p);
                    return;
                }
            }
        }

        OpenList.Add(p);
    }

    // 1-08:22분
    // 1-09:8분설명, Pathfinding해서 찾고자하는 목적지경로를 여섯개의 셀을 검색하여 찾음
    public List<HexGrid> GetNeighbors(HexGrid pos)
    {
        List<HexGrid> rtn = new List<HexGrid>();
        Point cur = pos.MapPos;

        if(pos.Passable == false)
        {
            return rtn;
        }

        foreach(Point p in Dirs)
        {
            Point tmp = p + cur;
            if(tmp.X + tmp.Y + tmp.Z == 0)
            {
                // 2-09:11분22초 기존코드 수정. 맵의 range를 벗어남. 패스파인딩할때 맵의 테두리 밖으로 넘어간것도 확인하려고해서 에러뜸
                if (tmp.X >= -MapSizeX && tmp.X <= MapSizeX && tmp.Y >= -MapSizeY && tmp.Y <= MapSizeY && tmp.Z >= -MapSizeZ && tmp.Z <= MapSizeZ)
                {
                    rtn.Add(GetHex(tmp.X, tmp.Y, tmp.Z));
                }
            }
        }
        return rtn;
    }

    public HexGrid GetHex(int x, int y, int z)
    {
        return Map[x + MapSizeX][y + MapSizeY][z + MapSizeZ];
    }

    public void SetHexColor(HexGrid hex, Color color)
    {
        hex.transform.renderer.material.color = color;
    }
}
