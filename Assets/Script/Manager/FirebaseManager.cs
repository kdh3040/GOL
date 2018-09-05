using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirebaseManager {


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
                Debug.Log("!!!!! TokenAsync was canceled.");
                return;
            }

            if (task.IsFaulted)
            {
                Debug.Log("!!!!! TokenAsync encountered an error: " + task.Exception);
                return;
            }

            string idToken = task.Result;

            Debug.Log("!!!!! Token: " + idToken);
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
                Debug.Log("!!!!!! User signed in IsCanceled");
                return;
            }
            if(Task.IsFaulted)
            {
                Debug.Log("!!!!!! User signed in IsFaulted");
                return;
            }

            user = Task.Result;
            TokenRefresh();
            Debug.Log("!!!!!! User signed in successfully");
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                    user.DisplayName, user.UserId);
        });
        
    }
     
    public void OnTokenReceived(object sender, Firebase.Messaging.TokenReceivedEventArgs token)
    {
        Debug.Log("!!!!! Received Registration Token: " + token.Token);
    }

    public void OnMessageReceived(object sender, Firebase.Messaging.MessageReceivedEventArgs e)
    {

        if (SettingManager.Instance.GetNotiStatus() == false)
            return;

        Debug.LogFormat("Received a new message from: " + e.Message.From);

        var notification = e.Message.Notification;
        if (notification != null)
        {
            Debug.Log("!!!!! title: " + notification.Title);
            Debug.Log("!!!!! body: " + notification.Body);

        }
        if (e.Message.From.Length > 0)
            Debug.Log("!!!!! from: " + e.Message.From);
        if (e.Message.Data.Count > 0)
        {
            Debug.Log("!!!!! data:");
            foreach (System.Collections.Generic.KeyValuePair<string, string> iter in e.Message.Data)
            {
                Debug.Log("!!!!!   " + iter.Key + ": " + iter.Value);
            }
        }
    }
 
}
