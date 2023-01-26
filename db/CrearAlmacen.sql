CREATE DATABASE IW_MYSQL;

USE IW_MYSQL;
PRAGMA foreign_keys = ON;


CREATE TABLE Usuario
(
email varchar (50) PRIMARY KEY,
password varchar (25) not null,
nombre varchar (25),
apellidos varchar (25),
dni varchar (9),
telefono int,
calle varchar (50),
codpos int,
fechanac date
);


CREATE TABLE Almacen
(
id INTEGER  NOT NULL PRIMARY KEY AUTOINCREMENT,
localidad VARCHAR (25),
nombre VARCHAR (25)
);

CREATE TABLE Categoria
(

categoria varchar (25) PRIMARY KEY,
empresa varchar (25)
);

CREATE TABLE Articulo
(
cod INTEGER  NOT NULL PRIMARY KEY AUTOINCREMENT,
nombre varchar (25),
especificaciones TEXT,
pvp number (6,2),
imagen text,
categoria varchar (25) not null,
CONSTRAINT fk_categoria FOREIGN KEY (categoria) REFERENCES Categoria (categoria)
);

CREATE TABLE Almacen_Articulo
(
    codAlm int CONSTRAINT fk_almacen   REFERENCES Almacen (id),
    codArt int CONSTRAINT fk_articulo   REFERENCES Articulo (cod),
    cantidad int not null,
    CONSTRAINT pk_almacen_articulo PRIMARY KEY (codAlm, codArt)
    
);


CREATE TABLE Pedido
(
numped INTEGER  NOT NULL PRIMARY KEY AUTOINCREMENT,
venta boolean DEFAULT true,
fecha_pedido date not null,
fecha_factura date
);


CREATE TABLE Linped
(
linea INTEGER  NOT NULL PRIMARY KEY AUTOINCREMENT,
cantidad int DEFAULT 0,
codArt int CONSTRAINT fk_articulo  REFERENCES Articulo (cod),
pedido int CONSTRAINT fk_pedido REFERENCES Pedido (numped)
);

create TABLE Movimiento
(
id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
almacen_salida int CONSTRAINT fk_almacen_salida REFERENCES Almacen (id),
almacen_entrada int CONSTRAINT fk_almacen_entrada REFERENCES Almacen (id),
articulo string,
cantidad int
);

create TABLE ArticuloMovimiento
(
    id INTEGER not NULL PRIMARY KEY AUTOINCREMENT,
    articulo string,
    cantidad int,
    idmovimiento int CONSTRAINT fk_movimiento REFERENCES Movimiento (id)
);




INSERT INTO Usuario VALUES ('user@alu.es', '123', 'usuario', 'user1 user1', '12345678A', 123456789, 'calle1', 03030 , '2020-02-20' );
INSERT INTO Usuario VALUES ('user2@alu.es', '123', 'usuario2', 'user2 user2', '98765432A', 123456789, 'calle2', 03031 , '2022-03-21' );

INSERT INTO Almacen (localidad, nombre) VALUES ('Alicante', 'Almacen1');
INSERT INTO Almacen (localidad, nombre) VALUES ('Valencia', 'Almacen2');

INSERT INTO Categoria VALUES ('Adidas', 'Adidas');
INSERT INTO Categoria VALUES ('Nike', 'Nike');

INSERT INTO Articulo (nombre, especificaciones, pvp, imagen, categoria) values ('zapas1', 'unas zapatillas de superputamadresocio', 35,"PRUEBA SIN IMAGEN" , 'Adidas');
INSERT INTO Articulo (nombre, especificaciones, pvp, imagen, categoria) values ('coche1', 'broom broom', 400000,"PRUEBA SIN IMAGEN" , 'Ferrari');
INSERT INTO Articulo (nombre, especificaciones, pvp, imagen, categoria) values ('zapas2', 'unas zapatillas de superputamadresocio', 35,"PRUEBA SIN IMAGEN" , 'Adidas');
INSERT INTO Articulo (nombre, especificaciones, pvp, imagen, categoria) values ('coche2', 'broom broom', 400000,"PRUEBA SIN IMAGEN" , 'Ferrari');
INSERT INTO Articulo (nombre, especificaciones, pvp, imagen, categoria) values ('zapas3', 'unas zapatillas de superputamadresocio', 35,"PRUEBA SIN IMAGEN" , 'Adidas');
INSERT INTO Articulo (nombre, especificaciones, pvp, imagen, categoria) values ('coche3', 'broom broom', 400000,"PRUEBA SIN IMAGEN" , 'Ferrari');

INSERT INTO Almacen_Articulo values(1,1,20);
INSERT INTO Almacen_Articulo values(1,2,30);
INSERT INTO Almacen_Articulo values(1,3,10);

INSERT INTO Pedido (fecha_pedido) values('2023-01-15');
INSERT INTO Pedido (venta,fecha_pedido) values(false,'2023-01-17');

INSERT INTO LinPed (cantidad,codArt,pedido) values (10,1,1);
INSERT INTO LinPed (cantidad,codArt,pedido) values (15,3,1);
INSERT INTO LinPed (cantidad,codArt,pedido) values (2,5,1);


COMMIT;