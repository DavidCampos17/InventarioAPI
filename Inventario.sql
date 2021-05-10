use master;
go 
if not exists (select * from sysdatabases where name = 'inventario')
begin 
	create database inventario;
end
GO

use inventario;
EXEC sp_changedbowner 'sa'
GO



create table Usuarios(
idUsuario int identity(1,1),
nombreUsuario nvarchar(20),
apellidoPadre nvarchar(20),
apellidoMadre nvarchar(20),
correo nvarchar(50),
usuario nvarchar(20),
contrasena nvarchar(100),
primary key clustered (idUsuario),
)
go

create table Categorias
(
	idCategoria int identity (1,1),
	categoria nvarchar(50),
	fechaCreacion datetime default getdate(),
	idUsuario int,
	primary key clustered (idCategoria),
	constraint FK_Categoria_Usuarios  foreign key (idUsuario) references Usuarios(idUsuario)
)
go

create table Marcas(
idMarca int identity (1,1),
marca nvarchar(50),
fechaCreacion datetime,
fechaModificacion datetime,
idUsuario int,
primary key clustered (idMarca),
constraint FK_Marca_Usuario  foreign key (idUsuario) references Usuarios(idUsuario)
)
go

create table Colores (
idColor int identity(1,1),
color nvarchar(50),
primary key clustered (idColor)
)
go

create table Monedas(
idMoneda int identity (1,1),
moneda nvarchar(50),
primary key clustered (idMoneda)
)
go

create table Productos
(
idProducto int identity,
producto nvarchar(50),
SKU nvarchar(100),
numeroProducto nvarchar(100),
fechaCompra datetime,
fechaVenta datetime,
cantidad int,
precio decimal (10,2),
descripcion nvarchar(200),
fechaCreacion datetime,
fechaModificacion datetime,
idCategoria int,
idMarca int,
idColor int,
idMoneda int,
idUsuario int
primary key clustered (idProducto),
constraint FK_Producto_Categoria foreign key (idCategoria) references Categorias (idCategoria),
constraint FK_Producto_Marca foreign key (idMarca) references Marcas(idMarca),
constraint FK_Producto_Colores foreign key (idColor) references Colores (idColor),
constraint FK_Producto_Moneda  foreign key (idMoneda) references Monedas(idMoneda),
constraint FK_Producto_Usuarios  foreign key (idUsuario) references Usuarios(idUsuario)
)
go

create table Clientes(
idCliente int identity,
nomCliente varchar(25),
apPCliente varchar(25),
apMCliente varchar(25),
telCliente varchar(10),
primary key clustered(idCliente)
)
go

create table Carrito
(
	idProducto int,
	idCliente int,
	idUsuario int,
	cantidad int,
	fechaReservacion datetime default getdate(),
	constraint fk_Carrito_Producto foreign key (idProducto) references Productos(idProducto),
	constraint fk_Carrito_Clientes foreign key (idCliente) references Clientes(idCliente),
    constraint fk_Carrito_Usuarios foreign key (idUsuario) references Usuarios(idUsuario)

)
go

create table Ordenes (
idOrden int primary key,
idCliente int,
idProducto int,
precioTotal decimal (10,2),
constraint fk_Orden_Producto foreign key (idProducto) references Productos(idProducto),
constraint fk_Orden_Cliente foreign key (idCliente) references Clientes(idCliente)
)
go

create table Orden_Detalle 
(
idOrden int,
idProducto int,
cantidad int,
precio decimal (10,2),
constraint fk_OrdenDetalle_Producto foreign key (idProducto) references Productos(idProducto),
constraint fk_OrdenDetalle_Orden foreign key (idOrden) references Ordenes(idOrden)
)

GO


/*CRUD PRODUCTOS*/
create proc spListarProductos
AS
	select producto, SKU, numeroProducto, fechaCompra, 
	fechaVenta, cantidad, precio from Productos
GO


create proc spNuevoProducto(
@nombre nvarchar(50),
@SKU nvarchar(100),
@numeroProducto nvarchar(100),
@fechaCompra datetime,
@fechaVenta datetime,
@cantidad int,
@precio decimal,
@descripcion nvarchar(200)
)
as
	insert into Productos 
	(producto, SKU, numeroProducto, fechaCompra, fechaVenta, cantidad, precio, descripcion) 
	values(@nombre, @SKU,@numeroProducto, @fechaCompra, @fechaVenta, @cantidad, @precio, @descripcion);
go

/*CRUD CATEGORIAS*/

create proc spListarCategorias
AS
	select idCategoria, Categoria from Categorias
GO

create proc spNuevaCategoria(@categoria nvarchar(50))
as
	insert into Categorias (categoria) values (@categoria)
go


create proc spGetCategoria(@idCategoria INT)
as
	SELECT [idCategoria]
      ,[categoria]
  FROM [dbo].[Categorias] where idCategoria= @idCategoria;
go

create proc spActualizarCategoria(@idCategoria int, @categoria nvarchar(50))
as
	update Categorias set categoria=@categoria where idCategoria = @idCategoria;
go

create proc spDelCategoria(@idCategoria int)
as
	delete Categorias where idCategoria=@idCategoria;
go

/*CRUD MARCA*/
create proc spNuevaMarca(@marca nvarchar(50))
as
	insert into Marcas(marca) values(@marca)
go

create proc spGetMarcas
as
	select idMarca, marca from Marcas
go

create proc spGetMarca(@idMarca int)
as
	select idMarca, marca from Marcas where idMarca = @idMarca;
go

create proc spActualizarMarca(@idMarca int, @marca nvarchar(50) )
as
	update Marcas set marca = @marca where idMarca= @idMarca;
go

create proc spEliminarMarca (@idMarca int)
as
	delete Marcas where idMarca=@idMarca
go

