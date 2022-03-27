using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Web.WebView2.Core;
using System;
using Microsoft.Web.WebView2.Wpf;

namespace WpfHost
{
    /// <summary>
    /// Provide the display for the CardList
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            // subscribe to unload event
            this.Unloaded += CardListDisplayControl_Unloaded;
            InitializeAsync();
        }

        /// <summary>
        /// Handle disposing of WebView2
        /// </summary>
        private void CardListDisplayControl_Unloaded(object sender, RoutedEventArgs e)
        {
            webView.Dispose();
        }

        /// <summary>
        /// Setup WebView2 - waiting for it to initalize
        /// </summary>
        async void InitializeAsync()
        {

            // create envrionment where the UserDataFolder is not in the Program folder
            var env = await CoreWebView2Environment.CreateAsync(null, "c://temp");

             // wait for the view to initialize
             await webView.EnsureCoreWebView2Async(env);

            string userDataFolder2 = webView.CoreWebView2.Environment.UserDataFolder;

            // handle when navigation is complete
            webView.NavigationCompleted += WebView_NavigationCompleted;

            // map a folder to allow WebView2 to navigate to the folder
            webView.CoreWebView2.SetVirtualHostNameToFolderMapping(
                "app.example",
                "jsapp",
                CoreWebView2HostResourceAccessKind.Deny
                );

            // load file web app from mapped folder
            webView.Source = new Uri("http://app.example/index.html");


        }

        /// <summary>
        /// Indicate if the first navigation is complete
        /// </summary>
        private Boolean webViewFirstNavigationCompleted = false;

        /// <summary>
        /// Handle when WebView2 completes navigating
        /// </summary>
        private void WebView_NavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            // upon successful navigation make the webView visible
            if (!webViewFirstNavigationCompleted && e.IsSuccess)
            {
                webViewFirstNavigationCompleted = true;
                webView.Visibility = Visibility.Visible;
            }
            webView.Visibility = Visibility.Visible;
        }

    }
}

