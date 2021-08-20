using UnityEngine;
using HutongGames.PlayMaker;
using VivoxUnity;
using UnityEngine.UI;

namespace EasyCodeForVivox.PlayMaker.Actions
{
    [ActionCategory("Vivox")]
    [HutongGames.PlayMaker.Tooltip("Recieved Messages From Current Channel")]
    public class ChannelMessageRecieved : FsmStateAction
    {
        [UIHint(UIHint.FsmString)]
        [HutongGames.PlayMaker.Tooltip("Channel Name Message Recieved From")]
        public FsmString channelName;
        [UIHint(UIHint.FsmString)]
        [HutongGames.PlayMaker.Tooltip("Message Recieved")]
        public FsmString messageRecieved;
        [UIHint(UIHint.FsmString)]
        [HutongGames.PlayMaker.Tooltip("Header Recieved")]
        public FsmString headerRecieved;
        [UIHint(UIHint.FsmString)]
        [HutongGames.PlayMaker.Tooltip("Details Recieved")]
        public FsmString detailsRecieved;
        [HutongGames.PlayMaker.Tooltip("Text Field To Display Recieved Messages")]
        public Text messages;

        EasyManager easyManager;

        public override void Reset()
        {
            channelName = null;
            messageRecieved = null;
            headerRecieved = null;
            detailsRecieved = null;

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

            EasyMessages.ChannelMessageRecieved += OnChannelMessageRecieved;
        }

        public override void OnExit()
        {
            EasyMessages.ChannelMessageRecieved -= OnChannelMessageRecieved;
        }


        public void OnChannelMessageRecieved(IChannelTextMessage message)
        {
            channelName.Value = message.ChannelSession.Channel.Name;
            messageRecieved.Value = $"{message.ReceivedTime} : From {message.Sender} : {message.Message}";
            headerRecieved.Value = message.ApplicationStanzaNamespace;
            detailsRecieved.Value = message.ApplicationStanzaBody;
            messages.text += $"\nFrom: {message.Sender.DisplayName} : {message.Message}";
            Finish();
        }
    }
}