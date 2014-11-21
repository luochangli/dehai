using System.Data;
using TomorrowSoft.DAL;

namespace TomorrowSoft.BLL
{
    public class QsService
    {
        private readonly QsRepository _repository = new QsRepository();

        public bool Add(string sendtime, string qs)
        {
            DeleteQs();
            return _repository.Add(sendtime, qs);
        }

        public void DeleteQs()
        {
            _repository.DeleteQs();
        }

        public DataTable GetAll()
        {
            return _repository.GetAll();
        }
    }
}