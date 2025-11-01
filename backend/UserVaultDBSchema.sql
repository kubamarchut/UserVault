CREATE DATABASE UserVaultDB;

USE UserVaultDB;

CREATE TABLE Users (
    Id INT PRIMARY KEY IDENTITY(1,1),
    FirstName NVARCHAR(50) NOT NULL,
    LastName NVARCHAR(150) NOT NULL,
    DateOfBirth DATE NOT NULL,
    Sex NVARCHAR(6) CHECK (Sex IN ('Male', 'Female'))
);

CREATE TABLE CustomProperties (
    Id INT PRIMARY KEY IDENTITY(1,1),
    UserId INT FOREIGN KEY REFERENCES Users(Id) ON DELETE CASCADE,
    Name NVARCHAR(150) NOT NULL,
    Value NVARCHAR(255) NOT NULL
);

INSERT INTO Users (FirstName, LastName, DateOfBirth, Sex) VALUES
('Jan', 'Kowalski', '1990-01-20', 'Male');

INSERT INTO Users (FirstName, LastName, DateOfBirth, Sex) VALUES
('Joanna', 'Nowakowska', '1985-12-05', 'Female');


INSERT INTO CustomProperties (UserId, Name, Value) VALUES
(1, 'Numer buta', '38'),
(1, 'Numer telefonu', '+48 789 654 123'),
(2, 'Zawód', 'In¿ynier budowlany')

SELECT * FROM Users LEFT JOIN CustomProperties On Users.Id = CustomProperties.UserId;
SELECT * FROM CustomProperties;

DELETE FROM Users WHERE Id = 11;