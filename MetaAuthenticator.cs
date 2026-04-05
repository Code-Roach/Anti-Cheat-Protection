using GorillaNetworking;
using Oculus.Platform;
using Oculus.Platform.Models;
using Photon.Pun;
using System;
using UnityEngine;

public class MetaAuthenticator : MonoBehaviour
{
    public static MetaAuthenticator Instance;

    [Header("OG BY Meta & Ires, remastered by MichaelServices")]
    public ulong UserID;
    public string OculusID;
    public string DisplayName;
    public string Nonce;
    public string AccessToken;

    public bool authed;
    public bool platformInitialized;
    public bool UserDataReceived;
    public bool NonceReceived;
    public bool AccessTokenReceived;
    public bool setPhotonnNicknameToOculusName;
    public bool varboseLogging;
    public bool testUsers;

    float startTime;
    float authTimeout = 10f;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
    }

    void Start()
    {
        BeginAuthentication();
    }

    void Update()
    {
        if (testUsers) return;

        if (!authed && Time.time - startTime > authTimeout)
        {
            OnFailAuth();
        }

        if (!authed && platformInitialized && UserDataReceived && NonceReceived && AccessTokenReceived)
        {
            FinalizeAuth();
        }
    }

    public void BeginAuthentication()
    {
        startTime = Time.time;

        try
        {
            Core.AsyncInitialize().OnComplete(OnPlatformInit);
        }
        catch
        {
            OnFailAuth();
        }
    }

    void OnPlatformInit(Message msg)
    {
        if (msg.IsError)
        {
            OnFailAuth();
            return;
        }

        platformInitialized = true;
        Users.GetLoggedInUser().OnComplete(OnUserReceived);
    }

    void OnUserReceived(Message<User> msg)
    {
        if (msg.IsError)
        {
            OnFailAuth();
            return;
        }

        User user = msg.Data;
        UserID = user.ID;
        OculusID = user.OculusID;
        DisplayName = user.DisplayName;

        UserDataReceived = true;
        Users.GetUserProof().OnComplete(OnNonceReceived);
    }

    void OnNonceReceived(Message<UserProof> msg)
    {
        if (msg.IsError)
        {
            OnFailAuth();
            return;
        }

        Nonce = msg.Data.Value;
        NonceReceived = true;

        Users.GetAccessToken().OnComplete(OnAccessToken);
    }

    void OnAccessToken(Message<string> msg)
    {
        if (msg.IsError)
        {
            OnFailAuth();
            return;
        }

        AccessToken = msg.Data;
        AccessTokenReceived = true;
    }

    void FinalizeAuth()
    {
        authed = true;

        if (setPhotonnNicknameToOculusName)
        {
            PhotonNetwork.LocalPlayer.NickName = DisplayName;
        }
    }

    void ResetAuth()
    {
        platformInitialized = false;
        UserDataReceived = false;
        NonceReceived = false;
        AccessTokenReceived = false;
        authed = false;

        UserID = 0;
        OculusID = "";
        DisplayName = "";
        Nonce = "";
        AccessToken = "";
    }

    public void ReAuth()
    {
        ResetAuth();
        BeginAuthentication();
    }

    public bool IsAuthed() => authed;
    public string GetNonce() => Nonce;
    public string GetAccessToken() => AccessToken;
    public string GetDisplayname() => DisplayName;
    public string GetOculusUserName() => OculusID;
    public ulong GetUserID() => UserID;

    void OnFailAuth()
    {
        Application.Quit();
    }
}