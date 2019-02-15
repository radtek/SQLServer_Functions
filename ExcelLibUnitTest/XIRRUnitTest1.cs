﻿using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Excel.Lib;
namespace XIRRUnitTest
{


    [TestClass]
    public class Tests
    {
        [TestMethod]
        public void SimpleDepot()
        {
            List<double> cashFlows = new List<Double>() { -100, 110 };
            List<DateTime> dates = new List<DateTime>() { new DateTime(2017, 12, 31), new DateTime(2018, 12, 31) };

            Double result = new XIRR(cashFlows, dates).Calculate(0.1,1); ;
            Assert.AreEqual(0.1, result);
        }

        [TestMethod]
        public void SimpleDepotIRR()
        {
            List<double> cashFlows = new List<Double>() { -100, 110 };

            IRR r = new IRR(cashFlows,0.1);



            Double result = new IRR(cashFlows).Calculate(0.1, 1); ;
            Assert.AreEqual(0.1, result);
        }

        [TestMethod]
        public void MicrosoftSampleXIRR()
        {
            //https://support.office.com/en-us/article/XIRR-function-DE1242EC-6477-445B-B11B-A303AD9ADC9D
            // excel precision for XIRR is 8digits
            List<double> cashFlows = new List<Double>() { -10000, 2750, 4250,3250, 2750 };
            List<DateTime> dates = new List<DateTime>() { new DateTime(2008, 01, 01), new DateTime(2008, 03, 01), new DateTime(2008, 10, 30), new DateTime(2009, 02, 15), new DateTime(2009, 04, 01) };

            Double result = new XIRR(cashFlows, dates).Calculate();
            Assert.AreEqual(0.37336253, result);
        }

        [TestMethod]
        public void MicrosoftSampleIRR()
        {
            //   https://support.office.com/en-us/article/IRR-function-64925EAA-9988-495B-B290-3AD0C163C1BC
            // excel precision for IRR is 8digits
            List<double> cashFlows = new List<Double>() { -70000, 12000, 15000, 18000, 21000, 26000 };
            Double result = new IRR(cashFlows).Calculate();
            Assert.AreEqual(0.087, result);
        }
        
        [TestMethod]
        public void LifeInsurance()
        {
            List<double> cashFlows = new List<Double>() { -500, -500, -500, -500, -500, 3000 };
            List<DateTime> dates = new List<DateTime>() { new DateTime(2008, 01, 01), new DateTime(2008, 03, 01), new DateTime(2008, 10, 30), new DateTime(2009, 02, 15), new DateTime(2009, 04, 01), new DateTime(2010, 01, 01) };
            Double result = new XIRR(cashFlows, dates).Calculate(0.000001, 6);
            Assert.AreEqual(0.145045, result);
        }

        [TestMethod]
        public void LifeInsuranceIRR()
        {
            List<double> cashFlows = new List<Double>() { -500, -500, -500, -500, -500, 3000 };            
            Double result = new IRR(cashFlows).Calculate(0.000001, 6);
            Assert.AreEqual(0.061402, result);
        }


        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void NoPositiveCashFlows()
        {
            var cashFlows = new List<Double>() { -100, 0 };
            var dates = new List<DateTime>() { new DateTime(2017, 12, 31), new DateTime(2018, 12, 31) };

            var result = new XIRR(cashFlows, dates);
            Assert.AreEqual(0.1, result);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void CountCashFlows()
        {
            var cashFlows = new List<Double>() { -100, 110 };
            var dates = new List<DateTime>() { new DateTime(2017, 12, 31) };

            var result = new XIRR(cashFlows, dates);
            Assert.AreEqual(0.1, result);
        }


    }
}