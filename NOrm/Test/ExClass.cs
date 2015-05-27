using NOrm.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOrm.Test
{
    [Attributes.ORMEntity("ExampleClass")]
    public class ExClass : BaseEntity
    {
        [Attributes.ORMPrimaryKey(true)]
        [Attributes.ORMColumn("id", "Identificador")]
        public int Identificador { get; set; }

        [Attributes.ORMColumn("nome", "Nome")]
        public string Nome { get; set; }

        [Attributes.ORMColumn("senha", "Senha")]
        public string Senha { get; set; }

        [Attributes.ORMColumn("data", "Data de Nascimento")]
        public DateTime Data { get; set; }

        public ExClass() : base()
        {

        }
    }
}
