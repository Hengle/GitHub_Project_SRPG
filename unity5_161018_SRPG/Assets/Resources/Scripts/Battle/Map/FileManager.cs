using UnityEngine;
using System.Xml;

// 3-05:20분 맵 불러오기
public class FileManager
{
    private static FileManager inst = null;
    public static FileManager GetInst()
    {
        if(inst == null)
        {
            inst = new FileManager();
        }
        return inst;
    }

    // 디바이스 체크
    private string m_sCheckDevice = "";
    // 디바이스가 안드로이드일 경우에 이걸로 로드한다
    private TextAsset m_textAsset = null;

	public MapInfo LoadMap()
    {
        Debug.Log("LoadMap start");
        MapInfo mapInfo = new MapInfo();

        XmlDocument xmlFile = new XmlDocument();

        // 빌드해서 테스트바람

        m_sCheckDevice = "Android";
        m_textAsset = (TextAsset)Resources.Load("test", typeof(TextAsset));
        xmlFile.LoadXml(m_textAsset.text);
        
        XmlNode mapSize = xmlFile.SelectSingleNode("MapInfo/MapSize");
        string mapSizeString = mapSize.InnerText;
        string[] sizes = mapSizeString.Split(' ');

        Debug.Log("MapSizeX: " + sizes[0]);
        Debug.Log("MapSizeY: " + sizes[1]);
        Debug.Log("MapSizeZ: " + sizes[2]);

        int mapSizeX = mapInfo.MapSizeX = int.Parse(sizes[0]);
        int mapSizeY = mapInfo.MapSizeY = int.Parse(sizes[1]);
        int mapSizeZ = mapInfo.MapSizeZ = int.Parse(sizes[2]);

        XmlNode background = xmlFile.SelectSingleNode("MapInfo/Background");
        mapInfo.BackgroundDir = background.InnerText;
        Debug.Log("background: " + mapInfo.BackgroundDir);

        XmlNodeList hexes = xmlFile.SelectNodes("MapInfo/Hex");

        foreach(XmlNode hex in hexes)
        {
            string mapPosStr = hex["MapPos"].InnerText;
            string[] mapPoses = mapPosStr.Split(' ');
            int mapPosX = int.Parse(mapPoses[0]);
            int mapPosY = int.Parse(mapPoses[1]);
            int mapPosZ = int.Parse(mapPoses[2]);

            string passableStr = hex["Passable"].InnerText;
            bool passable = passableStr == "True";

            // 3-05:22분50초 texture 지움
            //int textureIdx = int.Parse(hex["TextureIdx"].InnerText);
            HexInfo hexInfo = new HexInfo();
            hexInfo.MapPosX = mapPosX;
            hexInfo.MapPosY = mapPosY;
            hexInfo.MapPosZ = mapPosZ;
            hexInfo.Passable = passable;
            //hexInfo.TextureIdx = texture; //texture안씀
            mapInfo.HexInfos.Add(hexInfo);
        }

        return mapInfo;
    }
}