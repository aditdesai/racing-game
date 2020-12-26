using UnityEngine;

public class CheckpointChecker : MonoBehaviour
{
    public CarController aCar;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Checkpoint")
        {
            //print("Hit cp " + other.GetComponent<Checkpoint>().cpNumber);
            aCar.CheckpointHit(other.GetComponent<Checkpoint>().cpNumber);
        }
    }
}
