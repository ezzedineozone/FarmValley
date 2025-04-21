using UnityEngine;
using TMPro;
using System.Collections;

public class SleepIndicator : MonoBehaviour
{
    bool isVisible = false;

    [SerializeField] private GameEvents gameEvents;
    private TextMeshProUGUI countdownText;

    void Start()
    {
        countdownText = GetComponent<TextMeshProUGUI>(); // Automatically grabs the TMP component on "this"
        countdownText.gameObject.SetActive(false);
        gameEvents.onPlayerSleep.AddListener(onPlayerSleep);
    }

    void Update()
    {
        if (isVisible)
        {
            // Set the position to top-right of the screen
            RectTransform rectTransform = GetComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(1, 1); // Top-right anchor
            rectTransform.anchorMax = new Vector2(1, 1);
            rectTransform.pivot = new Vector2(1, 1);     // Set pivot to top-right
            rectTransform.anchoredPosition = new Vector2(-20, -20); // 20px from top-right corner
        }
        else {
            transform.position = new Vector3(0, 0, 0); // Reset position when not visible
            //set its position off limit
            transform.position = new Vector3(-100, -100, -100); // Off-screen position
        }
    }

    void onPlayerSleep()
    {
        this.isVisible = true;
        StartCoroutine(startSleepCountDown(5f));
    }

    IEnumerator startSleepCountDown(float seconds)
    {
        isVisible = true;
        gameObject.SetActive(true);
        countdownText.gameObject.SetActive(true);

        float remainingTime = seconds;

        while (remainingTime > 0)
        {
            countdownText.text = $"Sleeping in {Mathf.Ceil(remainingTime)}...";
            Debug.Log("Sleeping in " + Mathf.Ceil(remainingTime) + "...");
            yield return new WaitForSeconds(1f);
            remainingTime -= 1f;
        }

        countdownText.text = "Sleeping...";
        yield return new WaitForSeconds(0.5f);
        gameEvents.onDayPassed.Invoke();
        Debug.Log("Sleep Indicator: " + isVisible);
        isVisible = false;
    }
}
