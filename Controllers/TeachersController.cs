using Microsoft.AspNetCore.Mvc;
using System.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RestServiceOpgaver.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeachersController : ControllerBase
    {
        private TeacherRepository _teacherRepo;
        
        

        public TeachersController(TeacherRepository teacherRepository)
        {
            _teacherRepo = teacherRepository;
      
        }

        // GET: api/<TeachersController>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        public ActionResult<IEnumerable<Teacher>> Get()
        {
            
            var teachers = _teacherRepo.Read();
            if (teachers != null && teachers.Count() != 0)
                {
                    return Ok(teachers);
                }
                else
                    return NotFound(teachers);
          
           
           
        }

        // GET api/<TeachersController>/5
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public ActionResult<Teacher> Get(int id)
        {
            Teacher teacher = _teacherRepo.GetTeacher(id);
            if (teacher != null) 
            {
                return Ok(teacher);
            }
            else return NotFound(teacher);
        }

        // POST api/<TeachersController>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPost]
        public ActionResult<Teacher> Post([FromBody] Teacher teacher)
        {
            try
            {
                teacher.Validate();
                _teacherRepo.AddTeacher(teacher);
                return Ok(teacher);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        // PUT api/<TeachersController>/5
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPut("{id}")]
        public ActionResult<Teacher> Put(int id, [FromBody] Teacher updatedTeacher)
        {
            try
            {
                updatedTeacher.Validate();
                _teacherRepo.UpdateTeacher(id, updatedTeacher);
                return Ok(updatedTeacher);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        // DELETE api/<TeachersController>/5
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{id}")]
        public ActionResult<Teacher> Delete(int id)
        {
            if (_teacherRepo.GetTeacher(id) != null)
            {
                _teacherRepo.DeleteTeacher(id);
                return Ok(_teacherRepo.GetTeacher(id));
            }
            else return NotFound();
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("Amount")]
        public ActionResult<IEnumerable<Teacher>> GetAmount([FromHeader] string? amount)
        {
            if(amount == null)
            {
                return Ok("Intet er indtastet men det er fint");
            }
            if(int.TryParse(amount, out int amountParsed))
            {
                if (amountParsed < 0 || _teacherRepo.Read().Count()<amountParsed)
                {
                    return BadRequest();
                }
                else return Ok(_teacherRepo.Read().Take(amountParsed));
            }
            else return BadRequest();
   
        }
    }
}
