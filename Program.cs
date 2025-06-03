using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Threading;

namespace CloudQAFormTester
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("🚀 CloudQA Form Automation Test");
            Console.WriteLine("================================");
            Console.WriteLine("Starting automated testing...\n");

            var tester = new CloudQAFormTester();
            tester.RunAllTests();

            Console.WriteLine("\n✅ Testing Complete!");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }

    public class CloudQAFormTester
    {
        private IWebDriver driver;
        private WebDriverWait wait;
        private const string FORM_URL = "https://app.cloudqa.io/home/AutomationPracticeForm";

        // Test results tracking
        private int testsRun = 0;
        private int testsPassed = 0;
        private int testsFailed = 0;

        public CloudQAFormTester()
        {
            InitializeDriver();
        }

        private void InitializeDriver()
        {
            try
            {
                Console.WriteLine("🔧 Setting up Chrome browser...");
                var options = new ChromeOptions();
                options.AddArgument("--start-maximized");
                options.AddArgument("--disable-blink-features=AutomationControlled");

                driver = new ChromeDriver(options);
                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

                Console.WriteLine("✅ Browser setup complete!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Failed to setup browser: {ex.Message}");
                Console.WriteLine("💡 Make sure ChromeDriver is installed and in PATH");
                throw;
            }
        }

        public void RunAllTests()
        {
            try
            {
                // Navigate to the form
                Console.WriteLine($"\n🌐 Navigating to: {FORM_URL}");
                driver.Navigate().GoToUrl(FORM_URL);
                Thread.Sleep(3000); // Wait for page load
                Console.WriteLine("✅ Page loaded successfully!");

                // Run tests for 3 different fields
                TestField1_Name();
                TestField2_Email();
                TestField3_Phone();

                // Print final results
                PrintTestSummary();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Critical error during testing: {ex.Message}");
            }
            finally
            {
                driver?.Quit();
                Console.WriteLine("🔒 Browser closed.");
            }
        }

        // TEST 1: Name/First Name Field
        private void TestField1_Name()
        {
            Console.WriteLine("\n" + "=".PadRight(50, '='));
            Console.WriteLine("🧪 TEST 1: TESTING NAME FIELD");
            Console.WriteLine("=".PadRight(50, '='));

            var locatorStrategies = new List<(By locator, string description)>
            {
                // ADD YOUR SPECIFIC LOCATORS HERE BASED ON FORM EXPLORATION
                (By.Id("firstName"), "ID: firstName"),
                (By.Name("firstName"), "Name: firstName"),
                (By.XPath("//input[@placeholder='First Name']"), "Placeholder: First Name"),
                (By.XPath("//input[@placeholder='Enter First Name']"), "Placeholder: Enter First Name"),
                (By.XPath("//input[contains(@class,'form-control') and contains(@placeholder,'First')]"), "Class + Placeholder pattern"),
                (By.XPath("//label[contains(text(),'First Name')]/following-sibling::input"), "Label association"),
                (By.XPath("//label[contains(text(),'Name')]/following-sibling::input"), "Name label association"),
                (By.CssSelector("input[placeholder*='First']"), "CSS: placeholder contains 'First'"),
                (By.XPath("//input[@type='text'][1]"), "First text input")
            };

            TestFieldWithStrategies("Name", locatorStrategies, "John Doe");
        }

        // TEST 2: Email Field
        private void TestField2_Email()
        {
            Console.WriteLine("\n" + "=".PadRight(50, '='));
            Console.WriteLine("📧 TEST 2: TESTING EMAIL FIELD");
            Console.WriteLine("=".PadRight(50, '='));

            var locatorStrategies = new List<(By locator, string description)>
            {
                // ADD YOUR SPECIFIC LOCATORS HERE BASED ON FORM EXPLORATION
                (By.Id("userEmail"), "ID: userEmail"),
                (By.Id("email"), "ID: email"),
                (By.Name("userEmail"), "Name: userEmail"),
                (By.Name("email"), "Name: email"),
                (By.XPath("//input[@type='email']"), "Type: email"),
                (By.XPath("//input[@placeholder='Email']"), "Placeholder: Email"),
                (By.XPath("//input[@placeholder='Enter Email']"), "Placeholder: Enter Email"),
                (By.XPath("//input[contains(@placeholder,'email') or contains(@placeholder,'Email')]"), "Placeholder contains 'email'"),
                (By.CssSelector("input[type='email']"), "CSS: type email"),
                (By.XPath("//label[contains(text(),'Email')]/following-sibling::input"), "Email label association")
            };

            TestFieldWithStrategies("Email", locatorStrategies, "test@example.com");
        }

        // TEST 3: Phone Field
        private void TestField3_Phone()
        {
            Console.WriteLine("\n" + "=".PadRight(50, '='));
            Console.WriteLine("📱 TEST 3: TESTING PHONE FIELD");
            Console.WriteLine("=".PadRight(50, '='));

            var locatorStrategies = new List<(By locator, string description)>
            {
                // ADD YOUR SPECIFIC LOCATORS HERE BASED ON FORM EXPLORATION
                (By.Id("userNumber"), "ID: userNumber"),
                (By.Id("mobile"), "ID: mobile"),
                (By.Id("phone"), "ID: phone"),
                (By.Name("userNumber"), "Name: userNumber"),
                (By.Name("mobile"), "Name: mobile"),
                (By.XPath("//input[@placeholder='Mobile Number']"), "Placeholder: Mobile Number"),
                (By.XPath("//input[@placeholder='Phone Number']"), "Placeholder: Phone Number"),
                (By.XPath("//input[contains(@placeholder,'Mobile') or contains(@placeholder,'Phone')]"), "Placeholder contains Mobile/Phone"),
                (By.XPath("//input[@type='tel']"), "Type: tel"),
                (By.XPath("//label[contains(text(),'Mobile') or contains(text(),'Phone')]/following-sibling::input"), "Mobile/Phone label association")
            };

            TestFieldWithStrategies("Phone", locatorStrategies, "1234567890");
        }

        // Generic method to test any field with multiple strategies
        private void TestFieldWithStrategies(string fieldName, List<(By locator, string description)> strategies, string testValue)
        {
            testsRun++;

            foreach (var (locator, description) in strategies)
            {
                try
                {
                    Console.WriteLine($"🔍 Trying: {description}");

                    var element = wait.Until(driver =>
                    {
                        try
                        {
                            var elem = driver.FindElement(locator);
                            return elem.Displayed && elem.Enabled ? elem : null;
                        }
                        catch
                        {
                            return null;
                        }
                    });

                    if (element != null)
                    {
                        // Found the element! Now test it
                        Console.WriteLine($"✅ Found {fieldName} field using: {description}");

                        bool testResult = PerformFieldTest(element, fieldName, testValue);

                        if (testResult)
                        {
                            testsPassed++;
                            Console.WriteLine($"🎉 {fieldName} field test PASSED!");
                            return; // Success! No need to try other strategies
                        }
                        else
                        {
                            Console.WriteLine($"❌ {fieldName} field test FAILED!");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"⚠️ Strategy failed: {description} - {ex.Message}");
                    continue;
                }
            }

            // If we reach here, all strategies failed
            testsFailed++;
            Console.WriteLine($"❌ Could not locate or test {fieldName} field with any strategy!");
        }

        // Perform the actual field testing
        private bool PerformFieldTest(IWebElement element, string fieldName, string testValue)
        {
            try
            {
                // Scroll to element
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", element);
                Thread.Sleep(500);

                // Clear and enter value
                element.Clear();
                element.SendKeys(testValue);

                // Verify value was entered correctly
                string actualValue = element.GetAttribute("value");

                Console.WriteLine($"   📝 Expected: '{testValue}'");
                Console.WriteLine($"   📝 Actual: '{actualValue}'");

                if (actualValue == testValue)
                {
                    Console.WriteLine($"   ✅ Value verification PASSED");

                    // Additional checks
                    Console.WriteLine($"   📊 Field Properties:");
                    Console.WriteLine($"      - Enabled: {element.Enabled}");
                    Console.WriteLine($"      - Displayed: {element.Displayed}");
                    Console.WriteLine($"      - Tag: {element.TagName}");
                    Console.WriteLine($"      - Type: {element.GetAttribute("type") ?? "N/A"}");

                    return true;
                }
                else
                {
                    Console.WriteLine($"   ❌ Value verification FAILED");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"   ❌ Field interaction failed: {ex.Message}");
                return false;
            }
        }

        private void PrintTestSummary()
        {
            Console.WriteLine("\n" + "=".PadRight(60, '='));
            Console.WriteLine("📊 TEST EXECUTION SUMMARY");
            Console.WriteLine("=".PadRight(60, '='));
            Console.WriteLine($"Total Tests Run: {testsRun}");
            Console.WriteLine($"Tests Passed: {testsPassed} ✅");
            Console.WriteLine($"Tests Failed: {testsFailed} ❌");
            Console.WriteLine($"Success Rate: {(testsRun > 0 ? (testsPassed * 100 / testsRun) : 0)}%");
            Console.WriteLine("=".PadRight(60, '='));

            if (testsPassed == testsRun)
            {
                Console.WriteLine("🎉 ALL TESTS PASSED! Great job!");
            }
            else if (testsPassed > 0)
            {
                Console.WriteLine("⚠️ Some tests passed. Review failed tests for improvements.");
            }
            else
            {
                Console.WriteLine("❌ All tests failed. Check form structure and locators.");
            }
        }
    }
}