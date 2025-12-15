using AutoMapper;
using ReservasTucson.Domain.DTO;
using ReservasTucson.Domain.Entities;
using ReservasTucson.Repositories.Interfaces.UoW;
using ReservasTucson.Services.Interfaces;

namespace ReservasTucson.Services.Implementations
{
    public class MesaService : IMesaService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MesaService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<MesaDTO>> GetAll()
        {
            var mesas = await _unitOfWork.MesaRepository.GetAll();

            return _mapper.Map<List<MesaDTO>>(mesas);
        }

        public async Task<MesaDTO> GetById(int id)
        {
            var mesa = await _unitOfWork.MesaRepository.GetById(id);

            return _mapper.Map<MesaDTO>(mesa);
        }

        public async Task<MesaDTO> InsertMesa(MesaDTO entity)
        {
            var model = _mapper.Map<Mesa>(entity);

            var mesa = await _unitOfWork.MesaRepository.InsertMesa(model);

            var resultado = _mapper.Map<MesaDTO>(mesa);

            await _unitOfWork.SaveChanges();

            return resultado;
        }        

        public async Task<bool> EstaDisponibleAsync(
            int mesaId,
            DateTime fechaHora,
            int duracionMinutos,
            int? reservaIdExcluir = null)
        {
            var mesa = await _unitOfWork.MesaRepository.GetById(mesaId);

            if (mesa == null || !mesa.Activa)
                return false;

            var reservas = await _unitOfWork.ReservaRepository.GetAllAsync();

            var inicio = fechaHora;
            var fin = fechaHora.AddMinutes(duracionMinutos);

            var conflicto = reservas.Any(r =>
                r.Id != reservaIdExcluir &&
                r.ReservasMesas.Any(rm => rm.MesaId == mesaId) &&
                inicio < r.FechaHora.AddMinutes(r.TipoReserva.TiempoPermanenciaMinutos) &&
                r.FechaHora < fin);

            return !conflicto;
        }

        public async Task<List<MesaDTO>> GetMesasDisponiblesAsync(
            DateTime fechaHora,
            int duracionMinutos,
            int cantidadPersonas,
            bool soloVip)
        {
            var mesas = await _unitOfWork.MesaRepository.GetAll();

            var disponibles = new List<Mesa>();

            foreach (var mesa in mesas)
            {
                if (!mesa.Activa)
                    continue;

                if (soloVip && !mesa.EsVip)
                    continue;

                if (mesa.Capacidad < cantidadPersonas)
                    continue;

                var disponible = await EstaDisponibleAsync(
                    mesa.Id,
                    fechaHora,
                    duracionMinutos);

                if (disponible)
                    disponibles.Add(mesa);
            }

            return _mapper.Map<List<MesaDTO>>(disponibles);
        }     
    }
}
