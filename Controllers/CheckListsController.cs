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
    public class CheckListsController : ControllerBase
    {
        private readonly ICheckListsRepository _lists;

        public CheckListsController(ICheckListsRepository lists)
        {
            _lists = lists;
        }

        [HttpGet]
        [Route("id/{id}")]
        public async Task<ActionResult<CheckLists>> GetByID(Int64 id)
        {
            return await _lists.GetByID(id);
        }


        [HttpGet]
        [Route("cardid/{id}")]
        public async Task<ActionResult<List<CheckLists>>> GetByCardId(Int64 id)
        {
            return await _lists.GetByCardId(id);
        }


        [HttpPost]
        [Route("")]
        [AllowAnonymous]
        public async Task<ActionResult<CheckLists>> AddCheckList([FromBody] CheckLists model){
            
            var _list=new CheckLists 
                {
                 CHECKLISTID=-1,
                 TITLE=model.TITLE,
                 CARDID=model.CARDID,
                 USERID=model.USERID
                };

            var result= await _lists.AddCheckList(_list);
            
            if(result==null){
                return BadRequest("Record is not inserted 1");
            }

            if(result.CHECKLISTID==-1){
                return BadRequest("Record is not inserted 2");
            }

            return await Task.FromResult(result);
        }

        [HttpPut]
        [Route("")]
        [AllowAnonymous]
        public async Task<ActionResult<CheckLists>> UpdateCheckList([FromBody] CheckLists model){
             var _list=new CheckLists 
                {
                 CHECKLISTID=model.CHECKLISTID,
                 TITLE=model.TITLE,
                 CARDID=model.CARDID,
                 USERID=model.USERID
                };
            
            var result=await _lists.UpdateCheckList(_list);
            if(result==null){
                return BadRequest("Record is not updated 1");
            }

            if(result.CHECKLISTID==-1){
                return BadRequest("Record is not updated 2");
            }

            return await Task.FromResult(_list);
        }
        
        [HttpDelete]
        [Route("{listid}")]
        [AllowAnonymous]
        public async Task<ActionResult<Int64>> DeleteCheckList(Int64 listId){

            var result= await _lists.DeleteCheckList(listId);

            if(result!=0){
                return BadRequest("Item not Deleted");
            }
            return Ok("Item Deleted:" + listId);
        }

        
    }
}

