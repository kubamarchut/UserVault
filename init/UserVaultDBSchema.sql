CREATE DATABASE UserVaultDB;
GO

USE UserVaultDB;
GO

CREATE TABLE Users (
    Id INT PRIMARY KEY IDENTITY(1,1),
    FirstName NVARCHAR(50) NOT NULL,
    LastName NVARCHAR(150) NOT NULL,
    DateOfBirth DATE NOT NULL,
    Sex NVARCHAR(6) CHECK (Sex IN ('Male', 'Female'))
);
GO

CREATE TABLE CustomProperties (
    Id INT PRIMARY KEY IDENTITY(1,1),
    UserId INT FOREIGN KEY REFERENCES Users(Id) ON DELETE CASCADE,
    Name NVARCHAR(150) NOT NULL,
    Value NVARCHAR(255) NOT NULL
);
GO

INSERT INTO Users (FirstName, LastName, DateOfBirth, Sex) VALUES
('Jan', 'Kowalski', '1990-01-20', 'Male');

INSERT INTO Users (FirstName, LastName, DateOfBirth, Sex) VALUES
('Joanna', 'Nowakowska', '1985-12-05', 'Female');
GO

INSERT INTO CustomProperties (UserId, Name, Value) VALUES
(1, 'Numer buta', '38'),
(1, 'Numer telefonu', '+48 789 654 123'),
(2, 'Zawód', 'Inżynier budowlany');
GO