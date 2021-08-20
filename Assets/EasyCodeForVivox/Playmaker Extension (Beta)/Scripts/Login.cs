using UnityEngine;
using HutongGames.PlayMaker;
using VivoxUnity;

namespace EasyCodeForVivox.PlayMaker.Actions
{
    [ActionCategory("Vivox")]
    [HutongGames.PlayMaker.Tooltip("Login To Vivox Voice and Text Chat")]
    public class Login : FsmStateAction
    {
        [UIHint(UIHint.Variable)]
        [HutongGames.PlayMaker.Tooltip("Username To Log Into Vivox")]
        public FsmString loginName;
        
        EasyManager easyManager;

        public override void Reset()
        {
            loginName = null;
            if (easyManager != null)
            {
                easyManager = null;
            }
        }

        public override void OnEnter()
        {
            easyManager = GameObject.FindObjectOfType<EasyManager>();
            if(easyManager == null)
            {
                easyManager = new GameObject("EasyManager", typeof(EasyManager)).GetComponent<EasyManager>();
            }

            if (string.IsNullOrEmpty(loginName.Value)) 
            {
                Debug.Log("Username Variable is null");
                return; 
            }
            else
            {
                EasyLogin.LoggedIn += OnLoggedIn;
                easyManager.Login(loginName.Value);
            }
        }

        public override void OnExit()
        {
            EasyLogin.LoggedIn -= OnLoggedIn;
        }

        private void OnLoggedIn(ILoginSession loginSession)
        {
            Finish();
        }
    }

}