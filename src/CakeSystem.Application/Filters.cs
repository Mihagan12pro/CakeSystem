using System.Linq.Expressions;

namespace CakeSystem.Application
{
    public class Filters<T> : List<Expression<Func<T, bool>>>
    {
    }
}
