------------------------ Usuarios -------------------------------
INSERT INTO Usuarios (correo, user_password, nombre, apellidos, numero_identificacion) VALUES 
('user1@example.com', 'user1', 'John', 'Doe', 123),
('user2@example.com', 'user2', 'John', 'Doe', 1234),
('user3@example.com', 'user3', 'Alice', 'Smith', 12345),
('user4@example.com', 'user4', 'David', 'Smith', 123456);

------------------------ Administradores -------------------------------
INSERT INTO Administradores (correo, admin_password) VALUES 
('admin1@example.com', 'admin1'),
('admin2@example.com', 'admin2');

------------------------ Categorias -------------------------------
INSERT INTO Categorias (nombre_categoria) VALUES
('Electronica'),
('Ropa'),
('Hogar y Jardin');

------------------------ Productos -------------------------------
INSERT INTO Productos (nombre_producto, descripcion, detalles, precio, stock, id_categoria) VALUES
('Portatil', 'Un portatil de alto rendimiento.', '16GB RAM, 512GB SSD, Intel i7', 999.99, 50, 1),
('Smartphone', 'Un smartphone moderno.', '64GB, Camara de 12MP, Pantalla OLED', 699.99, 100, 1),
('Tablet', 'Una tablet ligera.', '10 pulgadas, 32GB, 4GB RAM', 299.99, 70, 1),
('Auriculares', 'Auriculares con cancelacion de ruido.', 'Bluetooth, 20 horas de bateria', 199.99, 150, 1),
('Camiseta', 'Una camiseta de algodon comoda.', 'Disponible en multiples tamanos y colores.', 19.99, 200, 2),
('Pantalones', 'Pantalones vaqueros.', 'Corte ajustado, varias tallas disponibles.', 39.99, 120, 2),
('Chaqueta', 'Chaqueta de invierno.', 'Resistente al agua, con capucha.', 89.99, 60, 2),
('Zapatos', 'Zapatos deportivos.', 'Disponibles en varias tallas.', 59.99, 80, 2),
('Silla de Jardin', 'Una silla de jardin duradera.', 'Hecha de materiales resistentes a la intemperie.', 49.99, 150, 3),
('Mesa de Jardin', 'Mesa de jardin para exteriores.', 'Mesa de aluminio, 4 plazas.', 119.99, 40, 3),
('Barbacoa', 'Barbacoa de carbon.', 'Tamaño mediano, facil de transportar.', 89.99, 30, 3),
('Lampara Solar', 'Lampara solar para exteriores.', 'Resistente al agua, 8 horas de iluminacion.', 29.99, 100, 3);
