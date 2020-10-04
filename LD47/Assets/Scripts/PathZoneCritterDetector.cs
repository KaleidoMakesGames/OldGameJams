using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathZoneCritterDetector : MonoBehaviour
{
    public PolygonCollider2D polygonCollider;
    public EdgeCollider2D edgeCollider;

    public List<CritterAIController> crittersCaught { get; private set; }

    // Start is called before the first frame update
    void Start() {
        crittersCaught = new List<CritterAIController>();
        var overlaps = new List<Collider2D>();
        polygonCollider.OverlapCollider(new ContactFilter2D().NoFilter(), overlaps);
        foreach (var overlap in overlaps) {
            var cmc = overlap.GetComponentInParent<CritterAIController>();
            if (cmc != null && edgeCollider.Distance(overlap).distance > 0) {
                crittersCaught.Add(cmc);
            }
        }

        Debug.Log(crittersCaught.Count);
    }
}
