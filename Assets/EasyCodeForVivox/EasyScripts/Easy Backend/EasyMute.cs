using System;
using System.Collections.Generic;
using UnityEngine;
using VivoxUnity;

namespace EasyCodeForVivox
{
    public class EasyMute
    {
        public static event Action<bool> UserMuted;
        public static event Action<bool> UserUnmuted;

        private void OnUserMuted(bool isMuted)
        {
            UserMuted?.Invoke(isMuted);
        }

        private void OnUserUnmuted(bool isMuted)
        {
            UserUnmuted?.Invoke(isMuted);
        }

        public void LocalToggleMuteRemoteUser(string userName, IChannelSession channelSession)
        {
            var participants = channelSession.Participants;
            string userToMute = EasySIP.GetUserSIP(EasySession.Issuer, userName, EasySession.Domain);
            Debug.Log($"Sip address - {userToMute}");
            if (participants[userToMute].InAudio && !participants[userToMute].IsSelf)
            {
                if (participants[userToMute].LocalMute)
                {
                    participants[userToMute].LocalMute = false;
                }
                else
                {
                    participants[userToMute].LocalMute = true;
                }
            }
            else
            {
                Debug.Log($"Failed to mute {participants[userToMute].Account.DisplayName}");
            }
        }

       


        public void LocalToggleMuteSelf(VivoxUnity.Client client, bool mute)
        {
            // cant localmute self
            // mutes local users microphone across all connected channels
            if (mute)
            {
                client.AudioInputDevices.Muted = true;
                OnUserMuted(mute);
            }
            else
            {
                client.AudioInputDevices.Muted = false;
                OnUserUnmuted(mute);
            }
        }

        public void CrossMuteUser(ILoginSession loginSession, bool mute)
        {
            if (mute)
            {
                Debug.Log($"Muting {loginSession.LoginSessionId.DisplayName}");
            }
            else
            {
                Debug.Log($"Unmuting {loginSession.LoginSessionId.DisplayName}");
            }
            // todo check if it works
            loginSession.SetCrossMutedCommunications(loginSession.LoginSessionId, mute, CrossMuteResult);
        }

        public void CrossMuteUsers(ILoginSession loginSessionToMute, bool mute)
        {
            List<AccountId> accountIds = new List<AccountId>();
            loginSessionToMute.SetCrossMutedCommunications(accountIds, mute, CrossMuteResult);
            // todo check if this actually works
            // add this callback listener
            // loginSessionToMute.CrossMutedCommunications.AfterKeyAdded +=
        }

        public void ClearAllCurrentCrossMutedAccounts(ILoginSession loginSession)
        {
            loginSession.ClearCrossMutedCommunications(CrossMuteResult);
        }

        public void CrossMuteResult(IAsyncResult ar)
        {
            try
            {
                Debug.Log("Successful");
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
                return;
            }
        }
    }
}