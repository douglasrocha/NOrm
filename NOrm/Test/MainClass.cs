using NOrm.Action;
using NOrm.Controller;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOrm.Test
{
    public class MainClass
    {
        public static void Main()
        {
            var ex1 = new ExClass { Identificador = 1,
                                    Nome = "Douglas",
                                    Senha = "MinhaSenha",
                                    Data = DateTime.Now,
                                    recordState = Enums.RecordState.insert };

            // Demonstração queries CRUD
            Console.WriteLine(QueryParserService.GetSelectAllQuery<ExClass>());
            Console.WriteLine(QueryParserService.GetSelectByIdQuery(ex1));
            Console.WriteLine(QueryParserService.GetInsertQuery(ex1));
            Console.WriteLine(QueryParserService.GetUpdateByIdQuery(ex1));
            Console.WriteLine(QueryParserService.GetDeleteByIdQuery(ex1));
            
            // Demonstração Object Mapper
            var dt = new DataTable();
            dt.Columns.Add("id");
            dt.Columns.Add("nome");
            dt.Columns.Add("senha");
            dt.Columns.Add("data");

            dt.Rows.Add(new object[] { ex1.Identificador, ex1.Nome, ex1.Senha, ex1.Data });

            var ex2 = new ExClass();
            ex2.recordState = Enums.RecordState.update;

            EntityMapperService.BindByColumnName(dt.Rows[0], ref ex2);

            // Demonstração ORMContext
            ORMContext.Save(ex1);
            ORMContext.Save(ex2);

            var getAll = ORMContext.GetAll<ExClass>();
            var getById = ORMContext.GetById(ref ex2);
        }
    }
}
