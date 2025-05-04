-- 1. Create the database
CREATE DATABASE ProductCatalogDb;
GO

-- 2. Use the database
USE ProductCatalogDb;
GO

-- 3. Create the Categories table
CREATE TABLE Categories (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL
);
GO

-- 4. Create the Products table
CREATE TABLE Products (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    Price DECIMAL(10, 2) NOT NULL,
    Stock INT NOT NULL,
    CategoryId INT NOT NULL,
    FOREIGN KEY (CategoryId) REFERENCES Categories(Id)
);
GO

-- Insert sample categories
INSERT INTO Categories (Name)
VALUES ('Electronics'), ('Furniture'), ('Books');
GO

-- Insert sample products
INSERT INTO Products (Name, Price, Stock, CategoryId)
VALUES 
('Wireless Mouse', 25.99, 100, 1),
('Office Chair', 120.00, 20, 2),
('Novel - Fiction', 15.00, 50, 3);
GO
