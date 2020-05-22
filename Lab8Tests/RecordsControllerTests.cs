using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lab8;
using System;
using System.Xml;

namespace Lab8Tests
{
    [TestClass]
    public class RecordsControllerTests
    {

        private RecordsController controller;

        [TestInitialize]
        public void setUp() {
            controller = new RecordsController();
        }

        [TestMethod]
        public void normalCase()
        {
            controller.loadReservationFile("_normal");
            Assert.AreEqual(1, controller.getRecords().Count);
            Assert.AreEqual(1, controller.getFilteredRecords().Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void brokenFormatCase()
        {
            controller.loadReservationFile("_broken_format"); 
        }

        [TestMethod]
        [ExpectedException(typeof(XmlException))]
        public void brokenXmlCase()
        {
            controller.loadReservationFile("_broken_xml");
        }

        [TestMethod]
        public void datesCheckCase()
        {
            controller.loadReservationFile("_dates_check");
            Assert.AreEqual(4, controller.getRecords().Count);
            Assert.AreEqual(4, controller.getFilteredRecords().Count);
        }

        [TestMethod]
        public void normalLongCase()
        {
            controller.loadReservationFile("_normal_long");
            Assert.AreEqual(100, controller.getRecords().Count);
            Assert.AreEqual(100, controller.getFilteredRecords().Count);
        }

        [TestMethod]
        public void easterEggCase()
        {
            controller.loadReservationFile("_easter_egg");
            Assert.AreEqual(100, controller.getRecords().Count);
            Assert.AreEqual(100, controller.getFilteredRecords().Count);
            controller.filterRecords("", "easter", "", true);
            Assert.AreEqual(100, controller.getRecords().Count);
            Assert.AreEqual(4, controller.getFilteredRecords().Count);
        }

        [TestMethod]
        public void emptyCase()
        {
            controller.loadReservationFile("_empty");
            Assert.AreEqual(0, controller.getRecords().Count);
            Assert.AreEqual(0, controller.getFilteredRecords().Count);
        }

        [TestMethod]
        [ExpectedException(typeof(XmlException))]
        public void gibberishCase()
        {
            controller.loadReservationFile("_gibberish");
        }

        [TestMethod]
        public void realDataCase()
        {
            controller.loadReservationFile("_real_data");
            Assert.AreEqual(5, controller.getRecords().Count);
            Assert.AreEqual(5, controller.getFilteredRecords().Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void missingXMLFieldCase()
        {
            controller.loadReservationFile("_missing_xml_field");
        }
    }
}
