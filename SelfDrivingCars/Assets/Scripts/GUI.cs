using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public class GUI : MonoBehaviour
{
    public Text boardText;
    private Dictionary<int, string> cars;

    private void Start() {
        cars = new Dictionary<int, string>();
        cars.Add(0, "WHITE");
        cars.Add(1, "ORANGE");
        cars.Add(2, "GREEN");
        cars.Add(3, string.Format("{0, -30}", "RED"));
    }

    public void UpdateBillboard(List<int> carsPositions, List<int> laps, List<string> times) {
        int carsNum = carsPositions.Count;
        int pos = 1;
        int[] standings = GetStandings(carsPositions);

        string tempText = "\t INTELLIGENT ROBOTICS CIRCUIT \n\n";        
        while(pos <= carsNum) {
            tempText += string.Format("{0, -35} \t| {1, -20} \t| {2, -10}", 
                                    $"{pos}. \t CAR {cars[standings[pos - 1]]}",
                                    $"Lap {laps[standings[pos - 1]]}",
                                    $"Time {times[standings[pos - 1]]}");
            tempText += "\n";

            pos++;
        }

        boardText.text = tempText;
    }


    public int[] GetStandings (List<int> carsPositions){
        int[] keys = {carsPositions[0],carsPositions[1],carsPositions[2],carsPositions[3]};
        int[] values = { 0, 1, 2, 3 };
        Array.Sort(keys, values);           
        return values;
    }

    public void SetWinner(int winner, int fastestCar, TimeSpan fastestLap){
        string lapTime = string.Format("{0}:{1}:{2}", fastestLap.Minutes, fastestLap.Seconds, fastestLap.Milliseconds);
        string tempText = "\t INTELLIGENT ROBOTICS CIRCUIT \n\n";

        tempText += string.Format("{0, -30}", $"\t Winner: CAR {cars[winner]}");
        tempText += "\n";

        if (fastestCar != 3) {
            tempText += string.Format("{0, -30}", $"\t Fastest Lap: CAR {cars[fastestCar]} - {lapTime}");
        } else {
            tempText += string.Format("{0, -30}", $"\t Fastest Lap: CAR RED - {lapTime}");
        }

        boardText.text = tempText;
    }
}
