namespace PokedexApi.Domain.Interfaces
{
    public interface IMapper<TIn,TOut> where TIn: class, new() where TOut : class, new()
    {
        TOut Map(TIn source);
    }
}
