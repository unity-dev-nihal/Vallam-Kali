using UnityEngine;

public class FinishTrigger : MonoBehaviour
{
    public RaceManager raceManager;

    private bool raceFinished = false;

    private void OnTriggerEnter(Collider other)
    {
        if (raceFinished)
            return;

        MainBoatController playerBoat =
            other.GetComponentInParent<MainBoatController>();

        if (playerBoat != null)
        {
            raceFinished = true;

            Debug.Log("PLAYER REACHED FINISH!");

            raceManager.FinishPlayer();
        }
    }
}