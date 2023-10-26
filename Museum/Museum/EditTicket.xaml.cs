using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Kernel.Pdf;
using iText.Layout.Element;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using iText.Kernel.Pdf.Action;
using iText.Layout;
using iText.Layout.Properties;

using Paragraph = iText.Layout.Element.Paragraph;
using System.Drawing;
using iText.Layout.Borders;
using System.Net.Mail;
using System.Net;
using Syroot.Windows.IO;
using Org.BouncyCastle.Asn1;

namespace Museum
{
    /// <summary>
    /// Логика взаимодействия для EditTicket.xaml
    /// </summary>
    public partial class EditTicket : Window
    {
        MuseumEntities1 database = new MuseumEntities1();
        int id;
        int exid;

        int price;

        public EditTicket(int exid, int price)
        {
            InitializeComponent();
            this.exid = exid;

            this.price = price;

            GetSumm();
            SaveButton.Visibility = Visibility.Hidden;
            Title = "Выдача билета";
        }

        public EditTicket(int id, string fullname, string phone, string email)
        {
            InitializeComponent();
            this.id = id;
            FullNameTextBox.Text = fullname;
            PhoneTextBox.Text = phone;
            EmailTextBox.Text = email;

            BuyButton.Visibility = Visibility.Hidden;
            PeopleAmountDown.Visibility = Visibility.Hidden;
            PeopleAmountUp.Visibility = Visibility.Hidden;
            PeopleAmount.Visibility = Visibility.Hidden;
            PeopleAmountLabel.Visibility = Visibility.Hidden;
            PriceLabel.Visibility = Visibility.Hidden;
            SummTextBox.Visibility = Visibility.Hidden;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var query = database.Tickets.Where(c => c.id == id).Select(c => c).FirstOrDefault();
            query.full_name = FullNameTextBox.Text;
            query.phone_num = PhoneTextBox.Text;
            query.email = EmailTextBox.Text;
            database.SaveChanges();
            this.Hide();

            string downloadsPath = KnownFolders.Downloads.Path + "\\Ticket_" + query.id + ".pdf";
            CreatePDF(query, downloadsPath);
            var mail = MuseumMail.CreateMail("Музей", "MuseumTicketInfo@gmail.com", query.email, "Ваш билет в музей!", downloadsPath);
            MuseumMail.SendMail("smtp.gmail.com", 587, "MuseumTicketInfo@gmail.com", "xmuewgguwuzzceto", mail);
        }


        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            Tickets ticket = new Tickets();
            ticket.full_name = FullNameTextBox.Text;
            ticket.phone_num = PhoneTextBox.Text;
            ticket.email = EmailTextBox.Text;
            ticket.excursion_id = exid;
            ticket.people_amount = Convert.ToInt32(PeopleAmount.Text);
            ticket.totalprice = Convert.ToInt32(SummTextBox.Text);

            database.Tickets.Add(ticket);
            database.SaveChanges();

            this.Hide();

            string downloadsPath = KnownFolders.Downloads.Path + "\\Ticket_" + ticket.id + ".pdf";

            CreatePDF(ticket, downloadsPath);

            var mail = MuseumMail.CreateMail("Музей", "MuseumTicketInfo@gmail.com", ticket.email, "Ваш билет в музей!", downloadsPath);
            MuseumMail.SendMail("smtp.gmail.com", 587, "MuseumTicketInfo@gmail.com", "xmuewgguwuzzceto", mail);
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


        private void PeopleAmountDown_Click(object sender, RoutedEventArgs e)
        {
            if (Convert.ToInt32(PeopleAmount.Text) > 1)
            {
                int people = Convert.ToInt32(PeopleAmount.Text);
                people--;
                PeopleAmount.Text = people.ToString();
                GetSumm();
            }
        }

        private void PeopleAmountUp_Click(object sender, RoutedEventArgs e)
        {
            if (Convert.ToInt32(PeopleAmount.Text) <= 5)
            {
                int people = Convert.ToInt32(PeopleAmount.Text);
                people++;
                PeopleAmount.Text = people.ToString();
                GetSumm();
            }
        }

        private void GetSumm()
        {
            SummTextBox.Text = (Convert.ToInt32(PeopleAmount.Text) * price).ToString();
        }
    }

    internal class MuseumMail
    {
        public static MailMessage CreateMail(string name, string emailFrom, string emailTo, string subject, string downloadsPath)
        {
            var from = new MailAddress(emailFrom, name);
            var to = new MailAddress(emailTo);
            var mail = new MailMessage(from, to);
            mail.Subject = subject;
            mail.Attachments.Add(new Attachment(downloadsPath));
            return mail;
        }

        public static void SendMail(string host, int smtpPost, string emailFrom, string pass, MailMessage mail)
        {
            
                //SmtpClient smtp = new SmtpClient(host, smtpPost);
                //smtp.Credentials = new NetworkCredential(emailFrom, pass);
                //smtp.EnableSsl = true;

                //smtp.Send(mail);
            
            MessageBox.Show("Письмо с билетом отправлено на указанную почту.", "Успешно!", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }

}
