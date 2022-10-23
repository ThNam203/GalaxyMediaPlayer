﻿#pragma checksum "..\..\..\..\..\Pages\NavContentPages\MusicDetailPage.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "165663F9B5B73A2408B3C31D16646D03DE345320"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using GalaxyMediaPlayer.Pages.NavContentPages;
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
    /// MusicDetailPage
    /// </summary>
    public partial class MusicDetailPage : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 64 "..\..\..\..\..\Pages\NavContentPages\MusicDetailPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnGoBack;
        
        #line default
        #line hidden
        
        
        #line 87 "..\..\..\..\..\Pages\NavContentPages\MusicDetailPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image SongImage;
        
        #line default
        #line hidden
        
        
        #line 96 "..\..\..\..\..\Pages\NavContentPages\MusicDetailPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        private System.Windows.Controls.StackPanel InfoPanel;
        
        #line default
        #line hidden
        
        
        #line 102 "..\..\..\..\..\Pages\NavContentPages\MusicDetailPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock tbSongTitle;
        
        #line default
        #line hidden
        
        
        #line 111 "..\..\..\..\..\Pages\NavContentPages\MusicDetailPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock tbSongInfo;
        
        #line default
        #line hidden
        
        
        #line 140 "..\..\..\..\..\Pages\NavContentPages\MusicDetailPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnFetchLyricsOptions;
        
        #line default
        #line hidden
        
        
        #line 149 "..\..\..\..\..\Pages\NavContentPages\MusicDetailPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel spFetchLyricsOptions;
        
        #line default
        #line hidden
        
        
        #line 151 "..\..\..\..\..\Pages\NavContentPages\MusicDetailPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnOpenLyricsFile;
        
        #line default
        #line hidden
        
        
        #line 160 "..\..\..\..\..\Pages\NavContentPages\MusicDetailPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnFetchLyrics;
        
        #line default
        #line hidden
        
        
        #line 170 "..\..\..\..\..\Pages\NavContentPages\MusicDetailPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock tbSongLyrics;
        
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
            System.Uri resourceLocater = new System.Uri("/GalaxyMediaPlayer;component/pages/navcontentpages/musicdetailpage.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\..\Pages\NavContentPages\MusicDetailPage.xaml"
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
            case 1:
            this.btnGoBack = ((System.Windows.Controls.Button)(target));
            
            #line 70 "..\..\..\..\..\Pages\NavContentPages\MusicDetailPage.xaml"
            this.btnGoBack.Click += new System.Windows.RoutedEventHandler(this.btnGoBack_Click);
            
            #line default
            #line hidden
            return;
            case 2:
            this.SongImage = ((System.Windows.Controls.Image)(target));
            return;
            case 3:
            this.InfoPanel = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 4:
            this.tbSongTitle = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 5:
            this.tbSongInfo = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 6:
            this.btnFetchLyricsOptions = ((System.Windows.Controls.Button)(target));
            
            #line 141 "..\..\..\..\..\Pages\NavContentPages\MusicDetailPage.xaml"
            this.btnFetchLyricsOptions.Click += new System.Windows.RoutedEventHandler(this.btnFetchLyricsOptions_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.spFetchLyricsOptions = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 8:
            this.btnOpenLyricsFile = ((System.Windows.Controls.Button)(target));
            
            #line 152 "..\..\..\..\..\Pages\NavContentPages\MusicDetailPage.xaml"
            this.btnOpenLyricsFile.Click += new System.Windows.RoutedEventHandler(this.btnOpenLyricsFile_Click);
            
            #line default
            #line hidden
            return;
            case 9:
            this.btnFetchLyrics = ((System.Windows.Controls.Button)(target));
            
            #line 161 "..\..\..\..\..\Pages\NavContentPages\MusicDetailPage.xaml"
            this.btnFetchLyrics.Click += new System.Windows.RoutedEventHandler(this.btnFetchLyrics_Click);
            
            #line default
            #line hidden
            return;
            case 10:
            this.tbSongLyrics = ((System.Windows.Controls.TextBlock)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

