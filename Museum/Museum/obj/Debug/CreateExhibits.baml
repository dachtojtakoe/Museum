   M S B A M L   `   `   `  ����A  =Museum, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:MuseumMuseum  5   	       Museum.CreateExhibits   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   &localclr-namespace:Museum     5   	   -   5          � Title$)  #Создание экспоната��5      $��258.667��6   $��498.667��60    � 
ResizeMode$ CanMinimize=�6Y    � WindowStartupLocation$ CenterScreen=�6r   .��5
      � .+�5   
   �� $��Тип экспоната��6   $
��Left=�6(   $��	174,1,0,0q�6C   $	��Top=�6V   $��16(�6n   �� 5   
   -   5         ��Name$ ExhibitTypeComboBox��5      $
��Left=�60   $��
174,32,0,0q�6K   $	��Top=�6_   $	��138��6w   �� 5   
   $)��#Название экспоната��6   $
��Left=�6-   $��10,1,0,0q�6H   $	��Top=�6Z   $��16(�6r   �� 5   
   -   5         ��Name$ ExhibitNameTextBox��5      $
��Left=�6.   $��61��6I   $��	10,32,0,0q�6U    �� TextWrapping$
 Wrap=�6h   $	��Top=�6|   $	��151��6�   $��16(�6�   �� 5   
   $��Зал��6   $
��Left=�6   $��	326,0,0,0q�69   $	��Top=�6L   $��16(�6d   �� 5   
   -   5        $ HallsComboBox��5      $
��Left=�6*   $��
326,32,0,0q�6E   $	��Top=�6Y   $	��130��6q   �� 5   
   -   5         ��Name$ CreateButton��5      $��Создать��6'   $
��Left=�69   $��
12,178,0,0q�6T   $	��Top=�6h   $	��149��6�   $��16(�6�   $��23��6�   �� 5   
   $��
Автор��6   $
��Left=�6    $��	12,95,0,0q�6;   $	��Top=�6N   $��16(�6f   �� 5   
   -   5        $ AuthorsComboBox��5      $
��Left=�6,   $��
12,126,0,0q�6G   $	��Top=�6[   $	��149��6s   �� 5   
   $��Описание��6   $
��Left=�6#   $��
174,62,0,0q�6>   $	��Top=�6R   $��16(�6j   �� 5   
   -   5        $ DescriptionTextBox��5      $
��Left=�6.   $	��109��6I   $��
174,92,0,0q�6V   $
 Wrap=�6j   $	��Top=�6~   $	��282��6�   $��16(�6�   5      5      