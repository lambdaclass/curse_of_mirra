// <copyright file="SigninSampleScript.cs" company="Google Inc.">
// Copyright (C) 2017 Google Inc. All Rights Reserved.
//
//  Licensed under the Apache License, Version 2.0 (the "License");
//  you may not use this file except in compliance with the License.
//  You may obtain a copy of the License at
//
//  http://www.apache.org/licenses/LICENSE-2.0
//
//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//  limitations

namespace SignInSample
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Google;
    using UnityEngine;
    using UnityEngine.UI;

    public class SigninSampleScript : MonoBehaviour
    {
        public Text statusText;

        public string webClientIdGoogle =
            "529212382177-822ukg0eeufi7pivtk1dpatqveqlqord.apps.googleusercontent.com";

        private GoogleSignInConfiguration configuration;

        // Defer the configuration creation until Awake so the web Client ID
        // Can be set via the property inspector in the Editor.
        void Awake()
        {
            configuration = new GoogleSignInConfiguration
            {
                WebClientId = webClientIdGoogle,
                RequestIdToken = true,
                RequestEmail = true,
                RequestProfile = true
            };
        }

        void Start()
        {
            AddStatusText("Welcome " + PlayerPrefs.GetString("GoogleUserName"));
            SignInWithCachedUser();
        }

        private void SignInWithCachedUser()
        {
            if (PlayerPrefs.GetString("GoogleUserId") != "")
            {
                GoogleSignIn.Configuration = configuration;
                GoogleSignIn.DefaultInstance
                    .SignInSilently()
                    .ContinueWith(
                        OnAuthenticationFinished,
                        TaskScheduler.FromCurrentSynchronizationContext()
                    );
            }
        }

        public void OnSignIn()
        {
            GoogleSignIn.Configuration = configuration;
            GoogleSignIn.Configuration.UseGameSignIn = false;
            GoogleSignIn.Configuration.RequestIdToken = true;
            AddStatusText("Calling SignIn");

            Task<GoogleSignInUser> user = GoogleSignIn.DefaultInstance.SignIn();
            user.ContinueWith(
                OnAuthenticationFinished,
                TaskScheduler.FromCurrentSynchronizationContext()
            );
        }

        public void OnSingOut()
        {
            GoogleSignIn.DefaultInstance.SignOut();
            AddStatusText("SingOut");
            PlayerPrefs.SetString("GoogleUserName", "");
            PlayerPrefs.SetString("GoogleUserId", "");
            PlayerPrefs.SetString("GoogleIdToke", "");
        }

        internal void OnAuthenticationFinished(Task<GoogleSignInUser> task)
        {
            print(task.Status);
            if (task.IsFaulted)
            {
                using (
                    IEnumerator<System.Exception> enumerator =
                        task.Exception.InnerExceptions.GetEnumerator()
                )
                {
                    if (enumerator.MoveNext())
                    {
                        GoogleSignIn.SignInException error = (GoogleSignIn.SignInException)
                            enumerator.Current;
                        AddStatusText("Got Error: " + error.Status + " " + error.Message);
                    }
                    else
                    {
                        AddStatusText("Got Unexpected Exception?!?" + task.Exception);
                    }
                }
            }
            else if (task.IsCanceled)
            {
                AddStatusText("Canceled");
            }
            else
            {
                AddStatusText("Welcome: " + task.Result.DisplayName + "!");
                print(task.Result.Email);
                print("IdtoKEN " + task.Result.IdToken);
                print("USERiD " + task.Result.UserId);
                print("AuthCode " + task.Result.AuthCode);
                if (PlayerPrefs.GetString("GoogleUserId") == "")
                {
                    PlayerPrefs.SetString("GoogleUserName", task.Result.DisplayName);
                    PlayerPrefs.SetString("GoogleUserId", task.Result.UserId);
                    PlayerPrefs.SetString("GoogleIdToke", task.Result.IdToken);
                }
            }
        }

        private List<string> messages = new List<string>();

        void AddStatusText(string text)
        {
            if (messages.Count == 5)
            {
                messages.RemoveAt(0);
            }
            messages.Add(text);
            string txt = "";
            foreach (string s in messages)
            {
                txt += "\n" + s;
            }
            statusText.text = txt;
        }
    }
}
