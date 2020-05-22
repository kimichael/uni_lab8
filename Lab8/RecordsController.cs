using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Windows.Forms.ListBox;

namespace Lab8
{
    class RecordsController
    {

        List<BookReservationRecord> records = new List<BookReservationRecord>();

        List<BookReservationRecord> filteredRecords = new List<BookReservationRecord>();

        public void loadReservationFile() {
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


            XDocument xdoc = XDocument.Load(reservationsFile);
            XElement recordsElement = xdoc.Root;
            var elements = xdoc.Element(BookReservationRecord.RESERVATIONS_TAG).Elements("BookReservation");

                foreach (XElement xelement in elements)
                {
                    var record = BookReservationRecord.fromXElement(xelement);
                    records.Add(record);
                    filteredRecords.Add(record);
                }
            }

        internal void saveToXML()
        {
            var filename = "reservations.xml";
            var currentDirectory = Directory.GetCurrentDirectory();
            var reservationsFile = Path.Combine(currentDirectory, filename);

            List<XElement> xelements = new List<XElement>();
            foreach (BookReservationRecord record in records)
            {
                xelements.Add(record.toXElement());
            }

            XDocument doc = new XDocument(
                    new XElement(BookReservationRecord.RESERVATIONS_TAG, xelements)
                );
            doc.Save(reservationsFile);
        }

        internal void filterRecords(string ticketNumber, string author, string publisher, bool isReturn)
        {
            filteredRecords = records.FindAll(record =>
            record.containsTicketNumber(ticketNumber) &&
            record.containsAuthor(author) &&
            record.containsPublisher(publisher) &&
            ((isReturn && record.IsExpired()) || (!isReturn && !record.IsExpired())));
        }

        internal void deleteRecords(SelectedIndexCollection selectedIndexCollection)
        {
            List<BookReservationRecord> selectedRecords = new List<BookReservationRecord>();
            foreach (int index in selectedIndexCollection)
            {
                selectedRecords.Add(filteredRecords[index]);
            }
            records = records.FindAll(record => !selectedRecords.Contains(record));
        }

        internal void addRecord(BookReservationRecord record)
        {
            records.Add(record);
        }

        internal IEnumerable<BookReservationRecord> getFilteredRecords()
        {
            return filteredRecords;
        }
    }
}
