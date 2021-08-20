using UnityEngine;
using HutongGames.PlayMaker;
using VivoxUnity;

namespace EasyCodeForVivox.PlayMaker.Actions
{
    [ActionCategory("Vivox")]
    [HutongGames.PlayMaker.Tooltip("Join Channel In Vivox")]
    public class JoinChannel : FsmStateAction
    {

        [UIHint(UIHint.FsmString)]
        [HutongGames.PlayMaker.Tooltip("Channel Name to Join Channel")]
        public FsmString channelName;
        [UIHint(UIHint.FsmBool)]
        [HutongGames.PlayMaker.Tooltip("Activate Audio In Channel")]
        public FsmBool isAudioChannel;
        [UIHint(UIHint.FsmBool)]
        [HutongGames.PlayMaker.Tooltip("Activate Text In Channel")]
        public FsmBool isTextChannel;
        [HutongGames.PlayMaker.Tooltip("Stop Audio and Text In Other Channels and Activate Audio and Text In This Channel")]
        public FsmBool switchToThisChannel;
        [UIHint(UIHint.FsmBool)]

        EasyManager easyManager;

        public override void Reset()
        {
            channelName = null;
            isAudioChannel = true;
            isTextChannel = true;
            switchToThisChannel = true;

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

            if (string.IsNullOrEmpty(channelName.Value))
            {
                Debug.Log("Channel Name Variable is null");
                return;
            }
            else
            {
                EasyChannel.ChannelConnected += OnChannelJoined;
                easyManager.JoinChannel(channelName.Value, isAudioChannel.Value, isTextChannel.Value, switchToThisChannel.Value, ChannelType.NonPositional);
            }
        }

        public override void OnExit()
        {
           EasyChannel.ChannelConnected -= OnChannelJoined;
        }

        private void OnChannelJoined(IChannelSession channelSession)
        {
            Finish();
        }   
    }
}
