using System.Threading.Tasks;
using Administradores.DataAccess;

namespace Administradores.Services
{
    public class AdministradorService
    {
        private readonly AdministradorRepository _administradorRepository;

        public AdministradorService(AdministradorRepository administradorRepository)
        {
            _administradorRepository = administradorRepository;
        }

        public async Task<bool> ValidateAdministradorAsync(string correo, string password)
        {
            return await _administradorRepository.ValidateAdministradorAsync(correo, password);
        }
    }
}
