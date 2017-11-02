using AllAboutMovie.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AllAboutMovie.Tests
{
    
    
    /// <summary>
    ///This is a test class for HtmlHelperTest and is intended
    ///to contain all HtmlHelperTest Unit Tests
    ///</summary>
	[TestClass()]
	public class HtmlHelperTest
	{


		private TestContext testContextInstance;

		/// <summary>
		///Gets or sets the test context which provides
		///information about and functionality for the current test run.
		///</summary>
		public TestContext TestContext
		{
			get
			{
				return testContextInstance;
			}
			set
			{
				testContextInstance = value;
			}
		}

		#region Additional test attributes
		// 
		//You can use the following additional attributes as you write your tests:
		//
		//Use ClassInitialize to run code before running the first test in the class
		//[ClassInitialize()]
		//public static void MyClassInitialize(TestContext testContext)
		//{
		//}
		//
		//Use ClassCleanup to run code after all tests in a class have run
		//[ClassCleanup()]
		//public static void MyClassCleanup()
		//{
		//}
		//
		//Use TestInitialize to run code before running each test
		//[TestInitialize()]
		//public void MyTestInitialize()
		//{
		//}
		//
		//Use TestCleanup to run code after each test has run
		//[TestCleanup()]
		//public void MyTestCleanup()
		//{
		//}
		//
		#endregion


		/// <summary>
		///A test for Decode
		///</summary>
		[TestMethod()]
		public void DecodeTest()
		{
			string s = "Ch&#322;opi, 166 мин"; 
			string expected = "Chłopi, 166 мин";
			string actual = HtmlHelper.Decode(s);
			Assert.AreEqual(expected, actual);

			s = "свидетелями&#133;";
			expected = "свидетелями…";
			actual = HtmlHelper.Decode(s);
			Assert.AreEqual(expected, actual);

			s = "свидетелями&#x8A;";
			expected = "свидетелями…";
			actual = HtmlHelper.Decode(s);
			Assert.AreEqual(expected, actual);
		}
	}
}
