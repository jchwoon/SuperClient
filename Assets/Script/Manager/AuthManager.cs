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
        PlayGamesPlatform.DebugLogEnabled = true; // Activate() 이후에 설정
        PlayGamesPlatform.Instance.Authenticate(SignIn);
    }

    public void SignIn(SignInStatus status)
    {
        if (status == SignInStatus.Success)
        {
            // 서버에서 접근 할 수 있는 Code(token) 요청
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
                Debug.LogError("Firebase 로그인 실패: " + task.Exception);
                return;
            }

            firebaseUser = task.Result;
            Debug.Log("Firebase 로그인 성공: " + firebaseUser.DisplayName);
            //firebaseUser.UserId
        });
    }
}
