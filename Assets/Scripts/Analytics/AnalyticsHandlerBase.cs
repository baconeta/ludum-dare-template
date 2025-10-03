using System.Collections.Generic;
using UnityEngine;
using Utils;
using static Utils.UnityHelper;

namespace Analytics
{
    public abstract class AnalyticsHandlerBase : EverlastingSingleton<AnalyticsHandlerBase>
    {
        // The string used to lookup PlayerPrefs for the last state of player consent.
        protected const string CONSENT_PREFS_KEY = "analytics_consent";

        /// Has the player granted explicit consent for data collection?
        protected bool _playerConsents;
        [SerializeField] protected bool _playerConsentsDebug;

        /// Starts false - when a player reaches the main-menu, we check if this is false.
        /// If it is, we will display the opt-in/opt-out prompt.
        /// Regardless, we will save to PlayerPrefs that they have been asked and never show the prompt to them again.
        protected bool _doPromptForConsent;
        
        protected virtual void OnEnable() {
            if (PlayerPrefs.HasKey(CONSENT_PREFS_KEY))
            {
                // TODO During early-access, we're forcing analytics usage. Restore normal behavior after leaving early-access.
                switch (1) // switch (PlayerPrefs.GetInt(CONSENT_PREFS_KEY)
                {
                    case -1: // Player has explicitly opted-out.
                        OptOut();
                        _doPromptForConsent = false;
                        break;
                    case 0: // Player has ask-me-again-later'd.
                        goto default;
                    case 1: // Player has explicitly opted-in.
                        OptIn();
                        _doPromptForConsent = false;
                        break;
                    default: // Player has not been asked yet.
                        _doPromptForConsent = true;
                        break;
                }
            }

            if (IsNotInUnityEditor)
            {
                _playerConsentsDebug = false;
            }
        }

        protected virtual void OnDisable()
        {
            // This seems to break the game lol
            // OptOut();
        }

        public virtual void RequestDataDeletion()
        {
            OptOut();
        }

        public virtual void OptIn()
        {
            _playerConsents = true;
            PlayerPrefs.SetInt(CONSENT_PREFS_KEY, 1);
        }

        public virtual void OptOut()
        {
            _playerConsents = false;
            PlayerPrefs.SetInt(CONSENT_PREFS_KEY, -1);
        }

        public virtual void OptLater()
        {
            _playerConsents = false;
            PlayerPrefs.SetInt(CONSENT_PREFS_KEY, 0);
        }

        public static bool GetAnalyticsState()
        {
            return PlayerPrefs.GetInt(CONSENT_PREFS_KEY, 0) == 1;
        }

        protected abstract void SendEventToBackend(string eventName, Dictionary<string, object> analytics);
        
        /// <summary>
        /// Log an AnalyticsEvent with UnityAnalytics. This function will add to the values dictionary.
        ///
        /// We use a dict and convert everything to strings for the analytics system to be more generic
        /// </summary>
        /// <param name="eventName">name of the event being logged.</param>
        /// <param name="analytics">Key-value pairs of an analytic being recorded and its value. Should contain "actIdentifier" and "levelIdentifier" when applicable.</param>
        public void LogEvent(string eventName, Dictionary<string, object> analytics)
        {
            analytics = analytics.MergeDictionary(GetDefaultAnalytics());            
            SendEventToBackend(eventName, analytics);
        }

        // Here we get the analytics data we want to send with every analytics event
        protected abstract Dictionary<string, object> GetDefaultAnalytics();
    }
}
