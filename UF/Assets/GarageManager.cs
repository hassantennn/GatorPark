using UnityEngine;

public class GarageManager : MonoBehaviour
{
    public int totalSpots = 100;
    private int availableSpots;

    // Tracks the number of people who have checked in and out of this garage
    public int TotalCheckIns { get; private set; }
    public int TotalCheckOuts { get; private set; }

    public bool isUserCheckedIn = false;

    void Start()
    {
        availableSpots = totalSpots;
    }

    public int GetAvailableSpots()
    {
        return availableSpots;
    }

    public int GetCheckIns()
    {
        return TotalCheckIns;
    }

    public int GetCheckOuts()
    {
        return TotalCheckOuts;
    }

    public void CheckIn()
    {
        if (!isUserCheckedIn && availableSpots > 0)
        {
            availableSpots--;
            isUserCheckedIn = true;
            TotalCheckIns++;
        }
    }

    public void CheckOut()
    {
        if (isUserCheckedIn)
        {
            availableSpots++;
            isUserCheckedIn = false;
            TotalCheckOuts++;
        }
    }
}
