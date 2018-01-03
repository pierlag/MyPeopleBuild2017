﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Eyup.Views;
using Eyup.Model;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Eyup.Services;
using Windows.ApplicationModel.Contacts;
using Windows.Foundation.Metadata;
using Windows.ApplicationModel.DataTransfer;

namespace Eyup
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        public static ObservableCollection<AppContact> AppContacts { get; set; }

        bool _isContactsInitialized = false;

        private async Task InitializeAllAsync()
        {
            AppContacts = MyDataService.Current.GetAllContacts();

            await MyContactStoreService.Current.InitializeAsync(AppContacts.ToList(), false);

            _isContactsInitialized = true;
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected async override void OnLaunched(LaunchActivatedEventArgs e)
        {
            if(!_isContactsInitialized)
            {
                await InitializeAllAsync();
            }

#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = false;
            }
#endif
            Frame rootFrame = CreateRootFrame(e);
            
            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    // When the navigation stack isn't restored navigate to the first page,
                    // configuring the new page by passing required information as a navigation
                    // parameter
                    rootFrame.Navigate(typeof(AppMainShell), e.Arguments);
                }
                // Ensure the current window is active
                Window.Current.Activate();
            }
        }

        private Frame CreateRootFrame(LaunchActivatedEventArgs e = null)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e?.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            return rootFrame;
        }

        protected async override void OnActivated(IActivatedEventArgs args)
        {
            if (!_isContactsInitialized)
            {
                await InitializeAllAsync();
            }

            Frame rootFrame = CreateRootFrame();

            switch (args.Kind)
            {
                case ActivationKind.Protocol:
                {
                    if (rootFrame.Content == null)
                    {
                        if (!rootFrame.Navigate(typeof(AppMainShell)))
                        {
                            throw new Exception("Failed to create initial page");
                        }
                    }

                    var appMainShell = rootFrame.Content as AppMainShell;
                    var contactRemoteIds = NavigationHelperService.Current.GetContactRemoteIds(args);
                    var scheme = NavigationHelperService.Current.GetProtocolScheme(args);

                    appMainShell.NavigateToPage(new NavigationParameter { Scheme = scheme, ContactRemoteIds = contactRemoteIds });

                    Window.Current.Activate();
                    break;
                }
                case ActivationKind.ContactPanel:
                {
                    if (rootFrame.Content == null)
                    {
                        if (!rootFrame.Navigate(typeof(AppContactPanelShell)))
                        {
                            throw new Exception("Failed to create initial page");
                        }
                    }

                    var appContactPanelShell = rootFrame.Content as AppContactPanelShell;
                    appContactPanelShell.NavigateToPage(args as ContactPanelActivatedEventArgs);

                    Window.Current.Activate();
                    break;
                }
            }
        }

        protected async override void OnShareTargetActivated(ShareTargetActivatedEventArgs args)
        {
            if (!_isContactsInitialized)
            {
                await InitializeAllAsync();
            }

            bool isPeopleShare = false;
            if (ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 5))
            {
                // Make sure the current OS version includes the My People feature before
                // accessing the ShareOperation.Contacts property
                isPeopleShare = (args.ShareOperation.Contacts.Count > 0);
            }

            if (isPeopleShare)
            {
                // Show share UI for MyPeople contact(s)

                Frame rootFrame = CreateRootFrame();
                AppContact selectedAppContact = (from c in App.AppContacts where c.ContactId == "1" select c).FirstOrDefault();
                rootFrame.Navigate(typeof(AppContactPanelShell));

                rootFrame.Navigate(typeof(ChatPage), selectedAppContact);

                if (args.ShareOperation.Data.Contains(StandardDataFormats.WebLink))
                {
                    try
                    {
                        Uri sharedWebLink = await args.ShareOperation.Data.GetWebLinkAsync();
                        ((ChatPage)rootFrame.Content).AddChat(args.ShareOperation.Data.Properties.Title);
                        ((ChatPage)rootFrame.Content).AddChat(sharedWebLink.AbsoluteUri);

                    }
                    catch
                    {
                        
                    }

                }

                
                //AppContact.ChatHistory.Add(ChatTextBox.Text);
            }
            else
            {
                // Show standard share UI for unpinned contacts
            }

        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }
    }
}
