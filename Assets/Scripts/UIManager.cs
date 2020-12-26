using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TMP_Text lapCounterText, lapTimeText, bestLapTimeText, positionText, raceStartCountdownText;

    public void LapUpdate(int currentLap, int totalLaps)
    {
        lapCounterText.text = currentLap + "/" + totalLaps;
    }

    public void LapTimeUpdate(float lapTime)
    {
        var ts = System.TimeSpan.FromSeconds(lapTime);//when you dont know the data type, use var
        //modern problems require modern solutions
        lapTimeText.text = string.Format("{0:00}m{1:00}.{2:000}s", ts.Minutes, ts.Seconds, ts.Milliseconds);
    }

    public void BestLapTimeUpdate(float bestLapTime)
    {
        var ts = System.TimeSpan.FromSeconds(bestLapTime);
        bestLapTimeText.text = string.Format("{0:00}m{1:00}.{2:000}s", ts.Minutes, ts.Seconds, ts.Milliseconds);
    }

    public void PositionTextUpdate(int position)
    {
        if (position == 1)
            positionText.text = "1st";
        else
            positionText.text = "2nd";
    }
}
