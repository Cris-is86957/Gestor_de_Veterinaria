
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 05/24/2026 20:44:53
-- Generated from EDMX file: C:\Users\crist\Gestor_de_Veterinaria\Diego Herrera - Prueba 2\Veterinaria.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [VeterinariaDB];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_Agenda_Mascota]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AGENDA] DROP CONSTRAINT [FK_Agenda_Mascota];
GO
IF OBJECT_ID(N'[dbo].[FK_Agenda_Usuario]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AGENDA] DROP CONSTRAINT [FK_Agenda_Usuario];
GO
IF OBJECT_ID(N'[dbo].[FK_Mascota_Dueño]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[MASCOTA] DROP CONSTRAINT [FK_Mascota_Dueño];
GO
IF OBJECT_ID(N'[dbo].[FK_Ventas_Producto]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[VENTAS] DROP CONSTRAINT [FK_Ventas_Producto];
GO
IF OBJECT_ID(N'[dbo].[FK_Ventas_Usuario]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[VENTAS] DROP CONSTRAINT [FK_Ventas_Usuario];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[AGENDA]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AGENDA];
GO
IF OBJECT_ID(N'[dbo].[DUEÑOS]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DUEÑOS];
GO
IF OBJECT_ID(N'[dbo].[MASCOTA]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MASCOTA];
GO
IF OBJECT_ID(N'[dbo].[PRODUCTOS]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PRODUCTOS];
GO
IF OBJECT_ID(N'[dbo].[USUARIO]', 'U') IS NOT NULL
    DROP TABLE [dbo].[USUARIO];
GO
IF OBJECT_ID(N'[dbo].[VENTAS]', 'U') IS NOT NULL
    DROP TABLE [dbo].[VENTAS];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'AGENDA'
CREATE TABLE [dbo].[AGENDA] (
    [ID_Cita] int IDENTITY(1,1) NOT NULL,
    [Rut_usuario] varchar(12)  NOT NULL,
    [ID_mascota] int  NOT NULL,
    [Fecha] datetime  NOT NULL,
    [Hora] time  NOT NULL
);
GO

-- Creating table 'DUEÑOS'
CREATE TABLE [dbo].[DUEÑOS] (
    [Rut_dueño] varchar(12)  NOT NULL,
    [Estado_dueño] bit  NOT NULL,
    [nombre] varchar(50)  NOT NULL,
    [apell_pat] varchar(50)  NOT NULL,
    [apell_mat] varchar(50)  NOT NULL
);
GO

-- Creating table 'MASCOTA'
CREATE TABLE [dbo].[MASCOTA] (
    [ID_mascota] int IDENTITY(1,1) NOT NULL,
    [Rut_dueño] varchar(12)  NOT NULL,
    [Estado_mascota] bit  NOT NULL,
    [nombre] varchar(50)  NOT NULL,
    [tipo] varchar(50)  NOT NULL,
    [raza] varchar(50)  NOT NULL,
    [edad] int  NOT NULL
);
GO

-- Creating table 'PRODUCTOS'
CREATE TABLE [dbo].[PRODUCTOS] (
    [ID_Producto] int IDENTITY(1,1) NOT NULL,
    [Estado_producto] bit  NOT NULL,
    [Nombre] varchar(100)  NOT NULL,
    [Stock] int  NOT NULL,
    [Precio_unidad] decimal(10,2)  NOT NULL
);
GO

-- Creating table 'USUARIO'
CREATE TABLE [dbo].[USUARIO] (
    [Rut_usuario] varchar(12)  NOT NULL,
    [Estado_usuario] bit  NOT NULL,
    [nombre] varchar(50)  NOT NULL,
    [apellido] varchar(50)  NOT NULL,
    [password] varchar(255)  NOT NULL
);
GO

-- Creating table 'VENTAS'
CREATE TABLE [dbo].[VENTAS] (
    [ID_venta] int IDENTITY(1,1) NOT NULL,
    [Rut_usuario] varchar(12)  NOT NULL,
    [ID_producto] int  NOT NULL,
    [Fecha_hora] datetime  NOT NULL,
    [Cantidad] int  NOT NULL,
    [Precio_unidad] decimal(10,2)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [ID_Cita] in table 'AGENDA'
ALTER TABLE [dbo].[AGENDA]
ADD CONSTRAINT [PK_AGENDA]
    PRIMARY KEY CLUSTERED ([ID_Cita] ASC);
GO

-- Creating primary key on [Rut_dueño] in table 'DUEÑOS'
ALTER TABLE [dbo].[DUEÑOS]
ADD CONSTRAINT [PK_DUEÑOS]
    PRIMARY KEY CLUSTERED ([Rut_dueño] ASC);
GO

-- Creating primary key on [ID_mascota] in table 'MASCOTA'
ALTER TABLE [dbo].[MASCOTA]
ADD CONSTRAINT [PK_MASCOTA]
    PRIMARY KEY CLUSTERED ([ID_mascota] ASC);
GO

-- Creating primary key on [ID_Producto] in table 'PRODUCTOS'
ALTER TABLE [dbo].[PRODUCTOS]
ADD CONSTRAINT [PK_PRODUCTOS]
    PRIMARY KEY CLUSTERED ([ID_Producto] ASC);
GO

-- Creating primary key on [Rut_usuario] in table 'USUARIO'
ALTER TABLE [dbo].[USUARIO]
ADD CONSTRAINT [PK_USUARIO]
    PRIMARY KEY CLUSTERED ([Rut_usuario] ASC);
GO

-- Creating primary key on [ID_venta] in table 'VENTAS'
ALTER TABLE [dbo].[VENTAS]
ADD CONSTRAINT [PK_VENTAS]
    PRIMARY KEY CLUSTERED ([ID_venta] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [ID_mascota] in table 'AGENDA'
ALTER TABLE [dbo].[AGENDA]
ADD CONSTRAINT [FK_Agenda_Mascota]
    FOREIGN KEY ([ID_mascota])
    REFERENCES [dbo].[MASCOTA]
        ([ID_mascota])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Agenda_Mascota'
CREATE INDEX [IX_FK_Agenda_Mascota]
ON [dbo].[AGENDA]
    ([ID_mascota]);
GO

-- Creating foreign key on [Rut_usuario] in table 'AGENDA'
ALTER TABLE [dbo].[AGENDA]
ADD CONSTRAINT [FK_Agenda_Usuario]
    FOREIGN KEY ([Rut_usuario])
    REFERENCES [dbo].[USUARIO]
        ([Rut_usuario])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Agenda_Usuario'
CREATE INDEX [IX_FK_Agenda_Usuario]
ON [dbo].[AGENDA]
    ([Rut_usuario]);
GO

-- Creating foreign key on [Rut_dueño] in table 'MASCOTA'
ALTER TABLE [dbo].[MASCOTA]
ADD CONSTRAINT [FK_Mascota_Dueño]
    FOREIGN KEY ([Rut_dueño])
    REFERENCES [dbo].[DUEÑOS]
        ([Rut_dueño])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Mascota_Dueño'
CREATE INDEX [IX_FK_Mascota_Dueño]
ON [dbo].[MASCOTA]
    ([Rut_dueño]);
GO

-- Creating foreign key on [ID_producto] in table 'VENTAS'
ALTER TABLE [dbo].[VENTAS]
ADD CONSTRAINT [FK_Ventas_Producto]
    FOREIGN KEY ([ID_producto])
    REFERENCES [dbo].[PRODUCTOS]
        ([ID_Producto])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Ventas_Producto'
CREATE INDEX [IX_FK_Ventas_Producto]
ON [dbo].[VENTAS]
    ([ID_producto]);
GO

-- Creating foreign key on [Rut_usuario] in table 'VENTAS'
ALTER TABLE [dbo].[VENTAS]
ADD CONSTRAINT [FK_Ventas_Usuario]
    FOREIGN KEY ([Rut_usuario])
    REFERENCES [dbo].[USUARIO]
        ([Rut_usuario])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Ventas_Usuario'
CREATE INDEX [IX_FK_Ventas_Usuario]
ON [dbo].[VENTAS]
    ([Rut_usuario]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------