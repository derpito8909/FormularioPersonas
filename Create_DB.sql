SET NOCOUNT ON;
SET XACT_ABORT ON;

DECLARE @DbName sysname = N'RegistroPersonas';

BEGIN TRY

IF DB_ID(@DbName) IS NULL
    BEGIN
        DECLARE @sql nvarchar(max) = N'CREATE DATABASE ' + QUOTENAME(@DbName) + N';';
        EXEC sys.sp_executesql @sql;
        PRINT 'Base de datos creada: ' + @DbName;
    END
    ELSE
    BEGIN
        PRINT 'La base de datos ya existe: ' + @DbName;
    END

    BEGIN TRAN;    
 
    IF OBJECT_ID(N'RegistroPersonas.dbo.Personas', N'U') IS NULL
    BEGIN
        EXEC(N'
        CREATE TABLE RegistroPersonas.dbo.Personas
        (
            PersonaId               INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Personas PRIMARY KEY,
            Nombres                 NVARCHAR(100) NOT NULL,
            Apellidos               NVARCHAR(100) NOT NULL,
            NumeroIdentificacion    NVARCHAR(30)  NOT NULL,
            Email                   NVARCHAR(200) NOT NULL,
            TipoIdentificacion      NVARCHAR(10)  NOT NULL,
            FechaCreacion           DATETIME2(0)  NOT NULL CONSTRAINT DF_Personas_FechaCreacion DEFAULT (SYSDATETIME()),            
            IdentificacionCompleta  AS (UPPER(LTRIM(RTRIM(TipoIdentificacion))) + N''-'' + LTRIM(RTRIM(NumeroIdentificacion))) PERSISTED,
            NombreCompleto          AS (LTRIM(RTRIM(Nombres)) + N'' '' + LTRIM(RTRIM(Apellidos))) PERSISTED
        );
        
        ALTER TABLE RegistroPersonas.dbo.Personas
            ADD CONSTRAINT CK_Personas_TipoIdentificacion
            CHECK (TipoIdentificacion IN (N''CC'', N''TI'', N''CE'', N''NIT'', N''PP''));

        ALTER TABLE RegistroPersonas.dbo.Personas
            ADD CONSTRAINT CK_Personas_NumeroIdentificacion_NoVacio
            CHECK (LEN(LTRIM(RTRIM(NumeroIdentificacion))) > 0);
        
        ALTER TABLE RegistroPersonas.dbo.Personas
            ADD CONSTRAINT CK_Personas_Email_FormatoBasico
            CHECK (Email LIKE N''%_@_%._%'');
        
        CREATE UNIQUE INDEX UX_Personas_Tipo_Numero
            ON RegistroPersonas.dbo.Personas(TipoIdentificacion, NumeroIdentificacion);
        
        CREATE INDEX IX_Personas_FechaCreacion ON RegistroPersonas.dbo.Personas(FechaCreacion);
        
        ');
    END
    ELSE
    BEGIN
        PRINT 'La tabla RegistroPersonas.dbo.Personas ya existe';
    END
    
    IF OBJECT_ID(N'RegistroPersonas.dbo.Usuarios', N'U') IS NULL
    BEGIN
        EXEC(N'
        CREATE TABLE RegistroPersonas.dbo.Usuarios
        (
            UsuarioId       INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Usuarios PRIMARY KEY,
            Usuario         NVARCHAR(100) NOT NULL,
            PassHash        VARBINARY(64) NOT NULL,
            PassSalt        VARBINARY(16) NOT NULL,
            FechaCreacion   DATETIME2(0)  NOT NULL CONSTRAINT DF_Usuarios_FechaCreacion DEFAULT (SYSUTCDATETIME())
        );
        
        CREATE UNIQUE INDEX UX_Usuarios_Usuario ON RegistroPersonas.dbo.Usuarios(Usuario);
        
        ALTER TABLE RegistroPersonas.dbo.Usuarios
            ADD CONSTRAINT CK_Usuarios_Usuario_NoVacio
            CHECK (LEN(LTRIM(RTRIM(Usuario))) >= 4);
        
        ');
    END
    ELSE
    BEGIN
        PRINT 'La tabla RegistroPersonas.dbo.Usuarios ya existe';
    END
    
    DECLARE @sqlProcedure nvarchar(max) = N'
        CREATE OR ALTER PROCEDURE dbo.sp_Personas_GetAll
            @TipoIdentificacion NVARCHAR(10) = NULL,
            @NumeroIdentificacion NVARCHAR(30) = NULL,
            @Email NVARCHAR(200) = NULL
        AS
        BEGIN
            SET NOCOUNT ON;

            SELECT
                p.PersonaId,
                p.Nombres,
                p.Apellidos,
                p.TipoIdentificacion,
                p.NumeroIdentificacion,
                p.IdentificacionCompleta,
                p.Email,
                p.NombreCompleto,
                p.FechaCreacion
            FROM dbo.Personas p
            WHERE
                (@TipoIdentificacion IS NULL OR p.TipoIdentificacion = @TipoIdentificacion)
                AND (@NumeroIdentificacion IS NULL OR p.NumeroIdentificacion = @NumeroIdentificacion)
                AND (@Email IS NULL OR p.Email = @Email)
            ORDER BY p.FechaCreacion DESC;
        END;
        ';

        EXEC RegistroPersonas.sys.sp_executesql @sqlProcedure;

    COMMIT TRAN;
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0
        ROLLBACK TRAN;

    DECLARE
        @ErrMsg NVARCHAR(4000) = ERROR_MESSAGE(),
        @ErrNum INT = ERROR_NUMBER(),
        @ErrState INT = ERROR_STATE(),
        @ErrLine INT = ERROR_LINE(),
        @ErrProc NVARCHAR(200) = ISNULL(ERROR_PROCEDURE(), N'(N/A)');

    PRINT '❌ Error creando estructura:';
    PRINT '   Número: ' + CAST(@ErrNum AS NVARCHAR(20));
    PRINT '   Estado: ' + CAST(@ErrState AS NVARCHAR(20));
    PRINT '   Línea : ' + CAST(@ErrLine AS NVARCHAR(20));
    PRINT '   Proc  : ' + @ErrProc;
    PRINT '   Msg   : ' + @ErrMsg;

    THROW;
END CATCH;