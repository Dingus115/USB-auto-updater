﻿#pragma checksum "..\..\..\..\views\engineEditor.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "71B34EBE1B26398E6844CFB40898AE3C0BB29856"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Distroflex_device_update2._0.views;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
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


namespace Distroflex_device_update2._0.views {
    
    
    /// <summary>
    /// engineEditor
    /// </summary>
    public partial class engineEditor : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 33 "..\..\..\..\views\engineEditor.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label serverStatus;
        
        #line default
        #line hidden
        
        
        #line 34 "..\..\..\..\views\engineEditor.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid existingEngine;
        
        #line default
        #line hidden
        
        
        #line 53 "..\..\..\..\views\engineEditor.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox deviceComboBox;
        
        #line default
        #line hidden
        
        
        #line 63 "..\..\..\..\views\engineEditor.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox textbox_addEngine;
        
        #line default
        #line hidden
        
        
        #line 70 "..\..\..\..\views\engineEditor.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox textbox_removeEngine;
        
        #line default
        #line hidden
        
        
        #line 77 "..\..\..\..\views\engineEditor.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox textbox_initialEngineName;
        
        #line default
        #line hidden
        
        
        #line 84 "..\..\..\..\views\engineEditor.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox textbox_newEngineName;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "8.0.7.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/Distroflex device update2.0;component/views/engineeditor.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\views\engineEditor.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "8.0.7.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.serverStatus = ((System.Windows.Controls.Label)(target));
            return;
            case 2:
            this.existingEngine = ((System.Windows.Controls.DataGrid)(target));
            return;
            case 3:
            this.deviceComboBox = ((System.Windows.Controls.ComboBox)(target));
            
            #line 55 "..\..\..\..\views\engineEditor.xaml"
            this.deviceComboBox.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.deviceComboBox_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 4:
            
            #line 60 "..\..\..\..\views\engineEditor.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.refreshDeviceList);
            
            #line default
            #line hidden
            return;
            case 5:
            this.textbox_addEngine = ((System.Windows.Controls.TextBox)(target));
            return;
            case 6:
            this.textbox_removeEngine = ((System.Windows.Controls.TextBox)(target));
            return;
            case 7:
            this.textbox_initialEngineName = ((System.Windows.Controls.TextBox)(target));
            return;
            case 8:
            this.textbox_newEngineName = ((System.Windows.Controls.TextBox)(target));
            return;
            case 9:
            
            #line 105 "..\..\..\..\views\engineEditor.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.addNewEngine);
            
            #line default
            #line hidden
            return;
            case 10:
            
            #line 109 "..\..\..\..\views\engineEditor.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.editExistingEngine);
            
            #line default
            #line hidden
            return;
            case 11:
            
            #line 113 "..\..\..\..\views\engineEditor.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.removeEngine);
            
            #line default
            #line hidden
            return;
            case 12:
            
            #line 117 "..\..\..\..\views\engineEditor.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.refreshdataGrid);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

