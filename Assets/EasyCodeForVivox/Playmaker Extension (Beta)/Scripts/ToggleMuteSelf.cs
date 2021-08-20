using UnityEngine;
using HutongGames.PlayMaker;

namespace EasyCodeForVivox.PlayMaker.Actions
{
    [ActionCategory("Vivox")]
    [HutongGames.PlayMaker.Tooltip("Toggle(On/Off) Mute Myself")]
    public class ToggleMuteSelf : FsmStateAction
    {   
        [UIHint(UIHint.FsmEvent)]
        [HutongGames.PlayMaker.Tooltip("Event to call when User is muted")]
        public FsmEvent muteUserEvent;      
        [UIHint(UIHint.FsmEvent)]
        [HutongGames.PlayMaker.Tooltip("Event to call when User is unmuted")]
        public FsmEvent unmuteUserEvent;    
        
        [UIHint(UIHint.FsmEvent)]
        [HutongGames.PlayMaker.Tooltip("Is User Muted")]
        public FsmBool isUserMuted;

        private EasyManager easyManager;
        private PlayMakerFSM playMaker;

        public override void Reset()
        {
            isUserMuted = null;

            if (easyManager != null)
            {
                easyManager = null;
            }
        }

        public override void OnEnter()
        {
            playMaker = Owner.gameObject.GetComponent<PlayMakerFSM>();
            easyManager = GameObject.FindObjectOfType<EasyManager>();
            if (easyManager == null)
            {
                easyManager = new GameObject("EasyManager", typeof(EasyManager)).GetComponent<EasyManager>();
            }

            EasyMute.UserMuted += OnUserMuted;
            EasyMute.UserUnmuted += OnUserUnmuted;

            easyManager.ToggleMuteSelf();

        }

        public override void OnExit()
        {
            EasyMute.UserMuted -= OnUserMuted;
            EasyMute.UserUnmuted -= OnUserUnmuted;
        }

        private void OnUserMuted(bool isMuted)
        {
            isUserMuted.Value = isMuted;
            playMaker.SendEvent(muteUserEvent.Name);
            Finish();
        }

        private void OnUserUnmuted(bool isMuted)
        {
            isUserMuted.Value = isMuted;
            playMaker.SendEvent(unmuteUserEvent.Name);
            Finish();
        }

    }
}