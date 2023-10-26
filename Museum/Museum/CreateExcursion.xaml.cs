using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Museum
{

    public partial class CreateExcursion : Window
    {
        MuseumEntities1 database = new MuseumEntities1();

        int id;

        public CreateExcursion()
        {
            InitializeComponent();
            Opening();
        }

        public CreateExcursion(int id, string name, string starttime, string price, string guide)
        {
            InitializeComponent();
            Opening();

            starttime = starttime.Substring(0,16);
            string[] start = starttime.Split(' ');
            string[] guidefname = guide.Split(' ');
            this.id = id;
            ExcursionTypeComboBox.Text = name;
            DatePicker.Text = start[0];
            TimeTextBox.Text = start[1];
            PriceTextBox.Text = price;
            GuideComboBox.Text = guidefname[0];

            CreateButton.Click += new RoutedEventHandler(EditButton_CLick);
            CreateButton.Click -= CreateButton_Click;
            CreateButton.Content = "Сохранить";
        }

        public void Opening()
        {
            ExcursionTypeComboBox.ItemsSource = database.ExcursionTypes.Where(c => c.deleted != true).Select(c => c.name).ToList();

            GuideComboBox.ItemsSource = database.Workers.Where(c => c.deleted != true).Where(c=>c.role_id == 2).Select(c => c.surname ).ToList();
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            Excursions excursion = new Excursions();
            excursion.extype_id = database.ExcursionTypes.Where(c => c.name == ExcursionTypeComboBox.Text).Select(c => c.id).FirstOrDefault();
            var date = DatePicker.Text;
            var time = TimeTextBox.Text;
            excursion.start_datetime = Convert.ToDateTime(date + " " + time);
            excursion.price = Convert.ToInt32(PriceTextBox.Text);
            excursion.guide_id = database.Workers.Where(c => c.surname == GuideComboBox.Text).Select(c => c.id).FirstOrDefault();

            database.Excursions.Add(excursion);
            database.SaveChanges();

            this.Hide();
        }

        private void EditButton_CLick(object sender, RoutedEventArgs e)
        {
            var query = database.Excursions.Where(c => c.id == id).Select(c => c).FirstOrDefault();
            query.extype_id = database.ExcursionTypes.Where(c => c.name == ExcursionTypeComboBox.Text).Select(c => c.id).FirstOrDefault();
            var date = DatePicker.Text;
            var time = TimeTextBox.Text;
            query.start_datetime = Convert.ToDateTime(date + " " + time);
            query.price = Convert.ToInt32(PriceTextBox.Text);
            query.guide_id = database.Workers.Where(c => c.surname == GuideComboBox.Text).Select(c => c.id).FirstOrDefault();
            database.SaveChanges();
            this.Hide();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }


        private void HoursUp_Click(object sender, RoutedEventArgs e)
        {
            string[] time = TimeTextBox.Text.Split(':');
            int hours = Convert.ToInt32(time[0]);
            int minutes = Convert.ToInt32(time[1]);
            if (hours == 23)
            {
                hours = 0;
                TimeTextBox.Text = hours + ":" + time[1];
            }
            else if (hours <= 23 )
            {
                hours += 1;
                TimeTextBox.Text = hours + ":" + time[1];
            }
        }

        private void MinutesUp_Click(object sender, RoutedEventArgs e)
        {
            string[] time = TimeTextBox.Text.Split(':');
            int hours = Convert.ToInt32(time[0]);
            int minutes = Convert.ToInt32(time[1]);

            if (hours == 23 && minutes == 59)
            {
                hours = 0;
                minutes = 0;
                TimeTextBox.Text = hours + ":" + minutes;

            }
            else if (hours <= 23 && minutes == 59)
            {
                minutes = 0;
                hours += 1;
                TimeTextBox.Text = hours + ":" + minutes;
            }
            else if (minutes < 59)
            {
                minutes += 1;
                TimeTextBox.Text = hours + ":" + minutes;
            }
        }

        private void HoursDown_Click(object sender, RoutedEventArgs e)
        {
            string[] time = TimeTextBox.Text.Split(':');
            int hours = Convert.ToInt32(time[0]);

            if (hours == 0)
            {
                hours = 23;
                TimeTextBox.Text = hours + ":" + time[1];
            }
            else if (hours >= 1)
            {
                hours -= 1;
                TimeTextBox.Text = hours + ":" + time[1];
            }
        }

        private void MinutesDown_Click(object sender, RoutedEventArgs e)
        {
            string[] time = TimeTextBox.Text.Split(':');
            int hours = Convert.ToInt32(time[0]);
            int minutes = Convert.ToInt32(time[1]);
            if (hours >= 1 && minutes == 0)
            {
                hours -= 1;
                minutes = 59;
                TimeTextBox.Text = hours + ":" + minutes;
            }
            else if (hours == 0 && minutes == 0)
            {
                hours = 23;
                minutes = 59;
                TimeTextBox.Text = hours + ":" + minutes;
            }
            else if (minutes >= 1)
            {
                minutes -= 1;
                TimeTextBox.Text = hours + ":" + minutes;
            }
        }
    }
}
