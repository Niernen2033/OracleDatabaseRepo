using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace OracleDatabaseProject
{
    class DatabaseManager
    {
        private Random random;
        private List<Accounts> m_accounts;
        private List<Groups> m_groups;
        private List<Subjects> m_subjects;
        private List<Marks> m_marks;
        private List<Students> m_students;
        private List<Teachers> m_teachers;
        private List<Subjects_Teachers> m_subjects_teachers;

        public ReadOnlyCollection<Accounts> GetAccounts { get { return this.m_accounts.AsReadOnly(); } }
        public ReadOnlyCollection<Groups> GetGroups { get { return this.m_groups.AsReadOnly(); } }
        public ReadOnlyCollection<Subjects> GetSubjects { get { return this.m_subjects.AsReadOnly(); } }
        public ReadOnlyCollection<Marks> GetMarks { get { return this.m_marks.AsReadOnly(); } }
        public ReadOnlyCollection<Students> GetStudents { get { return this.m_students.AsReadOnly(); } }
        public ReadOnlyCollection<Teachers> GetTeachers { get { return this.m_teachers.AsReadOnly(); } }
        public ReadOnlyCollection<Subjects_Teachers> GetSubjectsTeachers { get { return this.m_subjects_teachers.AsReadOnly(); } }

        public DatabaseManager()
        {
            this.random = new Random();
            this.m_accounts = new List<Accounts>();
            this.m_groups = new List<Groups>();
            this.m_marks = new List<Marks>();
            this.m_students = new List<Students>();
            this.m_teachers = new List<Teachers>();
            this.m_subjects = new List<Subjects>();
            this.m_subjects_teachers = new List<Subjects_Teachers>();
        }

        private bool GenerateGroups()
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
                this.m_groups.Add(group);
            }
            return true;
        }

        private bool GenerateSubjects()
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
                this.m_subjects.Add(subject);
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

        private string GenerateRandomDate()
        {
            //'YYYY-MM-DD'
            string result = string.Empty;
            int year = this.random.Next(1990, 2020);
            int month = this.random.Next(1, 13);
            int day = 0;
            if (month == 2)
            {
                day = this.random.Next(1, 29);
            }
            else
            {
                day = this.random.Next(1, 31);
            }
            result += year.ToString() + "-";
            if (month < 10)
            {
                result += "0" + month.ToString() + "-";
            }
            else
            {
                result += month.ToString() + "-";
            }
            if(day < 10)
            {
                result += "0" + day.ToString();
            }
            else
            {
                result += day.ToString();
            }
            return result;
        }

        private bool GenerateAccounts(uint count)
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
                account.create_date = this.GenerateRandomDate();
                this.m_accounts.Add(account);
            }

            return true;
        }

        private bool GenerateStudentsTeachers()
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
            foreach (Accounts account in this.m_accounts)
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
                    this.m_teachers.Add(teacher);
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
                    this.m_students.Add(student);
                }
            }
            return true;
        }

        private bool GenerateMarks(uint count)
        {
            for (int i = 0; i < count; i++)
            {
                Marks mark = new Marks();
                mark.mark_id = i + 1;
                mark.create_date = this.GenerateRandomDate();
                mark.mark = this.random.Next(2, 6);
                mark.student_id = this.random.Next(1, this.m_students.Count + 1);
                mark.subject_id = this.random.Next(1, this.m_subjects.Count + 1);
                this.m_marks.Add(mark);
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

        private bool Generate_Subjects_Teachers()
        {
            foreach(Teachers teacher in this.m_teachers)
            {
                int subjects_and_groups_teached_count = this.random.Next(1, 4);
                List<int> subjects_teached_indexes = this.GenerateNumbersWithoutRepetition(subjects_and_groups_teached_count, 1, this.m_subjects.Count);
                List<int> groups_teached_indexes = this.GenerateNumbersWithoutRepetition(subjects_and_groups_teached_count, 1, this.m_groups.Count);
                for (int i=0; i< subjects_and_groups_teached_count; i++)
                {
                    Subjects_Teachers subjects_teacher = new Subjects_Teachers();
                    subjects_teacher.teacher_id = teacher.teacher_id;
                    subjects_teacher.group_id = this.m_groups[groups_teached_indexes[i]].group_id;
                    subjects_teacher.subject_id = this.m_subjects[subjects_teached_indexes[i]].subject_id;
                    this.m_subjects_teachers.Add(subjects_teacher);
                }
            }
            return true;
        }

        private void ClearData()
        {
            this.m_accounts.Clear();
            this.m_groups.Clear();
            this.m_marks.Clear();
            this.m_students.Clear();
            this.m_teachers.Clear();
            this.m_subjects.Clear();
            this.m_subjects_teachers.Clear();
        }

        public bool GenerateDatabase(uint accounts_count, uint marks_count, bool save = false)
        {
            this.ClearData();

            if(!this.GenerateGroups())
            {
                return false;
            }
            else
            {
                if(save)
                {
                    if(!DataManager.Save<Groups>(GlobalVariables.GeneratedDataDatabseDirectory + "GroupsTable.txt", this.m_groups))
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
                    if (!DataManager.Save<Subjects>(GlobalVariables.GeneratedDataDatabseDirectory + "SubjectsTable.txt", this.m_subjects))
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
                    if (!DataManager.Save<Accounts>(GlobalVariables.GeneratedDataDatabseDirectory + "AccountsTable.txt", this.m_accounts))
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
                    if (!DataManager.Save<Students>(GlobalVariables.GeneratedDataDatabseDirectory + "StudentsTable.txt", this.m_students))
                    {
                        DebugManager.Instance.AddLog("Students saving error", this);
                    }
                    if (!DataManager.Save<Teachers>(GlobalVariables.GeneratedDataDatabseDirectory + "TeachersTable.txt", this.m_teachers))
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
                    if (!DataManager.Save<Marks>(GlobalVariables.GeneratedDataDatabseDirectory + "MarksTable.txt", this.m_marks))
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
                    if (!DataManager.Save<Subjects_Teachers>(GlobalVariables.GeneratedDataDatabseDirectory + "Subjects_TeachersTable.txt", this.m_subjects_teachers))
                    {
                        DebugManager.Instance.AddLog("Subjects_Teachers saving error", this);
                    }
                }
            }

            return true;
        }

        public bool LoadDatabaseFromFiles()
        {
            this.ClearData();
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
                        this.m_groups.Add(group);
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
                        this.m_accounts.Add(account);
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
                        this.m_marks.Add(mark);
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
                        student.student_id = int.Parse(table_data[4]);
                        this.m_students.Add(student);
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
                        this.m_subjects_teachers.Add(subject_teacher);
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
                        this.m_subjects.Add(subject);
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
                        this.m_teachers.Add(teacher);
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
