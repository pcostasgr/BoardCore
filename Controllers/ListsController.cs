using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BoardCore.Models;
using BoardCore.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace BoardCore.Controllers
{
    [Authorize]
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    [ApiController]
   public class ListsController : ControllerBase
    {
        private readonly IListsRepository _lists;

        public ListsController(IListsRepository lists)
        {
            _lists = lists;
        }

        [HttpGet]
        [Route("id/{id}")]
        public async Task<ActionResult<Lists>> GetByID(Int64 id)
        {
            return await _lists.GetByID(id);
        }


        [Route("userid/{id}")]
        public async Task<ActionResult<List<Lists>>> GetByUserId(Int64 id)
        {
            return await _lists.GetByUserId(id);
        }

        [HttpGet]
        [Route("all")]
        [AllowAnonymous]
        public async Task<ActionResult<List<Lists>>> GetAll(){
            return await _lists.GetAll();
        }

        [HttpPost]
        [Route("")]
        [AllowAnonymous]
        public async Task<ActionResult<Lists>> AddList([FromBody] Lists model){
            
            var _list=new Lists 
                {
                 LISTID=-1,
                 ListTitle=model.ListTitle,
                 USERID=model.USERID
                };
            
            var result= await _lists.AddList(_list);
            
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
        public async Task<ActionResult<Lists>> UpdateList([FromBody] Lists model){
             var _list=new Lists 
                {
                 LISTID=model.LISTID,
                 ListTitle=model.ListTitle,
                 USERID=model.USERID
                };
            
            var result=await _lists.UpdateList(_list);
            if(result==null){
                return BadRequest("Record is not updated 1");
            }

            if(result.LISTID==-1){
                return BadRequest("Record is not updated 2");
            }

            return await Task.FromResult(_list);
        }
        
        [HttpDelete]
        [Route("{listid}")]
        [AllowAnonymous]
        public async Task<ActionResult<Int64>> DeleteList(Int64 listId){

            var result= await _lists.DeleteList(listId);

            if(result!=0){
                return BadRequest("Item not Deleted");
            }
            return Ok("Item Deleted:" + listId);
        }

        
    }
}

