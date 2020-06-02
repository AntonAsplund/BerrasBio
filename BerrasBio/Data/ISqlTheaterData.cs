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
        Task<User> OnGetUser(int? id);
        Task<List<User>> OnGetUsers();
        Task<List<Seat>> FindSeats(int id);
        Order CreateOrder(List<int> seatIds, int viewingId);
        Task<List<Viewing>> GetViewingsById(int movieId, string order);
        Task<Order> GetOrder(int Id);
        Task<int> Update(Order order);
        int Update();
        bool OrderExists(int id);
    }
}
