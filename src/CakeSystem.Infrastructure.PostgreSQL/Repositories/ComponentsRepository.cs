using CakeSystem.Application.Services.Components;
using CakeSystem.Domain;
using Microsoft.EntityFrameworkCore;

namespace CakeSystem.Infrastructure.PostgreSQL.Repositories
{
    internal class ComponentsRepository : IComponentsRepository
    {
        private readonly AppDbContext _dbContext;

        public async Task AddAsync(IEnumerable<Component> components, CancellationToken token)
        {
            foreach (var component in components)
            {
                var storedComponent = await _dbContext.Components.FirstOrDefaultAsync(c => c.Id == component.Id, token);

                if (storedComponent != null)
                {
                    if (component.GetHashCode() != storedComponent.GetHashCode())
                    {
                        storedComponent.Count += component.Count;

                        storedComponent.Characteristics = component.Characteristics;
                        storedComponent.Gost = component.Gost;
                        storedComponent.PrePacking = component.PrePacking;
                        storedComponent.Title = component.Title;
                        storedComponent.Type = component.Type;
                        storedComponent.UnitMeasurement = component.UnitMeasurement;
                    }
                }
                else
                    await _dbContext.Components.AddAsync(component, token);
            }

            await _dbContext.SaveChangesAsync(token);
        }

        public ComponentsRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
