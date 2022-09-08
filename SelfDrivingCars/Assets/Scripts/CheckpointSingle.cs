using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointSingle : MonoBehaviour {

    private TrackCheckpoints trackCheckpoints;

    public bool IsCorrectCheckPoint(CarDriver carDriver) {
        return trackCheckpoints.CarThroughCheckpoint(this, carDriver.transform);
    }

    public bool IsLastCheckpoint() {
        return trackCheckpoints.LastCheckpoint(this);
    }

    public void SetTrackCheckpoints(TrackCheckpoints trackCheckpoints) {
        this.trackCheckpoints = trackCheckpoints;
    }
}
