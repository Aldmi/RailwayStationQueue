   M S B A M L   `   `   `  ����   TerminalUIWpf!    TerminalUIWpf.Views.AppView   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006       J� 	AllowDrop  .  |�.   � WindowState$ 	Maximized=� � WindowStyle$
 None=� � 	Resources    ButtonStyle �� 
TargetType �� Property �� Value �� Path �� Mode  TextBlockMainStyle	 �� Triggers
 P� Property P� Value  TextBlockErrorStyle��%  &	        &	     &	 |    �� " ��.���  � ��$ 65(���  � �� �� $ ColorBtn�$ OneWay=���  � ��$
 Bold"���  � ��
 �������  � ��$ 	1,20,1,30q���  � {� �� $ 	BtnEnable�$ OneWay=���  � ��$ Stretch=���  � ��$
 1000���� " ��	 P� 
 � {� . . ���  � y�$ Visible=�P� 
 � {� .  . ���  � y�$ 	Collapsed=��� " ��	 P� 
 � {� . . ���  � y�$ 	Collapsed=�P� 
 � {� .  . ���  � y�$ Visible=�.���  � RowDefinitions �� $��* ��� $
��0.7* ��� $
��0.7* �.+�� $��0��$��0��$��0,0,0,45q�$��Stretch=�$��Stretch=�
���<��.+��� $
��1200��$��Stretch=�$��Stretch=�$	��110(�#��[ 
������� �� Padding$ 5,50,5,5q�$D��>Какой билет вы хотите приобрести ?�� �� TextAlignment$ Center=�$
��Wrap=�{��� $ 	IsConnect�$ OneWay=��� $
��1200��$��Stretch=�$��Stretch=�$��70(�#��[ 
�������$ 5,90,5,5q�$s��mОТСУТСТВУЕТ СВЯЗЬ. Обратитесь пожалуйста к администратору.��$ Center=�$
��Wrap=�{��� $ 	IsConnect�$ OneWay=��� -   $��	BtnVilage��$��1��$��Электропоезд��#��[  �� -   $��BtnLongRoad��$��2��$6��0Поезд дальнего следования��#��[  