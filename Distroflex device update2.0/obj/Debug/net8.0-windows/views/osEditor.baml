   M S B A M L   `   `   `  ����V  RDistroflex device update2.0, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullW0clr-namespace:Distroflex_device_update2._0.views"Distroflex_device_update2._0.views  5      1    +Distroflex_device_update2._0.views.osEditor   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Blocal0clr-namespace:Distroflex_device_update2._0.views     5      .��5	      � ���#3b3c36  � ShowGridLines$  False��6     � ColumnDefinitions 5
   
   �� 5      �� 5      �� 5      �� 5      �� 5       � RowDefinitions 5   
   �� 5      $��30 �6   �� 5      $��30 �6   �� 5      $��30 �6   �� 5      $��30 �6   �� 5      $��30 �6   �� 5      $��30 �6   �� 5      $��30 �6   �� 5      $��30 �6   �� 5      �� 5      �� 5      �� 5      .+�5   
   ��    	labelBold#��[  6   $��	OS Editor��63   �� 5    
   #��[  6   $��Server status:��63   $��1��6L   $��Right=�6\   �� 5!   
   -   5        $��serverStatus��5!   B   #��[  6   $
��null��63   $��2��6V   $
��Left=�6f   &   System.Windows.Controls.DataGrid  5"   
   -   5        $��
existingOs��5"      $��1��5#      $��2��5$      $��20��5%      
�������
���   �$��10q�5(         Columns 5)      0  *System.Windows.Controls.DataGridTextColumn  5*      ,  &System.Windows.Controls.DataGridColumn   Header$ Current OS options��6%      Width5  /System.Windows.Controls.DataGridLengthConverter$ * 6A   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6K   �� 5,      �� 5.   
   $��Please select Device:��6   #��[  5/      $��1��50      $��2��51      $��Right=�52      �� 54   
   -   5        $��deviceComboBox��55      $��5q�54      $��1��56      $��3��57      6   �� 59   
   -   5        $��5q�59      $��1��5:      $��4��5;      $��Refresh devices��5<      6'   �� 5>   
   -   5        $��textbox_addOs��5?      
�������$��3��5@      $��3��5A      $��5q�5B      $��Center=�5C      $��Center=�5D      �� 5E   
   -   5        $��textbox_removeOs��5F      
�������$��4��5G      $��3��5H      $��5q�5I      $��Center=�5J      $��Center=�5K      �� 5L   
   -   5        $��textbox_initialOsName��5M      
�������$��6��5N      $��3��5O      $��5q�5P      $��Center=�5Q      $��Center=�5R      �� 5S   
   -   5        $��textbox_newOsName��5T      
�������$��6��5U      $��4��5V      $��5q�5W      $��Center=�5X      $��Center=�5Y      �� 5Z   
   #��[  6   $��Current OS Name��5[      $��3��5\      $��5��5]      $��Center=�5^      $��Bottom=�5_      �� 5`   
   #��[  6   $��New OS Name��5a      $��4��5b      $��5��5c      $��Center=�5d      $��Bottom=�5e      �� 5f   
   -	   5        $��2��5f      $��3��5g      $��5q�5h      $��
Add New OS��5i      6"   �� 5j   
   -
   5        $��2��5j      $��6��5k      $��5q�5l      $��Edit Existing OS name��5m      6-   �� 5n   
   -   5        $��2��5n      $��4��5o      $��5q�5p      $��	Remove OS��5q      6!   �� 5r   
   -   5        $��2��5r      $��7��5s      $��5q�5t      $��Refresh grid��5u      6$   5v      5w      