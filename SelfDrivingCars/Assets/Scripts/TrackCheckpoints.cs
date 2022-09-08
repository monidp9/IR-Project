using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class TrackCheckpoints : MonoBehaviour {

    [SerializeField] private List<Transform> carTransformList;
    [SerializeField] private GUI gui;


    private List<CheckpointSingle> checkpointSingleList;
    private List<int> nextCheckpointSingleIndexList;
    private List<int> laps;
    private List<TimeSpan> startTimes;
    private List<string> times;
    private List<int> carsPositions;

    private int CARS_NUMBERS;
    private int LAPS_MAX = 100000000;
    private int count = 1;
    private int numCarEnded = 0;
    private bool isGpFinished = false;
    private TimeSpan fastestLap = new TimeSpan(0);
    private int fastestCar = -1, winner = -1;

    
    private void Awake() {
        Transform checkpointsTransform = transform.Find("Checkpoints");

        checkpointSingleList = new List<CheckpointSingle>();
        foreach (Transform checkpointSingleTransform in checkpointsTransform) {
            CheckpointSingle checkpointSingle = checkpointSingleTransform.GetComponent<CheckpointSingle>();
            checkpointSingle.SetTrackCheckpoints(this);

            checkpointSingleList.Add(checkpointSingle);
        }

        nextCheckpointSingleIndexList = new List<int>();
        laps = new List<int>();
        startTimes = new List<TimeSpan>();
        times = new List<string>();
        carsPositions = new List<int>();

        CARS_NUMBERS = carTransformList.Count;

        int pos = 0;
        foreach (Transform carTransform in carTransformList) {
            laps.Add(0);
            nextCheckpointSingleIndexList.Add(0);
            carsPositions.Add(pos);
            
            startTimes.Add(DateTime.Now.TimeOfDay);
            times.Add("00:00:00");

            pos++;
        }
    }

    public bool CarThroughCheckpoint(CheckpointSingle checkpointSingle, Transform carTransform) {
        int carIndex = carTransformList.IndexOf(carTransform);
        int nextCheckpointSingleIndex = nextCheckpointSingleIndexList[carIndex];
        int checkpointIndex = checkpointSingleList.IndexOf(checkpointSingle);
        int firstCheckpointIndex = 0;

        CarDriver carDriver;
        GameObject carObj;

        if(checkpointIndex == nextCheckpointSingleIndex) {
            // The car hits the correct checkpoint
            
            UpdateCarLap(carIndex, checkpointIndex, firstCheckpointIndex);
            UpdateCarTime(carIndex, checkpointIndex, firstCheckpointIndex);
            UpdateCarOrder(carIndex, checkpointIndex, firstCheckpointIndex);

            if(laps[carIndex] == LAPS_MAX + 1){
                // The current car has finished the track
                carObj = carTransform.gameObject;
                carObj.SetActive(false);

                if (isGpFinished) {
                    // Last car has finished the track
                    foreach(Transform carTrans in carTransformList) {
                        carObj = carTrans.gameObject;
                        carObj.SetActive(true);

                        carDriver = carTrans.GetComponent<CarDriver>();
                        carDriver.StopMovement();
                        carDriver.SpawnPosition();
                    }

                    gui.SetWinner(winner, fastestCar, fastestLap);
                }
                
            }

            nextCheckpointSingleIndexList[carIndex] = (nextCheckpointSingleIndex + 1) % checkpointSingleList.Count;
            return true;
        } else {
            return false;
        }
    }

    public bool LastCheckpoint(CheckpointSingle checkpointSingle) {
        int checkpointIndex = checkpointSingleList.IndexOf(checkpointSingle);
        int lastCheckpointIndex = checkpointSingleList.Count - 1;

        return checkpointIndex == lastCheckpointIndex;
    }

    public void ResetCheckpoint(Transform carTransform) {
        int carIndex = carTransformList.IndexOf(carTransform);
        nextCheckpointSingleIndexList[carIndex] = 0;
    }

    public CheckpointSingle GetNextCheckpoint(Transform carTransform) {
        int carIndex = carTransformList.IndexOf(carTransform);
        int nextCheckpointIndex = nextCheckpointSingleIndexList[carIndex];

        return checkpointSingleList[nextCheckpointIndex];
    }

    private void UpdateCarTime(int carIndex, int checkpointIndex, int firstCheckpointIndex) {
        if(checkpointIndex == firstCheckpointIndex && laps[carIndex]!=1){
            TimeSpan start = startTimes[carIndex];
            TimeSpan end = DateTime.Now.TimeOfDay;
            TimeSpan diff = end - start;
            startTimes[carIndex] = end;
            times[carIndex] = string.Format("{0}:{1}:{2}", diff.Minutes, diff.Seconds, diff.Milliseconds);

            if(fastestCar == -1 || TimeSpan.Compare(fastestLap,diff) == 1 ){
                fastestLap = diff;
                fastestCar = carIndex;
            } 
        } 
    }

    private void UpdateCarOrder(int updatedCar, int checkpointIndex, int firstCheckpointIndex){
        if(checkpointIndex == firstCheckpointIndex){
            int carCurrentPosition = 0;
            for(int car = 0; car < CARS_NUMBERS; car++){
                if(car != updatedCar){
                    if(laps[updatedCar] > laps[car]){
                        if(carsPositions[updatedCar] > carsPositions[car]){
                            carsPositions[car] = carsPositions[car] + 1;
                        }
                    } else {
                        carCurrentPosition += 1;
                    }
                }
            }
            carsPositions[updatedCar] = carCurrentPosition;
            if(count == CARS_NUMBERS){
                gui.UpdateBillboard(carsPositions, laps, times);
            } else {
                count++;
            }
        }
    }

    public void UpdateCarLap(int carIndex, int checkpointIndex, int firstCheckpointIndex) {        
        if(checkpointIndex == firstCheckpointIndex) {
            laps[carIndex] += 1;
            if(laps[carIndex] == LAPS_MAX + 1){
                winner = (winner == -1) ? carIndex : winner;

                numCarEnded++;
            }
            isGpFinished = (numCarEnded == CARS_NUMBERS) ? true : isGpFinished;
        }
    }
}
