using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AccountOwnerServer.Controllers
{
   [Route("api/owner")]
   [ApiController]
   public class OwnerController : ControllerBase
   {
      private ILoggerManager _logger;
      private IRepositoryWrapper _repository;
      private IMapper _mapper;

      public OwnerController(ILoggerManager logger, IRepositoryWrapper repository, IMapper mapper)
      {
         _logger = logger;
         _repository = repository;
         _mapper = mapper;
      }
      [HttpGet]
      public IActionResult GetAllOwner()
      {
         try
         {
            var owners = _repository.Owner.GetAllOwners();
            _logger.LogInfo($"Return all owner from database.");

            var ownerResult = _mapper.Map<IEnumerable<OwnerDto>>(owners);
            return Ok(owners);
         }
         catch (Exception ex)
         {
            _logger.LogError($"Something went wrong inside GetAllOwners action: {ex.Message}");
            return StatusCode(500, "Internal server error");
         }
      }

      [HttpGet("{id}", Name = "OwnerById")]
      public IActionResult GetOwnerById(Guid id)
      {
         try
         {
            var owner = _repository.Owner.GetOwnerById(id);


            if (owner is null)
            {

               _logger.LogError($"Owner with id: {id}, hasn't been found in db.");
               return NotFound();
            }
            else
            {

               var ownerResult = _mapper.Map<OwnerDto>(owner);
               return Ok(ownerResult);
            }

         }
         catch (Exception ex)
         {
            _logger.LogError($"Something went wrong inside GetOwnerById action: {ex.Message}");
            return StatusCode(500, "Internal server error");
         }
      }

      [HttpGet("{id}/account")]
      public IActionResult GetOwnerWithDetails(Guid id)
      {

         try
         {
            var owner = _repository.Owner.GetOwnerWithDetails(id);
            if (owner == null)
            {
               _logger.LogError($"Owner with id: {id}, hasn't been found in db.");
               return NotFound();
            }
            else
            {
               _logger.LogInfo($"Returned owner with details for id: {id}");

               var ownerResult = _mapper.Map<OwnerDto>(owner);
               return Ok(ownerResult);

            }

         }
         catch (Exception ex)
         {

            _logger.LogError($"Something went wrong inside GetOwnerWithDetails action: {ex.Message}");
            return StatusCode(500, "Internal server error");
         }

      }

      [HttpPost]
      public IActionResult CreateOwner([FromBody] OwnerForCreationDto owner)
      {
         try
         {
            if (owner is null)
            {
               _logger.LogError("Owner object sent from client is null.");
               return BadRequest("Owner object is null");
            }

            if (!ModelState.IsValid)
            {
               _logger.LogError("Invalid owner object sent from client.");
               return BadRequest("Invalid model object");
            }

            var ownerEntity = _mapper.Map<Owner>(owner);

            _repository.Owner.CreateOwner(ownerEntity);
            _repository.Save();

            var createdOwner = _mapper.Map<OwnerDto>(ownerEntity);

            return CreatedAtRoute("OwnerById", new { id = createdOwner.Id }, createdOwner);
         }
         catch (Exception ex)
         {
            _logger.LogError($"Something went wrong inside CreateOwner action: {ex.Message}");
            return StatusCode(500, "Internal server error");
         }
      }

      [HttpPut("{id}")]
      public IActionResult UpadateOwner(Guid id, [FromBody] OwnerForUpdateDto owner)
      {
         try
         {
            if (owner is null)
            {
               _logger.LogError("Owner object sent from client is null.");
               return BadRequest("Owner object is null");
            }
            if (!ModelState.IsValid)
            {
               _logger.LogError("Invalid owner object sent from client.");
               return BadRequest("Invalid model object");
            }
            var ownerEntity = _repository.Owner.GetOwnerById(id);
            if (ownerEntity is null)
            {
               _logger.LogError($"Owner with id: {id}, hasn't been found in db.");
               return NotFound();
            }
            _mapper.Map(owner, ownerEntity);

            _repository.Owner.UpdateOwner(ownerEntity);
            _repository.Save();

            return NoContent();
         }
         catch (Exception ex)
         {

            _logger.LogError($"Something went wrong inside UpdateOwner action: {ex.Message}");
            return StatusCode(500, "Internal server error");

         }
      }

      [HttpDelete("{id}")]
      public IActionResult DeleteOwner(Guid id)
      {
         try
         {
            var owner = _repository.Owner.GetOwnerById(id);
            if (owner == null)
            {
               _logger.LogError($"Owner with id: {id}, hasn't been found in db.");
               return NotFound();
            }

            if (_repository.Account.AccountsByOwner(id).Any())
            {
               _logger.LogError($"Cannot delete owner with id: {id}. It has related accounts. Delete those accounts first");
               return BadRequest("Cannot delete owner. It has related accounts. Delete those accounts first");
            }

            _repository.Owner.DeleteOwner(owner);
            _repository.Save();

            return NoContent();
         }
         catch (Exception ex)
         {
            _logger.LogError($"Something went wrong inside DeleteOwner action: {ex.Message}");
            return StatusCode(500, "Internal server error");
         }
      }

      [HttpPost("{id}/account")]
      public IActionResult CreateAccount(Guid id, [FromBody] AccountForCreationDto account)
      {
         try
         {
            var owner = _repository.Owner.GetOwnerById(id);
            if (owner is null)
            {
               _logger.LogError($"Owner with id: {id}, hasn't been found in db.");
               return NotFound();

            }
            else
            {
               var accountEntity = _mapper.Map<Account>(account);
               _repository.Account.CreateAccount(accountEntity);

               var createdAcount = _mapper.Map<Account>(accountEntity);

               if (owner.Accounts == null)
               {
                  owner.Accounts = new List<Account>();
               }

               owner.Accounts.Add(createdAcount);
               _repository.Owner.Update(owner);
               _repository.Save();

               var ownerResult = _mapper.Map<OwnerDto>(owner);
               return Ok(ownerResult);
            }
         }
         catch (Exception ex)
         {
            _logger.LogError($"Something went wrong inside CreateOwner action: {ex.Message}");
            return StatusCode(500, "Internal server error");
         }
      }

      [HttpPut("account/{id}")]
      public IActionResult UpdateAccount(Guid id, [FromBody] AccountForUpdateDto account)
      {
         try
         {

            if (account is null)
            {
               _logger.LogError("Account object sent from client is null.");
               return BadRequest("Account object is null");
            }
            if (!ModelState.IsValid)
            {
               _logger.LogError("Invalid account object sent from client.");
               return BadRequest("Invalid model object");
            }

            var accountEntity = _repository.Account.GetAccountById(id);
            if (accountEntity is null)
            {
               _logger.LogError($"Owner with id: {id}, hasn't been found in db.");
               return NotFound();

            }

            _mapper.Map(account, accountEntity);
            _repository.Account.UpdateAccount(accountEntity);
            _repository.Save();

            return NoContent();

         }
         catch (Exception ex)
         {
            _logger.LogError($"Something went wrong inside CreateOwner action: {ex.Message}");
            return StatusCode(500, "Internal server error");
         }
      }

      [HttpDelete("account/{id}")]
      public IActionResult DeleteAccount(Guid id)
      {
         try
         {
            var account = _repository.Account.GetAccountById(id);
            if (account == null)
            {
               _logger.LogError($"Account with id: {id}, hasn't been found in db.");
               return NotFound();
            }

            _repository.Account.DeleteAccount(account);
            _repository.Save();

            return NoContent();
         }
         catch (Exception ex)
         {
            _logger.LogError($"Something went wrong inside DeleteOwner action: {ex.Message}");
            return StatusCode(500, "Internal server error");
         }
      }
   }
}
