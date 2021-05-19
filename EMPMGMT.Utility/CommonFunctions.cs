using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using EMPMGMT.Utility;
using System.IO;
using System.Xml;
using System.Data;
using OfficeOpenXml;
using System.Globalization;

namespace EMPMGMT.Utility
{
    public static class CommonFunctions
    {
        public static void SetCookie(CurrentUser loggedInUser, string pipeSeperatedPermissions, bool rememberMe)
        {
            // Success, create non-persistent authentication cookie.
            FormsAuthentication.SetAuthCookie(loggedInUser.EmailId, false);
            FormsAuthenticationTicket ticket1 =
               new FormsAuthenticationTicket(
                    1,                                   // version
                    loggedInUser.EmailId.Trim(),   // get username  from the form
                    DateTime.Now,                        // issue time is now
                    DateTime.Now.AddMinutes(2080),
                    false,      // cookie is not persistent
                    loggedInUser.RoleName + "|" + pipeSeperatedPermissions// role assignment is stored
                // in userData
                    );
            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket1));
            HttpContext.Current.Response.Cookies.Add(cookie);
            SessionManagement.LoggedInUser = loggedInUser;

            HttpCookie permissionCookie = HttpContext.Current.Response.Cookies["Permissions"];
            if (permissionCookie != null)
                permissionCookie = new HttpCookie("Permissions", "");
            permissionCookie = new HttpCookie("Permissions", pipeSeperatedPermissions);
            //permissionCookie.Expires = DateTime.Now.AddYears(1);
            HttpContext.Current.Response.Cookies.Add(permissionCookie);
            HttpCookie rememberCookie = new HttpCookie("RememberMe");
            if (rememberMe)
            {
                rememberCookie.Values["userName"] = loggedInUser.UserName;
                rememberCookie.Values["password"] = loggedInUser.Password;
                rememberCookie.Values["emailId"] = loggedInUser.EmailId;
                rememberCookie.Expires = DateTime.Now.AddYears(1);
            }
            else
            {
                rememberCookie.Expires = DateTime.Now.AddDays(-1);
            }
            HttpContext.Current.Response.Cookies.Add(rememberCookie);
        }
        public static void RemoveCookies()
        {
            HttpCookie rememberCookie = new HttpCookie("RememberMe");

            rememberCookie.Expires = DateTime.Now.AddDays(-1);
            HttpContext.Current.Response.Cookies.Add(rememberCookie);

        }
        public static void UploadFile(HttpPostedFileBase file, string fileSavingPath, int imageWidth = 0, int imageHeight = 0)
        {

            //if (!Directory.Exists(imageSavingPath))
            //    Directory.CreateDirectory(imageSavingPath);
            if (System.IO.File.Exists(fileSavingPath))
            {
                System.IO.File.Delete(fileSavingPath);
            }
            file.SaveAs(fileSavingPath);
            if (imageWidth > 0 && imageHeight > 0)
            {
                var resizeSettings = new ImageResizer.ResizeSettings
                {
                    Scale = ImageResizer.ScaleMode.DownscaleOnly,
                    Width = imageWidth,
                    Height = imageHeight,
                    Mode = ImageResizer.FitMode.Crop
                };
                var b = ImageResizer.ImageBuilder.Current.Build(fileSavingPath, resizeSettings);
                b.Save(fileSavingPath);
            }

        }
        public static string ToShortGuid(this Guid newGuid, int length)
        {
            string modifiedBase64 = Convert.ToBase64String(newGuid.ToByteArray())
                .Replace("+", " ").Replace("/", " ").Replace("-", " ") // avoid invalid URL characters
                .Substring(6, 6 + length);
            return modifiedBase64;
        }
        public static string ConcatenateStrings(string firstString, string secondString = "")
        {
            string concatenatedString = string.Empty;
            if (!string.IsNullOrEmpty(firstString) && !string.IsNullOrEmpty(secondString))
                concatenatedString = firstString + " " + secondString;
            else if (!string.IsNullOrEmpty(firstString))
                concatenatedString = firstString;
            else if (!string.IsNullOrEmpty(secondString))
                concatenatedString = secondString;

            return concatenatedString;
        }
        public static string GenerateUniqueNumber()
        {
            Guid g = Guid.NewGuid();
            string token = Convert.ToBase64String(g.ToByteArray());
            token = token.Replace("=", "");
            token = token.Replace("+", "");
            return token;
        }

        public static string GetRatingImage(string image, bool islastStage = false, bool isWinClose = false)
        {
            string fileName = "";

            if (islastStage && isWinClose)
                fileName = "winlead";
            else if (islastStage && !isWinClose)
                fileName = "lostlead";
            else
                fileName = image;
            return fileName;
        }
        //public static IList<Utility.WebClasses.Priority> GetPriorities()
        //{
        //    IList<Utility.WebClasses.Priority> priorities = new List<Utility.WebClasses.Priority>();
        //    Priority priority = null;
        //    Array values = Enum.GetValues(typeof(Utility.Enums.TaskPriority));

        //    foreach (Utility.Enums.TaskPriority item in values)
        //    {
        //        priority = new Priority();
        //        priority.PriorityId = (int)item;
        //        priority.PriorityName = Enum.GetName(typeof(Utility.Enums.TaskPriority), item);
        //        priorities.Add(priority);
        //    }
        //    return priorities;
        //}

        //public static IList<Module> GetModuls()
        //{
        //    List<Module> associatedModules = new List<Module>();
        //    Module module = new Module();
        //    module.ModuleId = (int)Utility.Enums.Module.Account;
        //    module.ModuleName = Enum.GetName(typeof(Utility.Enums.Module), Utility.Enums.Module.Account);
        //    associatedModules.Add(module);
        //    module = new Module();
        //    module.ModuleId = (int)Utility.Enums.Module.Lead;
        //    module.ModuleName = Enum.GetName(typeof(Utility.Enums.Module), Utility.Enums.Module.Lead);
        //    associatedModules.Add(module);
        //    module = new Module();
        //    module.ModuleId = (int)Utility.Enums.Module.Contact;
        //    module.ModuleName = Enum.GetName(typeof(Utility.Enums.Module), Utility.Enums.Module.Contact);
        //    associatedModules.Add(module);
        //    return associatedModules;
        //}

        public static string ReadFile(string filePath)
        {
            string text = string.Empty;

            if (File.Exists(filePath))
            {
                System.IO.StreamReader myFile =
            new System.IO.StreamReader(filePath);
                text = myFile.ReadToEnd();
                myFile.Close();
            }
            return text;
        }
        public static string ConfigureActivationMailBody(MailBodyTemplate mailBodyTemplate)
        {
            string messageBody = mailBodyTemplate.MailBody;
            messageBody = messageBody.Replace(Constants.Registration_User_Name, mailBodyTemplate.RegistrationUserName).Replace(Constants.Comment_By_Admin, mailBodyTemplate.Comment_By_Admin)
            .Replace(Constants.Account_Login_UserId, mailBodyTemplate.AccountLoginUserId)
            .Replace(Constants.Account_Login_Passowrd, mailBodyTemplate.AccountLoginPassowrd)
            .Replace(Constants.Account_Login_URL, mailBodyTemplate.AccountLoginUrl)
            .Replace(Constants.Access_For_Deployment_Module, mailBodyTemplate.AccessForDeploymentModule)
            .Replace(Constants.Module_Access_Valid_From, mailBodyTemplate.ModuleAccessValidFrom)
            .Replace(Constants.Module_Access_Valid_Till, mailBodyTemplate.ModuleAccessValidTill)
              .Replace(Constants.Modules_Access_Type, mailBodyTemplate.ModulesAccessType);
            return messageBody;
        }
        public static string ConfigureDeactivationMailBody(MailBodyTemplate mailBodyTemplate)
        {
            string messageBody = mailBodyTemplate.MailBody;
            messageBody = messageBody.Replace(Constants.Registration_User_Name, mailBodyTemplate.RegistrationUserName).Replace(Constants.Comment_By_Admin, mailBodyTemplate.Comment_By_Admin);
            return messageBody;
        }
        public static string ConfigureRegistrationMoreInfoMailBody(MailBodyTemplate mailBodyTemplate)
        {
            string messageBody = mailBodyTemplate.MailBody;
            messageBody = messageBody.Replace(Constants.Registration_User_Name, mailBodyTemplate.RegistrationUserName).Replace(Constants.Comment_By_Admin, mailBodyTemplate.Comment_By_Admin);
            return messageBody;
        }
        public static string ConfigureRegistrationMailBody(MailBodyTemplate mailBodyTemplate)
        {
            string messageBody = mailBodyTemplate.MailBody;
            messageBody = messageBody.Replace(Constants.Registration_User_Name, mailBodyTemplate.RegistrationUserName);
            return messageBody;
        }
        public static string ConfigurePasswordRecoveryMailBody(MailBodyTemplate mailBodyTemplate)
        {
            string messageBody = mailBodyTemplate.MailBody;
            messageBody = messageBody.Replace(Constants.Registration_User_Name, mailBodyTemplate.RegistrationUserName)
            .Replace(Constants.Account_Login_UserId, mailBodyTemplate.AccountLoginUserId)
            .Replace(Constants.Account_Login_Passowrd, mailBodyTemplate.AccountLoginPassowrd)
            .Replace(Constants.Account_Login_URL, mailBodyTemplate.WebSiteLogoPath);
            return messageBody;
        }
        public static string GetSubDomain()
        {
            var host = HttpContext.Current.Request.Headers["HOST"];
            if (!string.IsNullOrEmpty(host))
            {
                if (host.IndexOf(":") > 0)
                {
                    host = host.Substring(0, host.IndexOf(":"));
                }
            }
            else
            {
                host = HttpContext.Current.Request.Url.Host;
            }
            var index = host.IndexOf(".");

            if (index < 0)
            {
                return string.Empty;
            }

            var subdomain = host.Split('.')[0];
            if (subdomain == "www" || subdomain == "localhost")
            {
                return string.Empty;
            }

            return subdomain;
        }


        // new add by for modules name

        public static string GetGlobalizedLabel(string page, string label)
        {
            if (string.IsNullOrWhiteSpace(label))
                return label;
            if (label.Contains(' '))
                return label;


            //if (userCulture.CultureXML != null)
            //{
            string globalTextForLabel = label;

            string xmlSearchPattern = "/CultureInformation/" + page + "/" + label;

            // XmlNode objNode = userCulture.CultureXML.SelectSingleNode(xmlSearchPattern);

            //if (objNode != null)
            //{
            //    globalTextForLabel = objNode.InnerText;
            //}

            return globalTextForLabel;
            ///}
            //return label;
        }

        public static DataSet ImportExceltoDataset(string file)
        {
            int flag = 0;
            //string [] file1= file.ToString().Replace("/","\");
            DataSet data = new DataSet();
            try
            {
                DataTable tbl;
                FileInfo fileInfo = new FileInfo(file);
                if (!fileInfo.Exists)


                    throw new Exception("File " + file + " Does Not Exists");
                using (ExcelPackage xlPackage = new ExcelPackage(fileInfo))
                {
                    for (int i = 1; i <= 1; i++)
                    {
                        tbl = new DataTable();
                        ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets[i];

                        bool hasHeader = true;
                        foreach (var firstRowCell in worksheet.Cells[1, 1, 1, 3])
                        {
                            tbl.Columns.Add(hasHeader ? firstRowCell.Text.Trim() : string.Format("Column {0}", firstRowCell.Start.Column));
                        }
                        var startRow = hasHeader ? 2 : 1;
                        for (var rowNum = startRow; rowNum <= worksheet.Dimension.End.Row; rowNum++)
                        {
                            var wsRow = worksheet.Cells[rowNum, 1, rowNum, 3];
                            var row = tbl.NewRow();
                            foreach (var cell in wsRow)
                            {
                                if (cell.Text.Trim() != "")
                                {
                                    flag = 1;
                                    row[cell.Start.Column - 1] = cell.Text.Trim();
                                }
                            }

                            if (flag == 1)
                            {
                                tbl.Rows.Add(row);
                            }
                            flag = 0;
                        }
                        tbl.TableName = worksheet.Name;
                        data.Tables.Add(tbl);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;

            }
            return data;

        }

        public static int GetIso8601WeekOfYear(this DateTime time)
        {
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                time = time.AddDays(3);
            }

            // Return the week of our adjusted day
            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }

        public static List<string> GetDates(int year, int month)
        {
            return Enumerable.Range(1, DateTime.DaysInMonth(year, month))  // Days: 1, 2 ... 31 etc.
                             .Select(day => new DateTime(year, month, day).ToString("dd")) // Map each day to a date
                             .ToList(); // Load dates into a list
        }

        public static List<string> GetMonthNames(int Year)
        {
            return Enumerable.Range(1, 12)
                .Select(m => new DateTime(Year, m, 1).ToString("MM/yyyy"))
                .ToList();
        }

        //public static List<int> GetWeeksInYear(int year)
        //{
        //   // double weeks = (StratDate - EndDate).TotalDays / 7;
        //    List<int> Weeks = new List<int>();
        //    for (int i = 01; i <= 52; i++)
        //    {
        //        Weeks.Add(i);
        //    }

        //    return Weeks;
        //}

        public static DateTime WeekStartDate(this  DateTime date)
        {
            int delta = Convert.ToInt32(date.DayOfWeek);
            delta = delta == 0 ? delta + 7 : delta;
            DateTime moday = date.AddDays(1 - delta);
            return moday;
        }
        public static DateTime WeekEndDate(this  DateTime date)
        {

            int delta = Convert.ToInt32(date.DayOfWeek);
            delta = delta == 0 ? delta + 7 : delta;
            DateTime sunday = date.AddDays(7 - delta);
            return sunday;
        }

        public static DateTime MonthStartDate(this DateTime date) { return new DateTime(date.Year, date.Month, 1); }
        public static DateTime MonthEndDate(this DateTime date)
        {
            DateTime firstDayOfTheMonth = new DateTime(date.Year, date.Month, 1);
            return firstDayOfTheMonth.AddMonths(1).AddDays(-1);
        }
        public static DateTime YearStartDate(int year)
        {
            return new DateTime(year, 1, 1);
        }
        public static DateTime YearEndDate(int year)
        {
            return new DateTime(year, 12, 31);
        }
        public static List<MDTableHead> GetWeekColumns(DateTime date, bool displayPeriodIsRolling)
        {
            int startFrom = -4, endFrom = 7;
            if (displayPeriodIsRolling) { startFrom = -8; endFrom = 3; }
            List<MDTableHead> mdtableHeads = new List<MDTableHead>();
            int currentWeek = date.GetIso8601WeekOfYear();
            DateTime newStartDate = date.AddWeek(startFrom);
            DateTime newEndDate = date.AddWeek(endFrom);
            DateTime weekStartDate = DateTime.Now;
            DateTime weekEndDate = DateTime.Now;
            int oldWeekNumber = 0;
            for (DateTime counter = newStartDate; counter <= newEndDate; counter = counter.AddWeek(1))
            {
                MDTableHead mdTableHead = new Utility.MDTableHead();
                int weekNumber = counter.GetIso8601WeekOfYear();
                if (oldWeekNumber != weekNumber)
                {
                    weekStartDate = counter.WeekStartDate();
                    weekEndDate = counter.WeekEndDate();
                    mdTableHead.Number = weekNumber;
                    mdTableHead.StartDate = weekStartDate;
                    mdTableHead.EndDate = weekEndDate;
                    mdTableHead.Title = "Week " + weekNumber + ", " + counter.Year;
                    mdtableHeads.Add(mdTableHead);
                }
            }
            mdtableHeads.ForEach(x => x.Selected = x.Number == currentWeek);
            return mdtableHeads;
        }
        public static List<MDTableHead> GetMonthColumns(DateTime selectedDate, bool displayPeriodIsRolling)
        {

            int year = selectedDate.Year;
            List<MDTableHead> mdtableHeads = new List<MDTableHead>();
            DateTime monthStartDate = selectedDate;
            DateTime monthEndDate = selectedDate;


            if (displayPeriodIsRolling)
            {
                monthStartDate = monthStartDate.AddMonths(-8);
                monthEndDate = monthEndDate.AddMonths(3);
            }
            else
            {
                monthStartDate = YearStartDate(selectedDate.Year);
                monthEndDate = YearEndDate(selectedDate.Year);
            }
            string title = "";
            for (DateTime counter = monthStartDate; counter <= monthEndDate; counter = counter.AddMonths(1))
            {
                MDTableHead mdTableHead = new Utility.MDTableHead();
                DateTime date = counter;
                title = date.ToString("MMM yyyy");
                mdTableHead.Number = counter.Month;
                mdTableHead.StartDate = date.MonthStartDate();
                mdTableHead.EndDate = date.MonthEndDate();
                mdTableHead.Title = title;
                mdtableHeads.Add(mdTableHead);
            }
            mdtableHeads.ForEach(x => x.Selected = x.Number == selectedDate.Month);
            return mdtableHeads;
        }
        public static List<MDTableHead> GetDayColumns(DateTime date, bool displayPeriodIsRolling)
        {
            List<MDTableHead> mdtableHeads = new List<MDTableHead>();
            DateTime weekStartDate = date.WeekStartDate();
            DateTime weekEndDate = date.WeekEndDate();
            if (displayPeriodIsRolling)
            {
                weekStartDate = date.AddDays(-5);
                weekEndDate = date.AddDays(1);
            }
            for (DateTime day = weekStartDate; day <= weekEndDate; day = day.AddDays(1))
            {
                MDTableHead mdTableHead = new Utility.MDTableHead();
                mdTableHead.Number = day.Day;
                mdTableHead.StartDate = day;
                mdTableHead.EndDate = day;
                mdTableHead.Title = day.ToString("ddd, dd MMM yyyy");
                //mdTableHead.Title = day.ToString();
                mdtableHeads.Add(mdTableHead);
            }
            mdtableHeads.ForEach(x => x.Selected = x.Number == date.Day);
            return mdtableHeads;
        }
        public static DateTime AddWeek(this DateTime dateTime, int count)
        {
            return dateTime.AddDays(7 * count);
        }

        public static DateTime FirstDateOfWeekISO8601(int year, int weekOfYear)
        {
            DateTime jan1 = new DateTime(year, 1, 1);
            int daysOffset = DayOfWeek.Thursday - jan1.DayOfWeek;

            DateTime firstThursday = jan1.AddDays(daysOffset);
            var cal = CultureInfo.CurrentCulture.Calendar;
            int firstWeek = cal.GetWeekOfYear(firstThursday, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            var weekNum = weekOfYear;
            if (firstWeek <= 1)
            {
                weekNum -= 1;
            }
            var result = firstThursday.AddDays(weekNum * 7);
            return result.AddDays(-3);
        }

        public static List<HighChartParameters> GetNoOfWeeksInYear(DateTime StartDate, DateTime EndDate)
        {
            int oldWeekNumber = 0;
            List<HighChartParameters> weeks = new List<HighChartParameters>();

            for (DateTime counter = StartDate; counter <= EndDate; counter = counter.AddWeek(1))
            {
                int weekNumber = counter.GetIso8601WeekOfYear();
                if (oldWeekNumber != weekNumber)
                {
                    HighChartParameters hc = new HighChartParameters();
                    hc.Date = counter;
                    hc.Number = weekNumber;
                    weeks.Add(hc);
                }
            }
            return weeks;
        }

        public static int? NullIfZero(this int value)
        {
            if (value == 0)
                return null;
            else
                return value;
        }

        public static decimal? NullIfZero(this decimal value)
        {
            if (value == 0)
                return null;
            else
                return value;
        }

        public static List<HighChartParameters> GetDaysByDate(DateTime StartDate, DateTime EndDate)
        {
            List<HighChartParameters> days = new List<HighChartParameters>();
            for (DateTime day = StartDate; day <= EndDate; day = day.AddDays(1))
            {
                HighChartParameters hc = new HighChartParameters();
                hc.Date = day;
                hc.Number = Convert.ToInt32(day.ToString("dd"));
                days.Add(hc);
            }
            return days;
        }

        public static List<HighChartParameters> GetMonthNamesByDate(DateTime StartDate, DateTime EndDate)
        {
            List<HighChartParameters> months = new List<HighChartParameters>();
            for (DateTime month = StartDate; month <= EndDate; month = month.AddMonths(1))
            {
                HighChartParameters hc = new HighChartParameters();
                hc.Date = month;
                hc.Number = Convert.ToInt32(month.ToString("MM"));
                months.Add(hc);
            }
            return months;
        }
        //public static IList<TreeNode> BuildTree(this IEnumerable<TreeNode> source)
        public static List<T> BuildTree<T>(this List<T> source) where T : ITreeNode<T>
        {
            if (source == null || source.Count == 0) return source;
            var groups = source.GroupBy(i => i.ParentObjectId);

            var roots = groups.FirstOrDefault(g => g.Key == null).ToList();

            if (roots.Count > 0)
            {
                var dict = groups.Where(g => g.Key != null).ToDictionary(g => g.Key, g => g.ToList());
                for (int i = 0; i < roots.Count; i++)
                    AddChildren(roots[i], dict);
            }

            return roots;
        }

        public static void AddChildren<T>(T node, IDictionary<string, List<T>> source) where T : ITreeNode<T>
        {
            if (source.ContainsKey(node.ObjectId))
            {
                node.Children = source[node.ObjectId];
                for (int i = 0; i < node.Children.Count; i++)
                    AddChildren(node.Children[i], source);
            }
            else
            {
                node.Children = new List<T>();
            }
        }

        public static IList<EnumData> GetEnumValues(Type enumType)
        {
            IList<EnumData> EnumDataList = new List<EnumData>();
            EnumData enumData = null;
            List<string> values = Enum.GetNames(enumType).ToList();
            foreach (string v in values)
            {
                enumData = new EnumData();
                enumData.EnumID = (int)Enum.Parse(enumType, v);
                enumData.EnumText = v;
                EnumDataList.Add(enumData);
            }
            return EnumDataList;

        }

        public static string GetAnnualObjectiveUrl(string str)
        {
            string strAO = "";
            if (str != "")
            {
                string[] data = str.Split(',');

                for (int i = 0; i < data.Length; i++)
                {
                    string[] ao = data[i].Split('|');
                    strAO += "<a href='/Employee/ManageGoal/" + Convert.ToInt32(ao[0]).Encrypt() + "'>" + ao[1] + "<a>, ";
                }
            }
            return strAO == "" ? "" : strAO.Substring(0, strAO.Length - 2);
        }

      
        public  static int GenerateUniqueNumberNumeric()
        {
            Random rnd = new Random(DateTime.Now.Ticks.GetHashCode());
            int NewNumber = rnd.Next(100000000, 999999999);
            return NewNumber;
        }
    }
}
