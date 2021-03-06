﻿using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using InterestRateOfReturn;
using System.Linq;
using System.Collections;

namespace XIRRUnitTest
{
    [TestClass]
    public class Tests
    {
        [TestMethod]
        public void SimpleDepot()
        {
            System.Console.WriteLine("SimpleDepot");

            List<double> cashFlows = new List<Double>() { -100, 110 };
            List<DateTime> dates = new List<DateTime>() { new DateTime(2017, 12, 31), new DateTime(2018, 12, 31) };

            Double result = new XIRR(cashFlows, dates).Calculate(0.1,1); ;
            Assert.AreEqual(0.1, result);
        }

        [TestMethod]
        public void SimpleDepotIRR()
        {
            List<double> cashFlows = new List<Double>() { -100, 110 };
            Double result = new IRR(cashFlows).Calculate(0.1, 1); ;
            Assert.AreEqual(0.1, result);
            
        }
        [TestMethod]
        public void MicrosoftSampleXIRR2()
        {
            // excel precision for XIRR is 8digits
            List<double> cashFlows = new List<Double>() { -130052618.49, -400000, 17100, -400000, 133314986.32 };
            List<DateTime> dates = new List<DateTime>() { new DateTime(2013, 12, 31), new DateTime(2014, 11, 07), new DateTime(2014, 11, 18), new DateTime(2014, 11, 27), new DateTime(2014, 12, 31) };

            Double result = new XIRR(cashFlows, dates).Calculate();
            Assert.AreEqual(0.01905139, result);
        }

        [TestMethod]
        public void MicrosoftSampleXIRR3()
        {
            // excel precision for XIRR is 8digits
            List<double> cashFlows = new List<Double>() { -152878677.49, 4410000, 1700, 12754, 770, 47743488.63, 111644373.78 };
            List<DateTime> dates = new List<DateTime>() { new DateTime(2015, 12, 31), new DateTime(2016, 05, 27), new DateTime(2016, 08, 25),
                                                            new DateTime(2016, 11, 29), new DateTime(2016, 12, 06), new DateTime(2016, 12, 30), new DateTime(2016, 12, 31)};

            Double result = new XIRR(cashFlows, dates).Calculate();
            Assert.AreEqual(0.07261154, result);

            //            -152878677,5    4410000 1700    12754   770 47743488,63 111644373,8
            //             31 / 12 / 2015  27 / 05 / 2016  25 / 08 / 2016  29 / 11 / 2016  06 / 12 / 2016  30 / 12 / 2016  31 / 12 / 2016          0,072611538

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
            Assert.AreEqual(0.08663095, result);
        }

        [TestMethod]
        public void bugIRR()
        {
            List<double> cashFlows = new List<Double>() { -14413262, -825000, 3660000, 16240602 };
            List<DateTime> dates = new List<DateTime>() { new DateTime(2007, 12, 31), new DateTime(2015, 01, 28), new DateTime(2015, 04, 10), new DateTime(2019, 05, 31) };
            Double result = new XIRR(cashFlows, dates).Calculate(0.000001, 6);
            Assert.AreEqual(0.026323, result);
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

        [TestMethod]
        public void MicrosoftSampleCashFlowSameDate()
        {
            // some cashflow have got the same date
            List<double> cashFlows = new List<Double>() { -152878677.49, 4410000, 1700, 50000, 70000, 12754, 70000, 47743488.63, 111644373.78 };
            List<DateTime> dates = new List<DateTime>() { new DateTime(2015, 12, 31), new DateTime(2016, 05, 27), new DateTime(2016, 08, 25),
                                                            new DateTime(2016, 08, 25),new DateTime(2016, 08, 25),
                                                            new DateTime(2016, 11, 29), new DateTime(2016, 12, 06), new DateTime(2016, 12, 30), new DateTime(2016, 12, 31)};

            Double result = new XIRR(cashFlows, dates).Calculate();
            Assert.AreEqual(0.0738901, result);
        }
        [TestMethod]
        public void SampleNear100pc1()
        {
            // some cashflow have got the same date (error in excel)
            List<double> cashFlows = new List<Double>() { -5155970.71, -6076366.17, -32377.53, -2279151.97, -2279151.97, -2279151.97, 6125786.09 };
            List<DateTime> dates = new List<DateTime>() { new DateTime(2019, 02, 28), new DateTime(2019, 02, 28), new DateTime(2019, 02, 28),
                                                            new DateTime(2019, 02, 28),new DateTime(2019, 02, 28),new DateTime(2019, 02, 28),
                                                            new DateTime(2019, 03, 31)};

            Double result = new XIRR(cashFlows, dates).Calculate();
            Assert.AreEqual(-0.99999712, result);

        }
        [TestMethod]
        public void SampleNear100pc2()
        {
            // no error in excel , if merging cashflows
            List<double> cashFlows = new List<Double>() { -18102170.32, 6125786.09 };
            List<DateTime> dates = new List<DateTime>() { new DateTime(2019, 02, 28), new DateTime(2019, 03, 31)};

            Double result = new XIRR(cashFlows, dates).Calculate();
            Assert.AreEqual(-0.99999712, result);

        }

        [TestMethod]
        public void SampleNear0pc()
        {
            // error in excel
            List<double> cashFlows = new List<Double>() { 16.8, -33000, 32377.53 };
            List<DateTime> dates = new List<DateTime>() { new DateTime(2018, 12, 31), new DateTime(2019, 02, 05), new DateTime(2019, 02, 28) };

            Double result = new XIRR(cashFlows, dates).Calculate();
            Assert.AreEqual(-0.25498632, result);

        }


    }
}