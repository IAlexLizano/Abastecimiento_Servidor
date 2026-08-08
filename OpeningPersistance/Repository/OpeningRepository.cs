using OpeningApplication.Interfaces;
using OpeningPersistance.Context;
using Shared.Global;
using Domains.Entities;
using Microsoft.EntityFrameworkCore;
using OpeningApplication.DTOs.Opening;
using Shared;
using Shared.Application.Exceptions;

namespace OpeningPersistance.Repository
{
    public class OpeningRepository : IOpeningRepository
    {
        private readonly ApplicationContext _dbContext;
        private readonly InformationSession _global;

        public OpeningRepository(ApplicationContext dbContext, InformationSession global)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _global = global ?? throw new ArgumentNullException(nameof(global));
        }

        /// <summary>
        /// Obtiene información básica (id y código) de lotes, contenedores, pallets y cajas 
        /// que están en estado "EN_PROCESO"
        /// </summary>
        public async Task<IEnumerable<BasicInformationResponseDto>> GetBasicInformationInProcessAsync()
        {
            try
            {
                var response = await (
                    from l in _dbContext.Lot
                    join c in _dbContext.Container on l.IdLot equals c.IdLot
                    join p in _dbContext.Pallet on c.IdContainer equals p.IdContainer
                    join b in _dbContext.Box on p.IdPallet equals b.IdPallet
                    where p.Status == LoteStatus.InProcess && 
                          b.Status == LoteStatus.InProcess && 
                          b.BoxProductDetail.Any(d => d.Cardboard.Any())

                    // 1. Agrupamos todo el flujo por el Lote (Raíz de la cascada)
                    group new { c, p, b } by new { l.IdLot, l.LotCode } into loteGrupo

                    select new BasicInformationResponseDto
                    {
                        Lots = new List<BasicLotInfoDto>
                        {
                            new BasicLotInfoDto
                            {
                                IdLot = loteGrupo.Key.IdLot,
                                LotCode = loteGrupo.Key.LotCode,

                                // 2. Cascada: Los contenedores van DENTRO de su respectivo Lote
                                Containers = (from itemLote in loteGrupo
                                              // Agrupamos por Contenedor único dentro de este lote
                                              group itemLote by new { itemLote.c.IdContainer, itemLote.c.ContainerNumber } into contenedorGrupo
                                              select new BasicContainerInfoDto
                                              {
                                                  IdContainer = contenedorGrupo.Key.IdContainer,
                                                  ContainerCode = contenedorGrupo.Key.ContainerNumber,

                                                  // 3. Cascada: Los pallets van DENTRO de su respectivo Contenedor
                                                  Pallets = (from itemContenedor in contenedorGrupo
                                                             // Agrupamos por Pallet único dentro de este contenedor
                                                             group itemContenedor by new { itemContenedor.p.IdPallet, itemContenedor.p.PalletNumber } into palletGrupo
                                                             select new BasicPalletInfoDto
                                                             {
                                                                 IdPallet = palletGrupo.Key.IdPallet,
                                                                 PalletCode = palletGrupo.Key.PalletNumber,

                                                                 // 4. Cascada: Las cajas van DENTRO de su respectivo Pallet
                                                                 Boxes = (from itemPallet in palletGrupo
                                                                          select new BasicBoxInfoDto
                                                                          {
                                                                              IdBox = itemPallet.b.IdBox,
                                                                              BoxCode = itemPallet.b.BoxNumber
                                                                          }).ToList()
                                                             }).ToList()
                                              }).ToList()
                            }
                        }
                    }).ToListAsync();

                return response;
            }
            catch (Exception)
            {
                throw new ApiException("Error al obtener la información");
            }
        }
    }
}
