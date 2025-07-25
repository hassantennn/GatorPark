using UnityEngine;

public class GarageManager : MonoBehaviour
{
    public int totalSpots = 100;
    private int availableSpots;

    public bool isUserCheckedIn = false;

    void Start()
    {
        availableSpots = totalSpots;
    }

    public int GetAvailableSpots()
    {
        return availableSpots;
    }

    public void CheckIn()
    {
        if (!isUserCheckedIn && availableSpots > 0)
        {
            availableSpots--;
            isUserCheckedIn = true;
        }
    }

    public void CheckOut()
    {
        if (isUserCheckedIn)
        {
            availableSpots++;
            isUserCheckedIn = false;
        }
    }
}