using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirebaseManager : MonoBehaviour {


    public static FirebaseManager _instance = null;
    public static FirebaseManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new FirebaseManager();
            }
            return _instance;
        }
    }

    Firebase.Auth.FirebaseAuth auth;
    Firebase.Auth.FirebaseUser user;

    FirebaseManager()
    {
       
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
    }

    // Use this for initialization
    public void Start()
    {
      
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void TokenRefresh()
    {
        user.TokenAsync(true).ContinueWith(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("TokenAsync was canceled.");
                return;
            }

            if (task.IsFaulted)
            {
                Debug.LogError("TokenAsync encountered an error: " + task.Exception);
                return;
            }

            string idToken = task.Result;

            Debug.LogFormat("Token: " + idToken);
        });

    }
    public bool SingedInFirebase()
    {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        if (auth.CurrentUser != null)
        {
            user = auth.CurrentUser;
            TokenRefresh();
            return true;
        }

        return false;
    }


    public void LogIn()
    {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        auth.SignInAnonymouslyAsync().ContinueWith(Task =>
        {
            if (Task.IsCanceled)
            {
                return;
            }
            if(Task.IsFaulted)
            {
                return;
            }

            user = Task.Result;
            TokenRefresh();
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                    user.DisplayName, user.UserId);
        });
    }

    public void OnTokenReceived(object sender, Firebase.Messaging.TokenReceivedEventArgs token)
    {
        Debug.LogFormat("Received Registration Token: " + token.Token);
    }

    public void OnMessageReceived(object sender, Firebase.Messaging.MessageReceivedEventArgs e)
    {
        Debug.LogFormat("Received a new message from: " + e.Message.From);

        var notification = e.Message.Notification;
        if (notification != null)
        {
            Debug.LogFormat("title: " + notification.Title);
            Debug.LogFormat("body: " + notification.Body);

        }
        if (e.Message.From.Length > 0)
            Debug.LogFormat("from: " + e.Message.From);
        if (e.Message.Data.Count > 0)
        {
            Debug.LogFormat("data:");
            foreach (System.Collections.Generic.KeyValuePair<string, string> iter in e.Message.Data)
            {
                Debug.LogFormat("  " + iter.Key + ": " + iter.Value);
            }
        }
    }

}
