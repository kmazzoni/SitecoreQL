using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using GraphQL;
using GraphQL.Types;
using Newtonsoft.Json;

namespace SitecoreQL.Controllers
{
    public class SitecoreQLController : ApiController
    {
        private readonly ISchema _schema;
        private readonly IDocumentExecuter _executer;

        public SitecoreQLController(IDocumentExecuter executer,
            ISchema schema)
        {
            _executer = executer;
            _schema = schema;
        }

        [HttpPost]
        public async Task<IHttpActionResult> Post(GraphQlModel query)
        {
            var result = await _executer.ExecuteAsync(_ =>
            {
                _.Schema = _schema;
                _.Query = query.Query;
                _.OperationName = query.OperationName ?? string.Empty;
                _.Inputs = query.Variables.ToInputs();
            });

            return new JsonResult<ExecutionResult>(result, new JsonSerializerSettings(), Encoding.UTF8, this);
        }
    }

    public class GraphQlModel
    {
        public string OperationName { get; set; }
        public string NamedQuery { get; set; }
        public string Query { get; set; }
        public string Variables { get; set; }
    }
}