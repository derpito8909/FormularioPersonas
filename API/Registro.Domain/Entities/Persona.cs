namespace Registro.Domain.Entities;

public class Persona
{
    public int PersonaId { get; private set; }

    public string Nombres { get; private set; } = default!;
    public string Apellidos { get; private set; } = default!;
    public string NumeroIdentificacion { get; private set; } = default!;
    public string Email { get; private set; } = default!;
    public string TipoIdentificacion { get; private set; } = default!;
    public DateTime FechaCreacion { get; private set; }
    public string IdentificacionCompleta { get; private set; } = default!;
    public string NombreCompleto { get; private set; } = default!;

    private Persona() { }

    public Persona(string nombres, string apellidos, string tipoId, string numeroId, string email)
    {
        Nombres = nombres.Trim();
        Apellidos = apellidos.Trim();
        TipoIdentificacion = tipoId.Trim().ToUpperInvariant();
        NumeroIdentificacion = numeroId.Trim();
        Email = email.Trim().ToLowerInvariant();
    }
}