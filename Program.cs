using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Interactions;
using System.Threading;

namespace Midnight
{
    class Program
    {
        static void Main()
        {
            // Set up ChromeDriver
            IWebDriver driver = new ChromeDriver();

            // Navigate to the demo website
            driver.Navigate().GoToUrl("https://www.instagram.com/");

            Thread.Sleep(1000);

            IWebElement userName = driver.FindElement(By.CssSelector("input[name='username']"));
            IWebElement password = driver.FindElement(By.CssSelector("input[name='password']"));
            IWebElement enter = driver.FindElement(By.XPath("//*[@id='loginForm']/div/div[3]/button"));
            string userToFollow = "trazzo_joyeria";
            string userNameString = //"crimetmk";
                /*"feli20gutierrez";//*/"midnightbyantonia_";
            userName.SendKeys(userNameString);
            password.SendKeys(//"VKCN0Zh_x35DoLz");
                /*"+svzYvm<LrZs#24");//"*/"OliviaBlas2023");
            enter.Click();

            Thread.Sleep(3000);
            IWebElement saveLoginInfo = driver.FindElement(By.XPath("//*[text()='Not now']"), 5, displayed: true);
            saveLoginInfo.Click();

            Thread.Sleep(3000);
            IWebElement enableNotifications = driver.FindElement(By.XPath("//*[text()='Not Now']"), 5, displayed: true);
            enableNotifications.Click();

            var body = driver.FindElement(By.CssSelector("body"));

            body.SendKeys(Keys.Control + "t");
            //driver.SwitchTo().Window(driver.WindowHandles.Last());
            driver.SwitchTo().NewWindow(WindowType.Tab);

            IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)driver;
            String hover = "var evObj = document.createEvent('MouseEvents');" +
                "evObj.initMouseEvent(\"mouseover\",true, false, window, 0, 0, 0, 0, 0, false, false, false, false, 0, null);" +
                "arguments[0].dispatchEvent(evObj);";
            string click = "arguments[0].click();";
            string hide = "arguments[0].style.visibility='hidden'";
            string scroll = "arguments[0].scrollIntoView()";

            Actions action = new Actions(driver);
            Hashtable prevIter = new Hashtable();

            bool unfollow = true;

            if (unfollow)
            {
                driver.Navigate().GoToUrl("http://www.instagram.com/" + userNameString + "/following");
                
                Thread.Sleep(5000);

                int whileIter = 0;
                string h = "";
                Thread.Sleep(500);
                while (whileIter < 5000)
                {
                    Console.WriteLine("While iteration number: {0}", whileIter);
                    List<IWebElement> users = driver.FindElements(By.XPath("//*[@class='x1rg5ohu']")).ToList();
                    Hashtable currentIter = new Hashtable();
                    Thread.Sleep(1000);
                    string temp = "";
                    for (int i = 0; i < users.Count; ++i)
                    {
                        var childrenK = users[i].FindElement(By.XPath("./child::*"));
                        childrenK = childrenK.FindElement(By.XPath("./child::*"));
                        temp = childrenK.GetAttribute("href");
                        currentIter.Add(temp, users[i]);
                    }
                    if (users.Count == 0)
                    {
                        Console.WriteLine("No users, searching by Following or Requested text!");
                        users = driver.FindElements(By.XPath("//*[text()='Following' or text()='Requested']")).ToList();
                        if (users.Count > 0)
                        {
                            jsExecutor.ExecuteScript(scroll, users[users.Count - 1]);
                            continue;
                        }
                        Console.WriteLine("No users breaking!");
                        break;
                    }

                    Console.WriteLine("h: {0}, temp: {1}", h, temp);

                    foreach (DictionaryEntry entry in prevIter)
                    {
                        //Console.WriteLine("current: {0}, prev: {1}", currentIter[i], prevIter[i]);
                        if (currentIter.ContainsKey(entry.Key))
                        {
                            Console.WriteLine("Value: {0} already exists, popping!", entry.Key);
                            currentIter.Remove(entry.Key);
                        }
                    }
                    Console.WriteLine("Users' count: {0}", currentIter.Count);
                    if (currentIter.Count == 0)
                    {
                        break;
                    }
                    if (h != temp)
                    {
                        Console.WriteLine("Inside!");
                        foreach (DictionaryEntry entry in currentIter)
                        {
                            IWebElement user = (IWebElement)entry.Value;
                            Thread.Sleep(500);

                            driver.FindElement(By.CssSelector("body")).SendKeys(Keys.Control + "t");
                            driver.SwitchTo().NewWindow(WindowType.Tab);
                            driver.Navigate().GoToUrl((string)entry.Key + "following");
                            string[] splitUserString = ((string)entry.Key).Split('/');
                            //for (int i = 0; i < splitUserString.Length; ++i)
                            //{
                            //    Console.WriteLine("Iter: {0}= {1}", i, splitUserString[i]);
                            //}
                            string tempUserString = splitUserString[3];
                            Thread.Sleep(5000);

                            var followingCurrent = driver.FindElements(By.XPath("//*[text()='" + userNameString + "']"));
                            Thread.Sleep(1000);
                            if (followingCurrent.Count > 0)
                            {
                                Console.WriteLine("User: {1} Following current account: {0}", userNameString, tempUserString);
                            }
                            else
                            {
                                Console.WriteLine("User: {1} Not Following current account: {0}", userNameString, tempUserString);
                                var close = driver.FindElement(By.XPath("//*[@aria-label='Close']"), 5, displayed: true);
                                Thread.Sleep(300);
                                close.Click();
                                var followingCurrentUser = driver.FindElement(By.XPath("//*[text()='Following']"), 5, displayed: true);
                                var followingCurrentUserMeta = followingCurrentUser.FindElement(By.XPath(".."), 5, displayed: true);
                                followingCurrentUserMeta = followingCurrentUser.FindElement(By.XPath(".."), 5, displayed: true);
                                followingCurrentUserMeta = followingCurrentUser.FindElement(By.XPath(".."), 5, displayed: true);
                                followingCurrentUserMeta.Click();
                                Thread.Sleep(500);
                                var unfollowUser = driver.FindElement(By.XPath("//*[text()='Unfollow']"), 5, displayed: true);
                                unfollowUser.Click();
                                Thread.Sleep(1000);
                            }

                            Thread.Sleep(2000);

                            driver.SwitchTo().Window(driver.WindowHandles[2]).Close();
                            driver.SwitchTo().Window(driver.WindowHandles[1]);

                            jsExecutor.ExecuteScript(scroll, user);
                            Thread.Sleep(100);
                        }
                    }
                    //jsExecutor.ExecuteScript(scroll, users[users.Count - 1]);
                    h = temp;
                    foreach (DictionaryEntry entry in currentIter)
                    {
                        if (!prevIter.ContainsKey(entry.Key))
                        {
                            prevIter.Add(entry.Key, entry.Value);
                        }
                    }
                    whileIter++;
                    Thread.Sleep(3000);
                }

            }
            else
            {
                driver.Navigate().GoToUrl("http://www.instagram.com/" + userToFollow + "/followers");

                Thread.Sleep(5000);

                int whileIter = 0;
                string h = "";
                Thread.Sleep(500);
                while (whileIter < 500)
                {
                    Console.WriteLine("While iteration number: {0}", whileIter);
                    List<IWebElement> users = driver.FindElements(By.XPath("//*[@class='x1rg5ohu']")).ToList();
                    Hashtable currentIter = new Hashtable();
                    Thread.Sleep(1000);
                    string temp = "";
                    for (int i = 0; i < users.Count; ++i)
                    {
                        var childrenK = users[i].FindElement(By.XPath("./child::*"));
                        childrenK = childrenK.FindElement(By.XPath("./child::*"));
                        temp = childrenK.GetAttribute("href");
                        currentIter.Add(temp, users[i]);
                    }
                    if (users.Count == 0)
                    {
                        Console.WriteLine("No users, searching by Following or Requested text!");
                        users = driver.FindElements(By.XPath("//*[text()='Following' or text()='Requested']")).ToList();
                        if (users.Count > 0)
                        {
                            jsExecutor.ExecuteScript(scroll, users[users.Count - 1]);
                            continue;
                        }
                        Console.WriteLine("No users breaking!");
                        break;
                    }

                    Console.WriteLine("h: {0}, temp: {1}", h, temp);

                    foreach (DictionaryEntry entry in prevIter)
                    {
                        //Console.WriteLine("current: {0}, prev: {1}", currentIter[i], prevIter[i]);
                        if (currentIter.ContainsKey(entry.Key))
                        {
                            Console.WriteLine("Value: {0} already exists, popping!", entry.Key);
                            currentIter.Remove(entry.Key);
                        }
                    }
                    Console.WriteLine("Users' count: {0}", currentIter.Count);
                    if (currentIter.Count == 0)
                    {
                        break;
                    }
                    if (h != temp)
                    {
                        Console.WriteLine("Inside!");
                        foreach (DictionaryEntry entry in currentIter)
                        {
                            IWebElement user = (IWebElement)entry.Value;
                            Thread.Sleep(500);
                            IWebElement metaUser = user.FindElement(By.XPath(".."), 5, displayed: true);
                            for (int i = 0; i < 4; ++i)
                            {
                                metaUser = metaUser.FindElement(By.XPath(".."), 5, displayed: true);
                            }
                            //Thread.Sleep(1000);
                            //Console.WriteLine("Meta class name: {0}", metaUser.GetAttribute("class"));
                            var childElements = metaUser.FindElements(By.XPath(".//*"));
                            //Console.WriteLine("Number of child elements: {0}", childElements.Count);
                            if (childElements.Count > 20)
                            {
                                IWebElement following = childElements[20];
                                Console.WriteLine("Follows status: {0}", following.Text);
                                if (following.Text == "Follow")
                                {
                                    //Thread.Sleep(1000);
                                    var children = user.FindElement(By.XPath("./child::*"));
                                    children = children.FindElement(By.XPath("./child::*"));
                                    Thread.Sleep(200);
                                    string link = children.GetAttribute("href");
                                    Console.WriteLine("Link: {0}", link);
                                    driver.FindElement(By.CssSelector("body")).SendKeys(Keys.Control + "t");
                                    driver.SwitchTo().NewWindow(WindowType.Tab);
                                    driver.Navigate().GoToUrl(link);

                                    if (ShouldFollow(driver))
                                    {
                                        IWebElement followInside = driver.FindElement(By.XPath("//*[text()='Follow' or text()='Requested' or text()='Following']"), 5, displayed: true);
                                        if (followInside.Text == "Follow")
                                        {
                                            //Thread.Sleep(500);
                                            //Console.WriteLine("Follow!");
                                            followInside.Click();
                                        }
                                        Thread.Sleep(2000);
                                    }
                                    else
                                    {
                                        Console.WriteLine("Dont follow!");
                                        Thread.Sleep(1000);
                                    }
                                    driver.SwitchTo().Window(driver.WindowHandles[2]).Close();
                                    driver.SwitchTo().Window(driver.WindowHandles[1]);
                                }
                            }
                            jsExecutor.ExecuteScript(scroll, user);
                            Thread.Sleep(100);
                        }
                    }
                    //jsExecutor.ExecuteScript(scroll, users[users.Count - 1]);
                    h = temp;
                    foreach (DictionaryEntry entry in currentIter)
                    {
                        if (!prevIter.ContainsKey(entry.Key))
                        {
                            prevIter.Add(entry.Key, entry.Value);
                        }
                    }
                    whileIter++;
                    Thread.Sleep(3000);
                }
            }


            //driver.FindElement(By.CssSelector("body")).SendKeys(Keys.Control + "w");
            //IWebElement userNested = user.FindElement(By.XPath("//div"), 5, displayed: true);
            //Thread.Sleep(1000);
            //IWebElement userNestedNested = userNested.FindElement(By.XPath("//div"), 5, displayed: true);
            //Thread.Sleep(1000);

            //user.Click();

            //for (int i = 0; i < 100; ++i)
            //{
            //    var followers = driver.FindElements(By.XPath("//*[text()='Follow']"));
            //    Thread.Sleep(5000);
            //    Console.WriteLine("Elements: {0}", followers.Count);

            //    if (followers.Count == 0)
            //    {
            //        followers = driver.FindElements(By.XPath("//*[text()='Following' or text()='Requested']"));
            //        Thread.Sleep(2000);
            //        jsExecutor.ExecuteScript("arguments[0].scrollIntoView()", followers[followers.Count - 1]);
            //        followers = driver.FindElements(By.XPath("//*[text()='Follow']"));
            //        Thread.Sleep(5000);
            //        Console.WriteLine("Elements: {0}", followers.Count);
            //    }

            //    else
            //    {
            //        for (int j = 0; j < followers.Count; ++j)
            //        {
            //            jsExecutor.ExecuteScript(script, followers[j]);
            //            Thread.Sleep(1000);
            //        }
            //        jsExecutor.ExecuteScript("arguments[0].scrollIntoView()", followers[followers.Count - 1]);
            //        Thread.Sleep(1000);
            //    }
            //}
            // Close the browser
            driver.Quit();
        }
        private static double Lerp(double A, double B, double t)
        {
            return (1.0 - t) * A + t * B;
        }
        private static void NormalizePointToDouble(double w, double h, System.Drawing.Point A, System.Drawing.Point B, out double AOutX, out double AOutY, out double BOutX, out double BOutY)
        {
            double aX = (double)A.X / w;
            double aY = (double)A.Y / h;
            double bX = (double)B.X / w;
            double bY = (double)B.Y / h;
            AOutX = aX;
            AOutY = aY;
            BOutX = bX;
            BOutY = bY;
        }
        private static System.Drawing.Point Lerp(double aX, double aY, double bX, double bY, double w, double h, double t)
        {
            double cX = Lerp(aX, bX, t) * w;
            double cY = Lerp(aY, bY, t) * h;
            return new System.Drawing.Point((int)(cX * w), (int)(cY * h));
        }
        private static System.Drawing.Point[] MoveTo(IWebDriver driver, System.Drawing.Point A, System.Drawing.Point B, int steps)
        {
            System.Drawing.Size size = driver.Manage().Window.Size;
            System.Drawing.Point[] pointsOut = new System.Drawing.Point[steps];
            double aX = 0.0, aY = 0.0, bX = 0.0, bY = 0.0;
            double w = (double)size.Width, h = (double)size.Height;
            NormalizePointToDouble(w, h, A, B, out aX, out aY, out bX, out bY);
            double stepF = 1.0 / (double)steps;
            double step = 0.0;
            for (int i = 0; i < steps; ++i)
            {
                pointsOut[i] = Lerp(aX, aY, bX, bY, w, h, i);
                step += stepF;
            }
            return pointsOut;
        }
        private static bool ShouldFollow(IWebDriver driver)
        {
            Thread.Sleep(2000);
            var findElements = driver.FindElements(By.XPath("//*[@class='_aa_u']"));//"//*[text()='This Account is Private']"));
            Thread.Sleep(500);
            if (findElements.Count == 0)
            {
                Console.WriteLine("Not private!");
                return false;
            }

            IWebElement isPrivate = findElements[0];

            try
            {
                Console.WriteLine("Text: {0}", isPrivate.Text);
            }
            catch(Exception e)
            {
                return false;
            }
            string pathFollowers = "//*[text()=' followers']";
            //Console.WriteLine("href path: {0}", pathFollowers);
            IWebElement follows = driver.FindElement(By.XPath(pathFollowers), 5, displayed: true);
            Thread.Sleep(500);
            int numberOfFollowers = ComputeNumberFromString(follows.Text);
            //Console.WriteLine("Followers: {0}", numberOfFollowers);

            string pathFollowing = "//*[text()=' following']";
            IWebElement following = driver.FindElement(By.XPath(pathFollowing), 5, displayed: true);
            Thread.Sleep(500);
            int numberOfFollowing = ComputeNumberFromString(following.Text);
            //Console.WriteLine("Following: {0}", numberOfFollowing);

            if (numberOfFollowing > numberOfFollowers)
            {
                Console.WriteLine("Number of following bigger than number of followers!");
                return true;
            }
            else
            {
                Console.WriteLine("Number of following less than number of followers!");
                return false;
            }
        }

        private static int ComputeNumberFromString(string s)
        {
            Regex rxK = rxK = new Regex(@"K");
            string kRegex = rxK.Match(s).Value;

            Regex rx = new Regex(@"[0-9.,]+");
            string numberRegex = rx.Match(s).Value;

            int n = 1;
            if (kRegex.Length > 0)
            {
                n = 100;
            }
            int result = 0;
            for (int i = numberRegex.Length - 1; i >= 0; i--)
            {
                if (numberRegex[i] == '.' || numberRegex[i] == ',')
                {
                    continue;
                }
                int digit = (numberRegex[i] - 48) * n;
                result += digit;
                n *= 10;
            }
            return result;
        }
    }
    static class WebDriverExtensions
    {
        /// <summary>
        /// Find an element, waiting until a timeout is reached if necessary.
        /// </summary>
        /// <param name="context">The search context.</param>
        /// <param name="by">Method to find elements.</param>
        /// <param name="timeout">How many seconds to wait.</param>
        /// <param name="displayed">Require the element to be displayed?</param>
        /// <returns>The found element.</returns>
        public static IWebElement FindElement(this ISearchContext context, By by, uint timeout, bool displayed = false)
        {
            var wait = new DefaultWait<ISearchContext>(context);
            wait.Timeout = TimeSpan.FromSeconds(timeout);
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException));
            return wait.Until(ctx => {
                var elem = ctx.FindElement(by);
                if (displayed && !elem.Displayed)
                    return null;

                return elem;
            });
        }
    }
}