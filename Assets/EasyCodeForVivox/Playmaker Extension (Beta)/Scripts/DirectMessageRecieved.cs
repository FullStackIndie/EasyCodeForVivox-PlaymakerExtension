using UnityEngine;
using HutongGames.PlayMaker;
using VivoxUnity;
using UnityEngine.UI;

namespace EasyCodeForVivox.PlayMaker.Actions
{
    [ActionCategory("Vivox")]
    [HutongGames.PlayMaker.Tooltip("Recieved Direct Messages From Another User")]
    public class DirectMessageRecieved : FsmStateAction
    {
        [UIHint(UIHint.FsmString)]
        [HutongGames.PlayMaker.Tooltip("User Message Is Recieved From")]
        public FsmString messageFromUser;
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
            messageFromUser = null;
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

            EasyMessages.DirectMessageRecieved += OnDirectMessageRecieved;
        }

        public override void OnExit()
        {
            EasyMessages.DirectMessageRecieved -= OnDirectMessageRecieved;
        }

        private void OnDirectMessageRecieved(IDirectedTextMessage message)
        {
            // Passes message values to variables in Playmaker FSM
            messageFromUser.Value = message.Sender.DisplayName;
            messageRecieved.Value = $"{message.ReceivedTime} : From {message.Sender} : {message.Message}";
            headerRecieved.Value = message.ApplicationStanzaNamespace;
            detailsRecieved.Value = message.ApplicationStanzaBody;
            messages.text += $"\nFrom: {message.Sender.DisplayName} : {message.Message}";
            Finish();
        }
    }
}