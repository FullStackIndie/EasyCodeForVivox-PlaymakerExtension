using UnityEngine;
using HutongGames.PlayMaker;
using VivoxUnity;

namespace EasyCodeForVivox.PlayMaker.Actions
{
    [ActionCategory("Vivox")]
    [HutongGames.PlayMaker.Tooltip("Send Message To Everyone In Current Channel")]
    public class SendChannelMessage : FsmStateAction
    {

        [UIHint(UIHint.FsmString)]
        [HutongGames.PlayMaker.Tooltip("Channel Name to Send Message")]
        public FsmString channelName;
        [UIHint(UIHint.FsmString)]
        [HutongGames.PlayMaker.Tooltip("Message To Send to All Users in Channel")]
        public FsmString messageToSend;
        [UIHint(UIHint.FsmString)]
        [HutongGames.PlayMaker.Tooltip("Extra string info to pass along in message, Use keywords - Ex. UserTyping, PlayerDied, PlayerInRadius")]
        public FsmString hiddenMessageHeader;
        [UIHint(UIHint.FsmString)]
        [HutongGames.PlayMaker.Tooltip("Extra details to pass along as a hidden message[users in channel will not see this message], Keep related to Header(or not)")]
        public FsmString hiddenMessageDetails;


        EasyManager easyManager;

        public override void Reset()
        {
            channelName = null;
            hiddenMessageHeader = null;
            hiddenMessageDetails = null;

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
                EasyMessages.ChannelMesssageSent += OnChannelMessageSent;
                easyManager.SendChannelMessage(channelName.Value, messageToSend.Value, hiddenMessageHeader.Value, hiddenMessageDetails.Value);
            }
        }

        public override void OnExit()
        {
            EasyMessages.ChannelMesssageSent -= OnChannelMessageSent;
        }


        private void OnChannelMessageSent()
        {
            Finish();
        }

    }
}