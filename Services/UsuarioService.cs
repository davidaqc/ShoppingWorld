using Usuarios.DataAccess;

namespace Usuarios.Services
{
    public class UsuarioService
    {
        private readonly UsuarioRepository _usuarioRepository;

        public UsuarioService(UsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public async Task<bool> LoginAsync(string correo, string userPassword)
        {
            return await _usuarioRepository.LoginAsync(correo, userPassword);
        }

        public async Task<bool> RegistrarAsync(Usuario nuevoUsuario)
        {
            return await _usuarioRepository.RegistrarAsync(nuevoUsuario);
        }

        public async Task<List<Usuario>> GetUsuariosAsync()
        {
            return await _usuarioRepository.GetUsuariosAsync();
        }

        public async Task<bool> DeleteUsuarioAsync(string correo)
        {
            return await _usuarioRepository.DeleteUsuarioAsync(correo);
        }

        public async Task<List<Usuario>> GetUsuariosByNombreAsync(string nombre)
        {
            return await _usuarioRepository.GetUsuariosByNombreAsync(nombre);
        }

        public async Task<int> GetUsuariosCountAsync()
        {
            return await _usuarioRepository.GetUsuariosCountAsync();
        }
    }
}
