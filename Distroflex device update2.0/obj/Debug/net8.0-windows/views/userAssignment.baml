   M S B A M L   `   `   `  ����V  RDistroflex device update2.0, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullW0clr-namespace:Distroflex_device_update2._0.views"Distroflex_device_update2._0.views  5      7    1Distroflex_device_update2._0.views.userAssignment   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Blocal0clr-namespace:Distroflex_device_update2._0.views     5      .��5	      � ���#3b3c36  � ShowGridLines$  False��6     � ColumnDefinitions 5
   
   �� 5      �� 5      �� 5      �� 5      �� 5       � RowDefinitions 5   
   �� 5      $��30 �6   �� 5      $��30 �6   �� 5      $��30 �6   �� 5      $��30 �6   �� 5      $��30 �6   �� 5      $��30 �6   �� 5      $��30 �6   �� 5      $��30 �6   �� 5      $��30 �6   �� 5      $��30 �6   �� 5      $��30 �6   �� 5      $��30 �6   �� 5      $��* �6   .+�5    
   ��    	labelBold#��[  6   $��User Assignment��63   �� 5!   
   #��[  6   $��Server status:��63   $��1��6L   $��Right=�6\   �� 5"   
   -   5        $��serverStatus��5"   B   #��[  6   $
��null��63   $��2��6V   $
��Left=�6f   &   System.Windows.Controls.DataGrid  5#   
   -   5        $��existingUsers��5#      $��1��5$      $��2��5%      $��5��5&      
�������
���   �$��10q�5)         Columns 5*      0  *System.Windows.Controls.DataGridTextColumn  5+      ,  &System.Windows.Controls.DataGridColumn   Header$ Current Users��6%      Width5  /System.Windows.Controls.DataGridLengthConverter$ * 6<   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6F   �� 5-        5/   
   -   5        $��usertoDevicegrid��5/      $��7��50      $��2��51      $��6��52      
�������
���   �$��10q�55       56        57      $ Devices assigned to User:��6%   $ * 6H    6R   �� 59      �� 5;   
   -   5        $��addUsertextbox��5<      
�������$��1��5=      $��3��5>      $��5,0,5,5q�5?      $��Center=�5@      $��Center=�5A      $��20��64   $��Bottom=�6@   �� 5B   
   -   5        $��removeUserTextbox��5C      
�������$��2��5D      $��3��5E      $��5q�5F      $��Center=�5G      $��Center=�5H      �� 5I   
   -   5        $��initialUsername��5J      
�������$��4��5K      $��3��5L      $��5q�5M      $��Center=�5N      $��Center=�5O      �� 5P   
   -   5        $��newUsername��5Q      
�������$��4��5R      $��4��5S      $��5q�5T      $��Center=�5U      $��Center=�5V      �� 5W   
   #��[  6   $��Current User Name��5X      $��3��5Y      $��3��5Z      $��Center=�5[      $��Bottom=�5\      �� 5]   
   #��[  6   $��New User Name��5^      $��4��5_      $��3��5`      $��Center=�5a      $��Bottom=�5b      �� 5c   
   -   5        $��2��5c      $��1��5d      $��5q�5e      $��Add New User��5f      6(   �� 5g   
   -	   5        $��2��5g      $��4��5h      $��5q�5i      $��Edit Existing User name��5j      63   �� 5k   
   -
   5        $��2��5k      $��2��5l      $��5q�5m      $��Remove User��5n      6'   �� 5o   
   -   5        $��2��5o      $��5��5p      $��5q�5q      $��Refresh grid��5r      6(   �� 5t   
   $��Select User:��6   $��7��5u      $��2��5v      $��Right=�5w      #��[  5x      �� 5z   
   -   5        $��userCombobox��5}      $��7��5z      $��3��5{      $��5q�5|      5}      �� 5   
   $��Select device:��6   $��8��5�      $��2��5�      $��Right=�5�      #��[  5�      �� 5�   
   -   5        $��deviceComboBox��5�      $��8��5�      $��3��5�      $��5q�5�      5�      �� 5�   
   -   5        $��addDeviceToUser��5�      $��5q�5�      $��
Add Device��5�      $��8��5�      $��4��5�      �� 5�   
   -   5        $��removeDevicefromUser��5�      $��5q�5�      $��Remove Device��5�      $��10��5�      $��2��5�      5�      5�      