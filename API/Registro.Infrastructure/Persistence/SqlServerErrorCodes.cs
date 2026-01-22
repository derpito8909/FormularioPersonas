using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Registro.Infrastructure.Persistence;

public static class SqlServerErrorCodes
{
    public static bool IsUniqueViolation(DbUpdateException ex)
        => ex.InnerException is SqlException sqlEx && (sqlEx.Number == 2601 || sqlEx.Number == 2627);
}