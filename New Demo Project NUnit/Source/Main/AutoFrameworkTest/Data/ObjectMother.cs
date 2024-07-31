using Dapper;
using NUnit.Framework;
using System;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using IronXL;
using System.IO;
using System.Collections;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;

namespace AutoFrameworkTest.Data
{
    public class ObjectMother
    {
        public static TestDataRowTitle ReadTestData1()
        {
			var path = ConfigurationManager.AppSettings["TestDataSheetPath"];
			var data = File.ReadLines(path + "TestData3.csv");

			var Testdatarow = new TestDataRowTitle
			{
                rowdata = data.ElementAt(1),
			};

            return Testdatarow;
        }

        public static TestDataRowTitle ReadTestData2()
        {
            var path = ConfigurationManager.AppSettings["TestDataSheetPath"];
            var data = File.ReadLines(path + "TestData2.csv");

            var Testdatarow = new TestDataRowTitle
            {
                rowdata = data.ElementAt(1),
            };

            return Testdatarow;
        }

        public static TestDataRowTitle GetTestData(String data)
        {
            string[] Data = data.Split(',');
            var Testdata = new TestDataRowTitle
            {
                UserName = Data[0],
                Password = Data[1],
                Projects = Data[2],
                Projects2 = Data[3],
            };
            return Testdata;
        }



    }
}
