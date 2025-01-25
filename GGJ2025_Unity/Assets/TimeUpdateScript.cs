using TMPro;
using UnityEngine;

public class TimeUpdateScript : MonoBehaviour
{
    public GameObject textRef;

    private float timeRemaining;

    void Start()
    {
    }

    void Update()
    {
        if (textRef != null)
        {

            timeRemaining += Time.deltaTime;

            // Calculate minutes and seconds
            int minutes = Mathf.FloorToInt(timeRemaining / 60f);
            int seconds = Mathf.FloorToInt(timeRemaining % 60f);

            // Update the text
            TextMeshProUGUI TextMeshProRef = textRef.GetComponent<TextMeshProUGUI>();
            if (TextMeshProRef != null)
            {
                TextMeshProRef.SetText($"{minutes:D2}:{seconds:D2}");
            }

        }
    }
}