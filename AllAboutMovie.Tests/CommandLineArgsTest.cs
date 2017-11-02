using AllAboutMovie;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AllAboutMovie.Tests
{
    
    
    /// <summary>
    ///This is a test class for CommandLineArgsTest and is intended
    ///to contain all CommandLineArgsTest Unit Tests
    ///</summary>
	[TestClass()]
	public class CommandLineArgsTest
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
		///A test for ExtractTitleYearFromFilePath
		///</summary>
		[TestMethod()]
		public void CommandLineArgsParsingTest()
		{
			var commandLineArgs = new CommandLineArgs(new string[] {"", "d:\\Movies\\Tristan.I.Isolda.2006.avi"});
			Assert.AreEqual(commandLineArgs.SearchTitle, "Tristan I Isolda");
			Assert.AreEqual(commandLineArgs.SearchYear, "2006");

			commandLineArgs = new CommandLineArgs(new string[] { "", "d:\\Movies\\Season of the Witch HDRip.2011.avi" });
			Assert.AreEqual(commandLineArgs.SearchTitle, "Season of the Witch");
			Assert.AreEqual(commandLineArgs.SearchYear, "2011");


		}
	}
}
