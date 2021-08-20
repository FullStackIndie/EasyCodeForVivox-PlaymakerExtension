using UnityEngine;
using HutongGames.PlayMaker;
using VivoxUnity;

namespace EasyCodeForVivox.PlayMaker.Actions
{
    [ActionCategory("Vivox")]
    [HutongGames.PlayMaker.Tooltip("Logout of Vivox Voice and Text Chat")]
    public class Logout : FsmStateAction
    {
        EasyManager easyManager;

        public override void Reset()
        {
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

            EasyLogin.LoggedOut += OnLoggedOut;
            easyManager.Logout();
        }

        public override void OnExit()
        {
            EasyLogin.LoggedOut -= OnLoggedOut;
        }

        private void OnLoggedOut(ILoginSession loginSession)
        {
            Finish();
        }
    }
}