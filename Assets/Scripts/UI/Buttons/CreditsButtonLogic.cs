using System.Collections.Generic;
using System.Globalization;
using Unity.Services.Analytics;
using UnityEngine;

public class CreditsButtonLogic : MonoBehaviour
{
    private float _cachedTimeCreditsOpen;

    public void PressCreditsButton()
    {
        _cachedTimeCreditsOpen = Time.time;

        // The ‘creditsViewed’ event will get cached locally 
        //and sent during the next scheduled upload, within 1 minute
        AnalyticsService.Instance.CustomData("creditsViewed");

        // You can call Events.Flush() to send the event immediately
        AnalyticsService.Instance.Flush();
    }

    public void PressBackButton()
    {
        var currentTime = Time.time;
        var totalTimeOnScreen = currentTime - _cachedTimeCreditsOpen;

        //Define Custom Parameters
        Dictionary<string, object> parameters = new Dictionary<string, object>()
        {
            {"timeOnCreditsScreen", totalTimeOnScreen.ToString(CultureInfo.InvariantCulture)}
        };

        AnalyticsService.Instance.CustomData("creditsClosed", parameters);
        AnalyticsService.Instance.Flush();
    }
}