using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class Boat : MonoBehaviour
{
    public static Boat instance { get; private set; }

    public float movementSpeed;
    public float lookaheadDistance;
    public float turnDamping;

    public bool autoUpdate;

    public UnityEngine.U2D.SpriteShapeController animationMovementCurve;
    public float pathResolution;

    public float centralIslandAnimationDistance;
    public float freedomIslandAnimationDistance;
    public AnimationCurve progressChangeCurve;

    public int capacity;

    public List<IBoatLoadable> contents;

    private List<Vector2> splinePath;
    private float splineLength;

    [SerializeField]
    public enum Location { Central, Freedom }
    public Location initialLocation;
    public Location goalLocation;
    [ReadOnly][SerializeField] private Location _currentLocation;
    [ReadOnly] public float animationProgress;
    public Location currentLocation {
        get {
            return _currentLocation;
        }
    }
    [ReadOnly] [SerializeField] private bool _isDocked;

    public bool isDocked {
        get {
            return _isDocked;
        }
    }

    [EasyButtons.Button("Recompute Path")]
    public void RecomputeSpline() {
        splinePath = GeometryUtils2D.ParameterizeSpline(animationMovementCurve.spline, pathResolution, !animationMovementCurve.spline.isOpenEnded);
        splineLength = GeometryUtils2D.PathLength(splinePath, !animationMovementCurve.spline.isOpenEnded);
    }

    private void Awake() {
        if(instance != null) {
            Debug.LogWarning("Boat singleton already exists");
        }
        contents = new List<IBoatLoadable>();
        instance = this;
        RecomputeSpline();
        Dock(initialLocation);
    }

    private void Update() {
        if (Application.isPlaying) {
        } else {
            if(autoUpdate) {
                RecomputeSpline();
            }
            Dock(initialLocation);
        }
    }

    public void Depart() {
        if(isDocked) {
            IntroManager.instance.OnDepart();
            StartCoroutine(Depart(currentLocation == Location.Central ? Location.Freedom : Location.Central));
        }
    }

    private IEnumerator Depart(Location location) {
        _isDocked = false;
        float startingPathDistance = GetDistanceForPosition(_currentLocation);
        float goalPathDistance = GetDistanceForPosition(location);
        if(goalPathDistance < startingPathDistance) {
            goalPathDistance += splineLength;
        }

        float distanceToMove = goalPathDistance - startingPathDistance;
        float transitionTime = distanceToMove / movementSpeed;

        float timeElapsed = 0.0f;
        while(timeElapsed <= transitionTime) {
            SetBoatPostion(startingPathDistance + progressChangeCurve.Evaluate(timeElapsed / transitionTime) * distanceToMove);
            yield return null;
            timeElapsed += Time.deltaTime;
        }
        IntroManager.instance.OnArrival();
        Dock(location);
    }

    private void Dock(Location location) {
        _currentLocation = location;
        _isDocked = true;
        SetBoatPostion(GetDistanceForPosition(_currentLocation));
    }

    private void SetBoatPostion(float distanceAlongPath) {
        transform.position = GetPosition(distanceAlongPath);
        Vector3 nextPosition = GetPosition(distanceAlongPath + lookaheadDistance);
        Vector3 tangent = (nextPosition - transform.position).normalized;
        float maxDelta = Application.isPlaying ? Time.deltaTime / turnDamping : Mathf.Infinity;
        transform.localEulerAngles = Vector3.forward * Mathf.MoveTowardsAngle(transform.localEulerAngles.z, Mathf.Atan2(tangent.y, tangent.x) * Mathf.Rad2Deg, maxDelta);
    }

    private float GetDistanceForPosition(Location location) {
        return location == Location.Central ? centralIslandAnimationDistance: freedomIslandAnimationDistance;
    }

    private Vector3 GetPosition(float distance) {
        return animationMovementCurve.transform.TransformPoint(GeometryUtils2D.PointAtDistanceAlongPath(splinePath, splineLength, distance, !animationMovementCurve.spline.isOpenEnded));
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        for(int i = 0; i < splinePath.Count - 1; i++) {
            Gizmos.DrawLine(splinePath[i], splinePath[i + 1]);
        }
        if(!animationMovementCurve.spline.isOpenEnded) {
            Gizmos.DrawLine(splinePath[0], splinePath[splinePath.Count-1]);
        }
        Gizmos.DrawWireSphere(GetPosition(centralIslandAnimationDistance), 1.0f);
        Gizmos.DrawWireSphere(GetPosition(freedomIslandAnimationDistance), 1.0f);
    }

    public bool TryLoad(IBoatLoadable loadable) {
        if(contents.Count >= capacity) {
            return false;
        }
        loadable.Load();
        contents.Add(loadable);
        return true;
    }
    
    public void UnloadAll() {
        if(!isDocked) {
            return;
        }
        foreach(var content in new List<IBoatLoadable>(contents)) {
            Unload(content);
        }
        IntroManager.instance.OnUnload();
    }

    public void Unload(IBoatLoadable loaded) {
        if(contents.Contains(loaded)) {
            contents.Remove(loaded);

            loaded.Unload(DockZone.zoneForLocation[currentLocation].GetRandomPoint());
        }
    }
}
