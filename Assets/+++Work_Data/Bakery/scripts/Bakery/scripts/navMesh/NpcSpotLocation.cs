using UnityEngine;

public class NpcSpotLocation : MonoBehaviour
{
    public bool isOccupied;
    public Transform location;
    
    // the status if the spot is occupied changes 
    public void ChangeStatus(bool status)
    {
        isOccupied = status;
    }
}