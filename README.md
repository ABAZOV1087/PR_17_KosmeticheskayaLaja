CREATE DATABASE KosmeticheskayaLaja;
GO
USE KosmeticheskayaLaja;
GO
CREATE TABLE Roles (
    ID_Role INT PRIMARY KEY IDENTITY(1,1),
    RoleName NVARCHAR(50) NOT NULL
);

CREATE TABLE Users (
    ID_User INT PRIMARY KEY IDENTITY(1,1),
    FullName NVARCHAR(150) NOT NULL,
    Phone NVARCHAR(20) NOT NULL,
    Login NVARCHAR(50) NOT NULL UNIQUE,
    Password NVARCHAR(100) NOT NULL,
    FID_Role INT NOT NULL,
    IsFrozen BIT NOT NULL DEFAULT 0,
    FOREIGN KEY (FID_Role) REFERENCES Roles(ID_Role)
);

-- Таблица Производителей
CREATE TABLE Manufacturers (
    ID_Manufacturer INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL
);

-- Таблица Типов Товаров
CREATE TABLE ProductTypes (
    ID_ProductType INT PRIMARY KEY IDENTITY(1,1),
    TypeName NVARCHAR(100) NOT NULL
);

-- Таблица Товаров
CREATE TABLE Products (
    ID_Product INT PRIMARY KEY IDENTITY(1,1),
    Title NVARCHAR(150) NOT NULL,
    FID_ProductType INT NOT NULL,
    FID_Manufacturer INT NOT NULL,
    Price DECIMAL(10, 2) NOT NULL,
    Discount DECIMAL(5, 2) NOT NULL DEFAULT 0,
    Rating DECIMAL(3, 2) NOT NULL DEFAULT 0,
    Description NVARCHAR(MAX),
    IsFrozen BIT NOT NULL DEFAULT 0,
    FOREIGN KEY (FID_ProductType) REFERENCES ProductTypes(ID_ProductType),
    FOREIGN KEY (FID_Manufacturer) REFERENCES Manufacturers(ID_Manufacturer)
);

-- Таблица Типов Услуг
CREATE TABLE ServiceTypes (
    ID_ServiceType INT PRIMARY KEY IDENTITY(1,1),
    Title NVARCHAR(100) NOT NULL
);

-- Связующая таблица Мастер - Услуга (Многие-ко-многим)
CREATE TABLE MasterServices (
    FID_Master INT NOT NULL,
    FID_ServiceType INT NOT NULL,
    PRIMARY KEY (FID_Master, FID_ServiceType),
    FOREIGN KEY (FID_Master) REFERENCES Users(ID_User),
    FOREIGN KEY (FID_ServiceType) REFERENCES ServiceTypes(ID_ServiceType)
);

-- Таблица Записей на услуги
CREATE TABLE Appointments (
    ID_Appointment INT PRIMARY KEY IDENTITY(1,1),
    FID_Client INT NOT NULL,
    FID_Master INT NOT NULL,
    FID_ServiceType INT NOT NULL,
    AppointmentDate DATE NOT NULL,
    AppointmentTime TIME NOT NULL,
    PaymentMethod NVARCHAR(50) NOT NULL,
    Comment NVARCHAR(MAX),
    IsCompleted BIT NOT NULL DEFAULT 0,
    IsCanceled BIT NOT NULL DEFAULT 0,
    FOREIGN KEY (FID_Client) REFERENCES Users(ID_User),
    FOREIGN KEY (FID_Master) REFERENCES Users(ID_User),
    FOREIGN KEY (FID_ServiceType) REFERENCES ServiceTypes(ID_ServiceType)
);

-- Таблица Заказов косметики
CREATE TABLE Orders (
    ID_Order INT PRIMARY KEY IDENTITY(1,1),
    FID_Client INT NOT NULL,
    OrderDate DATE NOT NULL,
    DeliveryDate DATE NOT NULL,
    PaymentMethod NVARCHAR(50) NOT NULL,
    IsClosed BIT NOT NULL DEFAULT 0,
    FOREIGN KEY (FID_Client) REFERENCES Users(ID_User)
);

-- Таблица Содержимого заказа (Многие-ко-многим)
CREATE TABLE OrderDetails (
    FID_Order INT NOT NULL,
    FID_Product INT NOT NULL,
    Quantity INT NOT NULL DEFAULT 1,
    PRIMARY KEY (FID_Order, FID_Product),
    FOREIGN KEY (FID_Order) REFERENCES Orders(ID_Order),
    FOREIGN KEY (FID_Product) REFERENCES Products(ID_Product)
);
