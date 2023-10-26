using System.Linq;
using System.Windows;

namespace Museum
{
    public partial class CreateExhibits : Window
    {
        MuseumEntities1 database = new MuseumEntities1();
        int id;

        public CreateExhibits()
        {
            InitializeComponent();
            Opening();
        }

        public CreateExhibits(int id,string name, string type, string hall, string author, string description)
        {
            InitializeComponent();
            Opening();
            this.id = id;
            ExhibitNameTextBox.Text = name;
            ExhibitTypeComboBox.Text = type;
            HallsComboBox.Text = hall;
            AuthorsComboBox.Text = author;
            DescriptionTextBox.Text = description;
            CreateButton.Click += new RoutedEventHandler(EditButton_CLick);
            CreateButton.Click -= CreateButton_Click;
            CreateButton.Content = "Сохранить";
        }

        private void Opening()
        {
            ExhibitTypeComboBox.ItemsSource = database.ExhibitTypes.Where(c => c.deleted != true).Select(c => c.name).ToList();
            HallsComboBox.ItemsSource = database.Halls.Where(c => c.deleted != true).Select(c => c.name).ToList();
            AuthorsComboBox.ItemsSource = database.Authors.Where(c => c.deleted != true).Select(c => c.full_name).ToList();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            Exhibits exhibit = new Exhibits();
            exhibit.name = ExhibitNameTextBox.Text;
            exhibit.type_id = database.ExhibitTypes.Where(c => c.name == ExhibitTypeComboBox.Text).Select(c => c.id).FirstOrDefault();
            exhibit.hall_id = database.Halls.Where(c => c.name == HallsComboBox.Text).Select(c => c.id).FirstOrDefault();
            exhibit.author_id = database.Authors.Where(c => c.full_name == AuthorsComboBox.Text).Select(c => c.id).FirstOrDefault();
            exhibit.description = DescriptionTextBox.Text;
            
            database.Exhibits.Add(exhibit);
            database.SaveChanges();
            this.Hide();
        }

        private void EditButton_CLick(object sender, RoutedEventArgs e)
        {
            var query = database.Exhibits.Where(c => c.id == id).Select(c => c).FirstOrDefault();
            query.name = ExhibitNameTextBox.Text;
            query.type_id = database.ExhibitTypes.Where(c => c.name == ExhibitTypeComboBox.Text).Select(c => c.id).FirstOrDefault();
            query.hall_id = database.Halls.Where(c => c.name == HallsComboBox.Text).Select(c => c.id).FirstOrDefault();
            query.author_id = database.Authors.Where(c => c.full_name == AuthorsComboBox.Text).Select(c => c.id).FirstOrDefault();
            query.description = DescriptionTextBox.Text;

            database.SaveChanges();
            this.Hide();
        }
    }
}
