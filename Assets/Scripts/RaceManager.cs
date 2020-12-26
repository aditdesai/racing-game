using UnityEngine;
using TMPro;

public class RaceManager : MonoBehaviour
{
    public Checkpoint[] allCheckpoints;
    public int totalLaps = 2;

    public CarController playerCar, aiCar;
    public int playerPosition = 1;

    public UIManager uiManager;
    private bool isStarting = true;
    private float countdown = 4.0f;

    public GameObject mainCam, finishCanvas;

    private bool noNeedToCheckAgain = false;

    
    void Start()
    {
        uiManager.LapUpdate(1, totalLaps);
        for (int i = 0; i < allCheckpoints.Length; i++)
        {
            allCheckpoints[i].cpNumber = i;
        }

        finishCanvas = GameObject.Find("Finish Canvas");
        finishCanvas.SetActive(false);
    }

    private void Update()
    {
        if (playerCar.currentLap < totalLaps + 1)//no need to update position when 2 laps are over
            Invoke("PosUpdate", 0.2f);
        if (isStarting)
        {
            
            countdown -= Time.deltaTime;
            if ((int)countdown == 0)
            {
                uiManager.raceStartCountdownText.text = "GO";
                isStarting = false;
            }
            else
                uiManager.raceStartCountdownText.text = ((int)countdown).ToString();
            
        }
        else if(!noNeedToCheckAgain)
        {
            
            playerCar.enabled = true;
            aiCar.enabled = true;
            Destroy(GameObject.Find("Race Start Countdown"), 1f);
            noNeedToCheckAgain = true;
        }
        
    }

    

    void PosUpdate()
    {
        playerPosition = 1;

        if (playerCar.currentLap < aiCar.currentLap)//check which lap both cars are on
        {
            playerPosition = 2;
        }
        else if (playerCar.currentLap == aiCar.currentLap)
        {
            if (playerCar.nextCheckpoint < aiCar.nextCheckpoint && playerCar.nextCheckpoint != 0)//check which checkpoint cars are on
            {

                playerPosition = 2;
            }
            else if (aiCar.nextCheckpoint == 0 && playerCar.nextCheckpoint != 0)
                playerPosition = 2;
            else if (playerCar.nextCheckpoint == aiCar.nextCheckpoint)//check distance of both cars from next checkpoint
            {
                float playerCarDist = Vector3.Distance(playerCar.transform.position, allCheckpoints[playerCar.nextCheckpoint].transform.position);
                float aiCarDist = Vector3.Distance(aiCar.transform.position, allCheckpoints[aiCar.nextCheckpoint].transform.position);
                if (playerCarDist > aiCarDist)
                    playerPosition = 2;
            }
        }

        uiManager.PositionTextUpdate(playerPosition);
    }

    public void FinishRace(float bestLapTime)
    {
      
        playerCar.isAI = true;
        //stop them after finishing race
        uiManager.LapTimeUpdate(0);
        playerCar.rb.drag = 0.75f;
        //aiCar.rb.drag = 0.75f;
        mainCam.GetComponent<FollowCar>().enabled = false;
        mainCam.transform.position = new Vector3(-198.5f, 4.5f, mainCam.transform.position.z);
        mainCam.transform.rotation = Quaternion.Euler(0, 0, 0);
        LoadFinishScreen(bestLapTime);
    }

    public void LoadFinishScreen(float bestLapTime)
    {
        GameObject.Find("Canvas").SetActive(false);
        finishCanvas.SetActive(true);
        TMP_Text[] finishTexts = finishCanvas.GetComponentsInChildren<TMP_Text>();

        
        var ts = System.TimeSpan.FromSeconds(bestLapTime);
        finishTexts[1].text = string.Format("{0:00}m{1:00}.{2:000}s", ts.Minutes, ts.Seconds, ts.Milliseconds);

        if (playerPosition == 1)
            finishTexts[2].text = "1st";
        else
            finishTexts[2].text = "2nd";
        
    }
}
