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
    public class CheckListItemsController : ControllerBase
    {
        private readonly ICheckListItemsRepository _lists;

        public CheckListItemsController(ICheckListItemsRepository lists)
        {
            _lists = lists;
        }



        [HttpPost]
        [Route("")]
        [AllowAnonymous]
        public async Task<ActionResult<CheckListItems>> AddCheckListItem([FromBody] CheckListItems model){
            
            var _item=new CheckListItems
                {
                 CLITEMID=-1,
                 CHECKLISTID=model.CHECKLISTID,
                 ITEMTITLE=model.ITEMTITLE,
                 ISCHECKED=model.ISCHECKED,
                 USERID=model.USERID
                };

            var result= await _lists.AddItem(_item);
            
            if(result==null){
                return BadRequest("Record is not inserted 1");
            }

            if(result.CLITEMID==-1){
                return BadRequest("Record is not inserted 2");
            }

            return await Task.FromResult(result);
        }

        [HttpPut]
        [Route("")]
        [AllowAnonymous]
        public async Task<ActionResult<CheckListItems>> UpdateCheckListItem([FromBody] CheckListItems model){
             var _item=new CheckListItems
                {
                 CLITEMID=model.CLITEMID,
                 CHECKLISTID=model.CHECKLISTID,
                 ITEMTITLE=model.ITEMTITLE,
                 ISCHECKED=model.ISCHECKED,
                 USERID=model.USERID
                };

            var result= await _lists.UpdateItem(_item);
            
            if(result==null){
                return BadRequest("Record is not updated 1");
            }

            if(result.CLITEMID==-1){
                return BadRequest("Record is not updated 2");
            }

            return await Task.FromResult(result);
        }
        
        [HttpDelete]
        [Route("{itemid}")]
        [AllowAnonymous]
        public async Task<ActionResult<Int64>> DeleteCheckListItem(Int64 itemid){

            var result= await _lists.DeleteItem(itemid);

            if(result!=0){
                return BadRequest("Item not Deleted");
            }
            return Ok("Item Deleted:" + itemid);
        }

        
    }
}

