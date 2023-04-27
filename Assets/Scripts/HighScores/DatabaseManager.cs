#nullable enable

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Firebase.Firestore;
using Firebase.Extensions;
using System.Threading.Tasks;

public class DatabaseManager : MonoBehaviour
{
    static FirebaseFirestore? _db;

    void Start()
    {
        //TODO: Lock
        if (_db == null)
        {
            Debug.Log("Preparing database");
            Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
            {
                var dependencyStatus = task.Result;
                if (dependencyStatus == Firebase.DependencyStatus.Available)
                {
                    Debug.Log("Database ready");
                    _db = FirebaseFirestore.DefaultInstance;
                }
                else
                {
                    Debug.LogError($"Could not resolve all Firebase dependencies: {dependencyStatus}");
                    // Firebase Unity SDK is not safe to use here.
                    _db = null;
                }
            }).ContinueWith((_) =>
            {
                if (_db != null)
                {

                    Firebase.Auth.FirebaseAuth.DefaultInstance.SignInAnonymouslyAsync().ContinueWith(task =>
                    {
                        if (task.IsCanceled)
                        {
                            Debug.LogError("SignInAnonymouslyAsync was canceled.");
                        }
                        else if (task.IsFaulted)
                        {
                            Debug.LogError($"SignInAnonymouslyAsync encountered an error: {task.Exception.Message}");
                        }
                        else
                        {
                            Firebase.Auth.FirebaseUser newUser = task.Result;
                            Debug.LogFormat("User signed in successfully: {0} ({1})",
                                newUser.DisplayName, newUser.UserId);
                        }
                    });
                }
            });
        }
    }

    public void CreateUser(string username)
    {
        if (_db == null)
        {
            Debug.LogError("Failed to create user because Firestore is not initialized");
            return;
        }

        Debug.Log("Creating User");
        DocumentReference docRef = _db.Collection("users").Document(Firebase.Auth.FirebaseAuth.DefaultInstance.CurrentUser.UserId);
        Dictionary<string, object> user = new Dictionary<string, object>
        {
                { "Scores", new int[] { 10, 20, 50, 100, 10000 } },
                { "HighScore", 10000 },
                { "Name", Firebase.Auth.FirebaseAuth.DefaultInstance.CurrentUser.DisplayName},
        };
        docRef.SetAsync(user).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError($"DB Update FAILED: {task.Exception.Message}");
            }
            else
            {
                Debug.Log($"Added data to the {username} document in the users collection.");
            }
        });
    }
}
