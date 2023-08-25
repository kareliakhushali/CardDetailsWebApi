using CardDetailsWebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CardDetailsWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CardsController : Controller
    {
        private readonly AppDbContext _context;

        public CardsController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllCards()
        {
            var cards = await _context.Cards.ToListAsync();
            return Ok(cards);
        }
        [HttpPost]
        public async Task<IActionResult>AddCards([FromBody] Card cardobj)
        {
            cardobj.Id = Guid.NewGuid();
            await _context.Cards.AddAsync(cardobj);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCard), new { id = cardobj.Id }, cardobj);
        }
        [HttpGet]
        [Route("{id:Guid}")]
       public async Task<IActionResult> GetCard([FromRoute] Guid id)
        {
            var card = await _context.Cards.FirstOrDefaultAsync(x => x.Id == id);
                if(card != null)
            {
                return Ok(card);
            }
            return  NotFound("Card  not found");
        }
        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateCard([FromRoute] Guid id , Card updateCardRequest)
        {
            var card = await _context.Cards.FindAsync(id);
            if(card == null)
            {
                return NotFound();
            }
            card.CardHolderName = updateCardRequest.CardHolderName;
            card.CardNumber = updateCardRequest.CardNumber;
            card.ExpiryMonth = updateCardRequest.ExpiryMonth;
            card.ExpiryYear = updateCardRequest.ExpiryYear;
            card.CVC = updateCardRequest.CVC;
            await _context.SaveChangesAsync();
            return Ok(card);

        }

        [HttpDelete]
        [Route("{id:Guid}")]

        public async Task<IActionResult> DeleteCard([FromRoute] Guid id)
        {
            var card = await _context.Cards.FindAsync(id);
            if (card == null)
            {
                return NotFound();
            }
            _context.Cards.Remove(card);
            await _context.SaveChangesAsync();
            return Ok(card);

        }

    }
}
