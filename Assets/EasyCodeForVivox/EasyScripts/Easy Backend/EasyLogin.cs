using System;
using System.ComponentModel;
using VivoxUnity;
using UnityEngine;
using VivoxAccessToken;


namespace EasyCodeForVivox
{
    public class EasyLogin
    {
        public static event Action<ILoginSession> LoggingIn;
        public static event Action<ILoginSession> LoggedIn;
        public static event Action<ILoginSession> LoggedOut;
        public static event Action<ILoginSession> LoggingOut;


        public void Subscribe(ILoginSession loginSession)
        {
            loginSession.PropertyChanged += OnLoginPropertyChanged;
        }

        public void Unsubscribe(ILoginSession loginSession)
        {
            loginSession.PropertyChanged -= OnLoginPropertyChanged;
        }



        #region Login Events


        public void OnLoggingIn(ILoginSession loginSession)
        {
            if (loginSession != null)
            {
                LoggingIn?.Invoke(loginSession);
            }
        }

        public void OnLoggedIn(ILoginSession loginSession)
        {
            if (loginSession != null)
            {
                LoggedIn?.Invoke(loginSession);
            }
        }


        public void OnLoggingOut(ILoginSession loginSession)
        {
            if (loginSession != null)
            {
                LoggingOut?.Invoke(loginSession);
            }

        }

        public void OnLoggedOut(ILoginSession loginSession)
        {
            if (loginSession != null)
            {
                LoggedOut?.Invoke(loginSession);

                Unsubscribe(loginSession);
            }
        }


        #endregion


        #region Login Methods


        public void LoginToVivox(ILoginSession loginSession,
            Uri serverUri, string userName)
        {
            Subscribe(loginSession);
           var accessToken = AccessToken.Token_f(EasySession.SecretKey, EasySession.Issuer, 
               AccessToken.SecondsSinceUnixEpochPlusDuration(TimeSpan.FromSeconds(90)) , "login", EasySession.UniqueCounter, null, EasySIP.GetUserSIP(
                   EasySession.Issuer, userName, EasySession.Domain), null);
          

            loginSession.BeginLogin(serverUri, accessToken, ar =>
            {
                try
                {
                    loginSession.EndLogin(ar);
                }
                catch (Exception e)
                {
                    Unsubscribe(loginSession);
                    Debug.Log(e.StackTrace);
                }
            });
        }


        public void Logout(ILoginSession loginSession)
        {
            OnLoggingOut(loginSession);
            loginSession.Logout();
            OnLoggedOut(loginSession);
            Debug.Log($"Logging Out... Vivox does not have a Logging Out Event Callback. The events LoggingOut and LoggedOut are custom callback events. " +
                $"They will be called before and after the Logout method is called");
        }

        #endregion


        #region Login Callbacks

        // login status changed
        private void OnLoginPropertyChanged(object sender, PropertyChangedEventArgs propArgs)
        {
            var senderLoginSession = (ILoginSession)sender;

            if (propArgs.PropertyName == "State")
            {
                switch (senderLoginSession.State)
                {
                    case LoginState.LoggingIn:
                        OnLoggingIn(senderLoginSession);
                        break;
                    case LoginState.LoggedIn:
                        OnLoggedIn(senderLoginSession);
                        break;
                    case LoginState.LoggingOut:
                        OnLoggingOut(senderLoginSession);
                        break;
                    case LoginState.LoggedOut:
                        OnLoggedOut(senderLoginSession);
                        break;

                    default:
                        Debug.Log("Logging Callback Error - Logging In/Out failed");
                        break;
                }
            }
        }


        #endregion
    }
}