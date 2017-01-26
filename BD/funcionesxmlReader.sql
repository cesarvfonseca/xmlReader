SELECT * FROM CSRAPP.dbo.APDoc
SELECT * FROM CSRDEV.dbo.attachment
EXEC vData'201602'
--insertRecords 'CB3FCC75-62A4-4B6F-B671-461BBEF819FB','1539707','B','2016-05-12T12:20:36','ETN TURISTAR LUJO, S.A. DE C.V.','TLU080610C81','3212','C:\CFDi\fact55_cte17_cdfi.xml'
--TRUNCATE TABLE xmldata;

/*CREAR BD XMLDATA*/
USE [CSRAPP]
GO
/****** Object:  Table [dbo].[xmldata]    Script Date: 01/24/2017 08:30:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[xmldata](
	[FOLIO] [nvarchar](50) NULL,
	[NoteID] [nvarchar](50) NULL,
	[Location] [nvarchar](250) NULL,
	[SERIE] [nvarchar](50) NULL,
	[FECHA_TIMBRADO] [nvarchar](50) NULL,
	[EMISOR] [nvarchar](50) NULL,
	[RFC_E] [nvarchar](50) NULL,
	[UUID] [nvarchar](50) NULL,
	[ID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]
GO
/*CREAR BD XMLDATA*/
/*INSERTAR DATOS*/
CREATE PROCEDURE dbo.insertRecords(
	@UUID varchar(80),
	@folio varchar(15),
	@serie varchar(15),
	@fecha varchar(50),
	@cliente varchar(50),
	@rfc varchar(15),
	@noteid varchar(20),
	@location varchar(250)
	)
	AS
	IF NOT EXISTS (SELECT * FROM xmldata WHERE UUID=@UUID)
		BEGIN
			INSERT INTO xmldata (UUID,FOLIO,SERIE,FECHA_TIMBRADO,EMISOR,RFC_E,NoteID,Location)
			VALUES (@UUID,@folio,@serie,@fecha,@cliente,@rfc,@noteid,@location)
		END
/*INSERTAR DATOS*/
/*LISTAR DATOS*/
USE [CSRAPP]
GO
/****** Object:  StoredProcedure [dbo].[vData]    Script Date: 01/23/2017 14:00:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER procedure [dbo].[vData]
@periodo nchar(15)
as
SELECT ad.BatNbr,ad.PerPost,
att.NoteID,att.NameOfFile,att.Location
FROM CSRAPP.dbo.apdoc ad
INNER JOIN CSRDEV.dbo.attachment att
ON ad.NoteID=att.NoteID
WHERE ad.PerPost=@periodo AND att.Location LIKE '%.xml' ORDER BY att.NameOfFile
/*LISTAR DATOS*/
