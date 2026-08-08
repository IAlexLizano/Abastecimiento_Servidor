using ComponentsPersistance.Context;
using ComponentsApplication.Interfaces;
using ComponentsApplication.DTOs;
using Microsoft.EntityFrameworkCore;
using Shared.Application.Exceptions;
using Domains.Entities;

namespace ComponentsPersistance.Repository
{
    public class ComponentsRepository : IComponentsRepository
    {
        private readonly ApplicationContext _dbContext;

        public ComponentsRepository(ApplicationContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<string> CreateComponent(CreateComponentRequestDto request)
        {
            if (await ExistsPartCode(request.PartCode))
                throw new ApiException("El código de pieza ya existe.");

            var component = new Component
            {
                PartCode = request.PartCode,
                Description = request.Description,
                TechParameters = request.TechParameters,
                ImagePath = request.ImagePath
            };
            _dbContext.Component.Add(component);
            await _dbContext.SaveChangesAsync();
            return "Componente creado con éxito.";
        }

        public async Task<string> DeleteComponent(int id)
        {
            if (!await ExistsComponent(id))
                throw new ApiException("El componente no existe.");

            var component = await _dbContext.Component.FindAsync(id);
            _dbContext.Component.Remove(component);
            await _dbContext.SaveChangesAsync();
            return "Componente eliminado con éxito.";
        }



        public async Task<IEnumerable<ComponentResponseDto>> GetAllComponents()
        {
            try
            {
                var response = await (from cp in _dbContext.Component
                                     select new ComponentResponseDto
                                     {
                                         IdComponent = cp.IdComponent,
                                         PartCode = cp.PartCode,
                                         Description = cp.Description,
                                         TechParameters = cp.TechParameters,
                                         ImagePath = cp.ImagePath
                                     }).OrderBy(c => c.PartCode)
                                     .ToListAsync();

                return response ?? throw new ApiException("Error al cargar componentes.");
            }
            catch (Exception)
            {
                throw new ApiException("Error al cargar componentes.");
            }
        }

        public async Task<ComponentResponseDto> GetComponentById(int id)
        {
            try
            {
                var response = await (from cp in _dbContext.Component
                                      where cp.IdComponent == id
                                      select new ComponentResponseDto
                                      {
                                          IdComponent = cp.IdComponent,
                                          PartCode = cp.PartCode,
                                          Description = cp.Description,
                                          TechParameters = cp.TechParameters,
                                          ImagePath = cp.ImagePath
                                      }).FirstOrDefaultAsync();

                return response ?? throw new ApiException("El componente no existe.");
            }
            catch (Exception)
            {
                throw new ApiException($"Error al cargar componente {id}");
            }
        }

        public async Task<string> UpdateComponent(UpdateComponentRequestDto request)
        {
            try
            {
                var component = await _dbContext.Component.FindAsync(request.ComponentId) ?? throw new ApiException("El componente no existe.");
                component.Description = request.Description;
                component.TechParameters = request.TechParameters;
                component.ImagePath = request.ImagePath;
                _dbContext.Set<Component>().Update(component);
                await _dbContext.SaveChangesAsync();
                return "Componente actualizado con éxito.";
            }
            catch (Exception)
            {
                throw new ApiException($"Error al actualizar el componente {request.ComponentId}");
            }
        }

        public async Task<IEnumerable<RecipeResponseDto>> GetRecipesByModel(int idModel)
        {
            try
            {
                var recipes = await _dbContext.Recipe
                    .Include(r => r.IdComponentNavigation)
                    .Include(r => r.IdStationNavigation)
                    .Where(r => r.IdModel == idModel)
                    .Select(r => new RecipeResponseDto
                    {
                        IdRecipe = r.IdRecipe,
                        Component = new ComponentSimpleDto
                        {
                            Code = r.IdComponentNavigation.PartCode,
                            Description = r.IdComponentNavigation.Description,
                            TechParameters = r.IdComponentNavigation.TechParameters,
                            ImagePath = r.IdComponentNavigation.ImagePath
                        },
                        CantCar = r.CantCar,
                        StationCode = r.IdStationNavigation.CodeStation,
                        ProcessSheet = r.ProcessSheet,
                        Ownership = r.Ownership,
                        Total = r.Total
                    })
                    .ToListAsync();

                return recipes;
            }
            catch (Exception)
            {
                throw new ApiException($"Error al cargar las recetas del modelo {idModel}");
            }
        }

        public async Task<string> LoadRecipe(string modelCode, List<LoadRecipeRequestDto> requests)
        {
            try
            {
                // Obtener el ID del modelo global
                var model = await _dbContext.Models.FirstOrDefaultAsync(m => m.ModelCode == modelCode) ?? throw new ApiException($"El modelo con código {modelCode} no existe.");

                // Extraer todos los códigos únicos
                var partCodes = requests.Select(r => r.Component.Code).Distinct().ToList();
                var stationCodes = requests.Select(r => r.StationCode).Distinct().ToList();

                // Consultar base de datos una sola vez (Bulk query)
                var existingComponents = await _dbContext.Component
                    .Where(c => partCodes.Contains(c.PartCode))
                    .ToDictionaryAsync(c => c.PartCode);

                var existingStations = await _dbContext.WorkStation
                    .Where(w => stationCodes.Contains(w.CodeStation))
                    .ToDictionaryAsync(w => w.CodeStation);

                using var transaction = await _dbContext.Database.BeginTransactionAsync();
                // Procesar la lista
                foreach (var request in requests)
                {
                    // Componente
                    if (!existingComponents.TryGetValue(request.Component.Code, out var component))
                    {
                        component = new Component
                        {
                            PartCode = request.Component.Code,
                            Description = request.Component.Description,
                            TechParameters = request.Component.TechParameters,
                            ImagePath = request.Component.ImagePath
                        };
                        _dbContext.Component.Add(component);
                        existingComponents[request.Component.Code] = component; // Añadir al diccionario para evitar duplicados en la misma petición
                    }

                    // Estación
                    if (!existingStations.TryGetValue(request.StationCode, out var workStation))
                    {
                        workStation = new WorkStation
                        {
                            CodeStation = request.StationCode,
                            Name = request.StationCode,
                            IsActive = true
                        };
                        _dbContext.WorkStation.Add(workStation);
                        existingStations[request.StationCode] = workStation; // Añadir al diccionario para evitar duplicados
                    }

                    // Receta
                    var recipe = new Recipe
                    {
                        IdComponentNavigation = component, // Usar propiedades de navegación en lugar de IDs, ya que los nuevos aún tienen ID=0
                        CantCar = request.CantCar,
                        IdStationNavigation = workStation,
                        ProcessSheet = request.ProcessSheet,
                        Ownership = request.Ownership,
                        Total = request.Total ?? 0,
                        IdModel = model.IdModel
                    };

                    _dbContext.Recipe.Add(recipe);
                }

                // Guardar todos los cambios en un solo commit a la base de datos
                await _dbContext.SaveChangesAsync();
                await transaction.CommitAsync();

                return "Recetas cargadas con éxito.";
            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ApiException($"Error al cargar las recetas: {ex.Message}");
            }
        }

        private async Task<bool> ExistsComponent(int componentId)
        {
            return await _dbContext.Component.AnyAsync(c => c.IdComponent == componentId);
        }

        private async Task<bool> ExistsPartCode(string partCode)
        {
            return await _dbContext.Component.AnyAsync(c => c.PartCode == partCode);
        }
    }
}
