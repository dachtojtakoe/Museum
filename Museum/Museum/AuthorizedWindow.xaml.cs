using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using Syroot.Windows.IO;
using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace Museum
{
    public partial class AuthorizedWindow : Window
    {
        MuseumEntities1 database = new MuseumEntities1();
        MainWindow mw;
        CreateExcursion CExcursion;
        CreateExcursionType CExType;
        EditTicket etick;
        CreateExhibits exhbt;

        string search;

        Tables table;
        Tables currenttable { get { return table; } set { MenuItemsBackColor(Colors.Transparent); table = value; MenuItemsBackColor(Colors.White); } }
        enum Tables
        {
            Excursions,
            Extypes,
            Tickets,
            Exhibits,
            Halls,
            ExhibitTypes,
            Workers,
            Roles,
            Authors
        }

        public AuthorizedWindow()
        {
            InitializeComponent();

            GetDataExcursions(string.Empty);

            if (User.role == User.Roles.Admin)
            {
                this.Title = "Администрирование " + "[" + User.Surname + " " + User.Name + "]";
                HideForAdmin();
            }
            else if (User.role == User.Roles.Guide)
            {
                this.Title = "Информация для экскурсоводов " + "[" + User.Surname + " " + User.Name + "]";
                HideForGuides();
            }

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
            mw = new MainWindow();
            mw.Show();
        }

        private void GetDataExcursions(object sender, RoutedEventArgs e)
        {
            GetDataExcursions(string.Empty);
            SearchBox.Text = "";
        }

        private void GetDataExcursions(string str)
        {
            currenttable = Tables.Excursions;
            AdditionalGrid.Visibility = Visibility.Hidden;

            var quary = from excursion in database.Excursions
                        where excursion.deleted != true
                        where excursion.ExcursionTypes.name.Contains(str)
                        where excursion.start_datetime > DateTime.Now
                        select new { Id = excursion.id, Название = excursion.ExcursionTypes.name, Дата = excursion.start_datetime, Длительность = excursion.ExcursionTypes.duration, Цена = excursion.price, Гид = excursion.Workers.surname + " " + excursion.Workers.name, Описание = excursion.ExcursionTypes.description };

            if (OnlyMyExcursions.IsChecked == true)
            {
                quary = from excursion in database.Excursions
                        where excursion.deleted != true
                        where excursion.Workers.surname.Contains(User.Surname)
                        where excursion.ExcursionTypes.name.Contains(str)
                        where excursion.start_datetime > DateTime.Now
                        select new { Id = excursion.id, Название = excursion.ExcursionTypes.name, Дата = excursion.start_datetime, Длительность = excursion.ExcursionTypes.duration, Цена = excursion.price, Гид = excursion.Workers.surname + " " + excursion.Workers.name, Описание = excursion.ExcursionTypes.description };
            }

            if (NonActualExcursions.IsChecked == true)
            {
                quary = from excursion in database.Excursions
                        where excursion.deleted != true
                        where excursion.ExcursionTypes.name.Contains(str)
                        select new { Id = excursion.id, Название = excursion.ExcursionTypes.name, Дата = excursion.start_datetime, Длительность = excursion.ExcursionTypes.duration, Цена = excursion.price, Гид = excursion.Workers.surname + " " + excursion.Workers.name, Описание = excursion.ExcursionTypes.description };
            }

            DataGrid.ItemsSource = quary.ToList();
            LockButtons();
            CreateButton.IsEnabled = true;
            OnlyMyExcursions.IsEnabled = true;
            NonActualExcursions.IsEnabled = true;
        }

        private void GetDataExtypes(object sender, RoutedEventArgs e)
        {
            GetDataExtypes(string.Empty);
            SearchBox.Text = "";

        }

        private void GetDataExtypes(string str)
        {

            currenttable = Tables.Extypes;
            AdditionalGrid.Visibility = Visibility.Hidden;

            var quary = from extype in database.ExcursionTypes
                        where extype.deleted != true
                        where extype.name.Contains(str)
                        select new { Id = extype.id, Название = extype.name, Длительность = extype.duration, Описание = extype.description };

            DataGrid.ItemsSource = quary.ToList();
            LockButtons();
            CreateButton.IsEnabled = true;
        }
        
        private void GetDataTickets(object sender, RoutedEventArgs e)
        {
            GetDataTickets(string.Empty);
            SearchBox.Text = "";

        }

        private void GetDataTickets(string str)
        {
            int searchNum;
            int.TryParse(str, out searchNum);
            
            currenttable = Tables.Tickets;
            AdditionalGrid.Visibility = Visibility.Hidden;


            if (searchNum != 0)
            {
                var quary = from ticket in database.Tickets
                            where ticket.deleted != true
                            where ticket.id == searchNum
                            select new { Id = ticket.id, ФИО = ticket.full_name, Телефон = ticket.phone_num, Почта = ticket.email, IdЭкскурсии = ticket.Excursions.id, НазваниеЭкскурсии = ticket.Excursions.ExcursionTypes.name, Дата = ticket.Excursions.start_datetime, Людей = ticket.people_amount, Сумма = ticket.totalprice };

                if (OnlyMyExcursions.IsChecked == true)
                {
                    quary = from ticket in database.Tickets
                            where ticket.deleted != true
                            where ticket.Excursions.Workers.surname.Contains(User.Surname)
                            where ticket.id == searchNum
                            select new { Id = ticket.id, ФИО = ticket.full_name, Телефон = ticket.phone_num, Почта = ticket.email, IdЭкскурсии = ticket.Excursions.id, НазваниеЭкскурсии = ticket.Excursions.ExcursionTypes.name, Дата = ticket.Excursions.start_datetime, Людей = ticket.people_amount, Сумма = ticket.totalprice };
                }
                DataGrid.ItemsSource = quary.ToList();
            }
            else
            {
                var quary = from ticket in database.Tickets
                            where ticket.deleted != true
                            select new { Id = ticket.id, ФИО = ticket.full_name, Телефон = ticket.phone_num, Почта = ticket.email, IdЭкскурсии = ticket.Excursions.id, НазваниеЭкскурсии = ticket.Excursions.ExcursionTypes.name, Дата = ticket.Excursions.start_datetime, Людей = ticket.people_amount, Сумма = ticket.totalprice };

                if (OnlyMyExcursions.IsChecked == true)
                {
                    quary = from ticket in database.Tickets
                            where ticket.deleted != true
                            where ticket.Excursions.Workers.surname.Contains(User.Surname)
                            select new { Id = ticket.id, ФИО = ticket.full_name, Телефон = ticket.phone_num, Почта = ticket.email, IdЭкскурсии = ticket.Excursions.id, НазваниеЭкскурсии = ticket.Excursions.ExcursionTypes.name, Дата = ticket.Excursions.start_datetime, Людей = ticket.people_amount, Сумма = ticket.totalprice };
                }
                DataGrid.ItemsSource = quary.ToList();
            }


            LockButtons();
            OnlyMyExcursions.IsEnabled = true;

        }

        private void GetDataExhibits(object sender, RoutedEventArgs e)
        {
            GetDataExhibits(string.Empty);
            SearchBox.Text = "";

        }

        private void GetDataExhibits(string str)
        {
            currenttable = Tables.Exhibits;
            AdditionalGrid.Visibility = Visibility.Hidden;

            var quary = from exhibit in database.Exhibits
                        where exhibit.deleted != true
                        where exhibit.name.Contains(str)
                        select new { Id = exhibit.id, Название = exhibit.name, idТипа = exhibit.type_id, НазваниеТипа = exhibit.ExhibitTypes.name, idЗала = exhibit.hall_id, НазваниеЗала = exhibit.Halls.name, Автор = exhibit.Authors.full_name, Описание = exhibit.description };

            DataGrid.ItemsSource = quary.ToList();
            LockButtons();
            CreateButton.IsEnabled = true;

        }

        private void GetDataHalls(object sender, RoutedEventArgs e)
        {
            GetDataHalls(string.Empty);
            SearchBox.Text = "";

        }

        private void GetDataHalls(string str)
        {
            currenttable = Tables.Halls;
            AdditionalGrid.Visibility = Visibility.Hidden;

            var quary = from hall in database.Halls
                        where hall.deleted != true
                        where hall.name.Contains(str)
                        select new { Id = hall.id, Название = hall.name };

            DataGrid.ItemsSource = quary.ToList();
            LockButtons();
        }

        private void GetDataTypes(object sender, RoutedEventArgs e)
        {
            GetDataTypes(string.Empty);
            SearchBox.Text = "";

        }

        private void GetDataTypes(string str)
        {
            currenttable = Tables.ExhibitTypes;
            AdditionalGrid.Visibility = Visibility.Hidden;

            var quary = from type in database.ExhibitTypes
                        where type.deleted != true
                        where type.name.Contains(str)
                        select new { Id = type.id, Название = type.name };

            DataGrid.ItemsSource = quary.ToList();
            LockButtons();
        }

        private void GetDataWorkers(object sender, RoutedEventArgs e)
        {
            GetDataWorkers(string.Empty);
            SearchBox.Text = "";

        }

        private void GetDataWorkers(string str)
        {
            currenttable = Tables.Workers;
            AdditionalGrid.Visibility = Visibility.Hidden;

            var quary = from worker in database.Workers
                        where worker.deleted != true
                        where worker.surname.Contains(str)
                        select new { Id = worker.id, Фамилия = worker.surname, Имя = worker.name, Отчество = worker.patronymic, Должность = worker.Roles.name };

            DataGrid.ItemsSource = quary.ToList();
            LockButtons();
            //CreateButton.IsEnabled = true;

        }

        private void GetDataRoles(object sender, RoutedEventArgs e)
        {
            GetDataRoles(string.Empty);
            SearchBox.Text = "";

        }

        private void GetDataRoles(string str)
        {
            currenttable = Tables.Roles;
            AdditionalGrid.Visibility = Visibility.Hidden;

            var quary = from role in database.Roles
                        where role.deleted != true
                        where role.name.Contains(str)
                        select new { role.id, role.name };

            DataGrid.ItemsSource = quary.ToList();
            LockButtons();
        }

        private void GetDataAuthors(object sender, RoutedEventArgs e)
        {
            GetDataAuthors(string.Empty);
            SearchBox.Text = "";

        }

        private void GetDataAuthors(string str)
        {
            currenttable = Tables.Authors;
            AdditionalGrid.Visibility = Visibility.Hidden;

            var quary = from author in database.Authors
                        where author.deleted != true
                        where author.full_name.Contains(str)
                        select new { Id = author.id, ФИО = author.full_name };

            DataGrid.ItemsSource = quary.ToList();
            LockButtons();
        }


        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataGrid.SelectedItem != null)
            {
                object obj = DataGrid.SelectedItem;
                int idd = Convert.ToInt32((DataGrid.SelectedCells[0].Column.GetCellContent(obj) as TextBlock).Text);
                DeleteButton.IsEnabled = true;
            }

            else if (DataGrid.SelectedItem == null)
            {
                LockButtons();
            }

            if (DataGrid.SelectedItem != null && table == Tables.Excursions && User.role != User.Roles.Client)
            {
                AdditionalGrid.Visibility = Visibility.Visible;
                object obj = DataGrid.SelectedItem;
                int idd = Convert.ToInt32((DataGrid.SelectedCells[0].Column.GetCellContent(obj) as TextBlock).Text);

                var query = from ticket in database.Tickets
                            where ticket.excursion_id == idd
                            where ticket.deleted != true
                            orderby ticket.id
                            select new { Id = ticket.id, ФИО = ticket.full_name, Телефон = ticket.phone_num, Почта = ticket.email, Людей = ticket.people_amount, Сумма = ticket.totalprice };
                AdditionalGrid.ItemsSource = query.ToList();

                if (AdditionalGrid.Items.Count == 0)
                {
                    AdditionalGrid.Visibility = Visibility.Hidden;
                }
            }

            if (DataGrid.SelectedItem != null && (table == Tables.Excursions || currenttable == Tables.Tickets))
            {
                EditButton.IsEnabled = true;

                BuyTicket.IsEnabled = true;
            }
         

            if (DataGrid.SelectedItem != null && (table == Tables.Tickets || table == Tables.Exhibits || table == Tables.Extypes))
            {
                EditButton.IsEnabled = true;
            }

            if (DataGrid.SelectedItem != null && table == Tables.Extypes)
            {
                object obj = DataGrid.SelectedItem;
                int idd = Convert.ToInt32((DataGrid.SelectedCells[0].Column.GetCellContent(obj) as TextBlock).Text);

                var quary = database.ExcursionTypes.Where(c => c.id == idd).Select(c => c.Image).FirstOrDefault();

                if (quary != null)
                {
                    ExcursionImage.Visibility = Visibility.Visible;
                    ExcursionImage.Source = ToImage(quary);
                }
                else
                    ExcursionImage.Visibility = Visibility.Hidden;
            }
        }

        public BitmapImage ToImage(byte[] array)
        {
            using (var ms = new System.IO.MemoryStream(array))
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = ms;
                image.EndInit();
                return image;
            }
        }

        private void LockButtons()
        {
            CreateButton.IsEnabled = false;
            EditButton.IsEnabled = false;
            DeleteButton.IsEnabled = false;
            BuyTicket.IsEnabled = false;
            OnlyMyExcursions.IsEnabled = false;
            NonActualExcursions.IsEnabled = false;
            ExcursionImage.Visibility = Visibility.Hidden;

        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            object obj = DataGrid.SelectedItem;
            int idd = Convert.ToInt32((DataGrid.SelectedCells[0].Column.GetCellContent(obj) as TextBlock).Text);

            if (table == Tables.Excursions)
            {
                database.Excursions.Where(c => c.id == idd).Select(c => c).FirstOrDefault().deleted = true;
                database.SaveChanges();
                GetDataExcursions(string.Empty);
            }
            else if (table == Tables.Extypes)
            {
                database.ExcursionTypes.Where(c => c.id == idd).Select(c => c).FirstOrDefault().deleted = true;
                database.SaveChanges();
                GetDataExtypes(string.Empty);
            }
            else if (table == Tables.Tickets)
            {
                database.Tickets.Where(c => c.id == idd).Select(c => c).FirstOrDefault().deleted = true;
                database.SaveChanges();
                GetDataTickets(string.Empty);
            }
            else if (table == Tables.Exhibits)
            {
                database.Exhibits.Where(c => c.id == idd).Select(c => c).FirstOrDefault().deleted = true;
                database.SaveChanges();
                GetDataExhibits(string.Empty);
            }
            else if (table == Tables.Halls)
            {
                database.Halls.Where(c => c.id == idd).Select(c => c).FirstOrDefault().deleted = true;
                database.SaveChanges();
                GetDataHalls(string.Empty);
            }
            else if (table == Tables.ExhibitTypes)
            {
                database.ExhibitTypes.Where(c => c.id == idd).Select(c => c).FirstOrDefault().deleted = true;
                database.SaveChanges();
                GetDataTypes(string.Empty);
            }
            else if (table == Tables.Workers)
            {
                database.Workers.Where(c => c.id == idd).Select(c => c).FirstOrDefault().deleted = true;
                database.SaveChanges();
                GetDataWorkers(string.Empty);
            }
            else if (table == Tables.Roles)
            {
                database.Roles.Where(c => c.id == idd).Select(c => c).FirstOrDefault().deleted = true;
                database.SaveChanges();
                GetDataRoles(string.Empty);
            }
            else if (table == Tables.Authors)
            {
                database.Authors.Where(c => c.id == idd).Select(c => c).FirstOrDefault().deleted = true;
                database.SaveChanges();
                GetDataAuthors(string.Empty);
            }
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            if (table == Tables.Excursions)
            {
                CExcursion = new CreateExcursion();
                CExcursion.ShowDialog();
                GetDataExcursions(string.Empty);
            }
            else if (table == Tables.Extypes)
            {
                CExType = new CreateExcursionType();
                CExType.ShowDialog();
                GetDataExtypes(string.Empty);
            }
            else if (table == Tables.Exhibits)
            {
                exhbt = new CreateExhibits();
                exhbt.ShowDialog();
                GetDataExhibits(string.Empty);
            }
        }

        private void BuyTicket_Click(object sender, RoutedEventArgs e)
        {
            if (currenttable == Tables.Excursions)
            {
                object obj = DataGrid.SelectedItem;
                int exid = Convert.ToInt32((DataGrid.SelectedCells[0].Column.GetCellContent(obj) as TextBlock).Text);
                if (database.Tickets.Where(c => c.excursion_id == exid).Count() < 15)
                {
                    int price = Convert.ToInt32((DataGrid.SelectedCells[4].Column.GetCellContent(obj) as TextBlock).Text);

                    etick = new EditTicket(exid, price);
                    etick.ShowDialog();
                }
                else
                    MessageBox.Show("Количество выданных билетов на выбранную экскурсию уже достигло 15", "Невозможно выдать билет!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (currenttable == Tables.Tickets)
            {
                object obj = DataGrid.SelectedItem;
                int exid = Convert.ToInt32((DataGrid.SelectedCells[0].Column.GetCellContent(obj) as TextBlock).Text);
                string ticketPath = KnownFolders.Downloads.Path + "\\Ticket_" + exid + ".pdf";

                try
                {
                    Process.Start(ticketPath);
                }
                catch
                {
                    int price = Convert.ToInt32((DataGrid.SelectedCells[4].Column.GetCellContent(obj) as TextBlock).Text);
                    var ticket = database.Tickets.Where(c => c.id == exid).Select(c => c).First();
                    CreatePDF(ticket, ticketPath);

                }
            }

        }

        private void CreatePDF(Tickets ticket, string downloadsPath)
        {

            string name = ticket.full_name + " | человек: " + ticket.people_amount;
            int ids = ticket.excursion_id;
            var Excursion = database.Excursions.Where(c => c.id == ids).Select(c => c).First();
            var ExcursionType = database.ExcursionTypes.Where(c => c.id == Excursion.extype_id).Select(c => c).First();

            PdfWriter writer = new PdfWriter(downloadsPath);
            PdfDocument pdf = new PdfDocument(writer);
            Document document = new Document(pdf);
            var font = PdfFontFactory.CreateFont("C:\\Windows\\Fonts\\arial.ttf", "Identity-H");
            document.SetFont(font);

            // Table
            iText.Layout.Element.Table table = new iText.Layout.Element.Table(2, false);
            Cell cell11 = new Cell(1, 1)
               .SetBackgroundColor(ColorConstants.GRAY)
               .Add(new Paragraph("Билет №" + ticket.id).SetFontSize(18));
            Cell cell12 = new Cell(1, 1)
               .SetBackgroundColor(ColorConstants.GRAY)
               .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
               .SetBorderLeft(new DashedBorder(1))
               .Add(new Paragraph("№" + ticket.id).SetFontSize(18));

            Cell cell21 = new Cell(1, 1)
               .Add(new Paragraph("Экскурсия").SetFontSize(8))
               .Add(new Paragraph(ExcursionType.name).SetFontSize(16))
               .Add(new Paragraph("ФИО").SetFontSize(8))
               .Add(new Paragraph(name))
               .Add(new Paragraph("Начало экскурсии").SetFontSize(8))
               .Add(new Paragraph(Excursion.start_datetime.ToString()))
               .Add(new Paragraph("Стоимость билета").SetFontSize(8))
               .Add(new Paragraph(ticket.totalprice.ToString() + " Руб."));

            Cell cell22 = new Cell(1, 1)
               .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
               .SetBorderLeft(new DashedBorder(1))
               .SetBackgroundColor(ColorConstants.LIGHT_GRAY)
               .Add(new Paragraph("К\nО\nН\nТ\nР\nО\nЛ\nЬ").SetFontSize(10));

            table.AddCell(cell11);
            table.AddCell(cell12);
            table.AddCell(cell21);
            table.AddCell(cell22);

            document.Add(table);

            document.Close();
            Process.Start(downloadsPath);
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (table == Tables.Tickets)
            {
                object obj = DataGrid.SelectedItem;
                int id = Convert.ToInt32((DataGrid.SelectedCells[0].Column.GetCellContent(obj) as TextBlock).Text);

                string fullname = (DataGrid.SelectedCells[1].Column.GetCellContent(obj) as TextBlock).Text;
                string phone = (DataGrid.SelectedCells[2].Column.GetCellContent(obj) as TextBlock).Text;
                string email = (DataGrid.SelectedCells[3].Column.GetCellContent(obj) as TextBlock).Text;

                etick = new EditTicket(id, fullname, phone, email);
                etick.ShowDialog();
                GetDataTickets(string.Empty);
            }
            else if (table == Tables.Exhibits)
            {
                object obj = DataGrid.SelectedItem;
                int id = Convert.ToInt32((DataGrid.SelectedCells[0].Column.GetCellContent(obj) as TextBlock).Text);
                string name = (DataGrid.SelectedCells[1].Column.GetCellContent(obj) as TextBlock).Text;
                string type = (DataGrid.SelectedCells[3].Column.GetCellContent(obj) as TextBlock).Text;
                string hall = (DataGrid.SelectedCells[5].Column.GetCellContent(obj) as TextBlock).Text;
                string author = (DataGrid.SelectedCells[6].Column.GetCellContent(obj) as TextBlock).Text;
                string description = string.Empty;
                if ((DataGrid.SelectedCells[7].Column.GetCellContent(obj) as TextBlock).Text != null)
                {
                    description = (DataGrid.SelectedCells[7].Column.GetCellContent(obj) as TextBlock).Text;
                }

                exhbt = new CreateExhibits(id, name, type, hall, author, description);
                exhbt.ShowDialog();
                GetDataExhibits(string.Empty);
            }
            else if (table == Tables.Extypes)
            {
                object obj = DataGrid.SelectedItem;
                int id = Convert.ToInt32((DataGrid.SelectedCells[0].Column.GetCellContent(obj) as TextBlock).Text);
                string name = (DataGrid.SelectedCells[1].Column.GetCellContent(obj) as TextBlock).Text;
                string duration = (DataGrid.SelectedCells[2].Column.GetCellContent(obj) as TextBlock).Text;
                duration = duration.Substring(0, 5);
                string description = string.Empty;
                if ((DataGrid.SelectedCells[3].Column.GetCellContent(obj) as TextBlock).Text != null)
                {
                    description = (DataGrid.SelectedCells[3].Column.GetCellContent(obj) as TextBlock).Text;
                }
                ImageSource img = null;
                if (ExcursionImage.Visibility == Visibility.Visible)
                {
                    img = ExcursionImage.Source;
                }
                CExType = new CreateExcursionType(id, name, duration, description, img);
                CExType.ShowDialog();
                GetDataExtypes(string.Empty);
            }
            else if (table == Tables.Excursions)
            {
                object obj = DataGrid.SelectedItem;
                int id = Convert.ToInt32((DataGrid.SelectedCells[0].Column.GetCellContent(obj) as TextBlock).Text);
                string name = (DataGrid.SelectedCells[1].Column.GetCellContent(obj) as TextBlock).Text;
                string starttime = (DataGrid.SelectedCells[2].Column.GetCellContent(obj) as TextBlock).Text;
                string price = (DataGrid.SelectedCells[4].Column.GetCellContent(obj) as TextBlock).Text;
                string guide = (DataGrid.SelectedCells[5].Column.GetCellContent(obj) as TextBlock).Text;

                CExcursion = new CreateExcursion(id, name, starttime,price,guide);
                CExcursion.ShowDialog();
                GetDataExcursions(string.Empty);

            }
        }

        private void OnlyMyExcursions_Click(object sender, RoutedEventArgs e)
        {
            if (currenttable == Tables.Excursions)
            {
                GetDataExcursions(SearchBox.Text);
            }
            else if (currenttable == Tables.Tickets)
            {
                GetDataTickets(SearchBox.Text);
            }
        }

        private void NonActualExcursions_Click(object sender, RoutedEventArgs e)
        {
            GetDataExcursions(SearchBox.Text);
        }


        private void HideForAdmin()
        {
            OnlyMyExcursions.Visibility = Visibility.Hidden;
        }

        private void HideForGuides()
        {
            CreateButton.Visibility = Visibility.Hidden;
            EditButton.Visibility = Visibility.Hidden;
            DeleteButton.Visibility = Visibility.Hidden;
            NonActualExcursions.Visibility= Visibility.Hidden;

            WorkersMenuItem.Visibility = Visibility.Collapsed;
            RolesMenuItem.Visibility = Visibility.Collapsed;
        }


        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            search = SearchBox.Text;
            if (table == Tables.Excursions)
            {
                GetDataExcursions(SearchBox.Text);
            }
            else if (table == Tables.Extypes)
            {
                GetDataExtypes(SearchBox.Text);
            }
            else if (table == Tables.Tickets)
            {
                GetDataTickets(SearchBox.Text);
            }
            else if (table == Tables.Exhibits)
            {
                GetDataExhibits(SearchBox.Text);
            }
            else if (table == Tables.Halls)
            {
                GetDataHalls(SearchBox.Text);
            }
            else if (table == Tables.ExhibitTypes)
            {
                GetDataTypes(SearchBox.Text);
            }
            else if (table == Tables.Workers)
            {
                GetDataWorkers(SearchBox.Text);
            }
            else if (table == Tables.Roles)
            {
                GetDataRoles(SearchBox.Text);
            }
            else if (table == Tables.Authors)
            {
                GetDataAuthors(SearchBox.Text);
            }
        }

        private void MenuItemsBackColor(System.Windows.Media.Color color)
        {
            switch (table)
            {
                case Tables.Excursions:
                    ExcursionMenuItem.Background = new SolidColorBrush(color);
                    break;
                case Tables.Extypes:
                    ExtypesMenuItem.Background = new SolidColorBrush(color);
                    break;
                case Tables.Tickets:
                    TicketsMenuItem.Background = new SolidColorBrush(color);
                    break;
                case Tables.Exhibits:
                    ExhibitsMenuItem.Background = new SolidColorBrush(color);
                    break;
                case Tables.Halls:
                    HallsMenuItem.Background = new SolidColorBrush(color);
                    break;
                case Tables.ExhibitTypes:
                    ExhibittypesMenuItem.Background = new SolidColorBrush(color);
                    break;
                case Tables.Workers:
                    WorkersMenuItem.Background = new SolidColorBrush(color);
                    break;
                case Tables.Roles:
                    RolesMenuItem.Background = new SolidColorBrush(color);
                    break;
                case Tables.Authors:
                    AuthorsMenuItem.Background = new SolidColorBrush(color);
                    break;
                default:
                    break;
            }
        }

        private void DataGrid_AutoGeneratingColumn_1(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyName == "Дата")
            {
                DataGridTextColumn column = e.Column as DataGridTextColumn;
                Binding binding = column.Binding as Binding;
                binding.StringFormat = "dd.MM.yyyy HH:mm:ss";
            }
        }

      
    }
}
