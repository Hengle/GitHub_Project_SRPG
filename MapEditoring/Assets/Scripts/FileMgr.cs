using UnityEngine;
using System.Collections;
// xml사용하기 위해.
using System.Xml;

// 3-01 XML 저장 및 로드(테스트)
public class FileMgr
{
    private static FileMgr inst = null;
    public static FileMgr GetInst()
    {
        if(inst == null)
        {
            inst = new FileMgr();
        }
        return inst;
    }

    public void SaveData()
    {
        // 3-05:11분 맵정보 XML로 저장(실전)
        GUIMgr gm = GUIMgr.GetInst();
        MapMgr mm = MapMgr.GetInst();
        XmlDocument xmlFIle = new XmlDocument();
        xmlFIle.AppendChild(xmlFIle.CreateXmlDeclaration("1.0", "utf-8", "yes"));

        XmlNode rootNode = xmlFIle.CreateNode(XmlNodeType.Element, "MapInfo", string.Empty);
        xmlFIle.AppendChild(rootNode);

        XmlElement mapSize = xmlFIle.CreateElement("MapSize");
        mapSize.InnerText = gm.SizeX + " " + gm.SizeY + " " + gm.SizeZ;

        rootNode.AppendChild(mapSize);

        XmlElement background = xmlFIle.CreateElement("Background");
        background.InnerText = "Background01";
        rootNode.AppendChild(background);

        // 3-05:13분26초
        int mapSizeX = gm.SizeX;
        int mapSizeY = gm.SizeY;
        int mapSizeZ = gm.SizeZ;

        for(int x = -mapSizeX; x <= mapSizeX; x++)
        {
            for(int y = -mapSizeY; y <= mapSizeY; y++)
            {
                for(int z = -mapSizeZ; z <= mapSizeZ; z++)
                {
                    if(x + y + z == 0)
                    {
                        XmlNode hexNode = xmlFIle.CreateNode(XmlNodeType.Element, "Hex", string.Empty);
                        rootNode.AppendChild(hexNode);
                        XmlElement mapPos = xmlFIle.CreateElement("MapPos");
                        HexInfo hex = mm.Map[x + mapSizeX][y + mapSizeY][z + mapSizeZ];
                        mapPos.InnerText = hex.X + " " + hex.Y + " " + hex.Z;
                        hexNode.AppendChild(mapPos);

                        XmlElement passable = xmlFIle.CreateElement("Passable");
                        passable.InnerText = hex.Passable.ToString();
                        hexNode.AppendChild(passable);
                    }
                }
            }
        }
        Debug.Log("MapInfo XML Files Save");
        xmlFIle.Save("test.xml");
    }

    public void LoadData()
    {
        XmlDocument xmlFile = new XmlDocument();
        xmlFile.Load("test.xml");
        XmlNode mapSize = xmlFile.SelectSingleNode("MapInfo/MapSize");
        string innerText = mapSize.InnerText;
        Debug.Log(innerText);
    }
}