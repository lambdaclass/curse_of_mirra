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

        public string webClientId =
            "529212382177-hoe68k1qgi9tki3ejand6r15dg3p4a8g.apps.googleusercontent.com";

        public string webClientId2 =
            "529212382177-822ukg0eeufi7pivtk1dpatqveqlqord.apps.googleusercontent.com";

        private GoogleSignInConfiguration configuration;

        // Defer the configuration creation until Awake so the web Client ID
        // Can be set via the property inspector in the Editor.
        void Awake()
        {
            configuration = new GoogleSignInConfiguration
            {
                WebClientId = webClientId2,
                RequestIdToken = true,
                // ForceTokenRefresh = true,
                RequestEmail = true,
                RequestProfile = true
            };
        }

        public void OnSignIn()
        {
            GoogleSignIn.Configuration = configuration;
            GoogleSignIn.Configuration.UseGameSignIn = false;
            GoogleSignIn.Configuration.RequestIdToken = true;
            AddStatusText("Calling SignIn");

            Task<GoogleSignInUser> user = GoogleSignIn.DefaultInstance.SignIn();
            // print(user.Id);
            // print(user.Result);
            // print(user.Status);
            user.ContinueWith(
                OnAuthenticationFinished,
                TaskScheduler.FromCurrentSynchronizationContext()
            );
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
