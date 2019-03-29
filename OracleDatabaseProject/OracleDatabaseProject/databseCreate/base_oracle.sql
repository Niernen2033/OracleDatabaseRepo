DROP TABLE Groups CASCADE CONSTRAINTS PURGE;
DROP TABLE Subjects CASCADE CONSTRAINTS PURGE;
DROP TABLE Accounts CASCADE CONSTRAINTS PURGE;
DROP TABLE Teachers CASCADE CONSTRAINTS PURGE;
DROP TABLE Students CASCADE CONSTRAINTS PURGE;
DROP TABLE Marks CASCADE CONSTRAINTS PURGE;
DROP TABLE Subjects_Teachers CASCADE CONSTRAINTS PURGE;

CREATE TABLE Groups
(
    group_id INT NOT NULL PRIMARY KEY,
    name VARCHAR(50) NOT NULL
);

CREATE SEQUENCE seq_Groups START WITH 1 INCREMENT BY 1 CACHE 100;

CREATE OR REPLACE TRIGGER tri_Groups BEFORE INSERT ON Groups FOR EACH ROW
BEGIN
  :new.group_id := seq_Groups.nextval;
END;

CREATE TABLE Subjects
(
	subject_id INT NOT NULL PRIMARY KEY,
    title VARCHAR(50) NOT NULL
);

CREATE SEQUENCE seq_Subjects START WITH 1 INCREMENT BY 1 CACHE 100;

CREATE OR REPLACE TRIGGER tri_Subjects BEFORE INSERT ON Subjects FOR EACH ROW
BEGIN
  :new.subject_id := seq_Subjects.nextval;
END;

CREATE TABLE Accounts
(
	account_id INT NOT NULL PRIMARY KEY,
    login VARCHAR(50) NOT NULL,
	password VARCHAR(32) NOT NULL,
    email VARCHAR(50) NOT NULL,
    is_teacher NUMBER(1,0) NOT NULL,
    create_date DATE NOT NULL
);

CREATE SEQUENCE seq_Accounts START WITH 1 INCREMENT BY 1 CACHE 100;

CREATE OR REPLACE TRIGGER tri_Accounts BEFORE INSERT ON Accounts FOR EACH ROW
BEGIN
  :new.account_id := seq_Accounts.nextval;
END;

CREATE TABLE Teachers
(
	teacher_id INT NOT NULL PRIMARY KEY,
    first_name VARCHAR(50) NOT NULL,
    last_name VARCHAR(50) NOT NULL,
    professionally_active NUMBER(1,0) NOT NULL,
    account_id INT NOT NULL,
    
    CONSTRAINT account_id_teachers_fk
    FOREIGN KEY (account_id)
    REFERENCES Accounts(account_id)
);

CREATE SEQUENCE seq_Teachers START WITH 1 INCREMENT BY 1 CACHE 100;

CREATE OR REPLACE TRIGGER tri_Teachers BEFORE INSERT ON Teachers FOR EACH ROW
BEGIN
  :new.teacher_id := seq_Teachers.nextval;
END;

CREATE TABLE Students
(
	student_id INT NOT NULL PRIMARY KEY,
    first_name VARCHAR(50) NOT NULL,
    last_name VARCHAR(50) NOT NULL,
    student_index INT NOT NULL,
    account_id INT NOT NULL,
            
    CONSTRAINT account_id_students_fk
    FOREIGN KEY (account_id)
    REFERENCES Accounts(account_id)
);

CREATE SEQUENCE seq_Students START WITH 1 INCREMENT BY 1 CACHE 100;

CREATE OR REPLACE TRIGGER tri_Students BEFORE INSERT ON Students FOR EACH ROW
BEGIN
  :new.student_id := seq_Students.nextval;
END;

CREATE TABLE Marks
(
	mark_id INT NOT NULL PRIMARY KEY,
    student_id INT NOT NULL,
    subject_id INT NOT NULL,
    create_date DATE NOT NULL,
    mark INT NOT NULL,
    
	CONSTRAINT student_id_marks_fk
    FOREIGN KEY (student_id)
    REFERENCES Students(student_id),
    
	CONSTRAINT subject_id_marks_fk
    FOREIGN KEY (subject_id)
    REFERENCES Subjects(subject_id)
);

CREATE SEQUENCE seq_Marks START WITH 1 INCREMENT BY 1 CACHE 100;

CREATE OR REPLACE TRIGGER tri_Marks BEFORE INSERT ON Marks FOR EACH ROW
BEGIN
  :new.mark_id := seq_Marks.nextval;
END;

CREATE TABLE Subjects_Teachers
(
	subject_id INT NOT NULL,
    teacher_id INT NOT NULL,
    group_id INT NOT NULL,
    
	CONSTRAINT subject_id_subtea_fk
    FOREIGN KEY (subject_id)
    REFERENCES Subjects(subject_id),
    
	CONSTRAINT teacher_id_subtea_fk
    FOREIGN KEY (teacher_id)
    REFERENCES Teachers(teacher_id),
    
    CONSTRAINT group_id_subtea_fk
    FOREIGN KEY (group_id)
    REFERENCES Groups(group_id)
);