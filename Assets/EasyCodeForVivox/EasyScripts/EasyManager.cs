using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using VivoxUnity;

namespace EasyCodeForVivox
{
    public class EasyManager : MonoBehaviour
    {

        // guarantees to only Initialize client once
        public void InitializeClient()
        {
            // disable Debug.Log Statements in the build for better performance
#if UNITY_EDITOR
            Debug.unityLogger.logEnabled = true;
#else
                        Debug.unityLogger.logEnabled = false;
#endif

            if (EasySession.isClientInitialized)
            {
                Debug.Log($"{nameof(EasyManager)} : Vivox Client is already initialized, skipping...");
                return;
            }
            else
            {
                if (!EasySession.mainClient.Initialized)
                {
                    EasySession.mainClient.Uninitialize();
                    EasySession.mainClient.Initialize();
                    EasySession.isClientInitialized = true;
                    SubscribeToVivoxEvents();
                    Debug.Log("Vivox Client Initialzed");
                }
            }
        }


        public void UnitializeClient()
        {
            UnsubscribeToVivoxEvents();
            EasySession.mainClient.Uninitialize();
        }


        public void SubscribeToVivoxEvents()
        {
            EasyLogin.LoggingIn += OnLoggingIn;
            EasyLogin.LoggedIn += OnLoggedIn;
            EasyLogin.LoggedIn += OnLoggedInSetup;
            EasyLogin.LoggingOut += OnLoggingOut;
            EasyLogin.LoggedOut += OnLoggedOut;

            EasyChannel.ChannelConnecting += OnChannelConnecting;
            EasyChannel.ChannelConnected += OnChannelConnected;
            EasyChannel.ChannelDisconnecting += OnChannelDisconnecting;
            EasyChannel.ChannelDisconnected += OnChannelDisconnected;

            EasyVoiceChannel.VoiceChannelConnecting += OnVoiceConnecting;
            EasyVoiceChannel.VoiceChannelConnected += OnVoiceConnected;
            EasyVoiceChannel.VoiceChannelDisconnecting += OnVoiceDisconnecting;
            EasyVoiceChannel.VoiceChannelDisconnected += OnVoiceDisconnected;

            EasyTextChannel.TextChannelConnecting += OnTextChannelConnecting;
            EasyTextChannel.TextChannelConnected += OnTextChannelConnected;
            EasyTextChannel.TextChannelDisconnecting += OnTextChannelDisconnecting;
            EasyTextChannel.TextChannelDisconnected += OnTextChannelDisconnected;

            EasyMessages.ChannelMessageRecieved += OnChannelMessageRecieved;

            EasyMessages.DirectMessageRecieved += OnDirectMessageRecieved;
            EasyMessages.DirectMessageFailed += OnDirectMessageFailed;

            EasyUsers.UserJoinedChannel += OnUserJoinedChannel;
            EasyUsers.UserLeftChannel += OnUserLeftChannel;
            EasyUsers.UserValuesUpdated += OnUserValuesUpdated;

            EasyUsers.UserMuted += OnUserMuted;
            EasyUsers.UserUnmuted += OnUserUnmuted;
            EasyUsers.UserSpeaking += OnUserSpeaking;
            EasyUsers.UserNotSpeaking += OnUserNotSpeaking;

            EasyTextToSpeech.TTSMessageAdded += OnTTSMessageAdded;
            EasyTextToSpeech.TTSMessageRemoved += OnTTSMessageRemoved;
            EasyTextToSpeech.TTSMessageUpdated += OnTTSMessageUpdated;

            EasySubscriptions.SubscriptionAddAllowed += OnAddAllowedSubscription;
            EasySubscriptions.SubscriptionAddBlocked += OnAddBlockedSubscription;
            EasySubscriptions.SubscriptionAddPresence += OnAddPresenceSubscription;

            EasySubscriptions.SubscriptionRemoveAllowed += OnRemoveAllowedSubscription;
            EasySubscriptions.SubscriptionRemoveBlocked += OnRemoveBlockedSubscription;
            EasySubscriptions.SubscriptionRemovePresence += OnRemovePresenceSubscription;

            EasySubscriptions.SubscriptionUpdatePresence += OnUpdatePresenceSubscription;

        }

        public void UnsubscribeToVivoxEvents()
        {
            EasyLogin.LoggingIn -= OnLoggingIn;
            EasyLogin.LoggedIn -= OnLoggedIn;
            EasyLogin.LoggedIn -= OnLoggedInSetup;
            EasyLogin.LoggingOut -= OnLoggingOut;
            EasyLogin.LoggedOut -= OnLoggedOut;

            EasyChannel.ChannelConnecting -= OnChannelConnecting;
            EasyChannel.ChannelConnected -= OnChannelConnected;
            EasyChannel.ChannelDisconnecting -= OnChannelDisconnecting;
            EasyChannel.ChannelDisconnected -= OnChannelDisconnected;

            EasyVoiceChannel.VoiceChannelConnecting -= OnVoiceConnecting;
            EasyVoiceChannel.VoiceChannelConnected -= OnVoiceConnected;
            EasyVoiceChannel.VoiceChannelDisconnecting -= OnVoiceDisconnecting;
            EasyVoiceChannel.VoiceChannelDisconnected -= OnVoiceDisconnected;

            EasyTextChannel.TextChannelConnecting -= OnTextChannelConnecting;
            EasyTextChannel.TextChannelConnected -= OnTextChannelConnected;
            EasyTextChannel.TextChannelDisconnecting -= OnTextChannelDisconnecting;
            EasyTextChannel.TextChannelDisconnected -= OnTextChannelDisconnected;

            EasyMessages.ChannelMessageRecieved -= OnChannelMessageRecieved;

            EasyMessages.DirectMessageRecieved -= OnDirectMessageRecieved;
            EasyMessages.DirectMessageFailed -= OnDirectMessageFailed;

            EasyUsers.UserJoinedChannel -= OnUserJoinedChannel;
            EasyUsers.UserLeftChannel -= OnUserLeftChannel;
            EasyUsers.UserValuesUpdated -= OnUserValuesUpdated;

            EasyUsers.UserMuted -= OnUserMuted;
            EasyUsers.UserUnmuted -= OnUserUnmuted;
            EasyUsers.UserSpeaking -= OnUserSpeaking;
            EasyUsers.UserNotSpeaking -= OnUserNotSpeaking;

            EasyTextToSpeech.TTSMessageAdded -= OnTTSMessageAdded;
            EasyTextToSpeech.TTSMessageRemoved -= OnTTSMessageRemoved;
            EasyTextToSpeech.TTSMessageUpdated -= OnTTSMessageUpdated;

            EasySubscriptions.SubscriptionAddAllowed -= OnAddAllowedSubscription;
            EasySubscriptions.SubscriptionAddBlocked -= OnAddBlockedSubscription;
            EasySubscriptions.SubscriptionAddPresence -= OnAddPresenceSubscription;

            EasySubscriptions.SubscriptionRemoveAllowed -= OnRemoveAllowedSubscription;
            EasySubscriptions.SubscriptionRemoveBlocked -= OnRemoveBlockedSubscription;
            EasySubscriptions.SubscriptionRemovePresence -= OnRemovePresenceSubscription;

            EasySubscriptions.SubscriptionUpdatePresence -= OnUpdatePresenceSubscription;

        }


#region Vivox Backend Functionality Classes, Enums


        public enum VoiceGender { male, female }

        private EasyLogin login = new EasyLogin();
        private EasyChannel channel = new EasyChannel();
        private EasyVoiceChannel channelVoice = new EasyVoiceChannel();
        private EasyTextChannel channelText = new EasyTextChannel();
        private EasyUsers users = new EasyUsers();
        private EasyMessages messages = new EasyMessages();
        private EasyAudioSettings audioSettings = new EasyAudioSettings();
        private EasyMute mute = new EasyMute();
        private EasySubscriptions subscriptions = new EasySubscriptions();
        private EasyTextToSpeech textToSpeech = new EasyTextToSpeech();


#endregion



#region Main Methods


        public void RequestAndroidMicPermission()
        {
#if PLATFORM_ANDROID
            if (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
            {
                Permission.RequestUserPermission(Permission.Microphone);
            }
#endif
        }


        //public void RequestIOSMicrophoneAccess()
        //{
        //    // Refer to Vivox Documentation on how to implement this method. Currently a work in progress.NOT SURE IF IT WORKS
        //    // make sure you change the info list refer to Vivox documentation for this to work
        //    // Make sure NSCameraUsageDescription and NSMicrophoneUsageDescription
        //    // are in the Info.plist.
        //    Application.RequestUserAuthorization(UserAuthorization.Microphone);
        //}

        private bool FilterChannelAndUserName(string nameToFilter)
        {
            char[] allowedChars = new char[] { '0','1','2','3', '4', '5', '6', '7', '8', '9',
        'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n','o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z',
        'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I','J', 'K', 'L', 'M', 'N', 'O', 'P','Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z',
        '!', '(', ')', '+','-', '.', '=', '_', '~'};

            List<char> allowed = new List<char>(allowedChars);
            foreach (char c in nameToFilter)
            {
                if (!allowed.Contains(c))
                {
                    Debug.Log($"Can't join channel, Channel name has invalid character '{c}'");
                    return false;
                }
            }
            return true;
        }

        private IChannelSession GetExistingChannelSession(string channelName)
        {
            if (EasySession.mainChannelSessions[channelName].ChannelState == ConnectionState.Disconnected || EasySession.mainChannelSessions[channelName] == null)
            {
                EasySession.mainChannelSessions[channelName] = EasySession.mainLoginSession.GetChannelSession(new ChannelId(GetChannelSIP(channelName)));
            }

            return EasySession.mainChannelSessions[channelName];
        }

        private IChannelSession CreateNewChannel(string channelName, ChannelType channelType)
        {
            if (channelType == ChannelType.Positional)
            {
                foreach (KeyValuePair<string, IChannelSession> channel in EasySession.mainChannelSessions)
                {
                    if (channel.Value.Channel.Type == ChannelType.Positional)
                    {
                        Debug.Log($"{channel.Value.Channel.Name} Is already a Positional Channel. Can Only Have One 3D Positional Channel. Refer To Vivox Documentation : Returning Null");
                        return null;
                    }
                }

                EasySession.mainChannelSessions.Add(channelName, EasySession.mainLoginSession.GetChannelSession(new ChannelId(GetChannelSIP(channelType, channelName))));
            }
            else
            {
                EasySession.mainChannelSessions.Add(channelName, EasySession.mainLoginSession.GetChannelSession(new ChannelId(GetChannelSIP(channelType, channelName))));
            }

            return EasySession.mainChannelSessions[channelName];
        }

        private string GetChannelSIP(ChannelType channelType, string channelName)
        {
            switch (channelType)
            {
                case ChannelType.NonPositional:
                    return EasySIP.GetChannelSip(ChannelType.NonPositional, EasySession.Issuer, channelName, EasySession.Domain);

                case ChannelType.Echo:
                    return EasySIP.GetChannelSip(ChannelType.NonPositional, EasySession.Issuer, channelName, EasySession.Domain);

                case ChannelType.Positional:
                    return EasySIP.GetChannelSip(ChannelType.Positional, EasySession.Issuer, channelName, EasySession.Domain, new Channel3DProperties());

            }
            return EasySIP.GetChannelSip(ChannelType.NonPositional, EasySession.Issuer, channelName, EasySession.Domain);
        }

        private string GetChannelSIP(string channelName)
        {
            string result = "";
            foreach (var session in EasySession.mainChannelSessions)
            {
                if (session.Value.Channel.Name == channelName)
                {
                    result = GetChannelSIP(session.Value.Channel.Type, channelName);
                    return result;
                }
            }
            return result;
        }


        public void RemoveChannelSession(string channelName)
        {
            if (EasySession.mainChannelSessions.ContainsKey(channelName))
            {
                EasySession.mainChannelSessions.Remove(channelName);
            }
        }



        public void Login(string userName)
        {
            try
            {
                if (!FilterChannelAndUserName(userName)) { return; }

                EasySession.mainLoginSession = EasySession.mainClient.GetLoginSession(new AccountId(EasySession.Issuer, userName, EasySession.Domain));
                messages.SubscribeToDirectMessages(EasySession.mainLoginSession);
                textToSpeech.Subscribe(EasySession.mainLoginSession);
                subscriptions.Subscribe(EasySession.mainLoginSession);

                login.LoginToVivox(EasySession.mainLoginSession, EasySession.APIEndpoint, userName);
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
                Debug.Log(e.StackTrace);
                subscriptions.Unsubscribe(EasySession.mainLoginSession);
                messages.UnsubscribeFromDirectMessages(EasySession.mainLoginSession);
                textToSpeech.Unsubscribe(EasySession.mainLoginSession);

            }
        }


        public void Logout()
        {
            if (EasySession.mainLoginSession.State == LoginState.LoggedIn)
            {
                subscriptions.Unsubscribe(EasySession.mainLoginSession);
                messages.UnsubscribeFromDirectMessages(EasySession.mainLoginSession);
                textToSpeech.Unsubscribe(EasySession.mainLoginSession);
                login.Logout(EasySession.mainLoginSession);
            }
            else
            {
                Debug.Log($"Not logged in");
            }
        }


        public void JoinChannel(string channelName, bool includeVoice, bool includeText, bool switchTransmissionToThisChannel, ChannelType channelType, bool joinMuted = false)
        {
            if (!FilterChannelAndUserName(channelName)) { return; }
            IChannelSession channelSession = CreateNewChannel(channelName, channelType);

            try
            {
                channelText.Subscribe(channelSession);
                channelVoice.Subscribe(channelSession);
                users.SubscribeToParticipants(channelSession);
                messages.SubscribeToChannelMessages(channelSession);

                channel.JoinChannel(includeVoice, includeText, switchTransmissionToThisChannel, channelSession, joinMuted);
            }
            catch (Exception e)
            {
                Debug.Log(e.StackTrace);
                channelText.Unsubscribe(channelSession);
                channelVoice.Unsubscribe(channelSession);
                users.UnsubscribeFromParticipants(channelSession);
                messages.UnsubscribeFromChannelMessages(channelSession);
            }
        }

        public void LeaveChannel(string channelName)
        {
            if (EasySession.mainChannelSessions.ContainsKey(channelName))
            {
                users.UnsubscribeFromParticipants(EasySession.mainChannelSessions[channelName]);
                messages.UnsubscribeFromChannelMessages(EasySession.mainChannelSessions[channelName]);
                channel.LeaveChannel(EasySession.mainLoginSession, EasySession.mainChannelSessions[channelName]);
            }
        }

        public void SetVoiceActiveInChannel(string channelName, bool connect)
        {
            // todo fix error where channel disconects if both text and voice are disconnected and when you try and toggle 
            // you get an object null refenrce because channel name exists but channelsession doesnt exist
            IChannelSession channelSession = EasySession.mainLoginSession.GetChannelSession(new ChannelId(GetChannelSIP(channelName)));
            channelVoice.ToggleAudioChannelActive(channelSession, connect);
        }

        public void SetTextActiveInChannel(string channelName, bool connect)
        {
            // todo fix error where channel disconects if both text and voice are disconnected and when you try and toggle 
            // you get an object null refenrce because channel name exists but channelsession doesnt exist
            IChannelSession channelSession = EasySession.mainLoginSession.GetChannelSession(new ChannelId(GetChannelSIP(channelName)));
            channelText.ToggleTextChannelActive(channelSession, connect);
        }
        public void SendChannelMessage(string channelName, string msg, string title = "", string body = "")
        {
            messages.SendChannelMessage(GetExistingChannelSession(channelName),
                msg, title, body);
        }

        public void SendDirectMessage(string userToMsg, string msg, string title = "", string body = "")
        {
            // todo check if user is blocked and alert front end users
            messages.SendDirectMessage(EasySession.mainLoginSession, userToMsg, msg, title, body);
        }

        public void ToggleMuteSelf()
        {
            if (EasySession.mainClient.AudioInputDevices.Muted)
            {
                mute.LocalToggleMuteSelf(EasySession.mainClient, true);
            }
            else
            {
                mute.LocalToggleMuteSelf(EasySession.mainClient, false);
            }
        }

        public void ToggleMuteRemoteUser(string userName, string channelName)
        {
            mute.LocalToggleMuteRemoteUser(userName, GetExistingChannelSession(channelName));
        }

        public void AdjustLocalUserVolume(int volume)
        {
            audioSettings.AudioAdjustLocalPlayerVolume(volume, EasySession.mainClient);
        }

        public void AdjustRemoteUserVolume(string userName, string channelName, float volume)
        {
            IChannelSession channelSession = GetExistingChannelSession(channelName);
            audioSettings.AudioAdjustRemotePlayerVolume(userName, channelSession, volume);
        }

        public void AddFriend(string userName)
        {
            subscriptions.AddAllowPresence(EasySIP.GetUserSIP(EasySession.Issuer, userName, EasySession.Domain), EasySession.mainLoginSession);
        }

        public void RemoveFriend(string userName)
        {
            subscriptions.RemoveAllowedPresence(EasySIP.GetUserSIP(EasySession.Issuer, userName, EasySession.Domain), EasySession.mainLoginSession);
        }

        public void AddAllowedUser(string userName)
        {
            subscriptions.AddAllowedSubscription(EasySIP.GetUserSIP(EasySession.Issuer, userName, EasySession.Domain), EasySession.mainLoginSession);
        }

        public void RemoveAllowedUser(string userName)
        {
            subscriptions.RemoveAllowedSubscription(EasySIP.GetUserSIP(EasySession.Issuer, userName, EasySession.Domain), EasySession.mainLoginSession);
        }

        public void BlockUser(string userName)
        {
            subscriptions.AddBlockedSubscription(EasySIP.GetUserSIP(EasySession.Issuer, userName, EasySession.Domain), EasySession.mainLoginSession);
        }

        public void RemoveBlockedUser(string userName)
        {
            subscriptions.RemoveBlockedSubscription(EasySIP.GetUserSIP(EasySession.Issuer, userName, EasySession.Domain), EasySession.mainLoginSession);
        }

        public void SpeakTTS(string msg)
        {
            textToSpeech.TTSSpeak(msg, TTSDestination.QueuedLocalPlayback, EasySession.mainLoginSession);
        }

        public void SpeakTTS(string msg, TTSDestination playMode)
        {
            textToSpeech.TTSSpeak(msg, playMode, EasySession.mainLoginSession);
        }

        public void ChooseVoiceGender(VoiceGender voiceGender)
        {
            switch (voiceGender)
            {
                case VoiceGender.male:
                    textToSpeech.TTSChooseVoice(textToSpeech.maleVoice, EasySession.mainLoginSession);
                    break;

                case VoiceGender.female:
                    textToSpeech.TTSChooseVoice(textToSpeech.femaleVoice, EasySession.mainLoginSession);
                    break;
            }
        }



#endregion

        private void OnLoggedInSetup(ILoginSession loginSession)
        {
            RequestAndroidMicPermission();
            ChooseVoiceGender(VoiceGender.female);
        }



#region Login / Logout Callbacks


        public virtual void OnLoggingIn(ILoginSession loginSession)
        {
            Debug.Log($"Logging In : {loginSession.LoginSessionId.DisplayName}");
        }

        public virtual void OnLoggedIn(ILoginSession loginSession)
        {
            Debug.Log($"Logged in : {loginSession.LoginSessionId.DisplayName}  : Presence = {loginSession.Presence.Status}");
        }

        public virtual void OnLoggingOut(ILoginSession loginSession)
        {
            Debug.Log($"Logging out : {loginSession.LoginSessionId.DisplayName}  : Presence = {loginSession.Presence.Status}");
        }

        public virtual void OnLoggedOut(ILoginSession loginSession)
        {
            Debug.Log($"Logged out : {loginSession.LoginSessionId.DisplayName}  : Presence = {loginSession.Presence.Status}");
        }



        public virtual void OnLoginAdded(ILoginSession loginSession)
        {
            Debug.Log($"Login Added : {loginSession.LoginSessionId.DisplayName}  : Presence = {loginSession.Presence.Status}");
        }

        public virtual void OnLoginRemoved(ILoginSession loginSession)
        {
            Debug.Log($"Login Removed : {loginSession.LoginSessionId.DisplayName} : Presence = {loginSession.Presence.Status}");
        }

        public virtual void OnLoginUpdated(ILoginSession loginSession)
        {
            Debug.Log($"Login Updated : Login Updated : {loginSession.LoginSessionId.DisplayName} : Presence = {loginSession.Presence.Status}");
        }


#endregion


#region Audio / Text / Channel Callbacks


        public virtual void OnChannelConnecting(IChannelSession channelSession)
        {
            Debug.Log($"{channelSession.Channel.Name} Is Connecting");
        }

        public virtual void OnChannelConnected(IChannelSession channelSession)
        {
            Debug.Log($"{channelSession.Channel.Name} Has Connected");
            Debug.Log($"Channel Type == {channelSession.Channel.Type.ToString()}");
        }

        public virtual void OnChannelDisconnecting(IChannelSession channelSession)
        {
            Debug.Log($"{channelSession.Channel.Name} Is Disconnecting");
        }

        public virtual void OnChannelDisconnected(IChannelSession channelSession)
        {
            Debug.Log($"{channelSession.Channel.Name} Has Disconnected");
            RemoveChannelSession(channelSession.Channel.Name);
        }




        public virtual void OnVoiceConnecting(IChannelSession channelSession)
        {
            Debug.Log($"{channelSession.Channel.Name} Audio Is Connecting In Channel");
        }

        public virtual void OnVoiceConnected(IChannelSession channelSession)
        {
            Debug.Log($"{channelSession.Channel.Name} Audio Has Connected In Channel");
        }

        public virtual void OnVoiceDisconnecting(IChannelSession channelSession)
        {
            Debug.Log($"{channelSession.Channel.Name} Audio Is Disconnecting In Channel");
        }

        public virtual void OnVoiceDisconnected(IChannelSession channelSession)
        {
            Debug.Log($"{channelSession.Channel.Name} Audio Has Disconnected In Channel");
        }




        public virtual void OnTextChannelConnecting(IChannelSession channelSession)
        {
            Debug.Log($"{channelSession.Channel.Name} Text Is Connecting In Channel");
        }

        public virtual void OnTextChannelConnected(IChannelSession channelSession)
        {
            Debug.Log($"{channelSession.Channel.Name} Text Has Connected In Channel");
        }

        public virtual void OnTextChannelDisconnecting(IChannelSession channelSession)
        {
            Debug.Log($"{channelSession.Channel.Name} Text Is Disconnecting In Channel");
        }

        public virtual void OnTextChannelDisconnected(IChannelSession channelSession)
        {
            Debug.Log($"{channelSession.Channel.Name} Text Has Disconnected In Channel");
        }



#endregion


#region User Callbacks


        public virtual void OnUserJoinedChannel(IParticipant participant)
        {
            Debug.Log($"{participant.Account.DisplayName} Has Joined The Channel");
        }

        public virtual void OnUserLeftChannel(IParticipant participant)
        {
            Debug.Log($"{participant.Account.DisplayName} Has Left The Channel");

        }

        public virtual void OnUserValuesUpdated(IParticipant participant)
        {
            Debug.Log($"{participant.Account.DisplayName} Has updated itself in the channel");

        }

        public virtual void OnUserMuted(IParticipant participant)
        {
            // todo add option if statement to display debug messages
            Debug.Log($"{participant.Account.DisplayName} Is Muted : (Muted For All : {participant.IsMutedForAll})");

        }

        public virtual void OnUserUnmuted(IParticipant participant)
        {
            Debug.Log($"{participant.Account.DisplayName} Is Unmuted : (Muted For All : {participant.IsMutedForAll})");

        }

        public virtual void OnUserSpeaking(IParticipant participant)
        {
            Debug.Log($"{participant.Account.DisplayName} Is Speaking : Audio Energy {participant.AudioEnergy.ToString()}");

        }

        public virtual void OnUserNotSpeaking(IParticipant participant)
        {
            Debug.Log($"{participant.Account.DisplayName} Is Not Speaking");
        }


#endregion


#region Message Callbacks


        public virtual void OnChannelMessageRecieved(IChannelTextMessage textMessage)
        {
            Debug.Log($"From {textMessage.Sender.DisplayName} : {textMessage.ReceivedTime} : {textMessage.Message}");
        }

        public virtual void OnDirectMessageRecieved(IDirectedTextMessage directedTextMessage)
        {
            Debug.Log($"Recived Message From : {directedTextMessage.Sender.DisplayName} : {directedTextMessage.ReceivedTime} : {directedTextMessage.Message}");
        }

        public virtual void OnDirectMessageFailed(IFailedDirectedTextMessage failedMessage)
        {
            Debug.Log($"Failed To Send Message From : {failedMessage.Sender}");
        }


    

#endregion


#region Text-to-Speech Callbacks

        public virtual void OnTTSMessageAdded(ITTSMessageQueueEventArgs ttsArgs)
        {
            Debug.Log($"TTS Message Has Been Added : {ttsArgs.Message.Text}");
        }

        public virtual void OnTTSMessageRemoved(ITTSMessageQueueEventArgs ttsArgs)
        {
            Debug.Log($"TTS Message Has Been Removed : {ttsArgs.Message.Text}");
        }

        public virtual void OnTTSMessageUpdated(ITTSMessageQueueEventArgs ttsArgs)
        {
            Debug.Log($"TTS Message Has Been Updated : {ttsArgs.Message.Text}");
        }


#endregion




#region Work In Progress


#region Subscription / Presence Callbacks

        public virtual void OnAddAllowedSubscription(AccountId accountId)
        {
            Debug.Log($"{accountId.DisplayName} User Has Been Allowed Has Been Added");
        }

        public virtual void OnRemoveAllowedSubscription(AccountId accountId)
        {
            Debug.Log($"{accountId.DisplayName} User Has Been Allowed Has Been Removed");
        }

        public virtual void OnAddBlockedSubscription(AccountId accountId)
        {
            Debug.Log($"{accountId.DisplayName} Block On User Has Been Added");
        }

        public virtual void OnRemoveBlockedSubscription(AccountId accountId)
        {
            Debug.Log($"{accountId.DisplayName} Block On User Has Been Removed");
        }

        public virtual void OnAddPresenceSubscription(AccountId accountId)
        {
            Debug.Log($"{accountId.DisplayName} Presence Has Been Added");
        }

        public virtual void OnRemovePresenceSubscription(AccountId accountId)
        {
            Debug.Log($"{accountId.DisplayName} Presence Has Been Removed");
        }

        public virtual void OnUpdatePresenceSubscription(ValueEventArg<AccountId, IPresenceSubscription> presence)
        {
            Debug.Log($"{presence.Value.Key.DisplayName} Presence Has Been Updated");
        }

#endregion



#endregion


    }
}