using CakeSystem.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace CakeSystem.Application.Services.Components
{
    public interface IComponentsRepository
    {
        Task AddAsync(
            IEnumerable<Component> components,
            CancellationToken token);
    }
}
