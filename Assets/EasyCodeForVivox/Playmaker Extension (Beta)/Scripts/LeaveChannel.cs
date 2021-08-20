using UnityEngine;
using HutongGames.PlayMaker;
using VivoxUnity;

namespace EasyCodeForVivox.PlayMaker.Actions
{
    [ActionCategory("Vivox")]
    [HutongGames.PlayMaker.Tooltip("Leave Channel")]
    public class LeaveChannel : FsmStateAction
    {
        EasyManager easyManager;

        [UIHint(UIHint.FsmString)]
        [HutongGames.PlayMaker.Tooltip("Channel Name to Join Channel")]
        public FsmString channelName;

        public override void Reset()
        {
            channelName = null;
            if (easyManager != null)
            {
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

            if (channelName.Value != null)
            {
                EasyChannel.ChannelDisconnected += OnChannelDisconnected;
                easyManager.LeaveChannel(channelName.Value);
            }
        }

        public override void OnExit()
        {
            EasyChannel.ChannelConnected -= OnChannelDisconnected;
        }

        private void OnChannelDisconnected(IChannelSession channelSession)
        {
            Finish();
        }
    }
}