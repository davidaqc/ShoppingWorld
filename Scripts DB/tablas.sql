DROP TABLE IF EXISTS Usuarios;
DROP TABLE IF EXISTS Administradores;
DROP TABLE IF EXISTS Productos;
DROP TABLE IF EXISTS Categorias;

CREATE TABLE Usuarios(
    correo VARCHAR(255) NOT NULL,
    user_password VARCHAR(255) NOT NULL,
    nombre VARCHAR(255) NOT NULL,
    apellidos VARCHAR(255) NOT NULL,
    numero_identificacion INT UNIQUE NOT NULL,
    PRIMARY KEY("correo")
);

CREATE TABLE Administradores(
    correo VARCHAR(255) NOT NULL,
    admin_password VARCHAR(255) NOT NULL,
    PRIMARY KEY("correo")
);

CREATE TABLE Categorias (
    id_categoria SERIAL PRIMARY KEY,
    nombre_categoria VARCHAR(255) UNIQUE NOT NULL
);

CREATE TABLE Productos (
    id_producto SERIAL PRIMARY KEY,
    nombre_producto VARCHAR(255) NOT NULL,
    descripcion TEXT,
    detalles TEXT,
    precio DECIMAL(10, 2) NOT NULL,
    stock INT NOT NULL,
    id_categoria INT
);

ALTER TABLE Productos
ADD CONSTRAINT fk_id_categoria
FOREIGN KEY (id_categoria) REFERENCES Categorias(id_categoria);