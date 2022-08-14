using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TestTaskWebApi.Services;
using TestTaskWebApi.ViewModel;

namespace TestTaskWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CardController : Controller
    {
        private readonly ICardService _cardService;

        public CardController(ICardService cardService)
        {
            _cardService = cardService;
        }

        [HttpGet]
        [Route("get-card")]
        public async Task<IActionResult> GetCardAsync()
        {
            return Ok(await _cardService.GetCardsAsync());
        }

        [HttpPost]
        [Route("create-card")]
        public async Task<IActionResult> CreateCardAsync(AddCardViewModel cardViewModel)
        {
            await _cardService.AddCardAsync(cardViewModel);
            return Ok();
        }

        [HttpPost]
        [Route("update-card")]
        public async Task<IActionResult> UpdateCardAsync(GetCardViewModel cardViewModel)
        {
            await _cardService.UpdateCardAsync(cardViewModel);
            return Ok();
        }


        [HttpPost]
        [Route("delete-card")]
        public async Task<IActionResult> DeleteCardAsync(IdViewModel vm)
        {
            await _cardService.DeleteCardAsync(vm);
            return Ok();
        }

        [HttpPost]
        [Route("get-card-by-id")]
        public async Task<IActionResult> GetCardById(IdViewModel vm)
        {
            var card = await _cardService.GetCardById(vm);
            return Ok(card);
        }
    }
}
