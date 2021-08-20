using UnityEngine;
using HutongGames.PlayMaker;
using VivoxUnity;

namespace EasyCodeForVivox.PlayMaker.Actions
{
    [ActionCategory("Vivox")]
    [HutongGames.PlayMaker.Tooltip("Toggle(On/Off) Mute Remote User")]
    public class ToggleMuteRemoteUser : FsmStateAction
    {
        //[UIHint(UIHint.FsmEvent)]
        //[HutongGames.PlayMaker.Tooltip("Event to call when User is muted")]
        //public FsmEvent muteUserEvent;
        //[UIHint(UIHint.FsmEvent)]
        //[HutongGames.PlayMaker.Tooltip("Event to call when User is unmuted")]
        //public FsmEvent unmuteUserEvent;

        [UIHint(UIHint.Variable)]
        [HutongGames.PlayMaker.Tooltip("User to mute")]
        public FsmString userToMute;     
        [UIHint(UIHint.Variable)]
        [HutongGames.PlayMaker.Tooltip("The Channel To mute user in")]
        public FsmString channelToMuteIn;
        [UIHint(UIHint.Variable)]
        [HutongGames.PlayMaker.Tooltip("The Channel To mute user in")]
        public FsmBool isUserMuted;

        private EasyManager easyManager;
        private PlayMakerFSM playMaker;

        public override void Reset()
        {
            userToMute = null;
            channelToMuteIn = null;

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

            EasyUsers.UserMuted += OnUserMuted;
            EasyUsers.UserUnmuted += OnUserUnmuted;

            easyManager.ToggleMuteRemoteUser(userToMute.Value, channelToMuteIn.Value);
        }

        public override void OnExit()
        {
            EasyUsers.UserMuted -= OnUserMuted;
            EasyUsers.UserUnmuted -= OnUserUnmuted;
        }

        private void OnUserMuted(IParticipant participant)
        {
            isUserMuted.Value = participant.LocalMute;
            Finish();
        }

        private void OnUserUnmuted(IParticipant participant)
        {
            isUserMuted.Value = participant.LocalMute;
            Finish();
        }
    }
}