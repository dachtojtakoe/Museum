   M S B A M L   `   `   `  ����A  =Museum, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:MuseumMuseum  5   	        Museum.CreateExcursionType   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   &localclr-namespace:Museum     5   	   X TXceed.Wpf.Toolkit, Version=4.5.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f44xctk)http://schemas.xceed.com/wpf/xaml/toolkit  6,   -   5          � Title$2  ,Создание вида экскурсии��5
   	   $	��407��6)   $	��545��66    � 
ResizeMode$ CanMinimize=�6[    � WindowStartupLocation$ CenterScreen=�6t   .��5      � .+�5   
   �� $)��#Название экскурсии��6   $
��Left=�6-   $��	12,10,0,0q�6H   $	��Top=�6[   $��16(�6s   �� 5   
   $1��+Длительность экскурсии��6   $
��Left=�61   $��274,217,0,0q�6L   $	��Top=�6a   $��16(�6y   �� 5   
   $)��#Описание экскурсии��6   $
��Left=�6-   $��
274,10,0,0q�6H   $	��Top=�6\   $��16(�6t   �� 5   
   -   5         ��Name$ CreateButton��5      $��Создать��6'   $��	0,0,10,20q�69   $��16(�6g   $��26��6u   $��Bottom=�6�   $��Right=�6�   $	��120��6�   �� 5   
   -   5        $��NameTextBox��5      $
��Left=�6%   $��63��6@   $��	12,39,0,0q�6L    �� TextWrapping$
 Wrap=�6_   $	��Top=�6s   $	��243��6�   $��16(�6�   �� 5   
   -   5         ��Name$ DescriptionTextBox��5      $��274,39,10,195q�6.   $
 Wrap=�6E   $��16(�6Y   "  Xceed.Wpf.Toolkit.TimePicker  5   
   -   5        $��
TimePicker��5      $��274,253,137,0q�6,   $��24(�6C   $��53��6Q   $	��Top=�6]   �� 5   
   -   5        $��ExcursionImage��5      $
��Left=�6&   $	��169��6A   $��
14,135,0,0q�6N   $	��Top=�6b   $	��237��6z   �� 5   
   -   5        $ ChangeImageButton��5      $��Изменить фото��6,   $��	14,0,0,20q�6D   $��16(�6w   $��26��6�   $��Bottom=�6�   $
��Left=�6�   $	��128��6�   5      5      