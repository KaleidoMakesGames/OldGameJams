using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    public List<GameObject> typeMappings;

    public string tilesetFile;
    public List<LevelInfo> levels;
    public int currentLevel;

    public Transform objectsContainer;
    public Transform platformContainer;
    public PlayArea floorContainer;
    public HistorianManager historianManager;

    private void Start() {
        currentLevel--;
        NextLevel();
    }

    public void LoadLevel() {
        Dictionary<string, GameObject> mapping = new Dictionary<string, GameObject>();
        foreach(GameObject m in typeMappings) {
            mapping[m.name] = m;
        }

        LevelParser l = new LevelParser();
        string objectsFile = "Assets/Levels/" + levels[currentLevel].levelName + "_Objects.csv";
        string platformFile = "Assets/Levels/" + levels[currentLevel].levelName + "_Platform.csv";
        string floorFile = "Assets/Levels/" + levels[currentLevel].levelName + "_Floor.csv";

        Level level = l.ParseLevelFile(objectsFile, floorFile, platformFile, tilesetFile);
        
        Bounds allBounds = SpawnSet(level.floor, floorContainer.transform, mapping);
        SpawnSet(level.objects, objectsContainer, mapping);
        SpawnSet(level.platform, platformContainer, mapping);

        platformContainer.transform.position = -allBounds.center + Vector3.up * platformContainer.transform.position.y;
        objectsContainer.transform.position = -allBounds.center + Vector3.up * objectsContainer.transform.position.y;
        floorContainer.transform.position = -allBounds.center + Vector3.up * floorContainer.transform.position.y;
        Physics.SyncTransforms();
        floorContainer.UpdateBounds();
        historianManager.LoadHistorians();
        historianManager.Record();
    }

    private Bounds SpawnSet(List<Level.BlockDefinition> blocks, Transform container, Dictionary<string, GameObject> mapping) {
        Bounds areaBounds = new Bounds();
        bool initialized = false;
        foreach (var b in blocks) {
            Vector3 posVector = new Vector3(b.pos.x, 0.0f, b.pos.y);
            GameObject newObject = Instantiate(mapping[b.type], container);
            newObject.transform.localPosition = posVector;
            if (initialized) {
                areaBounds.Encapsulate(posVector);
            } else {
                areaBounds = new Bounds(posVector, Vector3.zero);
                initialized = true;
            }
        }
        return areaBounds;
    }

    public void ClearLevel(bool immediate = false) {
        DestroyAll(objectsContainer, immediate);
        DestroyAll(platformContainer, immediate);
        DestroyAll(floorContainer.transform, immediate);
        floorContainer.UpdateBounds();
    }

    private static void DestroyAll(Transform t, bool immediate) {
        for (int childIndex = t.childCount - 1; childIndex >= 0; childIndex--) {
            Transform child = t.GetChild(childIndex);
            if (immediate) {
                DestroyImmediate(child.gameObject);
            } else {
                Destroy(child.gameObject);
            }
        }
    }

    public void NextLevel() {
        ClearLevel(true);
        currentLevel++;
        LoadLevel();
    }

    public void ReloadLevel() {
        ClearLevel(true);
        LoadLevel();
    }
}
