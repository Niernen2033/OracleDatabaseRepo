create or replace TRIGGER trigger_name
  BEFORE INSERT ON Groups
  FOR EACH ROW
BEGIN
  :new.group_id := name_of_sequence.nextval;
END;

CREATE SEQUENCE name_of_sequence START WITH 1 INCREMENT BY 1 CACHE 100;

DROP TABLE Groups CASCADE CONSTRAINTS;
DROP TABLE Subjects CASCADE CONSTRAINTS;
DROP TABLE Accounts CASCADE CONSTRAINTS;
DROP TABLE Teachers CASCADE CONSTRAINTS;
DROP TABLE Students CASCADE CONSTRAINTS;
DROP TABLE Marks CASCADE CONSTRAINTS;
DROP TABLE Subjects_Teachers CASCADE CONSTRAINTS;

CREATE TABLE Groups
(
	group_id INT NOT NULL PRIMARY KEY,
    name VARCHAR(20) NOT NULL
);

CREATE TABLE Subjects
(
	subject_id INT NOT NULL PRIMARY KEY,
    title VARCHAR(20) NOT NULL
);

CREATE TABLE Accounts
(
	account_id INT NOT NULL PRIMARY KEY,
    login VARCHAR(3) NOT NULL,
	password VARCHAR(32) NOT NULL,
    email VARCHAR(30) NOT NULL,
    create_date DATE NOT NULL
);

CREATE TABLE Teachers
(
	teacher_id INT NOT NULL PRIMARY KEY,
    first_name VARCHAR(20) NOT NULL,
    last_name VARCHAR(20) NOT NULL,
    account_id INT NOT NULL,
    
    CONSTRAINT account_id_teachers_fk
    FOREIGN KEY (account_id)
    REFERENCES Accounts(account_id)
);

CREATE TABLE Students
(
	student_id INT NOT NULL PRIMARY KEY,
    first_name VARCHAR(20) NOT NULL,
    last_name VARCHAR(20) NOT NULL,
    account_id INT NOT NULL,
            
    CONSTRAINT account_id_students_fk
    FOREIGN KEY (account_id)
    REFERENCES Accounts(account_id)
);

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