CREATE DATABASE mydatabase;
use mydatabase;

CREATE TABLE Groups
(
	group_id INT NOT NULL PRIMARY KEY AUTO_INCREMENT,
    name VARCHAR(20) NOT NULL
);

CREATE TABLE Subjects
(
	subject_id INT NOT NULL PRIMARY KEY AUTO_INCREMENT,
    title VARCHAR(20) NOT NULL
);

CREATE TABLE Accounts
(
	account_id INT NOT NULL PRIMARY KEY AUTO_INCREMENT,
    login VARCHAR(3) NOT NULL,
	password VARCHAR(32) NOT NULL,
    email VARCHAR(30) NOT NULL,
    date DATETIME NOT NULL
);

CREATE TABLE Teachers
(
	teacher_id INT NOT NULL PRIMARY KEY AUTO_INCREMENT,
    first_name VARCHAR(20) NOT NULL,
    last_name VARCHAR(20) NOT NULL,
    account_id INT NOT NULL,
    
    CONSTRAINT account_id_teachers_fk
    FOREIGN KEY (account_id)
    REFERENCES Accounts(account_id)
    ON DELETE CASCADE
    ON UPDATE CASCADE
);

CREATE TABLE Students
(
	student_id INT NOT NULL PRIMARY KEY AUTO_INCREMENT,
    first_name VARCHAR(20) NOT NULL,
    last_name VARCHAR(20) NOT NULL,
    account_id INT NOT NULL,
            
    CONSTRAINT account_id_students_fk
    FOREIGN KEY (account_id)
    REFERENCES Accounts(account_id)
    ON DELETE CASCADE
    ON UPDATE CASCADE
);

CREATE TABLE Marks
(
	mark_id INT NOT NULL PRIMARY KEY AUTO_INCREMENT,
    student_id INT NOT NULL,
    subject_id INT NOT NULL,
    date DATETIME NOT NULL,
    mark INT NOT NULL,
    
	CONSTRAINT student_id_marks_fk
    FOREIGN KEY (student_id)
    REFERENCES Students(student_id)
    ON DELETE CASCADE
    ON UPDATE CASCADE,
    
	CONSTRAINT subject_id_marks_fk
    FOREIGN KEY (subject_id)
    REFERENCES Subjects(subject_id)
    ON DELETE CASCADE
    ON UPDATE CASCADE
);

CREATE TABLE Subjects_Teachers
(
	subject_id INT NOT NULL,
    teacher_id INT NOT NULL,
    group_id INT NOT NULL,
    
	CONSTRAINT subject_id_subtea_fk
    FOREIGN KEY (subject_id)
    REFERENCES Subjects(subject_id)
    ON DELETE CASCADE
    ON UPDATE CASCADE,
    
	CONSTRAINT teacher_id_subtea_fk
    FOREIGN KEY (teacher_id)
    REFERENCES Teachers(teacher_id)
    ON DELETE CASCADE
    ON UPDATE CASCADE,
    
    CONSTRAINT group_id_subtea_fk
    FOREIGN KEY (group_id)
    REFERENCES Groups(group_id)
    ON DELETE CASCADE
    ON UPDATE CASCADE
);