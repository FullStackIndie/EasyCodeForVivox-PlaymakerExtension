using UnityEngine;
using HutongGames.PlayMaker;
using VivoxUnity;

namespace EasyCodeForVivox.PlayMaker.Actions
{
    [ActionCategory("Vivox")]
    [HutongGames.PlayMaker.Tooltip("Send Direct(Private) Message To Specified Player")]
    public class SendDirectMessage : FsmStateAction
    {


        [UIHint(UIHint.FsmString)]
        [HutongGames.PlayMaker.Tooltip("Username to Send Direct Message")]
        public FsmString userName;
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
            userName = null;
            messageToSend = null;
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

            if (string.IsNullOrEmpty(userName.Value))
            {
                Debug.Log("User Name Variable is null");
                return;
            }
            else
            {
                EasyMessages.DirectMesssageSent += OnDirectMessageSent;
                easyManager.SendDirectMessage(userName.Value, messageToSend.Value, hiddenMessageHeader.Value, hiddenMessageDetails.Value);
            }
        }

        public override void OnExit()
        {
            EasyMessages.DirectMesssageSent -= OnDirectMessageSent;
        }

        private void OnDirectMessageSent()
        {
            Finish();
        }
    }
}
