using UnityEngine;
using HutongGames.PlayMaker;
using System;
using VivoxUnity;
using EasyCodeForVivox;

namespace EasyCodeForVivox.PlayMaker.Actions
{
    [ActionCategory("Vivox")]
    [HutongGames.PlayMaker.Tooltip("Setup Vivox Info From Developer Portal and other settings")]
    public class VivoxSetup : FsmStateAction
    {

        EasyManager easyManager;

        [UIHint(UIHint.Description)]
        [HutongGames.PlayMaker.Tooltip("Server URI Address (API End-Point) From Vivox Developer Portal")]
        public FsmString serverURI;     
        [UIHint(UIHint.Description)]
        [HutongGames.PlayMaker.Tooltip("Domain From Vivox Developer Portal")]
        public FsmString domain;      
        [UIHint(UIHint.Description)]
        [HutongGames.PlayMaker.Tooltip("Issuer From Vivox Developer Portal")]
        public FsmString tokenIssuer;     
        [UIHint(UIHint.Description)]
        [HutongGames.PlayMaker.Tooltip("Secret Key From Vivox Developer Portal")]
        public FsmString tokenKey;

        public override void Reset()
        {
            serverURI = null;
            domain = null;
            tokenIssuer = null;
            tokenKey = null;

            if (easyManager != null)
            {
                if (EasySession.mainLoginSession.State == LoginState.LoggedIn) easyManager.Logout();
                easyManager = null;
            }
        }

        public override void OnEnter()
        {
            easyManager = GameObject.FindObjectOfType<EasyManager>();
            if (easyManager == null)
            {
                easyManager = new GameObject("EasyManager", typeof(EasyManager)).GetComponent<EasyManager>();
            }
            EasySession.APIEndpoint = new Uri(serverURI.Value);
            EasySession.Domain = domain.Value;
            EasySession.Issuer = tokenIssuer.Value;
            EasySession.SecretKey = tokenKey.Value;
            easyManager.InitializeClient();
        }

        public override void OnExit()
        {
            
        }

    }
}