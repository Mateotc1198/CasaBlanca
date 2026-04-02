CREATE DATABASE CasaBlancaHotelDB;
GO

USE CasaBlancaHotelDB;
GO

CREATE TABLE Usuarios (
    IdUsuario INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Apellido NVARCHAR(100) NOT NULL,
    Correo NVARCHAR(150) NOT NULL UNIQUE,
    Telefono NVARCHAR(30) NULL,
    Usuario NVARCHAR(50) NOT NULL UNIQUE,
    Clave NVARCHAR(255) NOT NULL,
    EsAdmin BIT NOT NULL DEFAULT 0,
    FechaRegistro DATETIME NOT NULL DEFAULT GETDATE()
);
GO

CREATE TABLE Hoteles (
    IdHotel INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(150) NOT NULL,
    Ciudad NVARCHAR(100) NOT NULL,
    PrecioPorNoche DECIMAL(10,2) NOT NULL,
    CapacidadMaxima INT NOT NULL,
    Imagen NVARCHAR(255) NULL,
    Activo BIT NOT NULL DEFAULT 1
);
GO

CREATE TABLE Reservas (
    IdReserva INT IDENTITY(1,1) PRIMARY KEY,
    IdUsuario INT NULL,
    IdHotel INT NULL,
    Destino NVARCHAR(150) NOT NULL,
    FechaEntrada DATE NOT NULL,
    FechaSalida DATE NOT NULL,
    Huespedes INT NOT NULL,
    Estado NVARCHAR(30) NOT NULL DEFAULT 'Pendiente',
    FechaCreacion DATETIME NOT NULL DEFAULT GETDATE(),
    PrecioPorNoche DECIMAL(10,2) NULL,
    Noches INT NULL,
    Total DECIMAL(10,2) NULL,
    CONSTRAINT FK_Reservas_Usuarios FOREIGN KEY (IdUsuario) REFERENCES Usuarios(IdUsuario),
    CONSTRAINT FK_Reservas_Hoteles FOREIGN KEY (IdHotel) REFERENCES Hoteles(IdHotel)
);
GO

INSERT INTO Usuarios (Nombre, Apellido, Correo, Telefono, Usuario, Clave, EsAdmin)
VALUES (
    'Mateo',
    'Torres',
    'mateotorres.cardona@gmail.com',
    '1234567890',
    'admin',
    '240be518fabd2724ddb6f04eeb1da5967448d7e831c08c8fa822809f74c720a9',
    1
);
GO

INSERT INTO Hoteles (Nombre, Ciudad, PrecioPorNoche, CapacidadMaxima, Imagen, Activo)
VALUES
('Casa Blanca Resort & Spa', 'Punta Cana', 120.00, 4, 'hotel1.jpg', 1),
('Urban Grand Hotel', 'Bogota', 95.00, 3, 'hotel2.jpg', 1),
('Ocean View Suites', 'Cartagena', 140.00, 5, 'hotel3.jpg', 1);
GO
