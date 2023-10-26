using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Museum
{
    public partial class CreateExcursionType : Window
    {

        MuseumEntities1 database = new MuseumEntities1();
        BitmapImage NewImage = new BitmapImage();
        
        int id;

        public CreateExcursionType()
        {
            InitializeComponent();
            NewImage = null;

        }

        public CreateExcursionType(int id, string name, string duration, string description, ImageSource img)
        {
            InitializeComponent();
            NewImage = null;

            this.id = id;
            NameTextBox.Text = name;
            TimePicker.Text = duration;
            DescriptionTextBox.Text = description;
            ExcursionImage.Source = img;

            CreateButton.Click += new RoutedEventHandler(EditButton_CLick);
            CreateButton.Click -= CreateButton_Click;
            CreateButton.Content = "Сохранить";
        }


        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            ExcursionTypes extype = new ExcursionTypes();
            extype.name = NameTextBox.Text;
            extype.description = DescriptionTextBox.Text;
            extype.duration = Convert.ToDateTime(TimePicker.Text).TimeOfDay;

            if (NewImage != null)
                extype.Image = getJPGFromImageControl(NewImage);

            database.ExcursionTypes.Add(extype);
            database.SaveChanges();

            this.Hide();
        }

        private void EditButton_CLick(object sender, RoutedEventArgs e)
        {
            var query = database.ExcursionTypes.Where(c => c.id == id).Select(c => c).FirstOrDefault();
            query.name = NameTextBox.Text;
            query.description = DescriptionTextBox.Text;
            query.duration = Convert.ToDateTime(TimePicker.Text).TimeOfDay;
            if (NewImage != null)
                query.Image = getJPGFromImageControl(NewImage);
            database.SaveChanges();
            this.Hide();
        }

        private void ChangeImageButton_Click(object sender, RoutedEventArgs e)
        {
            var ofd = new Microsoft.Win32.OpenFileDialog() { Filter = "PNG Files (*.png)|*.png|JPEG Files (*.jpeg)|*.jpeg|JPG Files (*.jpg)|*.jpg"};
            var result = ofd.ShowDialog();
            if (result == false)
                return;

            var Image = new BitmapImage(new Uri(ofd.FileName));
            NewImage = Image;
            ExcursionImage.Source = Image;

        }

        public byte[] getJPGFromImageControl(BitmapImage imageC)
        {
            MemoryStream memStream = new MemoryStream();
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(imageC));
            encoder.Save(memStream);
            return memStream.ToArray();
        }
    }
}
