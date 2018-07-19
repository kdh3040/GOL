using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirebaseManager : MonoBehaviour {

    Firebase.Auth.FirebaseAuth auth;

	// Use this for initialization
	void Start () {
		
	}

    private void Awake()
    {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;    
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void SignUp()
    {
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

            Firebase.Auth.FirebaseUser newUser = Task.Result;

            Debug.LogFormat("User signed in successfully: {0} ({1})",
                    newUser.DisplayName, newUser.UserId);
        });
    }
}
