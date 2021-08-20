using UnityEngine;
using HutongGames.PlayMaker;
using VivoxUnity;

namespace EasyCodeForVivox.PlayMaker.Actions
{
    [ActionCategory("Vivox")]
    [HutongGames.PlayMaker.Tooltip("Toggle (On/Off) Text In Channel")]
    public class ToggleText : FsmStateAction
    {
        [UIHint(UIHint.FsmString)]
        [HutongGames.PlayMaker.Tooltip("Channel Name to Join Channel")]
        public FsmString channelName;
        [UIHint(UIHint.FsmBool)]
        [HutongGames.PlayMaker.Tooltip("Activate Text In Channel")]
        public FsmBool TextChannelActive;
        [UIHint(UIHint.FsmBool)]

        EasyManager easyManager;

        public override void Reset()
        {
            channelName = null;
            TextChannelActive = true;

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
                EasyTextChannel.TextChannelConnected += OnTextChannelJoined;
                easyManager.SetTextActiveInChannel(channelName.Value, TextChannelActive.Value);
            }
        }

        public override void OnExit()
        {
            EasyTextChannel.TextChannelConnected -= OnTextChannelJoined;
        }

        private void OnTextChannelJoined(IChannelSession channelSession)
        {
            Finish();
        }
    }
}
