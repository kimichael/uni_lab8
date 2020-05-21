using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace Lab8
{
    public partial class Библиотека : Form
    {

        private XDocument xdoc;

        List<BookReservationRecord> records = new List<BookReservationRecord>();

        List<BookReservationRecord> filteredRecords = new List<BookReservationRecord>();

        public Библиотека()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            loadReservations();
        }

        private void loadReservations() {
            var filename = "reservations.xml";
            var currentDirectory = Directory.GetCurrentDirectory();
            var reservationsFile = Path.Combine(currentDirectory, filename);
            if (!File.Exists(reservationsFile))
            {
                XDocument doc = new XDocument(
                        new XElement(BookReservationRecord.RESERVATIONS_TAG, XElement.EmptySequence)
                    );
                doc.Save(reservationsFile);
            }

            try {
                XDocument xdoc = XDocument.Load(reservationsFile);
                XElement recordsElement = xdoc.Root;
                var elements = xdoc.Element(BookReservationRecord.RESERVATIONS_TAG).Elements("BookReservation");

                try
                {
                    foreach (XElement xelement in elements)
                    {
                        var record = BookReservationRecord.fromXElement(xelement);
                        records.Add(record);
                        filteredRecords.Add(record);
                    }
                }
                catch (ArgumentException exception)
                {
                    MessageBox.Show("Неправильный формат файла", "Ошибка");
                }
            } catch (XmlException exception) {
                MessageBox.Show("XML файл поврежден", "Ошибка");
            }
            

            filterRecords();
        }

        private void saveToXML()
        {
            var filename = "reservations.xml";
            var currentDirectory = Directory.GetCurrentDirectory();
            var reservationsFile = Path.Combine(currentDirectory, filename);
            
            List<XElement> xelements = new List<XElement>();
            foreach (BookReservationRecord record in records) {
                xelements.Add(record.toXElement());
            }

            XDocument doc = new XDocument(
                    new XElement(BookReservationRecord.RESERVATIONS_TAG, xelements)
                );
            doc.Save(reservationsFile);
           
        }

        private void add_button_Click(object sender, EventArgs e)
        {
            if (
                    ticketNumber_Input.Text.Length <= 0 ||
                    readerFirstName_Input.Text.Length <= 0 ||
                    readerSecondName_Input.Text.Length <= 0 ||
                    takeoutDate_Input.Text.Length <= 0 ||
                    returnDays_Input.Text.Length <= 0 ||
                    author_Input.Text.Length <= 0 ||
                    name_Input.Text.Length <= 0 ||
                    publishYear_Input.Text.Length <= 0 ||
                    publisher_Input.Text.Length <= 0 ||
                    cost_Input.Text.Length <= 0
                ) {
                return;
            }
            
            addReservation(
                        Convert.ToInt32(ticketNumber_Input.Text),
                        readerFirstName_Input.Text,
                        readerSecondName_Input.Text,
                        takeoutDate_Input.Text,
                        Convert.ToInt32(returnDays_Input.Text),
                        author_Input.Text,
                        name_Input.Text,
                        Convert.ToInt32(publishYear_Input.Text),
                        publisher_Input.Text,
                        Convert.ToInt32(cost_Input.Text)
                    );
            ticketNumber_Input.Clear();
            readerFirstName_Input.Clear();
            readerSecondName_Input.Clear();
            takeoutDate_Input.Clear();
            returnDays_Input.Clear();
            author_Input.Clear();
            name_Input.Clear();
            publishYear_Input.Clear();
            publisher_Input.Clear();
            cost_Input.Clear();

        }

        private void addReservation(
                int ticketNumber,
                string readerFirstName,
                string readerSecondName,
                string takeoutDate,
                int returnDays,
                string author,
                string name,
                int publishYear,
                string publisher,
                int cost
            ) {
            var record = new BookReservationRecord(
                ticketNumber,
                readerFirstName,
                readerSecondName,
                DateTime.Parse(takeoutDate),
                returnDays,
                author,
                name,
                publishYear,
                publisher,
                cost
            );

            records.Add(record);
            updateList();
        }

        private void updateList() {
            reservationsList.Items.Clear();
            foreach (BookReservationRecord record in filteredRecords) {
                reservationsList.Items.Add(record.getRepresentationString());
            }
        }

        private void Библиотека_FormClosing(object sender, FormClosingEventArgs e)
        {
            saveToXML();
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void ticketNumber_Search_TextChanged(object sender, EventArgs e)
        {
            filterRecords();
        }

        private void filterRecords() {
            filteredRecords = records.FindAll(record =>
            record.containsTicketNumber(ticketNumber_Search.Text) &&
            record.containsAuthor(author_Search.Text) &&
            record.containsPublisher(publisher_Search.Text) &&
            ((return_Search.Checked && record.IsExpired()) || (!return_Search.Checked && !record.IsExpired())));
            updateList();
        }

        private void author_Search_TextChanged(object sender, EventArgs e)
        {
            filterRecords();
        }

        private void publisher_Search_TextChanged(object sender, EventArgs e)
        {
            filterRecords();
        }

        private void return_Search_CheckedChanged(object sender, EventArgs e)
        {
            filterRecords();
        }

        private void reservationsList_SelectedIndexChanged(object sender, EventArgs e)
        {
            delete_button.Visible = reservationsList.SelectedIndices.Count > 0;
        }

        private void delete_button_Click(object sender, EventArgs e)
        {
            List<BookReservationRecord> selectedRecords = new List<BookReservationRecord>();
            foreach (int index in reservationsList.SelectedIndices) {
                selectedRecords.Add(filteredRecords[index]);
            }
            records = records.FindAll(record => !selectedRecords.Contains(record));
            filterRecords();
            saveToXML();
        }
    }
}
