using UnityEngine;
using System.Collections;
// xml사용하기 위해.
using System.Xml;
// 3-01 XML 저장 및 로드
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
        XmlDocument xmlFIle = new XmlDocument();
        xmlFIle.AppendChild(xmlFIle.CreateXmlDeclaration("1.0", "utf-8", "yes"));

        XmlNode rootNode = xmlFIle.CreateNode(XmlNodeType.Element, "MapInfo", string.Empty);
        xmlFIle.AppendChild(rootNode);

        XmlElement mapSize = xmlFIle.CreateElement("MapSize");
        mapSize.InnerText = "test";

        rootNode.AppendChild(mapSize);

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