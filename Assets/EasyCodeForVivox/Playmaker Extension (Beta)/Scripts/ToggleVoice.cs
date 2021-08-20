using UnityEngine;
using HutongGames.PlayMaker;
using VivoxUnity;

namespace EasyCodeForVivox.PlayMaker.Actions
{
    [ActionCategory("Vivox")]
    [HutongGames.PlayMaker.Tooltip("Toggle (On/Off) Voice In Channel")]
    public class ToggleVoice : FsmStateAction
    {
        [UIHint(UIHint.FsmString)]
        [HutongGames.PlayMaker.Tooltip("Channel Name to Join Channel")]
        public FsmString channelName;
        [UIHint(UIHint.FsmBool)]
        [HutongGames.PlayMaker.Tooltip("Activate Audio In Channel")]
        public FsmBool AudioChannelActive;

        EasyManager easyManager;

        public override void Reset()
        {
            channelName = null;
            AudioChannelActive = true;

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
                EasyVoiceChannel.VoiceChannelConnected += OnVoiceChannelJoined;
                easyManager.SetVoiceActiveInChannel(channelName.Value, AudioChannelActive.Value);
            }
        }

        public override void OnExit()
        {
            EasyVoiceChannel.VoiceChannelConnected -= OnVoiceChannelJoined;
        }


        private void OnVoiceChannelJoined(IChannelSession channelSession)
        {
            Finish();
        }


    }
}