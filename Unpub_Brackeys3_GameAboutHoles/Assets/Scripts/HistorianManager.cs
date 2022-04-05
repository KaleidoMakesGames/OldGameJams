using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HistorianManager : MonoBehaviour
{
    public float transitionTime = 1.0f;

    public AudioClip onSpawnSound;
    public AudioSource audioSource;

    // TODO: Inefficient
    private List<Historian> historians;

    private int currentStateIndex;
    private int furthestStateIndex;

    public HoleChecker hchecker;

    public int movesInSolution {
        get {
            return furthestStateIndex;
        }
    }

    public int totalMoves { get; private set; }

    public bool isReady { get {
            if(historians == null) {
                return false;
            }
            foreach(Historian h in historians) {
                if(h.isTransitioning) {
                    return false;
                }
            }
            return true;
        }
    }

    private void Awake() {
        currentStateIndex = -1;
        historians = null;
    }

    public void LoadHistorians() {
        historians = new List<Historian>();
        currentStateIndex = -1;
        foreach(PushBody b in FindObjectsOfType<PushBody>()) {
            if(b.gameObject == null) {
                continue;
            }
            historians.Add(new PositionHistorian(b));
        }
        foreach(PlatformMover mover in FindObjectsOfType<PlatformMover>()) {
            if (mover.gameObject == null) {
                continue;
            }
            historians.Add(new PositionHistorian(mover));
        }

        totalMoves = -1;

        audioSource.PlayOneShot(onSpawnSound);
        hchecker.hasPlayed = false;
    }

    public void Record() {
        foreach (Historian h in historians) {
            h.DumpAfterInclusive(currentStateIndex + 1);
            h.Record();
        }
        currentStateIndex++;
        totalMoves++;
        furthestStateIndex = currentStateIndex;
    }

    public void Seek(int pos) {
        if (!IsValid(pos)) {
            return;
        }
        foreach (Historian h in historians) {
            h.Seek(pos, transitionTime);
        }
        currentStateIndex = pos;
    }

    private bool IsValid(int pos) {
        return pos >= 0 && pos <= furthestStateIndex;
    }

    public void Rewind() {
        Seek(currentStateIndex - 1);
    }

    public void Forward() {
        Seek(currentStateIndex + 1);
    }

    public bool CanRewind() {
        return IsValid(currentStateIndex - 1);
    }

    public bool CanForward() {
        return IsValid(currentStateIndex + 1);
    }
}
