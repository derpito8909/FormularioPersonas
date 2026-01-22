namespace Registro.Domain.Entities;

public class Usuario
{
    public int UsuarioId { get; private set; }

    public string UsuarioNombre { get; private set; } = default!;
    public byte[] PassHash { get; private set; } = default!;
    public byte[] PassSalt { get; private set; } = default!;
    public DateTime FechaCreacion { get; private set; }

    private Usuario() { }

    public Usuario(string usuarioNombre, byte[] passHash, byte[] passSalt)
    {
        UsuarioNombre = usuarioNombre.Trim();
        PassHash = passHash;
        PassSalt = passSalt;
    }
}