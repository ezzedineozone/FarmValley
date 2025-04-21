using UnityEngine;

public class DayCycle : MonoBehaviour
{
    [SerializeField] GameEvents gameEvents; 
    int day = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameEvents.onDayPassed.AddListener(nextDay);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void nextDay()
    {
        day++;
        Debug.Log("Day: " + day);
        Debug.Log("Day passed event triggered.");
    }
}
