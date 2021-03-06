﻿using BerrasBio.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BerrasBio.Data
{
    public interface ISqlTheaterData
    {
        Task<Movie> OnGetMovie(int? id);
        Task<List<Movie>> OnGetMovies(bool includeOld);
        Task<User> OnGetUser(int? id);
        Task<List<User>> OnGetUsers();
        Task<List<Seat>> FindSeats(int id);
        Order CreateOrder(List<int> seatIds, int viewingId, string userName);
        Task<List<Viewing>> GetViewingsById(int movieId, string order);
        Task<Order> GetOrder(int Id);
        Task<int> Update(Order order);
        int Update();
        bool OrderExists(int id);
        void LoadOrder(Order order);
        Movie AddMovie(Movie movie);
        EntityEntry<User> AddUser(User user);
        List<Salon> GetSalons();
        bool CreateViewing(Viewing viewing);
        Task<Movie> FindMovie(int? id);
        Task UpdateMovie(Movie movie);
        Task DeleteMovieAt(int id);
        bool DoesMovieExist(int id);
        bool MovieHasViewings(int movieId);
        List<int> GetSeatNumbers(int[] seatIds);
        Viewing GetViewingById(int id);
        Task<bool> OnDeleteMovie(int id);
    }
}
