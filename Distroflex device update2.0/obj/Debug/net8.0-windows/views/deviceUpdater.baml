   M S B A M L   `   `   `  ����V  RDistroflex device update2.0, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullW0clr-namespace:Distroflex_device_update2._0.views"Distroflex_device_update2._0.views  5      6    0Distroflex_device_update2._0.views.deviceUpdater   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Blocal0clr-namespace:Distroflex_device_update2._0.views     5      .��5	      � -   5        $��DeviceUpdater��5	   .   ���#3b3c36$��1��6!     � ColumnDefinitions  5
   
   �� 5      $��* �6   �� 5      $��* �6   �� 5      $��* �6   �� 5      $��* �6   �� 5      $��* �6   �� 5      $��* �6   �� 5      $��* �6   �� 5      $��* �6   �� 5      $��* �6    � RowDefinitions 5   
   �� 5      $��30 �6   �� 5      $��30 �6   �� 5      $��30 �6   �� 5      $��* �6   .+�5   
   �� $��Device connections:��6   $��Right=�5         	labelBold#��[  5      �� 5    
   -   5        $��deviceConnections��5       $
��null��5!      $
��Left=�5"      $��1��5#      #��[  5$      �� 5&   
   $��File Server Status:��6   $
��Left=�5'      $��2��5(      #��[  5)      �� 5+   
   -   5        $��serverStatus��5+      $
��null��5,      $
��Left=�5-      $��3��5.      #��[  5/      �� 51   
   -   5        $��Refresh DB Connection��51      $��4��52        GenericButton#��[ 53      $��5,5,5,5q�54      6"   �� 57   
   $��Device��6   $��1��58      #��[  59      $��Bottom=�5:      $��3��6+   �� 5;   
   -   5        $��device_combobox��5;      $��2��5<      $��5,5,5,0q�5=      $��20��6$   $	��Top=�60   6H   �� 5?   
   $��Engine��6   $��1��5@      $��1��5A      #��[  5B      $	��Top=�5C      �� 5D   
   -   5        $��engine_combobox��5D      $��2��5E      $��1��5F      $��5,5,5,5q�5G      �� 5I   
   $��OS��6   $��1��5J      $��2��5K      #��[  5L      $	��Top=�5M      �� 5O   
   -   5        $��os_combobox��5O      $��2��5P      $��2��5Q      $��5,5,5,5q�5R      �� 5T   
   -   5        $��
Update All��5T      $��7��5U      $��2��5V      $��5,5,5,5q�5W        UpdateButton#��[ 5X      5Y      �� 5Z   
   -	   5        $��
Cancel All��5Z      $��8��5[      $��2��5\      $��5,5,5,5q�5]        CancelButton#��[ 5^      5_      &   System.Windows.Controls.DataGrid  5c   
   -
   5        $��dataShowingCurrentDevices��5c      $��3��5d      $��9��5e      $��5,5,5,5q�5f      $��5q�5h      ���#1e2019���#737470��5g      �� *currentlyConnectedDevices+   Columns 5k      0  *System.Windows.Controls.DataGridTextColumn  5l      ,  &System.Windows.Controls.DataGridColumn   Header$ Device Port��6%      Width5  /System.Windows.Controls.DataGridLengthConverter$ * 5n   !   1  +System.Windows.Controls.DataGridBoundColumn   Binding 5m   !   �� *devicePortNumber+5n   !     5o      $ Connection Status��6%   $ * 5q   !    5p   !   �� *connectionStatus+5q   !     5r      $ 	File Name��6%   $ * 5t   !    5s   !   �� *
fileName+5t   !     5u      $ 
File count��6%   $ * 5w   !    5v   !   �� *	fileCount+5w   !   4  .System.Windows.Controls.DataGridTemplateColumn  5y      $ Progress��6)   $ * 6;      CellTemplate 5z      �� 5{      .R�5|      � $��pbStatus��6*   $��2 2 2 2q�6Y    � Value 6:   �� *fileProgress+6Y   5}      5        5�      $ Device��6%   $ * 66    6@   �� *
deviceType+  5�      $ Update��6)   $ * 69    5�      �� 5�      .R�5�      �� $��updateButton��5�   %   -   5        #��[ 5�   %   $��Update��5�   %   {�5�   %   �� *updateStatus+5�      5�        5�      $ Cancel��6)   $ * 69    5�      �� 5�      .R�5�      �� -   5        #��[ 5�   %   $��Cancel��5�   %   5�      5�      5�      5�      5�      