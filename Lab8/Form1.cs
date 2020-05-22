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
    public partial class Library : Form
    {

        private RecordsController controller;

        public Library()
        {
            InitializeComponent();

            controller = new RecordsController();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                loadReservations();
            }
            catch (ArgumentException exception)
            {
                MessageBox.Show("Неправильный формат файла", "Ошибка");
            }
            catch {
                MessageBox.Show("XML файл поврежден", "Ошибка");
            }
        }

        private void loadReservations() {

            controller.loadReservationFile("");

            filterRecords();
        }

        private void saveToXML()
        {
            controller.saveToXML();
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
            try
            {
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

                controller.addRecord(record);
                updateList();
            }
            catch (FormatException exception)
            {
                MessageBox.Show("Неверный формат записи", "Ошибка");
            }
        }

        private void updateList() {
            reservationsList.Items.Clear();
            foreach (BookReservationRecord record in controller.getFilteredRecords()) {
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
            controller.filterRecords(ticketNumber_Search.Text, author_Search.Text, publisher_Search.Text, return_Search.Checked);
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
            controller.deleteRecords(reservationsList.SelectedIndices);
            filterRecords();
            saveToXML();
        }
    }
}
