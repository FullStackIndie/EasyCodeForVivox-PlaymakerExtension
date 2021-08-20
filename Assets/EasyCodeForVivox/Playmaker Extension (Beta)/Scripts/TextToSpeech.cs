using UnityEngine;
using HutongGames.PlayMaker;
using VivoxUnity;

namespace EasyCodeForVivox.PlayMaker.Actions
{
    [ActionCategory("Vivox")]
    [HutongGames.PlayMaker.Tooltip("Text-To-Speech(TTS) Speak specified text locally, remotely, or both")]
    public class TextToSpeech : FsmStateAction
    {
        [UIHint(UIHint.FsmString)]
        [HutongGames.PlayMaker.Tooltip("Channel Name to Join Channel")]
        public FsmString messageToSpeak;

        EasyManager easyManager;

        public override void Reset()
        {
            messageToSpeak = null;

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

            EasyTextToSpeech.TTSMessageRemoved += OnTTSMessageRemoved;
            easyManager.SpeakTTS(messageToSpeak.Value, TTSDestination.QueuedRemoteTransmissionWithLocalPlayback);
              
        }

        public override void OnExit()
        {
            EasyTextToSpeech.TTSMessageRemoved -= OnTTSMessageRemoved;
        }

        private void OnTTSMessageRemoved(ITTSMessageQueueEventArgs ttsMessages)
        {
            Finish();
        }
    }
}
