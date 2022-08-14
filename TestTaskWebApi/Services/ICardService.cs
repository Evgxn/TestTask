using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TestTaskWebApi.Data;
using TestTaskWebApi.Models;
using TestTaskWebApi.ViewModel;

namespace TestTaskWebApi.Services
{
    public interface ICardService
    {
        Task<IEnumerable<GetCardViewModel>> GetCardsAsync();
        Task AddCardAsync(AddCardViewModel card);
        Task UpdateCardAsync(GetCardViewModel cardViewModel);
        Task DeleteCardAsync(IdViewModel vm);
        Task<Card> GetCardById(IdViewModel vm);
    }

    public class CardService : ICardService
    {
        private readonly AppDbContext _db;

        public CardService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<GetCardViewModel>> GetCardsAsync()
        {
            return await _db.Cards.Select(c => new GetCardViewModel
            {
                Id = c.Id,
                Name = c.Name,
                Img = c.Img
            }).ToListAsync();
        }

        public async Task AddCardAsync(AddCardViewModel cardViewModel)
        {
            var card = new Card
            {
                Name = cardViewModel.Name,
                Img = cardViewModel.Img
            };

            await _db.Cards.AddAsync(card);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateCardAsync(GetCardViewModel cardViewModel)
        {
            var card = await _db.Cards.FirstOrDefaultAsync(c => c.Id == cardViewModel.Id);

            card.Name = cardViewModel.Name;
            card.Img = cardViewModel.Img;

            await _db.SaveChangesAsync();
        }


        public async Task DeleteCardAsync(IdViewModel vm)
        {
            _db.Cards.Remove(await _db.Cards.FirstOrDefaultAsync(c => c.Id == vm.Id));
            await _db.SaveChangesAsync();
        }

        public async Task<Card> GetCardById(IdViewModel vm)
        {
            var card = await _db.Cards.FirstOrDefaultAsync(c => c.Id == vm.Id);
           
            return card;
        }
    }
}
