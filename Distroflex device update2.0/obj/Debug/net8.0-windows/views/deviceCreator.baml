   M S B A M L   `   `   `  ����V  RDistroflex device update2.0, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullW0clr-namespace:Distroflex_device_update2._0.views"Distroflex_device_update2._0.views  5      6    0Distroflex_device_update2._0.views.deviceCreator   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Blocal0clr-namespace:Distroflex_device_update2._0.views     5      .��5	      � ���#3b3c36  � ShowGridLines$  False��6     � ColumnDefinitions 5
   
   �� 5      �� 5      �� 5      �� 5      �� 5       � RowDefinitions 5   
   �� 5      $��30 �6   �� 5      $��30 �6   �� 5      $��30 �6   �� 5      $��30 �6   �� 5      $��30 �6   �� 5      $��30 �6   �� 5      �� 5      �� 5      �� 5      .+�5   
   ��    	labelBold#��[  6   $��Device Creator��63   �� 5   
   #��[  6   $��Server status:��63   $��1��6L   $��Right=�6\   �� 5   
   -   5        $��serverStatus��5   B   #��[  6   $
��null��63   $��2��6V   $
��Left=�6f   &   System.Windows.Controls.DataGrid  5    
   -   5        $��existingDevices��5       $��1��5!      $��2��5"      $��9��5#      
�������
���   �$��10q�5&         Columns 5'      0  *System.Windows.Controls.DataGridTextColumn  5(      ,  &System.Windows.Controls.DataGridColumn   Header$ Current Devices��6%      Width5  /System.Windows.Controls.DataGridLengthConverter$ * 6>   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6H   �� 5*      �� 5,   
   -   5        $��textbox_addDevice��5-      
�������$��1��5.      $��3��5/      $��5q�50      $��Center=�51      $��Center=�52      �� 53   
   -   5        $��textbox_removeDevice��54      
�������$��2��55      $��3��56      $��5q�57      $��Center=�58      $��Center=�59      �� 5:   
   -   5        $��textbox_initialDeviceName��5;      
�������$��4��5<      $��3��5=      $��5q�5>      $��Center=�5?      $��Center=�5@      �� 5A   
   -   5        $��textbox_newDeviceName��5B      
�������$��4��5C      $��4��5D      $��5q�5E      $��Center=�5F      $��Center=�5G      �� 5H   
   #��[  6   $��Current Device Name��5I      $��3��5J      $��3��5K      $��Center=�5L      $��Bottom=�5M      �� 5N   
   #��[  6   $��New Device Name��5O      $��4��5P      $��3��5Q      $��Center=�5R      $��Bottom=�5S      �� 5T   
   -   5        $��2��5T      $��1��5U      $��5q�5V      $��Add New Device��5W      6*   �� 5X   
   -   5        $��2��5X      $��4��5Y      $��5q�5Z      $��Edit Existing device name��5[      65   �� 5\   
   -	   5        $��2��5\      $��2��5]      $��5q�5^      $��Remove Device��5_      6)   �� 5`   
   -
   5        $��2��5`      $��5��5a      $��5q�5b      $��Refresh grid��5c      6(   5d      5e      