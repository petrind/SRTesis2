   M S B A M L   `   `   `  ����  
NAOCamera2    NAOserver.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   -   5          � Title$!  NAO Control Server Edition ��5   	   $	��350��6-   $	��400��6:    � 
ResizeMode$ CanMinimize=�5      .��5      � .+�5      �� -   5         ��Name$ connectButton��5      $��Start��6$   $
��Left=�64   $��126,113,0,0q�5      $	��Top=�6   $	��139��6.   $��70��6:   5	      �� 5
      -   5        $ disconnectButton��5
      $
��Stop��6'   $
��Left=�66   $��126,203,0,0q�5      $	��Top=�6   $	��139��6.   $��70��6:   5      �� 5      -   5         ��Name$ ipBox��5      $
��Left=�6   $��23��68   $��
175,78,0,0q�5       �� TextWrapping$
 Wrap=�6   $��69.88.163.51��6)   $	��Top=�6=   $��90��5       �� MaxLines$ 1��6    �� 	MaxLength$ 15��6   �� 5      -   5        $ portBox��5      $
��Left=�6   $��23��6:   $��
220,50,0,0q�5      $
 Wrap=�6   $	��443��6)   $	��Top=�64   $��44��6L   $ 4��5      $ 1��6    J� RenderTransformOrigin$ 
1.25,0.391&�6   �� 5      -   5         ��Name$ 	portLabel��5      $��WebSocket Port:��6   $
��Left=�69   $��27��5      $��
121,48,0,0q�6   $	��Top=�6!   $��97��69   �� 5      $��NAO IP:��6   $
��Left=�6   $��
121,75,0,0q�69   $	��Top=�5      5      5      