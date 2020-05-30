using BerrasBio.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BerrasBio.Data
{
    public interface ISqlTheaterData
    {
        Task<Movie> OnGetMovie(int? id);
        Task<List<Movie>> OnGetMovies();
        Task<List<Seat>> FindSeats(int id);
        int CreateOrder(List<int> ids, int viewingId);
        Task<List<Viewing>> GetViewingsById(int movieId);
        Task<Order> GetOrder(int Id);
        Task<int> Update(Order order);
        bool OrderExists(int id);
    }
}
