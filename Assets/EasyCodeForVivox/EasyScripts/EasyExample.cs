using UnityEngine;
using EasyCodeForVivox;
using UnityEngine.UI;
using VivoxUnity;
using System.Collections;

public class EasyExample : EasyManager
{
    [SerializeField] InputField userName;
    [SerializeField] InputField remotePlayerName;
    [SerializeField] InputField channelName;
    [SerializeField] InputField message;
    [SerializeField] Toggle voiceToggle;
    [SerializeField] Toggle textToggle;
    [SerializeField] Slider selfSlider;
    [SerializeField] Slider remotePlayerSlider;

    [SerializeField] Text newMessage;
    [SerializeField] Scrollbar scrollbar;

    private void OnApplicationQuit()
    {
        UnitializeClient();
    }
    private void Awake()
    {
        InitializeClient();
    }

    void Start()
    {
        // Enables a push-to-talk. This assumes players joined channel muted or are muted. If not it may not work for a few tries
        StartCoroutine(PushToTalk(KeyCode.Space));
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator PushToTalk(KeyCode key)
    {
        yield return new WaitUntil(() => Input.GetKeyDown(key));
        Debug.Log($"{key.ToString()} is down, Local Player Is Unmuted");
        ToggleMuteSelf();
        yield return new WaitUntil(() => Input.GetKeyUp(key));
        Debug.Log($"{key.ToString()} is up, Local Player Muted");
        ToggleMuteSelf();
        yield return PushToTalk(key);
    }



    public void ClearMessages()
    {
        newMessage.text = "";
        scrollbar.value = 1;
    }

    public void Login()
    {
        Login(userName.text);
    }

    public void LogoutPlayer()
    {
        Logout();
    }

    public void JoinChannel()
    {
        JoinChannel(channelName.text, true, true, true, ChannelType.NonPositional);
    }

    public void SendMessage()
    {
        SendChannelMessage(channelName.text, message.text);
    }

    public void SendDirectMessage()
    {
        SendDirectMessage(remotePlayerName.text, message.text);
    }

    public void LeaveChannel()
    {
        LeaveChannel(channelName.text);
    }

    public void ToggleAudioInChannel()
    {
        SetVoiceActiveInChannel(channelName.text, voiceToggle.isOn);
    }

    public void ToggleTextInChannel()
    {
        SetTextActiveInChannel(channelName.text, textToggle.isOn);
    }

    public void AdjustLocalSelfVolume()
    {
        AdjustLocalUserVolume(Mathf.RoundToInt(selfSlider.value));
    }

    public void ToggleMuteLocalPlayer()
    {
        ToggleMuteSelf();
    }

    public void ToggleMuteRemotePlayer()
    {
        ToggleMuteRemoteUser(remotePlayerName.text, channelName.text);
    }

    public void AdjustRemotePlayerVolume()
    {
        AdjustRemoteUserVolume(remotePlayerName.text, channelName.text, Mathf.RoundToInt(remotePlayerSlider.value));
    }

    public void TextToSpeech()
    {
        SpeakTTS(message.text, TTSDestination.QueuedRemoteTransmissionWithLocalPlayback);
    }



    // Login Callbacks

    public override void OnLoggingIn(ILoginSession loginSession)
    {
        base.OnLoggingIn(loginSession);
        newMessage.text += $"\nLogging In {loginSession.LoginSessionId.DisplayName}";
    }

    public override void OnLoggedIn(ILoginSession loginSession)
    {
        base.OnLoggedIn(loginSession);
        newMessage.text += $"\nLogged In {loginSession.LoginSessionId.DisplayName}";
    }

    public override void OnLoggingOut(ILoginSession loginSession)
    {
        base.OnLoggingOut(loginSession);
        newMessage.text += $"\nLogging Out {loginSession.LoginSessionId.DisplayName}";
    }

    public override void OnLoggedOut(ILoginSession loginSession)
    {
        base.OnLoggedOut(loginSession);
        newMessage.text += $"\nLogged Out {loginSession.LoginSessionId.DisplayName}";
    }


    // Channel Callbacks

    public override void OnChannelConnecting(IChannelSession channelSession)
    {
        base.OnChannelConnecting(channelSession);
        newMessage.text += $"\nChannel Connecting in : {channelSession.Channel.Name}";
    }

    public override void OnChannelConnected(IChannelSession channelSession)
    {
        base.OnChannelConnected(channelSession);
        newMessage.text += $"\nChannel Connected in : {channelSession.Channel.Name}";
    }

    public override void OnChannelDisconnecting(IChannelSession channelSession)
    {
        base.OnChannelDisconnecting(channelSession);
        newMessage.text += $"\nChannel Disconnecting in : {channelSession.Channel.Name}";
    }

    public override void OnChannelDisconnected(IChannelSession channelSession)
    {
        base.OnChannelDisconnected(channelSession);
        newMessage.text += $"\nChannel Disconnected in : {channelSession.Channel.Name}";
    }



    // Voice Channel Callbacks

    public override void OnVoiceConnecting(IChannelSession channelSession)
    {
        base.OnVoiceConnecting(channelSession);
        newMessage.text += $"\nVoice Connecting in Channel : {channelSession.Channel.Name}";
    }

    public override void OnVoiceConnected(IChannelSession channelSession)
    {
        base.OnVoiceConnected(channelSession);
        newMessage.text += $"\nVoice Connected in Channel : {channelSession.Channel.Name}";
    }

    public override void OnVoiceDisconnecting(IChannelSession channelSession)
    {
        base.OnVoiceDisconnecting(channelSession);
        newMessage.text += $"\nVoice Disconnecting in Channel : {channelSession.Channel.Name}";
    }

    public override void OnVoiceDisconnected(IChannelSession channelSession)
    {
        base.OnVoiceDisconnected(channelSession);
        newMessage.text += $"\nVoice Disconnected in Channel : {channelSession.Channel.Name}";
    }



    // Text Channels Callback

    public override void OnTextChannelConnected(IChannelSession channelSession)
    {
        base.OnTextChannelConnected(channelSession);
        newMessage.text += $"\nText Connected in Channel : {channelSession.Channel.Name}";
    }

    public override void OnTextChannelConnecting(IChannelSession channelSession)
    {
        base.OnTextChannelConnecting(channelSession);
        newMessage.text += $"\nText Connecting in Channel : {channelSession.Channel.Name}";
    }

    public override void OnTextChannelDisconnecting(IChannelSession channelSession)
    {
        base.OnTextChannelDisconnecting(channelSession);
        newMessage.text += $"\nText Disconnecting in Channel : {channelSession.Channel.Name}";
    }

    public override void OnTextChannelDisconnected(IChannelSession channelSession)
    {
        base.OnTextChannelDisconnected(channelSession);
        newMessage.text += $"\nText Disconnected in Channel : {channelSession.Channel.Name}";
    }



    // Message Callbacks

    public override void OnChannelMessageRecieved(IChannelTextMessage textMessage)
    {
        base.OnChannelMessageRecieved(textMessage);
        newMessage.text += $"\nFrom {textMessage.Sender.DisplayName} : {textMessage.Message}";
    }

    public override void OnDirectMessageRecieved(IDirectedTextMessage directedTextMessage)
    {
        base.OnDirectMessageRecieved(directedTextMessage);
        newMessage.text += $"\nFrom {directedTextMessage.Sender.DisplayName} : {directedTextMessage.Message}";
    }

    public override void OnDirectMessageFailed(IFailedDirectedTextMessage failedMessage)
    {
        base.OnDirectMessageFailed(failedMessage);
        newMessage.text += $"\nMessage failed from {failedMessage.Sender.DisplayName} : Status Code : {failedMessage.StatusCode}";
    }



    // User Callbacks

    public override void OnUserJoinedChannel(IParticipant participant)
    {
        base.OnUserJoinedChannel(participant);
        newMessage.text += $"\n {participant.Account.DisplayName} has joined the channel";
    }

    public override void OnUserLeftChannel(IParticipant participant)
    {
        base.OnUserLeftChannel(participant);
        newMessage.text += $"\n {participant.Account.DisplayName} has left the channel";
    }

    public override void OnUserValuesUpdated(IParticipant participant)
    {
        base.OnUserValuesUpdated(participant);
        // fires everytime user value is updated. Fires way to much but left it here if you need access to it
    }



    // User Audio Callbacks

    public override void OnUserMuted(IParticipant participant)
    {
        base.OnUserMuted(participant);
        newMessage.text += $"\n {participant.Account.DisplayName} has been muted";
    }

    public override void OnUserUnmuted(IParticipant participant)
    {
        base.OnUserUnmuted(participant);
        newMessage.text += $"\n {participant.Account.DisplayName} has been unmuted";
    }

    public override void OnUserNotSpeaking(IParticipant participant)
    {
        base.OnUserNotSpeaking(participant);
        // use to toggle speaking icons
    }

    public override void OnUserSpeaking(IParticipant participant)
    {
        base.OnUserSpeaking(participant);
        // use to toggle speaking icons
    }




    // Text-To-Speech Callbacks

    public override void OnTTSMessageAdded(ITTSMessageQueueEventArgs ttsArgs)
    {
        base.OnTTSMessageAdded(ttsArgs);
        newMessage.text += $"\n Text-To-Speech Message Added : {ttsArgs.Message.Text}";
    }

    public override void OnTTSMessageRemoved(ITTSMessageQueueEventArgs ttsArgs)
    {
        base.OnTTSMessageRemoved(ttsArgs);
        newMessage.text += $"\n Text-To-Speech Message Removed : {ttsArgs.Message.Text}";
    }

    public override void OnTTSMessageUpdated(ITTSMessageQueueEventArgs ttsArgs)
    {
        base.OnTTSMessageUpdated(ttsArgs);
        newMessage.text += $"\n Text-To-Speech Message Updated : {ttsArgs.Message.Text}";
    }


}
