using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class LevelParser
{
    public Level ParseLevelFile(string objectsFilename, string floorFilename, string platformFilename, string mappingFilename) {
        XmlReader reader = XmlReader.Create(mappingFilename);
        Dictionary<int, string> types = new Dictionary<int, string>();
        while (reader.Read()) {
            if (reader.NodeType == XmlNodeType.Element && reader.Name == "tile") {
                int id = int.Parse(reader.GetAttribute("id"));
                string type = reader.GetAttribute("type");
                types[id] = type;
            }
        }

        List<Level.BlockDefinition> objectBlocks = GetBlocks(objectsFilename, types);
        List<Level.BlockDefinition> floorBlocks = GetBlocks(floorFilename, types);
        List<Level.BlockDefinition> platformBlocks = GetBlocks(platformFilename, types);

        Level l = new Level();
        l.floor = floorBlocks;
        l.objects = objectBlocks;
        l.platform = platformBlocks;
        return l;
    }

    private List<Level.BlockDefinition> GetBlocks(string filename, Dictionary<int, string> mapping) { 
        List<Level.BlockDefinition> objectsBlocks = new List<Level.BlockDefinition>();
        string objectsString = System.IO.File.ReadAllText(filename).Trim();
        string[] rows = objectsString.Split('\n');
        for (int row = 0; row < rows.Length; row++) {
            string[] cols = rows[row].Split(',');
            for (int col = 0; col < cols.Length; col++) {
                try {
                    int id = int.Parse(cols[col].Trim());
                    if (id != -1) {
                        Level.BlockDefinition newBlock = new Level.BlockDefinition();
                        newBlock.pos = new Vector2Int(col, row);
                        newBlock.type = mapping[id];
                        objectsBlocks.Add(newBlock);
                    }
                } catch {
                    Debug.LogError("Error");
                }
            }
        }
        return objectsBlocks;
    }
}

public class Level {
    public struct BlockDefinition {
        public Vector2Int pos;
        public string type;
    }
    public List<BlockDefinition> floor;
    public List<BlockDefinition> platform;
    public List<BlockDefinition> objects;
}