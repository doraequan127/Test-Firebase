using Firebase.Analytics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnalyticsAhuhu : MonoBehaviour
{
    public void ClickButton()
    {
        FirebaseAnalytics.LogEvent("ViDuVeLogEvent");
        print("Vừa mới log event");
    }
}
