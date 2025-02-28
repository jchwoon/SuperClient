using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Firebase.Auth;
using Google;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine;
using UnityEngine.UI;

public class AuthManager
{
    private FirebaseAuth auth;
    private FirebaseUser firebaseUser;

    public AuthManager()
    {
        auth = FirebaseAuth.DefaultInstance;
    }

    public void RequestLogin()
    {
        PlayGamesPlatform.Activate();
        PlayGamesPlatform.DebugLogEnabled = true; // Activate() ���Ŀ� ����
        PlayGamesPlatform.Instance.Authenticate(SignIn);
    }

    public void SignIn(SignInStatus status)
    {
        if (status == SignInStatus.Success)
        {
            // �������� ���� �� �� �ִ� Code(token) ��û
            PlayGamesPlatform.Instance.RequestServerSideAccess(false, AuthenticateWithFirebase);
        }
        else
        {
            Debug.Log($"OnGoogleLogin Failed {status}");
        }
    }

    private void AuthenticateWithFirebase(string token)
    {
        Credential credential = GoogleAuthProvider.GetCredential(token, null);
        auth.SignInWithCredentialAsync(credential).ContinueWith(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.LogError("Firebase �α��� ����: " + task.Exception);
                return;
            }

            firebaseUser = task.Result;
            Debug.Log("Firebase �α��� ����: " + firebaseUser.DisplayName);
            //firebaseUser.UserId
        });
    }
}
