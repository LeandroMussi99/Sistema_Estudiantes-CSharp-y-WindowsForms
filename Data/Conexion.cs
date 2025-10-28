using LinqToDB;
using LinqToDB.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class Conexion : DataConnection
    {
        public Conexion() : base("LM1")
        {
        }

        public ITable<Estudiante> EstudianteTabla { get { return this.GetTable<Estudiante>(); } }
    }
}
