using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ExcursionsCompany.Models;
using Microsoft.Ajax.Utilities;
using System.Xml.Linq;
using System.Drawing;
using System.Diagnostics;
using System.IO;

using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Action;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

using System.Web.UI.WebControls;
using iText.Kernel.Font;
using iText.Layout.Borders;
using System.Net.Mail;
using Syroot.Windows.IO;

namespace ExcursionsCompany.Controllers
{
    public class ExcursionsController : Controller
    {
        private MuseumDataEntities db = new MuseumDataEntities();

        // GET: Excursions
        public ActionResult Index(string searchString)
        {

            var exc = from s in db.Excursions
                            select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                exc = exc.Where(s => s.ExcursionTypes.name.Contains(searchString));
            }

            return View(exc.ToList());
        }

        // GET: Excursions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Excursions excursions = db.Excursions.Find(id);
            if (excursions == null)
            {
                return HttpNotFound();
            }
            return View(excursions);
        }

        // GET: Excursions/Create
        public ActionResult Create()
        {
            ViewBag.extype_id = new SelectList(db.ExcursionTypes, "id", "name");
            ViewBag.guide_id = new SelectList(db.Workers, "id", "login");
            return View();
        }

        // POST: Excursions/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,extype_id,start_datetime,price,guide_id,deleted")] Excursions excursions)
        {
            if (ModelState.IsValid)
            {
                db.Excursions.Add(excursions);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.extype_id = new SelectList(db.ExcursionTypes, "id", "name", excursions.extype_id);
            ViewBag.guide_id = new SelectList(db.Workers, "id", "login", excursions.guide_id);
            return View(excursions);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Details([Bind(Include = "id,full_name,phone_num,email,excursion_id,people_amount,totalprice,deleted")] Tickets tickets)
        {
            if (ModelState.IsValid)
            {
                db.Tickets.Add(tickets);
                db.SaveChanges();

                string downloadsPath = KnownFolders.Downloads.Path;
                downloadsPath += "\\Ticket_" + tickets.id + ".pdf";

                CreatePDF(tickets, downloadsPath);

                var mail = MuseumMail.CreateMail("Музей", "MuseumTicketInfo@gmail.com", tickets.email, "Ваш билет в музей!", downloadsPath);
                MuseumMail.SendMail("smtp.gmail.com", 587, "MuseumTicketInfo@gmail.com", "xmuewgguwuzzceto", mail);
                return RedirectToAction("Index","ExcursionTypes");
            }
            return View("Index");
        }

        private void CreatePDF(Tickets ticket, string downloadsPath)
        {
            
            string name = ticket.full_name + " | человек: " + ticket.people_amount;
            int ids = ticket.excursion_id;
            var Excursion = db.Excursions.Where(c => c.id == ids).Select(c => c).First();
            var ExcursionType = db.ExcursionTypes.Where(c => c.id == Excursion.extype_id).Select(c => c).First();

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

            // Close document
            document.Close();
            Process.Start(downloadsPath);
        }

        // GET: Excursions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Excursions excursions = db.Excursions.Find(id);
            if (excursions == null)
            {
                return HttpNotFound();
            }
            ViewBag.extype_id = new SelectList(db.ExcursionTypes, "id", "name", excursions.extype_id);
            ViewBag.guide_id = new SelectList(db.Workers, "id", "login", excursions.guide_id);
            return View(excursions);
        }

        // POST: Excursions/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,extype_id,start_datetime,price,guide_id,deleted")] Excursions excursions)
        {
            if (ModelState.IsValid)
            {
                db.Entry(excursions).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.extype_id = new SelectList(db.ExcursionTypes, "id", "name", excursions.extype_id);
            ViewBag.guide_id = new SelectList(db.Workers, "id", "login", excursions.guide_id);
            return View(excursions);
        }

        // GET: Excursions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Excursions excursions = db.Excursions.Find(id);
            if (excursions == null)
            {
                return HttpNotFound();
            }
            return View(excursions);
        }

        // POST: Excursions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Excursions excursions = db.Excursions.Find(id);
            db.Excursions.Remove(excursions);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
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
            SmtpClient smtp = new SmtpClient(host, smtpPost);
            smtp.Credentials = new NetworkCredential(emailFrom, pass);
            smtp.EnableSsl = true;

            smtp.Send(mail);
        }
    }
}
