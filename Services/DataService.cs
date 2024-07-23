using System.Collections.Generic;
using System.Linq;
using CentralisationV0.Models.Entities; // Assurez-vous que le namespace est correct
using CentralisationV0.Models; // Assurez-vous que le namespace est correct
using System.Data.Entity;

namespace CentralisationV0.Services
{
    public interface IDataService
    {
        List<Data> GetData();
    }

    public class DataService : IDataService
    {
        public List<Data> GetData()
        {
            using (var context = new CentralisationContext())
            {
                // Utilisation de Include pour charger les thèmes associés
                return context.Datas.Include(d => d.Theme).ToList();
            }
        }
    }
}