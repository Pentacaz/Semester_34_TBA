using UnityEngine;

public class NpcSpotLocation : MonoBehaviour
{
    public Collider spotTrigger;
    public bool isOccupied;
    public Transform location;

    public void ChangeStatus(bool status)
    {
        //spotTrigger.enabled = !status;
        isOccupied = status;
    }
}
