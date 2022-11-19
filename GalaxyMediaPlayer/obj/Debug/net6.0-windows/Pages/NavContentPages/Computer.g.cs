﻿#pragma checksum "..\..\..\..\..\Pages\NavContentPages\Computer.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "2B41BE0B733876D26A8C1301EB6CC7BF341777B2"
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
using Wpf.Ui;
using Wpf.Ui.Common;
using Wpf.Ui.Controls;
using Wpf.Ui.Controls.Navigation;
using Wpf.Ui.Converters;
using Wpf.Ui.Markup;
using Wpf.Ui.ValidationRules;


namespace GalaxyMediaPlayer.Pages.NavContentPages {
    
    
    /// <summary>
    /// Computer
    /// </summary>
    public partial class Computer : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector, System.Windows.Markup.IStyleConnector {
        
        
        #line 407 "..\..\..\..\..\Pages\NavContentPages\Computer.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button BackBtn;
        
        #line default
        #line hidden
        
        
        #line 415 "..\..\..\..\..\Pages\NavContentPages\Computer.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock currentFolderName;
        
        #line default
        #line hidden
        
        
        #line 431 "..\..\..\..\..\Pages\NavContentPages\Computer.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox cbSortByOptions;
        
        #line default
        #line hidden
        
        
        #line 441 "..\..\..\..\..\Pages\NavContentPages\Computer.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image BrowseStyleImage;
        
        #line default
        #line hidden
        
        
        #line 453 "..\..\..\..\..\Pages\NavContentPages\Computer.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListBox browseListBox;
        
        #line default
        #line hidden
        
        
        #line 600 "..\..\..\..\..\Pages\NavContentPages\Computer.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid browseDataGrid;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "6.0.2.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/GalaxyMediaPlayer;component/pages/navcontentpages/computer.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\..\Pages\NavContentPages\Computer.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "6.0.2.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 13 "..\..\..\..\..\Pages\NavContentPages\Computer.xaml"
            ((GalaxyMediaPlayer.Pages.NavContentPages.Computer)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Page_Loaded);
            
            #line default
            #line hidden
            return;
            case 3:
            this.BackBtn = ((System.Windows.Controls.Button)(target));
            
            #line 412 "..\..\..\..\..\Pages\NavContentPages\Computer.xaml"
            this.BackBtn.Click += new System.Windows.RoutedEventHandler(this.BackBtn_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.currentFolderName = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 5:
            this.cbSortByOptions = ((System.Windows.Controls.ComboBox)(target));
            
            #line 432 "..\..\..\..\..\Pages\NavContentPages\Computer.xaml"
            this.cbSortByOptions.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.cbSortByOptions_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 6:
            this.BrowseStyleImage = ((System.Windows.Controls.Image)(target));
            
            #line 442 "..\..\..\..\..\Pages\NavContentPages\Computer.xaml"
            this.BrowseStyleImage.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.BrowseStyleImage_MouseLeftButtonDown);
            
            #line default
            #line hidden
            return;
            case 7:
            this.browseListBox = ((System.Windows.Controls.ListBox)(target));
            return;
            case 9:
            this.browseDataGrid = ((System.Windows.Controls.DataGrid)(target));
            return;
            }
            this._contentLoaded = true;
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "6.0.2.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        void System.Windows.Markup.IStyleConnector.Connect(int connectionId, object target) {
            System.Windows.EventSetter eventSetter;
            switch (connectionId)
            {
            case 2:
            eventSetter = new System.Windows.EventSetter();
            eventSetter.Event = System.Windows.Controls.Control.MouseDoubleClickEvent;
            
            #line 177 "..\..\..\..\..\Pages\NavContentPages\Computer.xaml"
            eventSetter.Handler = new System.Windows.Input.MouseButtonEventHandler(this.DataGridRow_MouseDoubleClick);
            
            #line default
            #line hidden
            ((System.Windows.Style)(target)).Setters.Add(eventSetter);
            break;
            case 8:
            
            #line 484 "..\..\..\..\..\Pages\NavContentPages\Computer.xaml"
            ((System.Windows.Controls.Border)(target)).MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.browseListBoxItem_MouseDown);
            
            #line default
            #line hidden
            break;
            }
        }
    }
}

