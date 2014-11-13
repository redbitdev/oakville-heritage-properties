using NUnit.Framework;
using System;
using Xamarin.UITest;
using System.Reflection;
using System.IO;
using Xamarin.UITest.Queries;
using System.Linq;

namespace HeritageProperties.UITest
{
    [TestFixture]
    public class HeritageAppTest
    {
        /// <summary>
        /// Object to use for platform specific queries
        /// </summary>
        IScreenQueries _queries;
        IApp _app;

        public string PathToIPA { get; private set; }

        public string PathToAPK { get; private set; }

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            if (TestEnvironment.IsTestCloud)
            {
                PathToAPK = String.Empty;
                PathToIPA = String.Empty;
            }
            else
            {

                string currentFile = new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath;
                FileInfo fi = new FileInfo(currentFile);
                string dir = fi.Directory.Parent.Parent.Parent.FullName;

                PathToIPA = Path.Combine(dir, "HeritageProperties.iOS", "bin", "iPhoneSimulator", "Debug", "HeritagePropertiesiOS.app");
                PathToAPK = Path.Combine(dir, "HeritageProperties.Android", "bin", "Release", "HeritageProperties.Droid.APK");
            }
        }


        [SetUp]
        public void SetUp()
        {
            if (TestEnvironment.Platform.Equals(TestPlatform.TestCloudiOS))
            {
                // set the queries for platform
                _queries = new iOSScreenQueries();

                // configure the app
                _app = ConfigureApp
                    .iOS
                    .StartApp();
            }
            else if (TestEnvironment.Platform.Equals(TestPlatform.TestCloudAndroid))
            {
                // set the queries for platform
                _queries = new AndroidScreenQueries();

                // configure the app
                _app = ConfigureApp
                    .Android
                    .StartApp();
            }
            else if (TestEnvironment.Platform.Equals(TestPlatform.Local))
            {
                CheckAndroidHomeEnvironmentVariable();

                // NOTE Enable or disable the lines depending on what platform you want ot test

//                 set the queries for platform
                 _queries = new iOSScreenQueries();

//                 configure the ios app
                _app = ConfigureApp.iOS
                    .ApiKey("YOUR_API_KEY")
                    .AppBundle(PathToIPA)
                    .StartApp();

                // set the queries for platform
//                _queries = new AndroidScreenQueries();
//
//                // configure the android app
//                _app = ConfigureApp
//                    .Android
//                    .ApkFile(PathToAPK)
                //    .ApiKey("YOUR_API_KEY")
//                    .StartApp();
            }
            else
            {
                throw new NotImplementedException(String.Format("I don't know this platform {0}", TestEnvironment.Platform));
            }
        }


        /// <summary>
        /// Check to make sure the list view is viewable when we switch to lsitview mode
        /// </summary>
        [Test]
        public void TestForListView()
        {
            /* ACT */
            // tap the button
            _app.Tap(_queries.NavigationButton);

            /* ASSERT */
            // make sure the listview is displayed
            Assert.IsTrue(_app.Query(_queries.ListView).Any(), "the list view is not displayed");
        }

        /// <summary>
        /// Check to make sure teh map view is visible when switched to it
        /// </summary>
        [Test]
        public void TestForMapView()
        {

            /* ACT */
            // tap the button
            // go to list
            _app.Tap(_queries.NavigationButton);

            // wait for 100ms - this is an extension method
            _app.WaitFor(TimeSpan.FromMilliseconds(100));

            // go back to map
            _app.Tap(_queries.NavigationButton);

            // wait for 100ms
            _app.WaitFor(TimeSpan.FromMilliseconds(100));

            // wait for the mapview
            _app.WaitForElement(_queries.MapView);

            /* ASSERT */
            // make sure the mapview is visuble
            Assert.IsTrue(_app.Query(_queries.MapView).Any(), "the map view is not displayed");
        }

        [Test]
        public void TestDetailsFromListView()
        {

            /* ACT */
            // tap the button
            // go to list
            _app.Tap(_queries.NavigationButton);

            // scroll the list
            _app.ScrollUp();

            // tap an item in the list
            _app.Tap(_queries.ListView);

            // make sure we have the deatils page by waiting for the webview
            Assert.IsTrue(_app.Query(_queries.WebView).Any(), "Webview not found in details");

            // go back
            _app.Back();

			_app.WaitForElement (_queries.ListView);
//			_app.Repl ();

            /* ASSERT */
            // make sure we are bank and see a listview
            Assert.IsTrue(_app.Query(_queries.ListView).Any(), "could not find list after going back from details");
        }
        private void CheckAndroidHomeEnvironmentVariable()
        {
            // TODO is this required?
            var androidHome = Environment.GetEnvironmentVariable("ANDROID_HOME");
            if (string.IsNullOrWhiteSpace(androidHome))
            {
                Environment.SetEnvironmentVariable("ANDROID_HOME", @"C:\Users\Mark\AppData\Local\Android\android-sdk\");
            }

            var jvd = Environment.GetEnvironmentVariable("JAVA_HOME");
            if (string.IsNullOrWhiteSpace(androidHome))
            {
                Environment.SetEnvironmentVariable("JAVA_HOME", @"C:\Program Files\Java\jdk1.7.0_67");
            }
        }
    }
}

