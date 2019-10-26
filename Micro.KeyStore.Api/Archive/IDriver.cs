using System.Threading.Tasks;

namespace Micro.KeyStore.Api.Archive
{
    public interface IDriver<in T>
    {
        Task Save(T item);
    }
}
