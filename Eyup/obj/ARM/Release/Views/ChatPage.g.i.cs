﻿#pragma checksum "C:\Projects\Eyup\Eyup\Views\ChatPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "612CE010961231A11D9AE6250B8003A3"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Eyup.Views
{
    partial class ChatPage : global::Windows.UI.Xaml.Controls.Page
    {


        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        private global::Windows.UI.Xaml.Shapes.Ellipse AvatarEllipse; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        private global::Windows.UI.Xaml.Controls.ListView ChatHistoryListView; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        private global::Windows.UI.Xaml.Controls.TextBox ChatTextBox; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        private bool _contentLoaded;

        /// <summary>
        /// InitializeComponent()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent()
        {
            if (_contentLoaded)
                return;

            _contentLoaded = true;

            global::System.Uri resourceLocator = new global::System.Uri("ms-appx:///Views/ChatPage.xaml");
            global::Windows.UI.Xaml.Application.LoadComponent(this, resourceLocator, global::Windows.UI.Xaml.Controls.Primitives.ComponentResourceLocation.Application);
        }

        
        private interface IChatPage_Bindings
        {
            void Initialize();
            void Update();
            void StopTracking();
        }
#pragma warning disable 0169    //  Proactively suppress unused field warning in case Bindings is not used.
        private IChatPage_Bindings Bindings;
#pragma warning restore 0169
    }
}


