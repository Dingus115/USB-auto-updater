   M S B A M L   `   `   `  ����V  RDistroflex device update2.0, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullW0clr-namespace:Distroflex_device_update2._0.views"Distroflex_device_update2._0.views  5      9    3Distroflex_device_update2._0.views.deviceFileEditor   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Blocal0clr-namespace:Distroflex_device_update2._0.views     5      .��5	      � ���#3b3c36  � ShowGridLines$  False��6     � ColumnDefinitions 5
   
   �� 5      �� 5      $	��650��6   �� 5      �� 5      �� 5      �� 5       � RowDefinitions 5   
   �� 5      $��30 �6   �� 5      $��30 �6   �� 5      $��30 �6   �� 5      $��30 �6   �� 5      $��30 �6   �� 5      $��30 �6   �� 5      $��30 �6   �� 5      $��30 �6   �� 5      $��30 �6   �� 5      $��30 �6   �� 5      $��30 �6   �� 5      $��30 �6   �� 5      $	��30* �6   .+�5!   
   ��    	labelBold#��[  6   $��Device File Editor��63   �� 5"   
   #��[  6   $��Server status:��63   $��1��6L   $��Right=�6\   �� 5#   
   -   5        $��serverStatus��5#   B   #��[  6   $
��null��63   $��2��6V   $
��Left=�6f   &   System.Windows.Controls.DataGrid  5$   
   -   5        $��existingFiles��5$      $��1��5%      $��2��5&      $��12��5'      
�������
���   �$��10q�5*      $��	0 0 100 0q�5+         
IsReadOnly$
 True��5,         	CellStyle 5-      �� 5.       �� 
TargetType*  $System.Windows.Controls.DataGridCell"  6    �� Triggers 5/      P� 50       P� Property �@ 
IsSelected P� Value$
 True��68   . �51      �� 	 �� Property	 � ��
 �� Value
 �#0078d76;   �� 52      	 � ��

 �����6;   �� 53      	 � ��

 �   �6<   �� 54      	 � ��$
 1q�6@   55      57         Columns 59      0  *System.Windows.Controls.DataGridTextColumn  5:      ,  &System.Windows.Controls.DataGridColumn   Header$
 File��6%      Width5  /System.Windows.Controls.DataGridLengthConverter$ * 5;   %   1  +System.Windows.Controls.DataGridBoundColumn   Binding 5<   %   �� *
filePath+  5=      $ 	Partition��6%      MaxWidth$ 54Q�5>   %      MinWidth$ 54Q�5?   %    5@   %   �� *	partition+4  .System.Windows.Controls.DataGridCheckBoxColumn  5B      $ Restart��6)   $ 50Q�5C   )   $ 50Q�5D   )    5E   )   �� *restartAfter+  5F      $ isConfig��6)   $ 52Q�5G   )   $ 52Q�5H   )    5I   )   �� *
configFile+  5J      $ 
File Order��6%   $ 65Q�5K      $ 65Q�5L       5M      �� *orderOfFiles+5P      �� 5R   
   -   5        $��moveUp��5R      $��Up��5S      $��2��5T      $��1��5U      $��Right=�5V      $��5 5 35 5q�5W      $��60��5X      6   �� 5Y   
   -   5        $��modeDown��5Y      $
��Down��5Z      $��3��5[      $��1��5\      $��Right=�5]      $��5 5 35 5q�5^      $��60��5_      6   �� 5`   
   -   5        $��deleteRecord��5`      $��Delete��5a      $��5��5b      $��1��5c      $��Right=�5d      $��5 5 35 5q�5e      $��60��5f      6   �� 5g   
   -   5        $��	deleteAll��5g      $��
Delete all��5h      $��6��5i      $��1��5j      $��Right=�5k      $��5 5 35 5q�5l      $��60��5m      6   �� 5q   
   $��Please select Device��6   #��[  5r      $��1��5s      $��2��5t      $��Center=�5u      �� 5v   
   -   5        $��deviceComboBox��5w      $��5q�5v      $��2��5x      $��2��5y      5z      �� 5|   
   $��Please select OS��6   #��[  5}      $��1��5~      $��3��5      $��Center=�5�      �� 5�   
   -   5        $��
osCombobox��5�      $��5q�5�      $��2��5�      $��3��5�      6#   �� 5�   
   $��Please select Engine��6   #��[  5�      $��1��5�      $��4��5�      $��Center=�5�      $��Bottom=�5�      �� 5�   
   -	   5        $��engineCombobox��5�      $��5q�5�      $��2��5�      $��4��5�      6#   �� 5�   
   -
   5        $��5q�5�      $��2��5�      $��5��5�      $��Refresh devices��5�      6'   �� 5�   
   -   5        $��fileSelected_textbox��5�      $��5q�5�      $��2��5�      $��4��5�      $��3��5�      $
��null��5�       �� 
IsReadOnly$
 True��5�      5�      �� 5�   
   -   5        $��fileSelectbutton��5�      $��5q�5�      $	��...��5�      $��4��5�      $��4��5�      $��30��5�      $��30��5�      $��Right=�5�      6-   �� 5�   
   -   5        $��Save and Commit��5�      $��5q�5�      $��5��5�      $��5��5�      $��Bottom=�5�      6,   �� 5�   
   -   5        $��addFile��5�      $��Add File��5�      $��5q�5�      $��5��5�      $��4��5�      $
��auto��5�      6   �� 5�   
   $��Does device need scritps?��6   #��[  5�      $��2��5�      $��5��5�      $��2��5�      $
��Left=�5�      $��Center=�5�      �� 5�   
   -   5        $��scripts��5�      $��5��5�      $��2��5�      $��2��5�      $��	150 5 5 5q�5�      $
��Left=�5�      $��Center=�5�       � LayoutTransform 5�      �� 5�       �� ScaleX$	 1.5Q�6!    �� ScaleY$	 1.5Q�6.   5�      � 5�   
   -   5        $��advancedOptions��5�      $��6��5�      $��2��5�      $��6��5�      $��4��5�      $y�Hidden=�5�       5�      �� 5�      �� 5�      �� 5�       5�      �� 5�      $��30 �6    �� 5�      $��30 �6    �� 5�      $��30 �6    �� 5�      $��30 �6    �� 5�      $��30 �6    �� 5�      $��30 �6    .+�5�      �� $��Patition #:��6   $��0��5�      $��0��5�      $
��Left=�5�      $��Center=�5�      #��[  5�      �� 5�      -   5         ��Name$ partitionNumber_Textbox��5�      $��0��67   $��0��5�      $
��Left=�5�      $��30��5�      $��65 5 5 5q�5�      �� 5�      $��Restart after?��6   #��[  5�      $��0��5�      $��1��5�      $
��Left=�5�      $��Center=�5�      �� 5�      -   5        $��restart��5�      $��1��5�      $��0��5�      $��85 5 5 5q�5�      $
��Left=�5�      $��Center=�5�       d� 	IsChecked 5�      �� *restartCheck+ 5�      �� 5�      $	 1.5Q�6%   $	 1.5Q�62   5�      �� 5�      $��Config?��6   #��[  5�      $��0��5�      $��2��5�      $
��Left=�5�      $��Center=�5�      �� 5      -   5        $��config��5      $��2��5     $��0��5     $��50 5 5 5q�5     $
��Left=�5     $��Center=�5      5     �� *	isChceked+ 5     �� 5     $	 1.5Q�6%   $	 1.5Q�62   5
     �� 5     $8��2Please select folder for scripts to be written to!��6   $��3��5     $��0��5     #��[  5     $��1��5     �� 5     -   5        $��folderSelected_textbox��5     $��5q�5     $��0��5     $��4��5     $��2��5     $
��null��5     $
 True��5     5     �� 5     -   5        $��folderSelectbutton��5     $��5q�5     $	��...��5     $��1��5     $��4��5     $��30��5     $��30��5     $��Right=�5      61   5!     5+     5,     