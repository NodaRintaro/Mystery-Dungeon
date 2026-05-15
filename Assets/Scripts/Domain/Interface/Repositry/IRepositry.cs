using Cysharp.Threading.Tasks;
using Layer.Domain;

namespace Layer.Domain
{
    public interface IRepositry
    {
        public void GetData<T>(int id) where T : class, IData;
    }
}
