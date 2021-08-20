﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VivoxUnity;

namespace EasyCodeForVivox
{
    public class Easy3DPositional : MonoBehaviour
    {
        [Header("3D Positional Settings")]
        public Transform listenerPosition;
        public Transform speakerPosition;
        private Vector3 lastListenerPosition;
        private Vector3 lastSpeakerPosition;

        private bool positionalChannelExists = false;
        private string channelName;


        private void Start()
        {
            StartCoroutine(Handle3DPositionUpdates(.3f));
        }



        IEnumerator Handle3DPositionUpdates(float nextUpdate)
        {
            yield return new WaitForSeconds(nextUpdate);
            if (EasySession.mainLoginSession.State == LoginState.LoggedIn)
            {
                if (positionalChannelExists)
                {
                    Update3DPosition();
                }
                else
                {
                    CheckIfChannelValid();
                }
            }

            StartCoroutine(Handle3DPositionUpdates(nextUpdate));
        }

        public bool CheckIfChannelValid()
        {
            foreach (KeyValuePair<string, IChannelSession> session in EasySession.mainChannelSessions)
            {
                if (session.Value.Channel.Type == ChannelType.Positional)
                {
                    channelName = session.Value.Channel.Name;
                    if (EasySession.mainChannelSessions[channelName].ChannelState == ConnectionState.Connected)
                    {
                        if (EasySession.mainChannelSessions[channelName].AudioState == ConnectionState.Connected)
                        {
                            positionalChannelExists = true;
                            return true;
                        }
                    }
                }
            }
            return false;
        }


        public void Update3DPosition()
        {
            if (listenerPosition.position != lastListenerPosition || speakerPosition.position != lastSpeakerPosition)
            {
                EasySession.mainChannelSessions[channelName].Set3DPosition(speakerPosition.position, listenerPosition.position, listenerPosition.forward, listenerPosition.up);
                Debug.Log($"{EasySession.mainChannelSessions[channelName].Channel.Name} 3D positon has been updated");
            }
            lastListenerPosition = listenerPosition.position;
            lastSpeakerPosition = speakerPosition.position;
        }
    }
}