using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiverResourceManager : MonoBehaviour
{
    public float proteinPerTime;
    public float oxygenPerProtein;
    public ProteinTracker proteinTracker;
    public OxygenTracker oxygenTracker;

    public Protein proteinPrefab;

    // Update is called once per frame
    void Update()
    {
        float room = proteinTracker.maxProtein - proteinTracker.currentProtein;

        float proteinToProduce = Mathf.Min(proteinPerTime * Time.deltaTime, room);
        float neededOxygen = oxygenPerProtein * proteinToProduce;

        float maxProteinProducable = oxygenTracker.currentOxygen / oxygenPerProtein;

        float producedProtein = Mathf.Min(maxProteinProducable, proteinToProduce);
        float consumedOxygen = producedProtein * oxygenPerProtein;
        oxygenTracker.currentOxygen -= consumedOxygen;
        proteinTracker.currentProtein += producedProtein;

        if(proteinTracker.currentProtein == 1.0f) {
            SpawnProtein();
        }
    }

    // Try to spawn protein in an area
    private void SpawnProtein() {
        for(int x = -1; x <= 1; x++) {
            for(int y = -1; y <= 1; y++) {
                Vector2Int point = new Vector2Int(x, y);
                if(point.magnitude == 1) {
                    bool canPlace = true;
                    foreach (GridElement e in WorldGrid.Instance.ElementsAtPosition(oxygenTracker.gridElement.position + point)) {
                        if (e.isObstacle || e.GetComponent<Protein>() != null) {
                            canPlace = false;
                            break;
                        }
                    }
                    if (canPlace) {
                        // Do spawn
                        GridElement newProtein = Instantiate(proteinPrefab.gameObject).GetComponent<GridElement>();
                        newProtein.position = point + oxygenTracker.gridElement.position;
                        proteinTracker.currentProtein -= 1;
                        return;
                    }
                }
            }
        }
    }
}
