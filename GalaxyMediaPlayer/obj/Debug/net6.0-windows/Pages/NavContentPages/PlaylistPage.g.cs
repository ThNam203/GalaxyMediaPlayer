﻿#pragma checksum "..\..\..\..\..\Pages\NavContentPages\PlaylistPage.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "3111E426C4E9F3E5E8F3BA04054F2ACEE3B69F22"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using GalaxyMediaPlayer.Converters;
using GalaxyMediaPlayer.CustomControls.VirtualizingWrapPanel;
using GalaxyMediaPlayer.Pages.NavContentPages;
using Microsoft.Windows.Themes;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.Integration;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace GalaxyMediaPlayer.Pages.NavContentPages {
    
    
    /// <summary>
    /// PlaylistPage
    /// </summary>
    public partial class PlaylistPage : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector, System.Windows.Markup.IStyleConnector {
        
        
        #line 404 "..\..\..\..\..\Pages\NavContentPages\PlaylistPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button BackBtn;
        
        #line default
        #line hidden
        
        
        #line 411 "..\..\..\..\..\Pages\NavContentPages\PlaylistPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock playlistNameHeader;
        
        #line default
        #line hidden
        
        
        #line 442 "..\..\..\..\..\Pages\NavContentPages\PlaylistPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button newPlaylistBtn;
        
        #line default
        #line hidden
        
        
        #line 463 "..\..\..\..\..\Pages\NavContentPages\PlaylistPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox cbSortByOptions;
        
        #line default
        #line hidden
        
        
        #line 478 "..\..\..\..\..\Pages\NavContentPages\PlaylistPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListBox playlistListBox;
        
        #line default
        #line hidden
        
        
        #line 562 "..\..\..\..\..\Pages\NavContentPages\PlaylistPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid playlistSongsDataGrid;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "6.0.10.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/GalaxyMediaPlayer;component/pages/navcontentpages/playlistpage.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\..\Pages\NavContentPages\PlaylistPage.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "6.0.10.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 2:
            this.BackBtn = ((System.Windows.Controls.Button)(target));
            return;
            case 3:
            this.playlistNameHeader = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 4:
            this.newPlaylistBtn = ((System.Windows.Controls.Button)(target));
            
            #line 443 "..\..\..\..\..\Pages\NavContentPages\PlaylistPage.xaml"
            this.newPlaylistBtn.Click += new System.Windows.RoutedEventHandler(this.newPlaylistBtn_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.cbSortByOptions = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 6:
            this.playlistListBox = ((System.Windows.Controls.ListBox)(target));
            return;
            case 7:
            this.playlistSongsDataGrid = ((System.Windows.Controls.DataGrid)(target));
            return;
            }
            this._contentLoaded = true;
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "6.0.10.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        void System.Windows.Markup.IStyleConnector.Connect(int connectionId, object target) {
            System.Windows.EventSetter eventSetter;
            switch (connectionId)
            {
            case 1:
            eventSetter = new System.Windows.EventSetter();
            eventSetter.Event = System.Windows.Controls.Control.MouseDoubleClickEvent;
            
            #line 174 "..\..\..\..\..\Pages\NavContentPages\PlaylistPage.xaml"
            eventSetter.Handler = new System.Windows.Input.MouseButtonEventHandler(this.DataGridRow_MouseDoubleClick);
            
            #line default
            #line hidden
            ((System.Windows.Style)(target)).Setters.Add(eventSetter);
            break;
            }
        }
    }
}

