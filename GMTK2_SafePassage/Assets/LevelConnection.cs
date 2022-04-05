using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class LevelConnection : MonoBehaviour
{
    public SpriteShapeController curve;

    public float speedFactor;
    
    private float approxLength;

    private CarController carController;

    private bool isMovingObject;
    
    private void Update() {
        if (!isMovingObject && 
            Mathf.Sign(carController.transform.position.x) == Mathf.Sign(GetPointAlongCurve(0).x) 
            && Mathf.Abs(carController.transform.position.x) >= Mathf.Abs(GetPointAlongCurve(0).x)) {
            isMovingObject = true;
            UpdateAllColliders();
            StartCoroutine(MoveAlongPath(carController.rb));
        }
    }

    public void UpdateAllColliders() {
        return;
        //float heightThreshold = GetPointAlongCurve(0).y;
        //foreach(Collider2D c in FindObjectsOfType<Collider2D>()) {
        //    if(c.GetComponentInParent<CarController>() == null) {
        //        c.enabled = true;
        //        bool enable = c.bounds.min.y < heightThreshold;
        //        c.enabled = enable;
        //        if (c.GetComponentInParent<Rigidbody2D>() != null) {
        //            c.GetComponentInParent<Rigidbody2D>().simulated = enable;
        //        }
        //    }
        //}
    }

    public static List<LevelConnection> GetSortedConnections() {
        List<LevelConnection> c = new List<LevelConnection>(FindObjectsOfType<LevelConnection>());
        c.Sort(delegate (LevelConnection a, LevelConnection b) {
            float yA = a.GetPointAlongCurve(0).y;
            float yB = b.GetPointAlongCurve(0).y;
            return yB.CompareTo(yA);
        });
        return c;
    }

    private void EnableHighest() {
        List<LevelConnection> c = GetSortedConnections();
        c[0].enabled = true;
        for(int i = 1; i < c.Count; i++) {
            c[i].enabled = false;
        }
    }

    private void EnableNext() {
        List<LevelConnection> c = GetSortedConnections();

        bool foundSelf = false;
        foreach(LevelConnection cc in c) {
            if(foundSelf) {
                cc.enabled = true;
                return;
            }
            if(cc == this) {
                foundSelf = true;
                enabled = false;
            }
        }
    }

    private void Awake() {
        EnableHighest();
    }

    private void Start() {
        carController = FindObjectOfType<CarController>();

        approxLength = 0.0f;
        for(float p = 0.1f; p <= 1.0f; p += 0.1f) {
            approxLength += Vector2.Distance(GetPointAlongCurve(p), GetPointAlongCurve(p - 0.1f));
        }
        isMovingObject = false;
    }

    private IEnumerator MoveAlongPath(Rigidbody2D r) {
        float progress = 0.0f;
        Spline s = curve.spline;
        Vector2 contactOffset = r.transform.position - curve.transform.TransformPoint(s.GetPosition(0));
        r.simulated = false;
        r.bodyType = RigidbodyType2D.Kinematic;
        while(true) {
            progress += Time.deltaTime * (Mathf.Abs(r.velocity.x) / approxLength) * speedFactor;
            progress = Mathf.Clamp01(progress);
            
            Vector2 c = Vector2.Lerp(contactOffset, Vector2.up*2.0f, progress);

            Vector2 point = GetPointAlongCurve(progress) + c;

            r.transform.position = point;

            if(progress == 1.0f) {
                r.simulated = true;
                r.bodyType = RigidbodyType2D.Dynamic;
                r.velocity = new Vector2(-r.velocity.x, r.velocity.y);
                r.transform.Rotate(Vector3.up * 180.0f);
                isMovingObject = false;
                EnableNext();
                FindObjectOfType<LevelTargeter>().Advance();
                yield break;
            }
            yield return null;
        }
    }

    public Vector2 GetPointAlongCurve(float progress) {
        var s = curve.spline;
        return curve.transform.TransformPoint(BezierUtility.BezierPoint(s.GetPosition(0), s.GetPosition(0) + s.GetRightTangent(0), s.GetPosition(1) + s.GetLeftTangent(1), s.GetPosition(1), progress));
    }
}
