   M S B A M L   `   `   `  ����  TerminalUIWpf!    TerminalUIWpf.Views.AppView   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	     J� 	AllowDrop  .  |�.   � WindowState$ 	Maximized=�5   	    � WindowStyle$
 None=�5	   	    � 	Resources 5         ButtonStyle �� 
TargetType �� Property �� Value �� Path �� Mode  TextBlockMainStyle	 �� Triggers
 P� Property P� Value  TextBlockErrorStyle��%U  &	        &	 �    &	 c    �� 5   
   " ��6$   .�5      ��  � ��$ 65(�6)   �� 5       � �� 6+   �� $ ColorBtn�$ OneWay=��� 5       � ��$
 Bold"�6+   �� 5       � ��
 �����6+   �� 5       � ��$ 	1,20,1,30q�6'   �� 5       � {� 6*   �� $ 	BtnEnable�$ OneWay=��� 5       � ��$ Stretch=�62   �� 5       � ��$
 1000��6)   5      �� 5   
   " ��6+   	 5      P� 5      
 � {� . . �5      ��  � y�$ Visible=�63   5      P� 5      
 � {� .  . �5      ��  � y�$ 	Collapsed=�63   5       5"      �� 5$   
   " ��6,   	 5%      P� 5&      
 � {� . . �5'      ��  � y�$ 	Collapsed=�63   5(      P� 5)      
 � {� .  . �5*      ��  � y�$ Visible=�63   5+      5-      5.      .��52      �  � RowDefinitions 53   
   �� 54      $��* �6   �� 55      $
��0.7* �6   �� 56      $
��0.7* �6   .+�59   
   � $��0��6   $��0��5:      $��0,0,0,45q�5;      $��Stretch=�5<      $��Stretch=�5=      
���<��.+�5@      �� $
��1200��6   $��Stretch=�5A      $��Stretch=�5B      $	��110(�5C      #��[ 5D      
������� �� Padding$ 5,50,5,5q�5F      $D��>Какой билет вы хотите приобрести ?��5G       �� TextAlignment$ Center=�5H      $
��Wrap=�5I      {�6,   �� $ 	IsConnect�$ OneWay=��� 5K      $
��1200��6   $��Stretch=�5L      $��Stretch=�5M      $��70(�5N      #��[ 5O      
�������$ 5,90,5,5q�5Q      $s��mОТСУТСТВУЕТ СВЯЗЬ. Обратитесь пожалуйста к администратору.��5R      $ Center=�5S      $
��Wrap=�5T      {�6,   �� $ 	IsConnect�$ OneWay=�5U      �� 5X   
   -   5        $��	BtnVilage��5X      $��1��5Y      $��Электропоезд��5Z      #��[  5[      �� 5]   
   -   5        $��BtnLongRoad��5]      $��2��5^      $6��0Поезд дальнего следования��5_      #��[  5`      5a      5b      