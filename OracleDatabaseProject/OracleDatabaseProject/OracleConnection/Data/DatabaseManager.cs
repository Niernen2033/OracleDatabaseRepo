using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace OracleDatabaseProject
{
    class DatabaseData
    {
        public List<Accounts> Accounts { get; set; }
        public List<Groups> Groups { get; set; }
        public List<Subjects> Subjects { get; set; }
        public List<Marks> Marks { get; set; }
        public List<Students> Students { get; set; }
        public List<Teachers> Teachers { get; set; }
        public List<Subjects_Teachers> Subjects_Teachers { get; set; }

        public DatabaseData()
        {
            this.Accounts = new List<Accounts>();
            this.Groups = new List<Groups>();
            this.Marks = new List<Marks>();
            this.Students = new List<Students>();
            this.Teachers = new List<Teachers>();
            this.Subjects = new List<Subjects>();
            this.Subjects_Teachers = new List<Subjects_Teachers>();
        }

        public void Clear()
        {
            this.Accounts.Clear();
            this.Groups.Clear();
            this.Marks.Clear();
            this.Students.Clear();
            this.Teachers.Clear();
            this.Subjects.Clear();
            this.Subjects_Teachers.Clear();
        }
    }
    class DatabaseManager
    {
        private Random random;
        private RandomGauss randomGauss;
        public DatabaseData DatabaseData { get; private set; }

        public DatabaseManager()
        {
            this.random = new Random();
            this.randomGauss = new RandomGauss();
            this.DatabaseData = new DatabaseData();
        }

        public bool GenerateGroups()
        {
            List<string> _names = new List<string>();
            if (!DataManager.Load(GlobalVariables.DataToGenerateDatabseDirectory + "gname_db.txt", out _names))
            {
                return false;
            }
            for(int i=0; i< _names.Count; i++)
            {
                Groups group = new Groups();
                group.group_id = i + 1;
                group.name = _names[i];
                this.DatabaseData.Groups.Add(group);
            }
            return true;
        }

        public bool GenerateSubjects()
        {
            List<string> _titles = new List<string>();
            if (!DataManager.Load(GlobalVariables.DataToGenerateDatabseDirectory + "title_db.txt", out _titles))
            {
                return false;
            }
            for (int i = 0; i < _titles.Count; i++)
            {
                Subjects subject = new Subjects();
                subject.subject_id = i + 1;
                subject.title = _titles[i];
                this.DatabaseData.Subjects.Add(subject);
            }
            return true;
        }

        private int GetTrueWithProb(int probForTrue)
        {
            int tempRandom = this.random.Next(0, 101);
            if(tempRandom < probForTrue)
            {
                return 1;
            }
            return 0;
        }

        private DateTime GenerateRandomDate(int startYear = 0, int startMonth = 0, int startDay = 0)
        {
            int defaultYear = (startYear != 0) ? startYear : 1990;
            int defaultMonth = (startMonth != 0) ? startMonth : 1;
            int defaultDay = (startDay != 0) ? startDay : 1;
            DateTime startDate = new DateTime(defaultYear, defaultMonth, defaultDay);
            int range = (DateTime.Today - startDate).Days;
            return startDate.AddDays(this.random.Next(range));
        }

        public bool GenerateAccounts(uint count)
        {
            List<string> _logins = new List<string>();
            List<string> _passwords = new List<string>();
            List<string> _emails = new List<string>();
            if (!DataManager.Load(GlobalVariables.DataToGenerateDatabseDirectory + "logins_db.txt", out _logins))
            {
                return false;
            }
            if (!DataManager.Load(GlobalVariables.DataToGenerateDatabseDirectory + "password_db.txt", out _passwords))
            {
                return false;
            }
            if (!DataManager.Load(GlobalVariables.DataToGenerateDatabseDirectory + "email_db.txt", out _emails))
            {
                return false;
            }

            for (int i = 0; i < count; i++)
            {
                Accounts account = new Accounts();
                account.account_id = i + 1;
                account.email = _emails[this.random.Next(0, _emails.Count)];
                account.password = _passwords[this.random.Next(0, _passwords.Count)];
                account.login = _logins[this.random.Next(0, _logins.Count)];
                account.is_teacher = this.GetTrueWithProb(10);
                account.create_date = this.GenerateRandomDate().ToShortDateString();
                this.DatabaseData.Accounts.Add(account);
            }

            return true;
        }

        public bool GenerateStudentsTeachers()
        {
            List<string> _firstnames = new List<string>();
            List<string> _surnames = new List<string>();
            if (!DataManager.Load(GlobalVariables.DataToGenerateDatabseDirectory + "firstnames_db.txt", out _firstnames))
            {
                return false;
            }
            if (!DataManager.Load(GlobalVariables.DataToGenerateDatabseDirectory + "surnames_db.txt", out _surnames))
            {
                return false;
            }

            int students_count = 1;
            int students_index_start = this.random.Next(100000, 110000);
            int teachers_count = 1;
            foreach (Accounts account in this.DatabaseData.Accounts)
            {
                if(account.is_teacher == 1)
                {
                    Teachers teacher = new Teachers();
                    teacher.teacher_id = teachers_count;
                    teacher.account_id = account.account_id;
                    teacher.first_name = _firstnames[this.random.Next(0, _firstnames.Count)];
                    teacher.professionally_active = this.GetTrueWithProb(65);
                    teacher.last_name = _surnames[this.random.Next(0, _surnames.Count)];
                    teachers_count++;
                    this.DatabaseData.Teachers.Add(teacher);
                }
                else
                {
                    Students student = new Students();
                    student.student_id = students_count;
                    student.account_id = account.account_id;
                    student.first_name = _firstnames[this.random.Next(0, _firstnames.Count)];
                    student.last_name = _surnames[this.random.Next(0, _surnames.Count)];
                    student.student_index = students_index_start + students_count;
                    students_count++;
                    this.DatabaseData.Students.Add(student);
                }
            }
            return true;
        }

        public bool GenerateMarks(uint count, bool realDate = true)
        {
            this.DatabaseData.Marks.Clear();
            for (int i = 0; i < count; i++)
            {
                Marks mark = new Marks();
                mark.mark_id = i + 1;
                mark.student_id = this.random.Next(1, this.DatabaseData.Students.Count + 1);
                mark.subject_id = this.random.Next(1, this.DatabaseData.Subjects.Count + 1);
                int chance = this.random.Next(0, 101);
                if(chance <= 20)
                {
                    mark.mark = this.randomGauss.Next(2, 4);
                }
                else if (chance <= 80)
                {
                    mark.mark = this.randomGauss.Next(3, 5);
                }
                else
                {
                    mark.mark = this.randomGauss.Next(4, 6);
                }
                if (realDate)
                {
                    Accounts account = this.DatabaseData.Accounts[this.DatabaseData.Students[mark.student_id - 1].account_id - 1];
                    DateTime accCreateDate = Convert.ToDateTime(account.create_date);
                    mark.create_date = this.GenerateRandomDate(accCreateDate.Year, accCreateDate.Month, accCreateDate.Day).ToShortDateString();
                }
                else
                {
                    mark.create_date = this.GenerateRandomDate().ToShortDateString();
                }
                this.DatabaseData.Marks.Add(mark);
            }
            return true;
        }

        private List<int> GenerateNumbersWithoutRepetition(int count, int begin, int end)
        {
            List<int> result = new List<int>();
            if((end-begin) < 1 || (end - begin) < count)
            {
                return result;
            }

            for(int i=0; i<count; i++)
            {
                while(true)
                {
                    int random_number = this.random.Next(begin, end);
                    if(!result.Contains(random_number))
                    {
                        result.Add(random_number);
                        break;
                    }
                }
            }

            return result;
        }

        public bool Generate_Subjects_Teachers()
        {
            foreach(Teachers teacher in this.DatabaseData.Teachers)
            {
                int subjects_and_groups_teached_count = this.random.Next(1, 4);
                List<int> subjects_teached_indexes = this.GenerateNumbersWithoutRepetition(subjects_and_groups_teached_count, 1, this.DatabaseData.Subjects.Count);
                List<int> groups_teached_indexes = this.GenerateNumbersWithoutRepetition(subjects_and_groups_teached_count, 1, this.DatabaseData.Groups.Count);
                for (int i=0; i< subjects_and_groups_teached_count; i++)
                {
                    Subjects_Teachers subjects_teacher = new Subjects_Teachers();
                    subjects_teacher.teacher_id = teacher.teacher_id;
                    subjects_teacher.group_id = this.DatabaseData.Groups[groups_teached_indexes[i]].group_id;
                    subjects_teacher.subject_id = this.DatabaseData.Subjects[subjects_teached_indexes[i]].subject_id;
                    this.DatabaseData.Subjects_Teachers.Add(subjects_teacher);
                }
            }
            return true;
        }

        public bool GenerateDatabase(uint accounts_count, uint marks_count, bool statistics, bool save = false)
        {
            this.DatabaseData.Clear();

            if(!this.GenerateGroups())
            {
                return false;
            }
            else
            {
                if(save)
                {
                    if(!DataManager.Save<Groups>(GlobalVariables.GeneratedDataDatabseDirectory + "GroupsTable.txt", this.DatabaseData.Groups))
                    {
                        DebugManager.Instance.AddLog("Groups saving error", this);
                    }
                }
            }

            if(!this.GenerateSubjects())
            {
                return false;
            }
            else
            {
                if (save)
                {
                    if (!DataManager.Save<Subjects>(GlobalVariables.GeneratedDataDatabseDirectory + "SubjectsTable.txt", this.DatabaseData.Subjects))
                    {
                        DebugManager.Instance.AddLog("Subjects saving error", this);
                    }
                }
            }

            if (!this.GenerateAccounts(accounts_count))
            {
                return false;
            }
            else
            {
                if (save)
                {
                    if (!DataManager.Save<Accounts>(GlobalVariables.GeneratedDataDatabseDirectory + "AccountsTable.txt", this.DatabaseData.Accounts))
                    {
                        DebugManager.Instance.AddLog("Accounts saving error", this);
                    }
                }
            }

            if (!this.GenerateStudentsTeachers())
            {
                return false;
            }
            else
            {
                if (save)
                {
                    if (!DataManager.Save<Students>(GlobalVariables.GeneratedDataDatabseDirectory + "StudentsTable.txt", this.DatabaseData.Students))
                    {
                        DebugManager.Instance.AddLog("Students saving error", this);
                    }
                    if (!DataManager.Save<Teachers>(GlobalVariables.GeneratedDataDatabseDirectory + "TeachersTable.txt", this.DatabaseData.Teachers))
                    {
                        DebugManager.Instance.AddLog("Teachers saving error", this);
                    }
                }
            }

            if (!this.GenerateMarks(marks_count))
            {
                return false;
            }
            else
            {
                if (save)
                {
                    if (!DataManager.Save<Marks>(GlobalVariables.GeneratedDataDatabseDirectory + "MarksTable.txt", this.DatabaseData.Marks))
                    {
                        DebugManager.Instance.AddLog("Marks saving error", this);
                    }
                }
            }

            if (!this.Generate_Subjects_Teachers())
            {
                return false;
            }
            else
            {
                if (save)
                {
                    if (!DataManager.Save<Subjects_Teachers>(GlobalVariables.GeneratedDataDatabseDirectory + "Subjects_TeachersTable.txt", this.DatabaseData.Subjects_Teachers))
                    {
                        DebugManager.Instance.AddLog("Subjects_Teachers saving error", this);
                    }
                }
            }

            if (statistics)
            {
                if (!this.CreateDatabaseStatistics())
                {
                    return false;
                }
            }

            return true;
        }

        private bool CreateDatabaseStatistics()
        {
            if(!this.LoadDatabaseFromFiles())
            {
                return false;
            }

            Dictionary<string, int> Students_Teachers_Count = new Dictionary<string, int>()
            {
                { "Students", 0 },
                { "Teachers", 0 }
            };
            foreach (Accounts item in this.DatabaseData.Accounts)
            {
                if(item.is_teacher == 1)
                {
                    Students_Teachers_Count["Teachers"]++;
                }
                else
                {
                    Students_Teachers_Count["Students"]++;
                }
            }

            if (!DataManager.Save<string,int>(GlobalVariables.GeneratedDataDatabseDirectory + "AccountsSTTableStatistics.txt", Students_Teachers_Count))
            {
                return false;
            }

            Dictionary<string, int> Students_Names = new Dictionary<string, int>();
            foreach(Students item in this.DatabaseData.Students)
            {
                if(!Students_Names.ContainsKey(item.first_name))
                {
                    Students_Names.Add(item.first_name, 1);
                    continue;
                }
                else
                {
                    Students_Names[item.first_name]++;
                }
            }

            if (!DataManager.Save<string, int>(GlobalVariables.GeneratedDataDatabseDirectory + "StudentsNamesTableStatistics.txt", Students_Names))
            {
                return false;
            }

            Dictionary<int, int> Marks_Count = new Dictionary<int, int>();
            Dictionary<int, int> Marks_Date = new Dictionary<int, int>();
            foreach (Marks item in this.DatabaseData.Marks)
            {
                if (!Marks_Count.ContainsKey(item.mark))
                {
                    Marks_Count.Add(item.mark, 1);
                }
                else
                {
                    Marks_Count[item.mark]++;
                }
                DateTime date = Convert.ToDateTime(item.create_date);
                if(!Marks_Date.ContainsKey(date.Month))
                {
                    Marks_Date.Add(date.Month, 1);
                }
                else
                {
                    Marks_Date[date.Month]++;
                }
            }

            if (!DataManager.Save<int, int>(GlobalVariables.GeneratedDataDatabseDirectory + "MarksCountTableStatistics.txt", Marks_Count))
            {
                return false;
            }
            if (!DataManager.Save<int, int>(GlobalVariables.GeneratedDataDatabseDirectory + "MarksDateTableStatistics.txt", Marks_Date))
            {
                return false;
            }

            return true;
        }

        public bool LoadDatabaseFromFiles()
        {
            this.DatabaseData.Clear();
            List<string> temp_data = new List<string>();

            //Group *******************************************
            if(!DataManager.Load(GlobalVariables.GeneratedDataDatabseDirectory + "GroupsTable.txt", out temp_data))
            {
                return false;
            }
            else
            {
                foreach (string line in temp_data)
                {
                    string[] table_data = line.Split(';');
                    Groups group = new Groups();
                    try
                    {
                        group.group_id = int.Parse(table_data[0]);
                        group.name = table_data[1];
                        this.DatabaseData.Groups.Add(group);
                    }
                    catch(Exception exc)
                    {
                        DebugManager.Instance.AddLog(exc.Message, this);
                        return false;
                    }
                    
                }
            }

            //Accounts *******************************************
            if (!DataManager.Load(GlobalVariables.GeneratedDataDatabseDirectory + "AccountsTable.txt", out temp_data))
            {
                return false;
            }
            else
            {
                foreach (string line in temp_data)
                {
                    string[] table_data = line.Split(';');
                    Accounts account = new Accounts();
                    try
                    {
                        account.account_id = int.Parse(table_data[0]);
                        account.login = table_data[1];
                        account.password = table_data[2];
                        account.email = table_data[3];
                        account.is_teacher = int.Parse(table_data[4]);
                        account.create_date = table_data[5];
                        this.DatabaseData.Accounts.Add(account);
                    }
                    catch (Exception exc)
                    {
                        DebugManager.Instance.AddLog(exc.Message, this);
                        return false;
                    }

                }
            }

            //Marks *******************************************
            if (!DataManager.Load(GlobalVariables.GeneratedDataDatabseDirectory + "MarksTable.txt", out temp_data))
            {
                return false;
            }
            else
            {
                foreach (string line in temp_data)
                {
                    string[] table_data = line.Split(';');
                    Marks mark = new Marks();
                    try
                    {
                        mark.mark_id = int.Parse(table_data[0]);
                        mark.student_id = int.Parse(table_data[1]);
                        mark.subject_id = int.Parse(table_data[2]);
                        mark.create_date = table_data[3];
                        mark.mark = int.Parse(table_data[4]);
                        this.DatabaseData.Marks.Add(mark);
                    }
                    catch (Exception exc)
                    {
                        DebugManager.Instance.AddLog(exc.Message, this);
                        return false;
                    }

                }
            }

            //Students *******************************************
            if (!DataManager.Load(GlobalVariables.GeneratedDataDatabseDirectory + "StudentsTable.txt", out temp_data))
            {
                return false;
            }
            else
            {
                foreach (string line in temp_data)
                {
                    string[] table_data = line.Split(';');
                    Students student = new Students();
                    try
                    {
                        student.student_id = int.Parse(table_data[0]);
                        student.first_name = table_data[1];
                        student.last_name = table_data[2];
                        student.student_index = int.Parse(table_data[3]);
                        student.account_id = int.Parse(table_data[4]);
                        this.DatabaseData.Students.Add(student);
                    }
                    catch (Exception exc)
                    {
                        DebugManager.Instance.AddLog(exc.Message, this);
                        return false;
                    }

                }
            }

            //Subjects_Teachers *******************************************
            if (!DataManager.Load(GlobalVariables.GeneratedDataDatabseDirectory + "Subjects_TeachersTable.txt", out temp_data))
            {
                return false;
            }
            else
            {
                foreach (string line in temp_data)
                {
                    string[] table_data = line.Split(';');
                    Subjects_Teachers subject_teacher = new Subjects_Teachers();
                    try
                    {
                        subject_teacher.subject_id = int.Parse(table_data[0]);
                        subject_teacher.teacher_id = int.Parse(table_data[1]);
                        subject_teacher.group_id = int.Parse(table_data[2]);
                        this.DatabaseData.Subjects_Teachers.Add(subject_teacher);
                    }
                    catch (Exception exc)
                    {
                        DebugManager.Instance.AddLog(exc.Message, this);
                        return false;
                    }

                }
            }

            //Subjects *******************************************
            if (!DataManager.Load(GlobalVariables.GeneratedDataDatabseDirectory + "SubjectsTable.txt", out temp_data))
            {
                return false;
            }
            else
            {
                foreach (string line in temp_data)
                {
                    string[] table_data = line.Split(';');
                    Subjects subject = new Subjects();
                    try
                    {
                        subject.subject_id = int.Parse(table_data[0]);
                        subject.title = table_data[1];
                        this.DatabaseData.Subjects.Add(subject);
                    }
                    catch (Exception exc)
                    {
                        DebugManager.Instance.AddLog(exc.Message, this);
                        return false;
                    }

                }
            }

            //Teachers *******************************************
            if (!DataManager.Load(GlobalVariables.GeneratedDataDatabseDirectory + "TeachersTable.txt", out temp_data))
            {
                return false;
            }
            else
            {
                foreach (string line in temp_data)
                {
                    string[] table_data = line.Split(';');
                    Teachers teacher = new Teachers();
                    try
                    {
                        teacher.teacher_id = int.Parse(table_data[0]);
                        teacher.first_name = table_data[1];
                        teacher.last_name = table_data[2];
                        teacher.professionally_active = int.Parse(table_data[3]);
                        teacher.account_id = int.Parse(table_data[4]);
                        this.DatabaseData.Teachers.Add(teacher);
                    }
                    catch (Exception exc)
                    {
                        DebugManager.Instance.AddLog(exc.Message, this);
                        return false;
                    }

                }
            }

            return true;
        }
    }
}
