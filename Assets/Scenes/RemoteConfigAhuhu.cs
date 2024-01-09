using Firebase;
using Firebase.Extensions;
using Firebase.RemoteConfig;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class RemoteConfigAhuhu : MonoBehaviour
{
    private void Awake()
    {
        //FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        //{
        //    var dependencyStatus = task.Result;
        //    if (dependencyStatus == DependencyStatus.Available)
        //    {
        //        // Create and hold a reference to your FirebaseApp,
        //        // where app is a Firebase.FirebaseApp property of your application class.

        //        //app = Firebase.FirebaseApp.DefaultInstance;
        //        print("Ket noi Firebase thanh cong");

        //        // Set a flag here to indicate whether Firebase is ready to use by your app.
        //    }
        //    else
        //    {
        //        Debug.LogError(String.Format("Could not resolve all Firebase dependencies: {0}", dependencyStatus));
        //        // Firebase Unity SDK is not safe to use here.
        //    }
        //});


        //Dictionary<string, object> defaults = new Dictionary<string, object>();
        //// These are the values that are used if we haven't fetched data from the server yet, or if we ask for values that the server doesn't have:
        //defaults.Add("config_test_string", "default local string");
        //defaults.Add("config_test_int", 1);
        //defaults.Add("config_test_float", 1.0);
        //defaults.Add("config_test_bool", false);
        //FirebaseRemoteConfig.DefaultInstance.SetDefaultsAsync(defaults).ContinueWithOnMainThread(task => 
        //{
        //    print("Da them 1 so config default");
        //});


        FetchDataAsync();
        //FirebaseRemoteConfig.DefaultInstance.OnConfigUpdateListener += ConfigUpdateListenerEventHandler;
    }

    public Task FetchDataAsync()
    {
        Debug.Log("Fetching data...");
        Task fetchTask = FirebaseRemoteConfig.DefaultInstance.FetchAsync(TimeSpan.Zero);
        return fetchTask.ContinueWithOnMainThread(FetchComplete);
    }

    private void FetchComplete(Task fetchTask)
    {
        if (!fetchTask.IsCompleted)
        {
            Debug.LogError("Retrieval hasn't finished.");
            return;
        }

        var remoteConfig = FirebaseRemoteConfig.DefaultInstance;
        var info = remoteConfig.Info;
        if (info.LastFetchStatus != LastFetchStatus.Success)
        {
            Debug.LogError($"{nameof(FetchComplete)} was unsuccessful\n{nameof(info.LastFetchStatus)}: {info.LastFetchStatus}");
            return;
        }

        // Fetch successful. Parameter values must be activated to use.
        remoteConfig.ActivateAsync().ContinueWithOnMainThread(task => 
        {
            Debug.Log($"Remote data loaded and ready for use. Last fetch time {info.FetchTime}.");

            print("Có tất cả " + remoteConfig.AllValues.Count + " phần tử trong AllValues RemoteConfig (Tính cả các config default)");
            print(remoteConfig.GetValue("elclassico").StringValue);
            print(remoteConfig.GetValue("DayLaMotNumber").LongValue);
        });
    }


    //// Handle real-time Remote Config events.
    //void ConfigUpdateListenerEventHandler(object sender, ConfigUpdateEventArgs args)
    //{
    //    if (args.Error != RemoteConfigError.None)
    //    {
    //        Debug.Log(String.Format("Error occurred while listening: {0}", args.Error));
    //        return;
    //    }

    //    Debug.Log("Updated keys: " + string.Join(", ", args.UpdatedKeys));
    //    // Activate all fetched values and then display a welcome message.
    //    FirebaseRemoteConfig.DefaultInstance.ActivateAsync().ContinueWithOnMainThread(task => 
    //    {
    //        print("Vừa mới update xong");
    //    });
    //}

    //// Stop the listener.
    //void OnDestroy()
    //{
    //    FirebaseRemoteConfig.DefaultInstance.OnConfigUpdateListener -= ConfigUpdateListenerEventHandler;
    //}


    public void ClickButton()
    {
        FetchDataAsync();
    }    
}
