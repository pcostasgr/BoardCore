using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BoardCore.Models;
using BoardCore.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace BoardCore.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    [ApiController]
    public class CardsController : ControllerBase
    {
        private readonly ICardsRepository _cards;

        public CardsController(ICardsRepository cards)
        {
            _cards = cards;
        }

        [HttpGet]
        [Route("id/{id}")]
        public async Task<ActionResult<Cards>> GetByID(Int64 id)
        {
            return await _cards.GetByID(id);
        }


        [HttpGet]
        [Route("listid/{id}")]
        public async Task<ActionResult<List<Cards>>> GetByListId(Int64 id)
        {
            return await _cards.GetByListId(id);
        }


        [HttpPost]
        [Route("")]
        [AllowAnonymous]
        public async Task<ActionResult<Cards>> AddList([FromBody] Cards model){
            
            var _card=new Cards 
                {
                 CARDID=-1,
                 CARDTITLE=model.CARDTITLE ,
                 CARDDESCR=model.CARDDESCR,
                 USERID=model.USERID,
                 LISTID=model.LISTID,
                 CARDDATE=model.CARDDATE
                };

           // return BadRequest("listid:" + _card.ToString()); 
            var result= await _cards.AddCard(_card);
            
            if(result==null){
                return BadRequest("Record is not inserted 1");
            }

            if(result.LISTID==-1){
                return BadRequest("Record is not inserted 2");
            }

            return await Task.FromResult(result);
        }

        [HttpPut]
        [Route("")]
        [AllowAnonymous]
        public async Task<ActionResult<Cards>> UpdateList([FromBody] Cards model){
            var _card=new Cards 
                {
                 CARDID=model.CARDID,
                 CARDTITLE=model.CARDTITLE ,
                 CARDDESCR=model.CARDDESCR,
                 USERID=model.USERID,
                 LISTID=model.LISTID,
                 CARDDATE=model.CARDDATE
                };
            // return BadRequest("listid:" + _card.ToString()); 
            var result=await _cards.UpdateCard(_card);
            if(result==null){
                return BadRequest("Record is not updated 1");
            }

            if(result.CARDID==-1){
                return BadRequest("Record is not updated 2");
            }

            return await Task.FromResult(_card);
        }
        
        [HttpDelete]
        [Route("{cardid}")]
        [AllowAnonymous]
        public async Task<ActionResult<Int64>> DeleteList(Int64 cardid){

            var result= await _cards.DeleteCard(cardid);

            if(result!=0){
                return BadRequest("Item not Deleted");
            }
            return Ok("Item Deleted:" + cardid);
        }

        
    }
}

