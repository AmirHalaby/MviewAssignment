
-- Question 1. b
Drop table if exists Tests;
Create table Tests
(TestDate DateTime Not Null,
ClassName nvarchar(100) Not Null,
StudentID Int Not Null,
Grade Float);

-- Question 1. a
Drop table if exists Student;
Create table Student (StudentID Int Not Null,
StudentFirstName nvarchar(100) not null,
StudentLastName nvarchar(100) not null
);


-- Question 2
Alter table Student add constraint PK_Student primary key (StudentID);
Alter table Tests add constraint PK_Tests primary key (StudentID, ClassName, TestDate);
Alter table Tests add constraint FK_Tests_Student foreign key (StudentID) references Student (StudentID);

-- Question 3
Insert into Student 
Select 1, 'Haiem', 'Daniel' Union all
Select 2, 'Gogo', 'Armali' Union all
Select 3, 'Sash', 'zuzi' Union all
Select 4, 'Victor', 'Baroch' Union all
Select 5, 'Moshee', 'Adani';

Truncate Table Tests;
Insert into Tests 
Select GetDate() - 10, 'Phisics', 1, 90 Union all
Select GetDate() - 3, 'Phisics', 1, 93 Union all
Select GetDate() - 60, 'Mathmatics', 1, 79 Union all
Select GetDate() - 30, 'Mathmatics', 1, 67 Union all
--
Select GetDate() - 10, 'Phisics', 2, 55 Union all
Select GetDate() - 3, 'Phisics', 2, 66 Union all
Select GetDate() - 60, 'Mathmatics', 2, 90 Union all
Select GetDate() - 30, 'Mathmatics', 2, 96 Union all
--
Select GetDate() - 10, 'Phisics', 3, 81 Union all
Select GetDate() - 3, 'Phisics', 3, 88 Union all
Select GetDate() - 60, 'Mathmatics', 3, 88 Union all
Select GetDate() - 30, 'Mathmatics', 3, 63 Union all
--
Select GetDate() - 10, 'Phisics',   4, 81 Union all
Select GetDate() - 3, 'Phisics',    4, 88 Union all
Select GetDate() - 60, 'Mathmatics',4, 88 Union all
Select GetDate() - 30, 'Mathmatics',4, 63;


-- Question 4. a
select t.StudentID, AVG(Grade)
from Student s inner join Tests t on s.StudentID = t.StudentID
Group by t.StudentID , t.ClassName


-- Question 4. b
select t.StudentID, AVG(Grade)
from Student s inner join Tests t on s.StudentID = t.StudentID
Group by t.StudentID



-- Question 4. c

declare @TotalAverage Float = 0;
select @TotalAverage = AVG(Grade) from Tests t 

select 
	t.ClassName, 
	AVG(Grade), 
	@TotalAverage as TotalAvarage, 
	@TotalAverage - AVG(Grade) as Difference,
	(avg(Grade)* 100.0/ (SUM(SUM(Grade)) OVER() / SUM(count(*))over () ))- 100.0 as deviation_from_overall_average
from Tests t 
Group by t.ClassName

-- Question 4. d 1 a
select t.StudentID,  t.ClassName, AVG(Grade) as Avgrage
from Student s inner join Tests t on s.StudentID = t.StudentID
Group by t.StudentID , t.ClassName
-- Question 4. d 1 b
Select StudentID,  ClassName, Avgrage from (
SELECT t.StudentID, t.ClassName, 
         AVG(Grade) OVER(
			PARTITION BY t.StudentID,  t.ClassName  ORDER BY t.StudentID) AS Avgrage,
		        ROW_NUMBER() OVER(
			PARTITION BY t.StudentID,  t.ClassName  ORDER BY t.StudentID) AS RN
  FROM Tests as t) Q
  where Rn = 1;
-- Question 4. d 1 c
  SELECT Distinct t.StudentID, t.ClassName, 
         AVG(Grade) OVER(
			PARTITION BY t.StudentID,  t.ClassName  ORDER BY t.StudentID) AS Avgrage
  FROM Tests as t


-- Question 4. d 2 

  declare @TotalAvg Float = 0;
select @TotalAvg = AVG(Grade) from Tests t 

 Select   ClassName, 
		  Avgrage,  
		  @TotalAvg as TotalAvg,  
		  @TotalAvg - Avgrage as Difference,
		  (Avgrage - @TotalAvg)/@TotalAvg*100 AS percentage_deviation
 from (
    SELECT  t.StudentID, t.ClassName, 
			AVG(Grade) OVER(
			PARTITION BY  t.ClassName  ORDER BY t.ClassName) AS Avgrage,
			ROW_NUMBER() OVER(
			PARTITION BY  t.ClassName  ORDER BY t.ClassName) AS RN
	FROM Tests as t) Q
    WHERE Rn = 1;


