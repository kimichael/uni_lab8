using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Lab8
{
    public class BookReservationRecord
    {

        public const string RESERVATIONS_TAG = "reservations";
               
        public const string TICKET_NUMBER_TAG = "ticketNumber";
               
        public const string READER_FIRST_NAME_TAG = "firstName";
               
        public const string READER_SECOND_NAME_TAG = "secondName";
               
        public const string TAKEOUT_DATE_TAG = "takeoutDate";
               
        public const string RETURN_DAYS_TAG = "returnDays";
               
        public const string AUTHOR_TAG = "author";
               
        public const string NAME_TAG = "name";
               
        public const string PUBLISH_YEAR_TAG = "publishYear";
               
        public const string PUBLISHER_TAG = "publisher";
               
        public const string COST_TAG = "cost";

        private int ticketNumber;

        private string readerFirstName;

        private string readerSecondName;

        private DateTime takeoutDate;

        private int returnDays;

        private string author;

        private string name;

        private int publishYear;

        private string publisher;

        private int cost;

        public BookReservationRecord(int ticketNumber, string readerFirstName, string readerSecondName, DateTime takeoutDate, int returnDays, string author, string name, int publishYear, string publisher, int cost)
        {
            this.ticketNumber = ticketNumber;
            this.readerFirstName = readerFirstName;
            this.readerSecondName = readerSecondName;
            this.takeoutDate = takeoutDate;
            this.returnDays = returnDays;
            this.author = author;
            this.name = name;
            this.publishYear = publishYear;
            this.publisher = publisher;
            this.cost = cost;
        }

        public string getRepresentationString() {
            StringBuilder sb = new StringBuilder();
            sb.Append("Номер билета: ");
            sb.Append(ticketNumber);
            sb.Append(", ");
            sb.Append("Имя читателя: ");
            sb.Append(readerFirstName);
            sb.Append(", ");
            sb.Append("Фамилия читателя: ");
            sb.Append(readerSecondName);
            sb.Append(", ");
            sb.Append("Дата выдачи: ");
            sb.Append(takeoutDate.ToString("dd MMMM yyyy"));
            sb.Append(", ");
            sb.Append("Дни возврата: ");
            sb.Append(returnDays);
            sb.Append(", ");
            sb.Append("Автор: ");
            sb.Append(author);
            sb.Append(", ");
            sb.Append("Название: ");
            sb.Append(name);
            sb.Append(", ");
            sb.Append("Год выпуска: ");
            sb.Append(publishYear);
            sb.Append(", ");
            sb.Append("Издательство: ");
            sb.Append(publisher);
            return sb.ToString();
        }


        public XElement toXElement() {
            return new XElement("BookReservation",
                    new XElement(TICKET_NUMBER_TAG, ticketNumber),
                    new XElement(READER_FIRST_NAME_TAG, readerFirstName),
                    new XElement(READER_SECOND_NAME_TAG, readerSecondName),
                    new XElement(TAKEOUT_DATE_TAG, takeoutDate.ToString()),
                    new XElement(RETURN_DAYS_TAG, returnDays),
                    new XElement(AUTHOR_TAG, author),
                    new XElement(NAME_TAG, name),
                    new XElement(PUBLISH_YEAR_TAG, publishYear),
                    new XElement(PUBLISHER_TAG, publisher),
                    new XElement(COST_TAG, cost)
                );
        }

        public static BookReservationRecord fromXElement(XElement element) {
            var subElements = element.Elements("BookReservation");

            try
            {
                Nullable<int> ticketNumber = Convert.ToInt32(element.Element(TICKET_NUMBER_TAG).Value);
                string readerFirstName = element.Element(READER_FIRST_NAME_TAG).Value;
                string readerSecondName = element.Element(READER_SECOND_NAME_TAG).Value;
                Nullable<DateTime> takeoutDate = DateTime.Parse(element.Element(TAKEOUT_DATE_TAG).Value);
                Nullable<int> returnDays = Convert.ToInt32(element.Element(RETURN_DAYS_TAG).Value);
                string author = element.Element(AUTHOR_TAG).Value;
                string name = element.Element(NAME_TAG).Value;
                Nullable<int> publishYear = Convert.ToInt32(element.Element(PUBLISH_YEAR_TAG).Value);
                string publisher = element.Element(PUBLISHER_TAG).Value;
                Nullable<int> cost = Convert.ToInt32(element.Element(COST_TAG).Value);

                if (ticketNumber == null ||
                    readerFirstName == null ||
                    readerSecondName == null ||
                    takeoutDate == null ||
                    returnDays == null ||
                    author == null ||
                    name == null ||
                    publishYear == null ||
                    publisher == null ||
                    cost == null)
                {
                    throw new ArgumentException("Missing xml property");
                }

                return new BookReservationRecord(
                        ticketNumber.Value,
                        readerFirstName,
                        readerSecondName,
                        takeoutDate.Value,
                        returnDays.Value,
                        author,
                        name,
                        publishYear.Value,
                        publisher,
                        cost.Value
                    );
            }
            catch (FormatException exception)
            {
                throw new ArgumentException("Wrong string format");
            }
            catch (NullReferenceException exception) { 
                throw new ArgumentException("Missing field");
            }
            
        }

        internal bool containsTicketNumber(string text)
        {
            return ticketNumber.ToString().Contains(text);
        }

        internal bool containsAuthor(string text)
        {
            return author.Contains(text);
        }

        internal bool containsPublisher(string text)
        {
            return publisher.Contains(text);
        }

        internal bool IsExpired()
        {
            return !(takeoutDate.AddDays(returnDays) > DateTime.Now);
        }
    }
}
